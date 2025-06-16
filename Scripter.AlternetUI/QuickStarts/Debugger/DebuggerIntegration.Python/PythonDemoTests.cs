using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Common;
using Alternet.Scripter.Python;
using Alternet.UI;

using Python.Runtime;

namespace DebuggerIntegration.Python
{
    public static class PythonDemoTests
    {
        static PythonDemoTests()
        {
        }

        public static void TestDynamicObjectExtender()
        {
            ScriptEngine.InitializeRuntime();

            using (Py.GIL())
            {
                MyManagedObject dotNetObject = new();
                PyObject pyObject = dotNetObject.ToPython();

                dynamic pyScope = Py.CreateScope();

                pyScope.Set("ManagedObject", pyObject);

                pyScope.Exec(@"
print(ManagedObject.Message)
ManagedObject.Message = 'Changed message'
print(ManagedObject.Message)
print(ManagedObject.AddNumbers(5, b = 10, rrr=25))

# print(ManagedObject.AddNumbers(5, b = 10))
");
            }

        }

        public class MyManagedObject
        {
            private string message = "Hello from .NET!";

            public string Message
            {
                get
                {
                    App.Log($"MyManagedObject.Message getter called.");
                    return message;
                }

                set
                {
                    App.Log($"MyManagedObject.Message setter called.");
                    message = value;
                }
            }

            public int AddNumbers(int a, int b)
            {
                App.Log($"MyManagedObject.AddNumbers({a}, {b}) called.");
                return a + b;
            }
        }
    }
}