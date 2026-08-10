using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunScriptMethods
{
    public partial class ScriptClass
    {
        public static ScriptClass SharedDefault { get; } = new ScriptClass();

        // Instance field initialized inline
        private int count = 5;

        // Static field initialized inline
        public static string globalVar = "just a global var";

        // Compile-time constant
        public const double Pi = 3.14159265358979;

        public static string GlobalProperrty
        {
            get => globalVar;
            set => globalVar = value;
        }

        public string InstanceProperty
        {
            get => globalVar;
            set => globalVar = value;
        }

        // Read-only field - can be initialized inline or in the constructor
        private readonly DateTime createdAt = DateTime.UtcNow;

        public static void WithException()
        {
            throw new Exception("Exception raised from script with some message");
        }

        public static void Main(string text)
        {
            Console.WriteLine("Main app says: " + text + " and " + globalVar);
        }

        public static async void MainAsync(string text)
        {
            Console.WriteLine("MainAsync says: " + text + " and " + globalVar);
        }

        public static int GetData()
        {
            return 11;
        }

        public double InstanceMethod()
        {
            return Pi;
        }
    }
}
