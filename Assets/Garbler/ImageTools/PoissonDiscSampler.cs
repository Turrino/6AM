//using BayeuxBundle.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace BayeuxBundle.ImageTools
//{
//    // Adapted from http://gregschlom.com/devlog/2014/06/29/Poisson-disc-sampling-Unity.html
//    public class PoissonDiscSampler
//    {
//        private const int k = 30;  // Maximum number of attempts before marking a sample as inactive.

//        private readonly int RectWidth;
//        private readonly int RectHeight;
//        private readonly float radius2;  // radius squared
//        private readonly float cellSize;
//        private ImgPoint[,] grid;
//        private List<ImgPoint> activeSamples = new List<ImgPoint>();
//        private Random Random = LocalRandom.Rnd;

//        /// Create a sampler with the following parameters:
//        ///
//        /// width:  each sample's x coordinate will be between [0, width]
//        /// height: each sample's y coordinate will be between [0, height]
//        /// radius: each sample will be at least `radius` units away from any other sample, and at most 2 * `radius`.
//        public PoissonDiscSampler(int width, int height, float radius)
//        {
//            RectWidth = width;
//            RectHeight = height;
//            radius2 = radius * radius;
//            cellSize = (float)(radius / Math.Sqrt(2));
//            grid = new ImgPoint[(int)Math.Ceiling(width / cellSize),
//                               (int)Math.Ceiling(height / cellSize)];
//        }

//        /// Return a lazy sequence of samples. You typically want to call this in a foreach loop, like so:
//        ///   foreach (Point sample in sampler.Samples()) { ... }
//        public IEnumerable<ImgPoint> Samples()
//        {
//            // First sample is choosen randomly
//            yield return AddSample(new ImgPoint((int)(Random.NextDouble() * RectWidth), (int)(Random.NextDouble() * RectHeight)));

//            while (activeSamples.Count > 0)
//            {

//                // Pick a random active sample
//                int i = (int)Random.NextDouble() * activeSamples.Count;
//                ImgPoint sample = activeSamples[i];

//                // Try `k` random candidates between [radius, 2 * radius] from that sample.
//                bool found = false;
//                for (int j = 0; j < k; ++j)
//                {

//                    var angle = (float)(2 * Math.PI * Random.NextDouble());
//                    var r = (float)Math.Sqrt(Random.NextDouble() * 3 * radius2 + radius2); // See: http://stackoverflow.com/questions/9048095/create-random-number-within-an-annulus/9048443#9048443
//                    var candidate = new ImgPoint(sample.X + (int)(Math.Cos(angle) * r), sample.Y + (int)(Math.Sin(angle) * r));

//                    // Accept candidates if it's inside the rect and farther than 2 * radius to any existing sample.
//                    if (rect.Contains(candidate) && IsFarEnough(candidate))
//                    {
//                        found = true;
//                        yield return AddSample(candidate);
//                        break;
//                    }
//                }

//                // If we couldn't find a valid candidate after k attempts, remove this sample from the active samples queue
//                if (!found)
//                {
//                    activeSamples[i] = activeSamples[activeSamples.Count - 1];
//                    activeSamples.RemoveAt(activeSamples.Count - 1);
//                }
//            }
//        }

//        private bool IsFarEnough(ImgPoint sample)
//        {
//            GridPos pos = new GridPos(sample, cellSize);

//            int xmin = Math.Max(pos.x - 2, 0);
//            int ymin = Math.Max(pos.y - 2, 0);
//            int xmax = Math.Min(pos.x + 2, grid.GetLength(0) - 1);
//            int ymax = Math.Min(pos.y + 2, grid.GetLength(1) - 1);

//            for (int y = ymin; y <= ymax; y++)
//            {
//                for (int x = xmin; x <= xmax; x++)
//                {
//                    ImgPoint s = grid[x, y];
//                    if (s.X != 0 && s.Y != 0)
//                    {
//                        var d = new ImgPoint(s.X - sample.X, s.Y - sample.Y);
//                        if (d.X * d.X + d.Y * d.Y < radius2) return false;
//                    }
//                }
//            }

//            return true;

//            // Note: we use the zero vector to denote an unfilled cell in the grid. This means that if we were
//            // to randomly pick (0, 0) as a sample, it would be ignored for the purposes of proximity-testing
//            // and we might end up with another sample too close from (0, 0). This is a very minor issue.
//        }

//        /// Adds the sample to the active samples queue and the grid before returning it
//        private ImgPoint AddSample(ImgPoint sample)
//        {
//            activeSamples.Add(sample);
//            GridPos pos = new GridPos(sample, cellSize);
//            grid[pos.x, pos.y] = sample;
//            return sample;
//        }

//        /// Helper struct to calculate the x and y indices of a sample in the grid
//        private struct GridPos
//        {
//            public int x;
//            public int y;

//            public GridPos(ImgPoint sample, float cellSize)
//            {
//                x = (int)(sample.X / cellSize);
//                y = (int)(sample.Y / cellSize);
//            }
//        }
//    }
//}
