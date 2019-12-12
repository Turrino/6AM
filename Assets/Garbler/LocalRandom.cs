using System;
using System.Collections.Generic;
using System.Text;

namespace BayeuxBundle
{
    //TODO must be bade compatible with unity's random
    public static class LocalRandom
    {
        public static Random Rnd;

        public static void ConfigureRandom(Random random)
        {
            Rnd = random;
        }

        public static int Range(int min, int max)
        {
            return Rnd.Next(min, max);
        }
    }
}
