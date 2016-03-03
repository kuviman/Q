using System.Reflection;
using System.Runtime.CompilerServices;

namespace QE {

    public static class Resource {

        static System.IO.Stream Stream(Assembly assembly, string name) {
            return assembly.GetManifestResourceStream(assembly.GetName().Name + "." + name.Replace('/', '.'));
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static System.IO.Stream Stream(string name) {
            return Stream(Assembly.GetCallingAssembly(), name);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string String(string name) {
            var sr = new System.IO.StreamReader(Stream(Assembly.GetCallingAssembly(), name));
            return sr.ReadToEnd();
        }

    }

}