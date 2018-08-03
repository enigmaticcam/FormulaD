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
        private Result[][][][][][][] _results;

        public void Initialize(int maxLaps, int maxSpots, int maxWpTire, int maxWpBreaks, int maxWpGear, int maxWpEngines, int maxGear) {
            _results = new Result[maxLaps][][][][][][];
            for (int lap = 0; lap < maxLaps; lap++) {
                _results[lap] = new Result[maxSpots][][][][][];
                for (int spot = 0; spot < maxSpots; spot++) {
                    _results[lap][spot] = new Result[maxWpTire][][][][];
                    for (int wpTire = 0; wpTire < maxWpTire; wpTire++) {
                        _results[lap][spot][wpTire] = new Result[maxWpBreaks][][][];
                        for (int wpBreak = 0; wpBreak < maxWpBreaks; wpBreak++) {
                            _results[lap][spot][wpTire][wpBreak] = new Result[maxWpGear][][];
                            for (int wpGear = 0; wpGear < maxWpGear; wpGear++) {
                                _results[lap][spot][wpTire][wpBreak][wpGear] = new Result[maxWpEngines][];
                                for (int wpEngine = 0; wpEngine < maxWpEngines; wpEngine++) {
                                    _results[lap][spot][wpTire][wpBreak][wpGear][wpEngine] = new Result[maxGear];
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

