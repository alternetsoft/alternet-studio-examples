using System;
using Alternet.Drawing;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;

public class Catcher : System.ComponentModel.Component
{
    public void CatchButton()
    {
        ScriptGlobalClass.RunButton.Text = "Catch me if you can";
    }

    public void ChangeButtonLocation()
    {
        Random autoRand = new Random();
        int x = (int)(35 * autoRand.Next(0, 10) + 1);
        int y = (int)(15 * autoRand.Next(0, 10) + 1);
        ScriptGlobalClass.RunButton.Margin = new Thickness(x, y, 0, 0);
    }

    public void RunButtonClick(object obj, EventArgs args)
    {
        t.Stop();
        ScriptGlobalClass.RunButton.Text = "Test Button";
    }

    public static Catcher Main()
    {
        Catcher f = new Catcher();
        f.CatchButton();
        return f;
    }

    private IDispatcherTimer t;

    public Catcher()
    {
        t = Microsoft.Maui.Controls.Application.Current.Dispatcher.CreateTimer();
        t.Interval = TimeSpan.FromMilliseconds(1000);
        t.Tick += (s, e) => ChangeButtonLocation();
        ScriptGlobalClass.RunButton.Clicked += new EventHandler(RunButtonClick);
        t.Start();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ScriptGlobalClass.RunButton.Clicked -= new EventHandler(RunButtonClick);
            t.Stop();
        }
        base.Dispose(disposing);
    }

}