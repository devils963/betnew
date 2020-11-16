using System;

namespace ConsoleApp1.Constants
{
    class SpiProbabilityIntervall
    {
        public static ProbabilityIntervall[] spiProbabilityIntervall =
        {
            new ProbabilityIntervall(0.35, 0.4),
            new ProbabilityIntervall(0.4, 0.45),
            new ProbabilityIntervall(0.45, 0.5),
            new ProbabilityIntervall(0.5, 0.55),
            new ProbabilityIntervall(0.55, 0.6),
            new ProbabilityIntervall(0.6, 0.65),
            new ProbabilityIntervall(0.65, 0.7),
            new ProbabilityIntervall(0.7, 0.75),
            new ProbabilityIntervall(0.75, 0.8),
            new ProbabilityIntervall(0.8, 0.85),
            new ProbabilityIntervall(0.85, 0.9),
            new ProbabilityIntervall(0.9, 0.95),
            new ProbabilityIntervall(0.95, 1)
        };

        public static ProbabilityIntervall GetIntervall(double probability)
        {
            foreach (var intervall in spiProbabilityIntervall)
            {
                if (probability > intervall.minimumProbabilityBorder && probability < intervall.maximumProbabilityBorder)
                {
                    return intervall;
                }
            }
            throw new Exception("no intervall found");
        }
    }

    public class ProbabilityIntervall
    {
        public double minimumProbabilityBorder;
        public double maximumProbabilityBorder;

        public ProbabilityIntervall(double _minBorder, double _maxBorder)
        {
            minimumProbabilityBorder = _minBorder;
            maximumProbabilityBorder = _maxBorder;
        }
    }
}
