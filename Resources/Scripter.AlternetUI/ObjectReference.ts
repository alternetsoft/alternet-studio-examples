///<reference path="clr.d.ts" />

class Catcher
{
    public CatchButton() {
        RunButton.Text = "Catch me if you can";
    }
    
    public ChangeButtonLocation(obj: System.Object, args: System.EventArgs) {
        let autoRand: System.Random = new System.Random();
        let x: number = (<number>(((35 * autoRand.Next(0, 10)) 
                    + 1)));
        let y: number = (<number>(((15 * autoRand.Next(0, 10)) 
                    + 1)));
        RunButton.Margin = new Alternet.UI.Thickness(x, y, 0, 0)
    }
    
    public RunButtonClick() {
        timer.Stop();
        RunButton.Text = "Test Button";
    }
    
    public constructor () {
        timer.Interval = 1000;
        timer.Tick.connect(this.ChangeButtonLocation);
        timer.Start();
    }
    
    public Dispose() {
		timer.Dispose();
    }
}

function RunMe()
{
	let f: Catcher = new Catcher();
	f.CatchButton();
	return f;
}