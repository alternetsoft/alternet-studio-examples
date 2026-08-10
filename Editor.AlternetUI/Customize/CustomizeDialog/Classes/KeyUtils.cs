using System;

using Alternet.UI;

namespace Alternet.Editor.CustomizeDialog.AlternetUI
{
    public static class KeyUtils
    {
        private static string sControl = "CONTROL";
        private static string sCtrl = "CTRL";
        private static string sAlt = "ALT";
        private static string sShift = "SHIFT";

        public static Keys KeyDataFromString(string? txt)
        {
            if (txt is null || txt.Length == 0)
                return Keys.None;

            string[] stringArray
                = txt.Split(new char[] { ' ', '+' }, StringSplitOptions.RemoveEmptyEntries);
            bool isCtrl = false;
            bool isAlt = false;
            bool isShift = false;
            var result = Keys.None;

            foreach (var s in stringArray)
            {
                var s2 = s.Trim();

                if (s2 == "+")
                    continue;
                if (s2.Equals(sControl, StringComparison.OrdinalIgnoreCase))
                    isCtrl = true;
                else
                if (s2.Equals(sCtrl, StringComparison.OrdinalIgnoreCase))
                    isCtrl = true;
                else
                if (s2.Equals(sAlt, StringComparison.OrdinalIgnoreCase))
                    isAlt = true;
                else
                if (s2.Equals(sShift, StringComparison.OrdinalIgnoreCase))
                    isShift = true;
                else
                if(result == Keys.None)
                {
                    Keys res = Keys.None;

                    if (Enum.TryParse<Keys>(
                        s2,
                        ignoreCase: true,
                        out res))
                    {
                        result |= res;
                    }
                    else
                    {
                        return Keys.None;
                    }
                }
                else
                {
                    return Keys.None;
                }
            }

            if(result == Keys.None)
            {
                return Keys.None;
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
                {
                    result = (result != string.Empty)
                        ? string.Format("{0} + {1}", result, trims) : trims;
                }
            }

            if (isAlt)
                result = (result != string.Empty) ? string.Format("{0} + {1}", sAlt, result) : sAlt;
            
            if (isShift)
            {
                result = (result != string.Empty)
                    ? string.Format("{0} + {1}", sShift, result) : sShift;
            }

            if (isCtrl)
            {
                result = (result != string.Empty)
                    ? string.Format("{0} + {1}", sCtrl, result) : sCtrl;
            }

            return result;
        }
    }
}