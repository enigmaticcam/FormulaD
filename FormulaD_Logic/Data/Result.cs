using System.Collections.Generic;

namespace FormulaD_Logic.Data {
    public class Result {
        public Spot CurrentSpot { get; set; }
        public double ExpectedTurnsToWin { get; set; }
        public double ExpectedWpTire { get; set; }
        public double ExpectedWpBrake { get; set; }
        public double ExpectedWpEngine { get; set; }
        public double ExpectedWpGear { get; set; }
        public enumShiftDirection SuggestedGearChange { get; set; }

        public enum enumShiftDirection {
            Down4 = -4,
            Down3 = -3,
            Down2 = -2,
            Down = -1,
            Stay = 0,
            Up = 1
        }
    }

    public class ResultRef {
        private Result[,,,,,,,] _results;

        public void Initialize(int maxLaps, int maxSpots, int maxTurnCounts, int maxWpTire, int maxWpBreaks, int maxWpGear, int maxWpEngines, int maxGear) {
            _results = new Result[maxLaps + 1, maxSpots + 1, maxTurnCounts + 1, maxWpTire, maxWpBreaks, maxWpGear, maxWpEngines, maxGear];
        }

        public Result this[int lap, int spot, int turnCount, int wpTire, int wpBreak, int wpGear, int wpEngine, int gear] {
            get { return _results[lap, spot - 1, turnCount, wpTire, wpBreak, wpGear, wpEngine, gear - 1]; }
            set { _results[lap, spot - 1, turnCount, wpTire, wpBreak, wpGear, wpEngine, gear - 1] = value; }
        }
    }
}

