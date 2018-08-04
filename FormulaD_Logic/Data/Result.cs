using System.Collections.Generic;

namespace FormulaD_Logic.Data {
    public class Result {
        public Spot CurrentSpot { get; set; }
        public double ExpectedTurnsToWin { get; set; }
        public double ExpectedTireReduction { get; set; }
        public enumGearChange SuggestedGearChange { get; set; }

        public enum enumGearChange {
            ShiftUp,
            ShiftDown,
            Stay
        }
    }

    public class ResultRef {
        private Result[,,,,,,,] _results;

        public void Initialize(int maxLaps, int maxSpots, int maxTurnCounts, int maxWpTire, int maxWpBreaks, int maxWpGear, int maxWpEngines, int maxGear) {
            _results = new Result[maxLaps, maxSpots, maxTurnCounts, maxWpTire, maxWpBreaks, maxWpGear, maxWpEngines, maxGear];
        }

        public Result this[int lap, int spot, int turnCount, int wpTire, int wpBreak, int wpGear, int wpEngine, int gear] {
            get { return _results[lap - 1, spot - 1, turnCount, wpTire, wpBreak, wpGear, wpEngine, gear - 1]; }
            set { _results[lap - 1, spot - 1, turnCount, wpTire, wpBreak, wpGear, wpEngine, gear - 1] = value; }
        }
    }
}

