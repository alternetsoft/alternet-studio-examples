using System;

namespace FormDesigner.InMemory.Wpf
{
    public class ToolBoxItem
    {
        private object instance;

        public object Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Activator.CreateInstance(Type);
                }

                return instance;
            }
        }

        public string Name
        {
            get
            {
                return Type.Name;
            }
        }

        public Type Type { get; set; }
    }
}