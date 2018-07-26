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
        private List<Result> _results = new List<Result>();
        private Dictionary<int, List<Result>> _resultsBySpotNumber = new Dictionary<int, List<Result>>();
        
        public void AddResult(Result result) {
            _results.Add(result);
            if (!_resultsBySpotNumber.ContainsKey(result.CurrentSpot.SpotNumber)) {
                _resultsBySpotNumber.Add(result.CurrentSpot.SpotNumber, new List<Result>());
            }
            _resultsBySpotNumber[result.CurrentSpot.SpotNumber].Add(result);
        }

        public IEnumerable<Result> Enumerate {
            get { return _results; }
        }

        public bool ContainsResultBySpotNumber(int spotNumber) {
            return _resultsBySpotNumber.ContainsKey(spotNumber);
        }
    }
}

