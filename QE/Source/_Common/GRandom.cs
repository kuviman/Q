using System;
using System.Collections.Generic;

namespace QE {

    public static class GRandom {

        static Random gen = new Random();
        static object Lock = new object();
        
        public static bool Probable(double p) {
            lock (Lock)
                return gen.Probable(p);
        }

        public static T Choice<T>(IList<T> a) {
            lock (Lock)
                return gen.Choice(a);
        }
        
        public static bool Coin() {
            lock (Lock)
                return gen.Coin();
        }
        
        public static void Seed(int seed) {
            lock (Lock)
                gen = new Random(seed);
        }
        
        public static int Next() {
            lock (Lock)
                return gen.Next();
        }
        
        public static int Next(int maxValue) {
            lock (Lock)
                return gen.Next(maxValue);
        }
        
        public static int Next(int minValue, int maxValue) {
            lock (Lock)
                return gen.Next(minValue, maxValue);
        }
        
        public static double NextDouble() {
            lock (Lock)
                return gen.NextDouble();
        }
        
        public static double NextDouble(double minValue, double maxValue) {
            lock (Lock)
                return gen.NextDouble(minValue, maxValue);
        }
        
        public static void NextBytes(byte[] buffer) {
            lock (Lock)
                gen.NextBytes(buffer);
        }

    }

}