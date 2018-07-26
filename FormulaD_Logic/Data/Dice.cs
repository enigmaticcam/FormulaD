using System.Collections.Generic;

namespace FormulaD_Logic.Data {
    public class Die {
        public int DieNum { get; set; }
        public int DieMin { get; set; }
        public int DieMax { get; set; }
    }

    public class DiceRef {
        private Dictionary<int, Die> _diceByNum = new Dictionary<int, Die>();
        private List<Die> _dice = new List<Die>();

        private Die _highestDie;
        public Die HighestDie {
            get { return _highestDie; }
        }

        private Die _lowestDie;
        public Die LowestDie {
            get { return _lowestDie; }
        }

        public IEnumerable<Die> Enumerate {
            get { return _dice; }
        }

        public Die GetByNum(int num) {
            return _diceByNum[num];
        }

        public void AddDie(Die die) {
            _diceByNum.Add(die.DieNum, die);
            if (_highestDie == null) {
                _highestDie = die;
            } else if (die.DieNum > _highestDie.DieNum) {
                _highestDie = die;
            }
            if (_lowestDie == null) {
                _lowestDie = die;
            } else if (die.DieNum < _lowestDie.DieNum) {
                _lowestDie = die;
            }
            _dice.Add(die);
        }
    }
}
