using System;
using System.Collections.Generic;

namespace QE {

    /// <summary>
    /// Provides extensions for Random class
    /// </summary>
    public static class ExtRandom {
        
        public static bool Probable(this Random rnd, double p) {
            return rnd.NextDouble() < p;
        }

        public static T Choice<T>(this Random rnd, IList<T> a) {
            return a[rnd.Next(a.Count)];
        }
        
        public static bool Coin(this Random rnd) {
            return rnd.Next(2) == 0;
        }
        
        public static double NextDouble(this Random rnd, double minValue, double maxValue) {
            return minValue + rnd.NextDouble() * (maxValue - minValue);
        }

    }

}