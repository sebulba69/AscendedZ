using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    public class Controls
    {
        public static readonly string UP = "up";
        public static readonly string DOWN = "down";
        public static readonly string LEFT = "left";
        public static readonly string RIGHT = "right";

        public static readonly string CONFIRM = "menuConfirm";
        public static readonly string BACK = "menuBack";

        private static Dictionary<string, string> _controlMap = new Dictionary<string, string>
        {
            { CONFIRM, "Z/Enter" },
            { BACK, "X" }
        };

        public static string GetControlString(string control)
        {
            return _controlMap[control];
        }
    }
}
