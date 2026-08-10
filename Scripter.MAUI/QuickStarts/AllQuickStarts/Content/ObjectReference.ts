///<reference path="clr.d.ts" />

class Catcher {
    public CatchButton() {
        RunButton.Text = "Catch me if you can";
    }

    public ChangeButtonLocation() {
        let x: number = (<number>(((35 * autoRand.Next(0, 10))
            + 1)));
        let y: number = (<number>(((12 * autoRand.Next(0, 10))
            + 1)));
        RunButton.Margin = new Microsoft.Maui.Thickness(x, y, 0, 0);
    }

    public constructor() {
    }
}

var f: Catcher;

function RunMe() {
    f = new Catcher();
    f.CatchButton();
    return f;
}

