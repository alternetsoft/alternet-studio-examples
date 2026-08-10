///<reference path="clr.d.ts" />

function CatchButton() {
	RunButton.Text = "Catch me if you can";
}

function ChangeButtonLocation() {
	var x = (Math.ceil(((35 * autoRand.Next(0, 10))
		+ 1)));
	var y = (Math.ceil(((12 * autoRand.Next(0, 10))
		+ 1)));
	RunButton.Margin = new Microsoft.Maui.Thickness(x, y, 0, 0);
}

function RunMe() {
	var f = {
		"ChangeButtonLocation": ChangeButtonLocation
	};
	CatchButton();
	return f;
}