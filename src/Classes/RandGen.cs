using System;

namespace DescendBelow {
    public static class RandGen {
        private static Random random = new Random();

        public static double RandomDoubleBetween(double min, double max) {
            return random.NextDouble() * (max - min) + min;
        }

        public static int RandomIntBetween(int min, int max) {
            return random.Next(min, max);
        }
    }
}