using System;
using System.Collections.Generic;
using System.Diagnostics;
class Test{
    
    public static void Main()
    {
        List<int> my_int = new(){
            15,
            -25,
            18,
            455,
            0,
        };

        string specialCharacters0 = "!\"#$%&'()*+,-./:;<=>?@[\\]^_{|}~";
        string specialCharacters1 = " \t\n\r\f\v";
        string specialCharacters2 = "ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½";
        string specialCharacters3 = "\u0001\u0002\u0000\u0003\u0004\u0005\u0006\u0007\u0008\u000B\u000C\u000E\u000F" +
            "\u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019\u001A\u001B\u001C\u001D\u001E\u001F";
        string specialCharacters4 = "a_\\_\t_\t_\r_\n_b";

        string escaped_tab = "Escaped tab\\tis tabbed";
        string tabbed = "Normal tab\tis tabbed";
        List<string> my_list = new(){
        "this\\tuses escaped tab",
        "this\\tuses escaped tab",
        "this\tuses a tab",
        "this\tuses a tab",
        "this\tuses a tab",};

        Log(specialCharacters0);
        Log(specialCharacters1);
        Log(specialCharacters2);
        Log(specialCharacters3);
        Log(specialCharacters4);

        Log(escaped_tab);
        Log(tabbed);
    }
    
    private static void Log(string s)
    {
        Debug.WriteLine(s);
    }
}

