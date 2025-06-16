using System;

using Alternet.UI;

namespace Customize
{
    public static class KeyUtils
    {
        private static string sControl = "CONTROL";
        private static string sCtrl = "CTRL";
        private static string sAlt = "ALT";
        private static string sShift = "SHIFT";

        public static Keys KeyDataFromString(string txt)
        {
            string[] stringArray = txt.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            bool isCtrl = false;
            bool isAlt = false;
            bool isShift = false;
            var result = Keys.None;

            foreach (var s in stringArray)
            {
                if (s == "+")
                    continue;
                if (s.Equals(sCtrl, StringComparison.OrdinalIgnoreCase))
                    isCtrl = true;
                else
                    if (s.Equals(sAlt, StringComparison.OrdinalIgnoreCase))
                    isAlt = true;
                else
                if (s.Equals(sShift, StringComparison.OrdinalIgnoreCase))
                    isShift = true;
                else
                {
                    Keys res = Keys.None;
                    if (Enum.TryParse(s, out res))
                        result |= res;
                }
            }

            if (isAlt)
                result |= Keys.Alt;
            if (isShift)
                result |= Keys.Shift;
            if (isCtrl)
                result |= Keys.Control;

            return result;
        }

        public static string KeyDataToString(Keys keyData)
        {
            string result = keyData.ToString();
            string[] stringArray = result.Split(',');
            bool isCtrl = false;
            bool isAlt = false;
            bool isShift = false;
            result = string.Empty;
            foreach (var s in stringArray)
            {
                var trims = s.Trim();
                if (trims.Equals(sControl, StringComparison.OrdinalIgnoreCase))
                    isCtrl = true;
                else
                if (trims.Equals(sAlt, StringComparison.OrdinalIgnoreCase))
                    isAlt = true;
                else
                if (trims.Equals(sShift, StringComparison.OrdinalIgnoreCase))
                    isShift = true;
                else
                    result = (result != string.Empty) ? string.Format("{0} + {1}", result, trims) : trims;
            }

            if (isAlt)
                result = (result != string.Empty) ? string.Format("{0} + {1}", sAlt, result) : sAlt;
            if (isShift)
                result = (result != string.Empty) ? string.Format("{0} + {1}", sShift, result) : sShift;
            if (isCtrl)
                result = (result != string.Empty) ? string.Format("{0} + {1}", sCtrl, result) : sCtrl;
            return result;
        }
    }
}
