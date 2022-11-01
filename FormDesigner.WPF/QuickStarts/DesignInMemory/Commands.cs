using System.Windows.Input;

namespace FormDesigner.InMemory.Wpf
{
    public static class Commands
    {
        private static RoutedCommand saveToolboxToFile = new RoutedCommand();
        private static RoutedCommand loadToolboxFromFile = new RoutedCommand();
        private static RoutedCommand resetToolbox = new RoutedCommand();
        private static RoutedCommand addControlsFromAssembly = new RoutedCommand();
        private static RoutedCommand alignBottom = new RoutedCommand();
        private static RoutedCommand alignCenter = new RoutedCommand();
        private static RoutedCommand alignLeft = new RoutedCommand();
        private static RoutedCommand alignMiddle = new RoutedCommand();
        private static RoutedCommand alignRight = new RoutedCommand();
        private static RoutedCommand alignTop = new RoutedCommand();
        private static RoutedCommand bringToFront = new RoutedCommand();
        private static RoutedCommand lockControls = new RoutedCommand();
        private static RoutedCommand run = new RoutedCommand();
        private static RoutedCommand sendToBack = new RoutedCommand();
        private static RoutedCommand stretchToSameHeight = new RoutedCommand();
        private static RoutedCommand stretchToSameWidth = new RoutedCommand();
        private static RoutedCommand rasterPlacement = new RoutedCommand();
        private static RoutedCommand snaplinePlacement = new RoutedCommand();

        public static RoutedCommand ResetToolbox
        {
            get
            {
                return resetToolbox;
            }

            set
            {
                resetToolbox = value;
            }
        }

        public static RoutedCommand RasterPlacement
        {
            get
            {
                return rasterPlacement;
            }

            set
            {
                rasterPlacement = value;
            }
        }

        public static RoutedCommand SnaplinePlacement
        {
            get
            {
                return snaplinePlacement;
            }

            set
            {
                snaplinePlacement = value;
            }
        }

        public static RoutedCommand SaveToolboxToFile
        {
            get
            {
                return saveToolboxToFile;
            }

            set
            {
                saveToolboxToFile = value;
            }
        }

        public static RoutedCommand LoadToolboxFromFile
        {
            get
            {
                return loadToolboxFromFile;
            }

            set
            {
                loadToolboxFromFile = value;
            }
        }

        public static RoutedCommand AddControlsFromAssembly
        {
            get
            {
                return addControlsFromAssembly;
            }

            set
            {
                addControlsFromAssembly = value;
            }
        }

        public static RoutedCommand AlignBottom
        {
            get
            {
                return alignBottom;
            }

            set
            {
                alignBottom = value;
            }
        }

        public static RoutedCommand AlignCenter
        {
            get
            {
                return alignCenter;
            }

            set
            {
                alignCenter = value;
            }
        }

        public static RoutedCommand AlignLeft
        {
            get
            {
                return alignLeft;
            }

            set
            {
                alignLeft = value;
            }
        }

        public static RoutedCommand AlignMiddle
        {
            get
            {
                return alignMiddle;
            }

            set
            {
                alignMiddle = value;
            }
        }

        public static RoutedCommand AlignRight
        {
            get
            {
                return alignRight;
            }

            set
            {
                alignRight = value;
            }
        }

        public static RoutedCommand AlignTop
        {
            get
            {
                return alignTop;
            }

            set
            {
                alignTop = value;
            }
        }

        public static RoutedCommand BringToFront
        {
            get
            {
                return bringToFront;
            }

            set
            {
                bringToFront = value;
            }
        }

        public static RoutedCommand LockControls
        {
            get
            {
                return lockControls;
            }

            set
            {
                lockControls = value;
            }
        }

        public static RoutedCommand Run
        {
            get
            {
                return run;
            }

            set
            {
                run = value;
            }
        }

        public static RoutedCommand SendToBack
        {
            get
            {
                return sendToBack;
            }

            set
            {
                sendToBack = value;
            }
        }

        public static RoutedCommand StretchToSameHeight
        {
            get
            {
                return stretchToSameHeight;
            }

            set
            {
                stretchToSameHeight = value;
            }
        }

        public static RoutedCommand StretchToSameWidth
        {
            get
            {
                return stretchToSameWidth;
            }

            set
            {
                stretchToSameWidth = value;
            }
        }
    }
}