using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace QE {

    public static class Log {

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Info(string message) {
            var method = new StackFrame(1, true).GetMethod();
            Console.WriteLine(" INFO " + method.DeclaringType.Name + "." + method.Name + ": " + message);
        }

    }

}