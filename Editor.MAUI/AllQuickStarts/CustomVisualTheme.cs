using Alternet.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllQuickStarts;

public class CustomVisualTheme : StandardVisualTheme
{
    public CustomVisualTheme()
        : base("MyCustomTheme")
    {
    }

    protected override VisualThemeColors GetColors()
    {
        var colors = DarkVisualTheme.Instance.Colors.Clone();
        colors.Reswords = System.Drawing.Color.Red;
        colors.WindowBackground = System.Drawing.Color.FromArgb(40, 40, 40);
        return colors;
    }
}
