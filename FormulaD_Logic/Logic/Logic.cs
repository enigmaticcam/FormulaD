using FormulaD_Logic.Data;
using FormulaD_Logic.Data.Tracks;
using FormulaD_Logic.Logic.Actions;

namespace FormulaD_Logic.Logic {
    public class BuildResults {
        private Track _track = new Monaco();
        private ResultRef _results = new ResultRef();
        private RollRef _rolls;
        private int _spotNumber;
        private int _laps;
        private int _wpTire;
        private int _wpBreaks;
        private int _wpGear;
        private int _wpEngine;
        private int _turnCount;
        private Die _die;
        private int _moveCount;
        private Roll _roll;

        public Result Perform(int laps) {
            _laps = laps;
            CalculateAllMoves();
            _spotNumber = 17;
            FindAllStartingPossibilities();
            return null;
        }

        private void CalculateAllMoves() {
            ActionGetChildren action = new ActionGetChildren(_track.Chain, _track.Dice, _track.Grid);
            _rolls = action.Perform();
        }

        private void FindAllStartingPossibilities() {
            var spot = _track.Grid.GetBySpotNumber(_spotNumber);
            for (_wpTire = 0; _wpTire <= 14; _wpTire++) {
                for (_wpBreaks = 0; _wpBreaks <= 7; _wpBreaks++) {
                    for (_wpGear = 0; _wpGear <= 7; _wpGear++) {
                        for (_wpEngine = 0; _wpEngine <= 7; _wpEngine++) {
                            for (_turnCount = 0; _turnCount <= spot.TurnCount; _turnCount++) {
                                foreach (var die in _track.Dice.Enumerate) {
                                    _die = die;
                                    CalcExpectedValues();
                                }
                            }
                            
                        }
                    }
                }
            }
        }

        private void CalcExpectedValues() {
            for (_moveCount = _die.DieMin; _moveCount <= _die.DieMax; _moveCount++) {
                foreach (var roll in _rolls.RollsBySpotByDie(spotNumber, _moveCount)) {
                    _roll = roll;
                    
                }
            }
        }
    }
}
