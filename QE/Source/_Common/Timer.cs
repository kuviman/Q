﻿using System;
using System.Collections.Generic;

namespace QE {

    [Serializable]
    public class Timer {

        const int MAX_FRAMES = 20;
        static long MAX_DURATION = 2 * System.Diagnostics.Stopwatch.Frequency;

        Queue<long> frames = new Queue<long>();
        long duration = 0;

        long startTime = -1;
        long previousTick = -1;
        
        public double Tick() {
            long currentTick = System.Diagnostics.Stopwatch.GetTimestamp();
            if (startTime == -1)
                startTime = currentTick;
            var dt = previousTick == -1 ? 0 : currentTick - previousTick;
            frames.Enqueue(dt);
            duration += dt;
            previousTick = currentTick;

            while (duration > MAX_DURATION || frames.Count > MAX_FRAMES)
                duration -= frames.Dequeue();

            return (double)dt / System.Diagnostics.Stopwatch.Frequency;
        }
        
        public double FPS {
            get {
                if (frames.Count == 0)
                    return 0;
                return frames.Count / ((double)duration / System.Diagnostics.Stopwatch.Frequency);
            }
        }

        public double RunningTime {
            get { return (double)(System.Diagnostics.Stopwatch.GetTimestamp() - startTime) / System.Diagnostics.Stopwatch.Frequency; }
        }

    }

}