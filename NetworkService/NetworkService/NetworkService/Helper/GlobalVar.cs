using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NetworkService.Helper
{
    public static class GlobalVar
    {
        public static bool IsSaved = false;
        public static bool IsCanvasLoaded = false;
        public static bool IsHelpOpen = false;
        public static List<ToolTip> toolTips = new List<ToolTip>();
        public static List<Button> buttons = new List<Button>();
    }
}
