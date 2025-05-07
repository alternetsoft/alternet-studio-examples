///<reference path="clr.d.ts" />

function reverseString(str: string)
{
    var newString = "";
    for (var i = str.length - 1; i >= 0; i--)
        newString += str[i];
    return newString;
}

var text = "Hello World";
System.Windows.Forms.MessageBox.Show(reverseString(text));