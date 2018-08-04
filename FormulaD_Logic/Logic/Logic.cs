using FormulaD_Logic.Data;
using FormulaD_Logic.Data.Tracks;
using FormulaD_Logic.Logic.Actions;

namespace FormulaD_Logic.Logic {
    public class BuildResults {
        private Track _track;
        private ResultRef _results;
        private RollRef _rolls;
        private StartProperties _start = new StartProperties();
        private Score _bestScore = new Score();
        private Score _currentScore = new Score();
        private AllScores _currentAllScores = new AllScores();
        private AllScores _bestAllScores = new AllScores();

        public BuildResults(RollRef rolls, ResultRef results, Track track) {
            _rolls = rolls;
            _results = results;
            _track = track;
        }

        public void Perform(int spotNumber) {
            _start.SpotNumber = spotNumber;
            FindAllStartingPossibilities();
        }

        private void FindAllStartingPossibilities() {
            _start.Spot = _track.Grid.GetBySpotNumber(_start.SpotNumber);
            for (_start.Lap = 0; _start.Lap <= _track.MaxLaps; _start.Lap++) {
                if (_start.Lap == 2) { // --------------Testing----------------
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
                
            }
        }

        private void ChooseBestShiftAction() {
            _bestAllScores.ExpectedTurnsToWin = double.MaxValue;
            _bestAllScores.WpTire = double.MaxValue;
            CalcExpectedValues(Result.enumShiftDirection.Stay);
            if (_start.Die.DieNum < _track.Dice.HighestDie.DieNum) {
                CalcExpectedValues(Result.enumShiftDirection.Up);
            }
            if (_start.Die.DieNum > 1) {
                CalcExpectedValues(Result.enumShiftDirection.Down);
            }
            if (_start.Die.DieNum > 2 && _start.WpGear >= 1) {
                CalcExpectedValues(Result.enumShiftDirection.Down2);
            }
            if (_start.Die.DieNum > 3 && _start.WpGear >= 1 && _start.WpBreaks >= 1) {
                CalcExpectedValues(Result.enumShiftDirection.Down3);
            }
            if (_start.Die.DieNum > 4 && _start.WpGear >= 1 && _start.WpBreaks >= 1 && _start.WpEngine >= 1) {
                CalcExpectedValues(Result.enumShiftDirection.Down4);
            }
            SaveBestAllScores();
        }

        private void CalcExpectedValues(Result.enumShiftDirection shift) {
            Die newDie = _track.Dice.GetByNum(_start.Die.DieNum + (int)shift);
            _currentAllScores.ExpectedTurnsToWin = 0;
            _currentAllScores.WpTire = 0;
            for (_start.MoveCount = newDie.DieMin; _start.MoveCount <= newDie.DieMax; _start.MoveCount++) {
                PickBestMove();
                _currentAllScores.ExpectedTurnsToWin += _bestScore.ExpectedTurnsToWin;
                _currentAllScores.WpTire += _bestScore.WpTire;
            }
            int count = newDie.DieMax - newDie.DieMin + 1;
            _currentAllScores.ExpectedTurnsToWin /= count;
            _currentAllScores.WpTire /= count;
            SetBestAllScores(shift);
        }

        private void SetBestAllScores(Result.enumShiftDirection shift) {
            if (_currentAllScores.ExpectedTurnsToWin < _bestAllScores.ExpectedTurnsToWin) {
                _bestAllScores.ExpectedTurnsToWin = _currentAllScores.ExpectedTurnsToWin;
                _bestAllScores.WpTire = _currentAllScores.WpTire;
                _bestAllScores.Shift = shift;
            }
        }

        private void SaveBestAllScores() {
            var result = new Result() {
                CurrentSpot = null,
                ExpectedTireReduction = _bestAllScores.WpTire,
                ExpectedTurnsToWin = _bestAllScores.ExpectedTurnsToWin,
                SuggestedGearChange = _bestAllScores.Shift
            };
            _results[_start.Lap, _start.SpotNumber, _start.TurnCount, _start.WpTire, _start.WpBreaks, _start.WpGear, _start.WpEngine, _start.Die.DieNum] = result;
        }

        private void PickBestMove() {
            _bestScore.AnyCost = uint.MaxValue;
            _bestScore.BringsCostBelowZero = 1;
            _bestScore.ExpectedTurnsToWin = double.MaxValue;
            _bestScore.WpTire = 0;
            foreach (var roll in _rolls.RollsBySpotByDie(_start.SpotNumber, _start.MoveCount)) {
                _start.Roll = roll;
                _currentScore.AnyCost = 0;
                CalcTireReduction();
                if (_currentScore.BringsCostBelowZero <= _bestScore.BringsCostBelowZero) {
                    CalcExpectedTurns();
                    if (_currentScore.ExpectedTurnsToWin <= _bestScore.ExpectedTurnsToWin) {
                        if (_currentScore.AnyCost < _bestScore.AnyCost) {
                            SetBestScore();
                        }
                    }
                }
            }
        }

        private void CalcTireReduction() {
            if (_start.Spot.IsTurn && _start.TurnCount < _start.Spot.TurnCount && _start.Roll.OvershootTurnCount > 0) {
                uint wpTire = _start.Roll.OvershootCount;
                if (wpTire > _start.WpTire) {
                    _currentScore.BringsCostBelowZero = 1;
                } else {
                    _currentScore.BringsCostBelowZero = 0;
                }
                _currentScore.AnyCost += wpTire;
                _currentScore.WpTire = wpTire;
            }
        }

        private void CalcExpectedTurns() {
            if (_start.Lap == _track.MaxLaps && _start.Roll.DoesCrossFinish) {
                _currentScore.ExpectedTurnsToWin = 1;
            } else {
                var result = _results[_start.Lap, _start.Roll.EndSpot, _start.TurnCount, _start.WpTire, _start.WpBreaks, _start.WpGear, _start.WpEngine, _start.Die.DieNum];
                if (result == null) {
                    BuildResults buildResults = new BuildResults(_rolls, _results, _track);
                    buildResults.Perform(_start.Roll.EndSpot);
                    result = _results[_start.Lap, _start.Roll.EndSpot, _start.TurnCount, _start.WpTire, _start.WpBreaks, _start.WpGear, _start.WpEngine, _start.Die.DieNum];
                }
                _currentScore.ExpectedTurnsToWin = 1 + result.ExpectedTurnsToWin;
            }
        }

        private void SetBestScore() {
            _bestScore.AnyCost = _currentScore.AnyCost;
            _bestScore.BringsCostBelowZero = _currentScore.BringsCostBelowZero;
            _bestScore.ExpectedTurnsToWin = _currentScore.ExpectedTurnsToWin;
            _bestScore.CurrentRoll = _start.Roll;
            _bestScore.WpTire = _currentScore.WpTire;
        }

        public class AllScores {
            public double ExpectedTurnsToWin { get; set; }
            public double WpTire { get; set; }
            public Result.enumShiftDirection Shift { get; set; }
        }

        private class StartProperties {
            public int Lap { get; set; }
            public int MoveCount { get; set; }
            public int SpotNumber { get; set; }
            public int TurnCount { get; set; }
            public int WpTire { get; set; }
            public int WpBreaks { get; set; }
            public int WpGear { get; set; }
            public int WpEngine { get; set; }
            public Die Die { get; set; }
            public Grid Spot { get; set; }
            public Roll Roll { get; set; }
        }

        private class Score {
            public int BringsCostBelowZero { get; set; }
            public double ExpectedTurnsToWin { get; set; }
            public uint AnyCost { get; set; }
            public Roll CurrentRoll { get; set; }
            public uint WpTire { get; set; }
        }
    }
}
