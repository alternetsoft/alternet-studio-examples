using System;
using System.Windows.Forms;
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
            MessageBox.Show(vl);
        }
        else
        {
            MessageBox.Show("Error in JsonConvert.DeserializeObject");
        }
      
    }
}