using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbbCom
{
    public class RapidDataChangeEventArgs : EventArgs
    {
        public string ControllerName
        {
            get;
            set;
        }

        public string VariableName
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public string Index
        {
            get;
            set;
        }

        public RapidDataChangeEventArgs(string controller, string variable, string value, string index)
        {
            ControllerName = controller;
            VariableName = variable;
            Value = value;
            Index = index;
        }
    }
}
