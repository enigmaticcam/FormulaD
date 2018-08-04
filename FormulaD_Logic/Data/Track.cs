using System.Linq;

namespace FormulaD_Logic.Data {
    public abstract class Track {
        public abstract int MaxLaps { get; }

        public int MaxSpots {
            get { return Grid.Enumerate.Max(x => x.SpotNumber); }
        }

        public int MaxTurnCount {
            get { return Grid.MaxTurnCount; }
        }

        private GridChainRef _chain = new GridChainRef();
        public GridChainRef Chain {
            get { return _chain; }
        }

        private GridRef _grid = new GridRef();
        public GridRef Grid {
            get { return _grid; }
        }

        private DiceRef _dice = new DiceRef();
        public DiceRef Dice {
            get { return _dice; }
        }

        public void GenerateChain(int rowStart, int rowEnd, int nextStart, GridChain.enumDirectionType direction) {
            for (int index = 0; index <= rowEnd - rowStart; index++) {
                int fromSpot = rowStart + index;
                int toSpot = nextStart + index;
                Chain.AddGridChain(new GridChain() {
                    Direction = direction,
                    StopNumberFrom = fromSpot,
                    StopNumberTo = toSpot
                });
            }
        }

        public void GenerateGrid(int rowStart, int rowEnd, bool isTurn, int turnCount) {
            for (int index = rowStart; index <= rowEnd; index++) {
                Grid.AddGrid(new Grid() {
                    IsFinish = false,
                    IsStart = false,
                    IsTurn = isTurn,
                    SpotNumber = index,
                    TurnCount = turnCount
                });
            }
        }

        public void ConvertTurnsToStraightDirection() {
            foreach (var chain in Chain.Enumerate) {
                if (Grid.GetBySpotNumber(chain.StopNumberFrom).IsTurn) {
                    chain.Direction = GridChain.enumDirectionType.Straight;
                }
            }
        }

        public void GenerateStandardDice() {
            Dice.AddDie(new Die() {
                DieMax = 2,
                DieMin = 1,
                DieNum = 1
            });
            Dice.AddDie(new Die() {
                DieMax = 4,
                DieMin = 2,
                DieNum = 2
            });
            Dice.AddDie(new Die() {
                DieMax = 8,
                DieMin = 4,
                DieNum = 3
            });
            Dice.AddDie(new Die() {
                DieMax = 12,
                DieMin = 7,
                DieNum = 4
            });
            Dice.AddDie(new Die() {
                DieMax = 20,
                DieMin = 11,
                DieNum = 5
            });
            Dice.AddDie(new Die() {
                DieMax = 30,
                DieMin = 21,
                DieNum = 6
            });
        }
    }
}
