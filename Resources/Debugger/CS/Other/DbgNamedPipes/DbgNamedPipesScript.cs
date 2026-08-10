using System;
using System.Diagnostics;

External.Log("Start Test " + DateTime.Now.ToString("HH:mm:ss"), "Black");

for (int i = 0; i < 10; i++)
{
	Trace.WriteLine(i);

	External.Log("Test " + i, "Blue");
}

External.Log("End Test", "Red");




