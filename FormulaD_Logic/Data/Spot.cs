using System.Collections.Generic;
using System.Linq;

namespace FormulaD_Logic.Data {
    public class Spot {
        public int SpotNumber { get; set; }
        public int Lap { get; set; }
        public int Gear { get; set; }
        public int TurnCount { get; set; }
    }

    public class SpotRef {
        private Dictionary<int, Spot> _spotBySpotNumber;

        public SpotRef(IEnumerable<Spot> dice) {
            _spotBySpotNumber = dice.ToDictionary(x => x.SpotNumber, x => x);
        }

        public Spot GetBySpotNumber(int num) {
            return _spotBySpotNumber[num];
        }
    }
}
