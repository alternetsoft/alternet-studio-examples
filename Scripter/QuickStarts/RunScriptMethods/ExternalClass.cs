using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunScriptMethods;

public class ExternalClass
{
    private readonly MainForm owner;

    internal ExternalClass(MainForm owner)
    {
        this.owner = owner;
    }

    public int X { get; set; } = 100;

    public int Y { get; set; } = 200;

    public string MainWindowText
    {
        get => owner.Text;
        set => owner.Text = value;
    }
}