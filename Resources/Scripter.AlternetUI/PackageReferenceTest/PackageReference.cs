#nullable enable

using System;
using System.Diagnostics;
using Newtonsoft.Json;

public class ScriptClass
{
    public static void Main()
    {
		Console.WriteLine("Press Enter to start...");
		Console.ReadLine();

        string data = @"{
              a: 1,
              name: ""Bill Smith"",
              isTall: true
            }";

		dynamic? obj = JsonConvert.DeserializeObject(data);
        
		if (obj != null)
        {
            var s = string.Format("Object name: {0}", obj.name);
            Console.WriteLine(s);
        }

		Console.WriteLine();
		Console.WriteLine("Done. Press Enter to exit...");
		Console.ReadLine();
	}
}
