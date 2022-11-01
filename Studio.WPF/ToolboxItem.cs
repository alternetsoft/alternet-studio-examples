using System;

namespace AlternetStudio.Wpf.Demo
{
    public class ToolBoxItem
    {
        private object instance;

        public Type Type { get; set; }

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
    }
}
