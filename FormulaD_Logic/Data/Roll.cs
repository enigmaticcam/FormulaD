using System.Collections.Generic;

namespace FormulaD_Logic.Data {
    public class Roll {
        public bool DoesCrossFinish { get; set; }
        public GridChain.enumDirectionType Direction { get; set; }
        public int EndSpot { get; set; }
        public uint OvershootCount { get; set; }
        public int OvershootTurnCount { get; set; }
        public int StartSpot { get; set; }
    }

    public class RollRef {
        private Dictionary<int, Dictionary<int, List<Roll>>> _rollBySpotByDie = new Dictionary<int, Dictionary<int, List<Roll>>>();
        private List<Roll> _rolls = new List<Roll>();

        public void AddRoll(int startSpot, int dieNum, int endSpot, int overshootTurnCount, uint overshootCount, GridChain.enumDirectionType direction) {
            if (!_rollBySpotByDie.ContainsKey(startSpot)) {
                _rollBySpotByDie.Add(startSpot, new Dictionary<int, List<Roll>>());
            }
            if (!_rollBySpotByDie[startSpot].ContainsKey(dieNum)) {
                _rollBySpotByDie[startSpot].Add(dieNum, new List<Roll>());
            }
            Roll roll = new Roll() {
                Direction = direction,
                EndSpot = endSpot,
                OvershootCount = overshootCount,
                OvershootTurnCount = overshootTurnCount,
                StartSpot = startSpot
            };
            _rollBySpotByDie[startSpot][dieNum].Add(roll);
            _rolls.Add(roll);
        }

        public int Count {
            get { return _rolls.Count; }
        }

        public IEnumerable<Roll> Enumerate {
            get { return _rolls; }
        }

        public IEnumerable<Roll> RollsBySpotByDie(int startSpot, int die) {
            return _rollBySpotByDie[startSpot][die];
        }
    }
}
