using System.ComponentModel;

using Alternet.Common;

namespace DebuggerNamedPipes;

public partial class Form1 : DebuggerIntegration.Form1
{
    private readonly RichTextBox OutputRichTextBox;
    private Task? serverTask;
    private CancellationTokenSource cts = new();

    static Form1()
    {
        DebuggerIntegration.Form1.ProjectSearchDirectories = new[] { ".", @"..\..\..\..\..\..\..\" };
        DebuggerIntegration.Form1.StartupProjectFileSubPath = @"Resources\Debugger\CS\Other\DbgNamedPipes\DbgNamedPipes.csproj";
    }

    public Form1()
    {
        Debugger.StateChanged += Debugger_StateChanged;

        OutputRichTextBox = new RichTextBox
        {
            Dock = DockStyle.Fill,
        };

        Shown += Form1_Shown;
        Closing += Form1_Closing;

        RestartTask();
    }

    internal Task? ServerTask => serverTask;

    public static async Task<string> LogViaPipes(string s, string color = "Black")
    {
        return await DbgNamedPipes.SendRequest("Log", new string[] { s, color });
    }

    private void CancelTask()
    {
        if (serverTask != null)
        {
            cts.Cancel();
            serverTask.Wait();
            serverTask = null;
        }
    }

    private void RestartTask()
    {
        CancelTask();

        serverTask = Task.Run(() => DbgNamedPipes.InitServer(HandleRequest, cts.Token), cts.Token);
    }

    private void Form1_Closing(object? sender, CancelEventArgs e)
    {
        cts.Cancel();
    }

    private DbgNamedPipes.Response HandleRequest(DbgNamedPipes.Request request)
    {
        DbgNamedPipes.Response response = new();

        if (request is null)
        {
            return response;
        }

        if (request.Method == "Log")
        {
            var message = request?.Args?[0] ?? string.Empty;
            var color = request?.Args?.Length > 1 ? request.Args[1] : "Black";
            Log(message, color);
            response.StringResult = "Logged: " + message;
        }
        else
        {
            response.StringResult = "Unknown method: " + request.Method;
        }

        return response;
    }

    private void Form1_Shown(object? sender, EventArgs e)
    {
        Shown -= Form1_Shown;
        Log("Form1 initialized.", "Green");
    }

    private void Log(string text, string color)
    {
        Log(text, Color.FromName(color));
    }

    private void Log(string text, Color color)
    {
        Invoke((MethodInvoker)(() =>
        {
            DebuggerPanels.Output.CustomLog($"{text} ({color})" + Environment.NewLine);
        }));

        if (OutputRichTextBox.Parent is null)
            return;

        OutputRichTextBox.Invoke((MethodInvoker)(() =>
        {
            // Move caret to end
            OutputRichTextBox.SelectionStart = OutputRichTextBox.TextLength;
            OutputRichTextBox.SelectionLength = 0;

            // Apply color
            OutputRichTextBox.SelectionColor = color;

            // Append text
            OutputRichTextBox.AppendText(text + Environment.NewLine);

            // Reset color if you want subsequent text to be default
            OutputRichTextBox.SelectionColor = OutputRichTextBox.ForeColor;

            // Scroll to end
            OutputRichTextBox.ScrollToCaret();
        }));
    }

    private void Debugger_StateChanged(object? sender, Alternet.Scripter.Debugger.DebuggerStateChangedEventArgs e)
    {
        var state = e.NewState;
        DebuggerPanels.Output.CustomLog($"Debugger state changed to: {state}" + Environment.NewLine);

        if (state == Alternet.Scripter.Debugger.DebuggerState.Startup)
        {
        }
    }
}
