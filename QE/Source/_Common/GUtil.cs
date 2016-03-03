using System;
using System.IO;

namespace QE {

    public static class GUtil {

        public static void Swap<T>(ref T a, ref T b) {
            T c = a;
            a = b;
            b = c;
        }

        public static byte[] ReadToEnd(this Stream stream) {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream()) {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }

        public static string GetString(byte[] bytes) {
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        public static byte[] GetBytes(string s) {
            return System.Text.Encoding.UTF8.GetBytes(s);
        }

        public static string Escape(string s) {
            string quoted = Newtonsoft.Json.JsonConvert.ToString(s);
            return quoted.Substring(1, quoted.Length - 2);
        }

        public static string Escape(char c) {
            string quoted = Newtonsoft.Json.JsonConvert.ToString(c);
            return quoted.Substring(1, quoted.Length - 2);
        }

        public static void StartThread(Action action) {
            var t = new System.Threading.Thread(() => action());
            t.IsBackground = true;
            t.Start();
        }

    }

}