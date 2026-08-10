///<reference path="clr.d.ts" />

var t;

function CatchButton() {
	RunButton.Text = "Catch me if you can";
}

function ChangeButtonLocation(obj, args) {
	var autoRand = new System.Random();
	var x = (Math.ceil(((35 * autoRand.Next(0, 10)) 
				+ 1)));
	var y = (Math.ceil(((15 * autoRand.Next(0, 10)) 
				+ 1)));
	RunButton.Margin = new Alternet.UI.Thickness(x, y, 0, 0)
}

function RunButtonClick() {
	timer.Stop();
	RunButton.Text = "Test Button";
}

function Initialize () {
	timer.Interval = 1000;
	timer.Tick.connect(ChangeButtonLocation);
	timer.Start();
}

function Dispose() {
	timer.Dispose();
}

function RunMe()
{
	var f = {
		"Dispose": Dispose
	};
	CatchButton();
	Initialize();
	return f;
}