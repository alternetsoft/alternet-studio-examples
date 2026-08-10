using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;

public static class External
{
    public static void Log(string s, string color = "Black")
    {
#pragma warning disable
        SendRequest("Log", new string[] { s, color }).Wait();
#pragma warning restore
    }

    public static string PipeName = "AlternetDebuggerPipe";

    private static NamedPipeClientStream? client;
    private static StreamReader? reader;
    private static StreamWriter? writer;

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
        await writer.WriteLineAsync(JsonSerializer.Serialize(request));

        var responseJson = await reader.ReadLineAsync();

        if (responseJson == null)
            return string.Empty;

        var response = JsonSerializer.Deserialize<Response>(responseJson);
        return response?.StringResult ?? string.Empty;
    }

    [MemberNotNull(nameof(writer))]
    [MemberNotNull(nameof(reader))]
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