using System;
using System.Collections.Generic;
using QE;
using QE.Engine;
using UI = QE.Engine.UI;

namespace Q {

    class QLauncher {
        static void Main(string[] args) {
            var ops = new CmdOp(args);
            Log.Info(ops.ToString());
            if (ops["server"] != null) {
                new Server().Run();
            } else if (ops["connect"] != null) {
                App.Run(new Client(ops["nick"], ops["connect"]));
            } else {
                GUtil.StartThread(() => new Server().Run());
                App.Run(new Client("kuviman", "127.0.0.1"));
            }
        }
    }

}