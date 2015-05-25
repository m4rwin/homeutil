using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MUtility
{
    public enum Argument
    {
        Add,
        Sub,
        Mul,
        Div
    }

    public class EasyParsing
    {
        public Argument ParseCommand(string command)
        {
            return (Argument)(Enum.Parse(typeof(Argument), command));
        }
    }
}
