using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarwinTool
{
    public static class Extension
    {
        public static string ToString2(this TimeSpan obj)
        {
            return string.Format("{0}:{1}:{2}", obj.Hours, obj.Minutes, obj.Seconds);
        }
    }
}
