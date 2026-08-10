using System;
using System.Diagnostics;

using Newtonsoft.Json;

public class ScriptClass
{
    public static void Main()
    {
        string data = @"{
              a: 1,
              name: ""Bill Smith"",
              isTall: true
            }";
        dynamic? obj = JsonConvert.DeserializeObject(data);
        if (obj != null)
        {
            string vl = $"Object name: {obj.name}";
            Console.WriteLine(vl);
        }
        else
        {
            Console.WriteLine("Error in JsonConvert.DeserializeObject");
        }
       
        Console.Write("Press Enter to continue...");
        Console.ReadLine();        
    }
}