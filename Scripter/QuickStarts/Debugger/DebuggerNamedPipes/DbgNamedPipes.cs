using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DebuggerNamedPipes;

public static class DbgNamedPipes
{
    private static NamedPipeClientStream? client;
    private static StreamReader? reader;
    private static StreamWriter? writer;

    public static string PipeName { get; set; } = "AlternetDebuggerPipe";

    public static void DoneClient()
    {
        reader?.Dispose();
        writer?.Dispose();
        reader = null;
        writer = null;
        client?.Dispose();
        client = null;
    }

    public static async Task<string> SendRequest(string method, string[] args)
    {
        InitClient();
        var request = new Request { Method = method, Args = args };
        await writer!.WriteLineAsync(JsonSerializer.Serialize(request));

        var responseJson = await reader!.ReadLineAsync();

        if (responseJson == null)
            return string.Empty;

        var response = JsonSerializer.Deserialize<Response>(responseJson);
        return response?.StringResult ?? string.Empty;
    }

    public static void InitClient()
    {
        if (client is null)
        {
            client = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut);
            client.Connect();
        }

        reader ??= new StreamReader(client);
        writer ??= new StreamWriter(client) { AutoFlush = true };
    }

    public static async Task InitServer(Func<Request, Response> handleRequest, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            using var server = new NamedPipeServerStream(
                PipeName,
                PipeDirection.InOut,
                1, // max server instances
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);

            await server.WaitForConnectionAsync(token);

            using var reader = new StreamReader(server);
            using var writer = new StreamWriter(server) { AutoFlush = true };

            try
            {
                while (!token.IsCancellationRequested && server.IsConnected)
                {
                    var requestJson = await reader.ReadLineAsync();
                    if (requestJson == null) break;

                    var request = JsonSerializer.Deserialize<Request>(requestJson);
                    if (request != null)
                    {
                        var response = handleRequest(request);
                        await writer.WriteLineAsync(JsonSerializer.Serialize(response));
                        if (response.Exit) return; // exit entire server loop
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Server cancelled gracefully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in InitServer: {ex.Message}");
            }
        }
    }

    public class Request
    {
        public string? Method { get; set; }

        public string[]? Args { get; set; }
    }

    public class Response
    {
        public bool Exit { get; set; } = false;

        public bool Success { get; set; } = false;

        public string? StringResult { get; set; }
    }
}
