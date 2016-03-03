using System;
using System.Linq;
using System.Collections.Generic;

namespace QE {

    [Serializable]
    public class CmdOp {

        Dictionary<string, string> valArgs = new Dictionary<string, string>();
        
        public CmdOp(string[] args) {
            foreach (var arg in args) {
                int sepIndex = arg.IndexOf('=');
                if (sepIndex == -1) {
                    valArgs[arg] = "true";
                } else {
                    valArgs[arg.Substring(0, sepIndex)] = arg.Substring(sepIndex + 1);
                }
            }
        }

        public string this[string arg] {
            get {
                string result = null;
                valArgs.TryGetValue(arg, out result);
                return result;
            }
        }

        public override string ToString() {
            return "{" + string.Join(", ", from entry in valArgs select entry.Key + "=" + entry.Value) + "}";
        }

    }

}