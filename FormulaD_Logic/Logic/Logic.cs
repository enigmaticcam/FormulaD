using FormulaD_Logic.Data;
using FormulaD_Logic.Data.Tracks;
using FormulaD_Logic.Logic.Actions;

namespace FormulaD_Logic.Logic {
    public class BuildResults {
        private Track _track = new Monaco();
        private ResultRef _results = new ResultRef();
        private RollRef _rolls;
        private int _laps;
        private StartProperties _start = new StartProperties();
        private CostProperties _cost = new CostProperties();
        private EndProperties _end = new EndProperties();
        private uint _bestScore;
        private uint _score;

        public enum enumShiftDirection {
            Down4 = -4,
            Down3 = -3,
            Down2 = -2,
            Down = -1,
            Stay = 0,
            Up = 1
        }

        public Result Perform(int laps) {
            CalculateAllMoves();
            _start.SpotNumber = 17;
            FindAllStartingPossibilities();
            return null;
        }

        private void CalculateAllMoves() {
            ActionGetChildren action = new ActionGetChildren(_track.Chain, _track.Dice, _track.Grid);
            _rolls = action.Perform();
        }

        private void FindAllStartingPossibilities() {
            _start.Spot = _track.Grid.GetBySpotNumber(_start.SpotNumber);
            for (_start.WpTire = 0; _start.WpTire <= 14; _start.WpTire++) {
                for (_start.WpBreaks = 0; _start.WpBreaks <= 7; _start.WpBreaks++) {
                    for (_start.WpGear = 0; _start.WpGear <= 7; _start.WpGear++) {
                        for (_start.WpEngine = 0; _start.WpEngine <= 7; _start.WpEngine++) {
                            for (_start.TurnCount = 0; _start.TurnCount <= _start.Spot.TurnCount; _start.TurnCount++) {
                                foreach (var die in _track.Dice.Enumerate) {
                                    _start.Die = die;
                                    ChooseBestShiftAction();
                                }
                            }
                            
                        }
                    }
                }
            }
        }

        private void ChooseBestShiftAction() {
            CalcExpectedValues(enumShiftDirection.Stay);
            if (_start.Die.DieNum < _track.Dice.HighestDie.DieNum) {
                CalcExpectedValues(enumShiftDirection.Up);
            }
            if (_start.Die.DieNum > 1) {
                CalcExpectedValues(enumShiftDirection.Down);
            }
            if (_start.Die.DieNum > 2) {
                CalcExpectedValues(enumShiftDirection.Down);
            }
            if (_start.Die.DieNum > 3 && _start.WpGear >= 1) {
                CalcExpectedValues(enumShiftDirection.Down2);
            }
            if (_start.Die.DieNum > 4 && _start.WpGear >= 1 && _start.WpBreaks >= 1) {
                CalcExpectedValues(enumShiftDirection.Down3);
            }
            if (_start.Die.DieNum > 5 && _start.WpGear >= 1 && _start.WpBreaks >= 1 && _start.WpEngine >= 1) {
                CalcExpectedValues(enumShiftDirection.Down4);
            }
        }

        private void CalcExpectedValues(enumShiftDirection shift) {
            Die newDie = _track.Dice.GetByNum(_start.Die.DieNum + (int)shift);
            for (_start.MoveCount = newDie.DieMin; _start.MoveCount <= newDie.DieMax; _start.MoveCount++) {
                PickBestMove(shift);
            }
        }

        private void PickBestMove(enumShiftDirection shift) {
            _bestScore = uint.MaxValue;
            foreach (var roll in _rolls.RollsBySpotByDie(_start.SpotNumber, _start.MoveCount)) {
                _start.Roll = roll; // may not need this, if we don't call any other functions

                // Order rolls so as to pick the best one.
                // All actions that bring a cost below zero come last.
                _cost.AddedBelowZeroCostAlready = false;
                _score = 0;
                CalcTireReduction();
                if (_score < _bestScore) {

                }
            }
        }

        private void CalcTireReduction() {
            if (_start.Spot.IsTurn && _start.TurnCount < _start.Spot.TurnCount && _start.Roll.OvershootTurnCount > 0) {
                _cost.WpTire = _start.Roll.OvershootCount;
                if (_cost.WpTire > _start.WpTire && !_cost.AddedBelowZeroCostAlready) {
                    _score += 2147483648; //2^32
                    _cost.AddedBelowZeroCostAlready = true;
                }
                _score += _cost.WpTire;
            }
        }

        private class StartProperties {
            public int Lap { get; set; }
            public int MoveCount { get; set; }
            public int SpotNumber { get; set; }
            public int TurnCount { get; set; }
            public uint WpTire { get; set; }
            public int WpBreaks { get; set; }
            public int WpGear { get; set; }
            public int WpEngine { get; set; }
            public Die Die { get; set; }
            public Grid Spot { get; set; }
            public Roll Roll { get; set; }
        }

        private class CostProperties {
            public bool AddedBelowZeroCostAlready { get; set; }
            public uint WpTire { get; set; }
        }

        private class EndProperties {
            public uint WpTire { get; set; }
        }
    }
}
