using FormulaD_Logic.Data;
using System;

namespace FormulaD_Logic.Logic.Actions {
    public class ActionBuildResults {
        private Track _track;
        private ResultRef _results;
        private RollRef _rolls;
        private StartProperties _start = new StartProperties();
        private CostProperties _cost = new CostProperties();
        private Score _bestScore = new Score();
        private Score _currentScore = new Score();
        private AllScores _currentAllScores = new AllScores();
        private AllScores _bestAllScores = new AllScores();

        public ActionBuildResults(RollRef rolls, ResultRef results, Track track) {
            _rolls = rolls;
            _results = results;
            _track = track;
        }

        public void Perform(int spotNumber, int lap) {
            _start.SpotNumber = spotNumber;
            _start.Lap = lap;
            FindAllStartingPossibilities();
            _results.AddSpotCompleted(spotNumber, lap);
        }

        private void FindAllStartingPossibilities() {
            _start.Spot = _track.Grid.GetBySpotNumber(_start.SpotNumber);
            for (_start.WpTire = 0; _start.WpTire <= 14; _start.WpTire++) {
                for (_start.WpBreaks = 0; _start.WpBreaks <= 7; _start.WpBreaks++) {
                    for (_start.WpGear = 0; _start.WpGear <= 7; _start.WpGear++) {
                        for (_start.WpEngine = 0; _start.WpEngine <= 7; _start.WpEngine++) {
                            int startTurnCount = 0;
                            if (_start.Spot.IsTurn) {
                                startTurnCount = 1;
                            }
                            for (_start.TurnCount = startTurnCount; _start.TurnCount <= _start.Spot.TurnCount; _start.TurnCount++) {
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
            _bestAllScores.SetToMaxValues();
            _cost.Reset();
            CalcExpectedValues(Result.enumShiftDirection.Stay);
            if (_start.Die.DieNum < _track.Dice.HighestDie.DieNum) {
                CalcExpectedValues(Result.enumShiftDirection.Up);
            }
            if (_start.Die.DieNum > 1) {
                CalcExpectedValues(Result.enumShiftDirection.Down);
            }
            if (_start.Die.DieNum > 2 && _start.WpGear >= 1) {
                _cost.WpGear = 1;
                CalcExpectedValues(Result.enumShiftDirection.Down2);
            }
            if (_start.Die.DieNum > 3 && _start.WpGear >= 1 && _start.WpBreaks >= 1) {
                _cost.WpGear = 1;
                _cost.WpBreaks = 1;
                CalcExpectedValues(Result.enumShiftDirection.Down3);
            }
            if (_start.Die.DieNum > 4 && _start.WpGear >= 1 && _start.WpBreaks >= 1 && _start.WpEngine >= 1) {
                _cost.WpGear = 1;
                _cost.WpBreaks = 1;
                _cost.WpEngine = 1;
                CalcExpectedValues(Result.enumShiftDirection.Down4);
            }
            SaveBestAllScores();
        }

        private void CalcExpectedValues(Result.enumShiftDirection shift) {
            Die newDie = _track.Dice.GetByNum(_start.Die.DieNum + (int)shift);
            _currentAllScores.Reset();
            for (_start.MoveCount = newDie.DieMin; _start.MoveCount <= newDie.DieMax; _start.MoveCount++) {
                PickBestMove();
                _currentAllScores.ExpectedTurnsToWin += _bestScore.ExpectedTurnsToWin;
                _currentAllScores.WpTire += _bestScore.WpTire;
            }
            _currentAllScores.Average(newDie.DieMax - newDie.DieMin + 1);
            SetBestAllScores(shift);
        }

        private void PickBestMove() {
            _bestScore.AnyCost = int.MaxValue;
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
                int wpTire = (int)_start.Roll.OvershootCount;
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
            if (_currentScore.BringsCostBelowZero == 1) {
                _currentScore.ExpectedTurnsToWin = int.MaxValue;
            } else if (_start.Lap == _track.MaxLaps && _start.Roll.DoesCrossFinish) {
                _currentScore.ExpectedTurnsToWin = 0;
            } else {
                int lap = (_start.Roll.DoesCrossFinish ? _start.Lap + 1 : _start.Lap);
                if (!_results.IsSpotCompleted(_start.Roll.EndSpot, lap)) {
                    ActionBuildResults action = new ActionBuildResults(_rolls, _results, _track);
                    action.Perform(_start.Roll.EndSpot, lap);
                }
                var result = GetResultAfterCost();
                _currentScore.ExpectedTurnsToWin = 1 + result.ExpectedTurnsToWin;
            }
        }

        private void SetBestAllScores(Result.enumShiftDirection shift) {
            if (_currentAllScores.ExpectedTurnsToWin < _bestAllScores.ExpectedTurnsToWin) {
                _bestAllScores.CopyFrom(_currentAllScores);
                _bestAllScores.Shift = shift;
            }
        }

        private void SaveBestAllScores() {
            var result = _bestAllScores.ToResult();
            _results[_start.Lap, _start.SpotNumber, _start.TurnCount, _start.WpTire, _start.WpBreaks, _start.WpGear, _start.WpEngine, _start.Die.DieNum] = result;
        }

        private Result GetResultAfterCost() {
            return _results[
                _start.Lap,
                _start.Roll.EndSpot,
                GetNextTurnCount(),
                _start.WpTire - _currentScore.WpTire,
                _start.WpBreaks - _cost.WpBreaks,
                _start.WpGear - _cost.WpGear,
                _start.WpEngine - _cost.WpEngine,
                _start.Die.DieNum];
        }

        private int GetNextTurnCount() {
            if (_start.Spot.IsTurn) {
                return Math.Min(_start.Spot.TurnCount, _start.TurnCount + 1);
            } else {
                return 0;
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
            public double WpBreaks { get; set; }
            public double WpGear { get; set; }
            public double WpEngine { get; set; }
            public Result.enumShiftDirection Shift { get; set; }

            public void Reset() {
                ExpectedTurnsToWin = 0;
                WpTire = 0;
                WpBreaks = 0;
                WpEngine = 0;
                WpGear = 0;
            }
            
            public void SetToMaxValues() {
                ExpectedTurnsToWin = double.MaxValue;
                WpTire = double.MaxValue;
                WpBreaks = double.MaxValue;
                WpEngine = double.MaxValue;
                WpGear = double.MaxValue;
            }

            public void Average(int count) {
                ExpectedTurnsToWin /= count;
                WpTire /= count;
                WpBreaks /= count;
                WpEngine /= count;
                WpGear /= count;
            }
            
            public void CopyFrom(AllScores other) {
                ExpectedTurnsToWin = other.ExpectedTurnsToWin;
                WpTire = other.WpTire;
                WpBreaks = other.WpBreaks;
                WpEngine = other.WpEngine;
                WpGear = other.WpGear;
            }

            public Result ToResult() {
                return new Result() {
                    CurrentSpot = null,
                    ExpectedWpTire = WpTire,
                    ExpectedTurnsToWin = 1 + ExpectedTurnsToWin,
                    SuggestedGearChange = Shift
                };
            }
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

        public class CostProperties {
            public int WpBreaks { get; set; }
            public int WpGear { get; set; }
            public int WpEngine { get; set; }

            public void Reset() {
                WpBreaks = 0;
                WpEngine = 0;
                WpGear = 0;
            }
        }

        private class Score {
            public int BringsCostBelowZero { get; set; }
            public double ExpectedTurnsToWin { get; set; }
            public int AnyCost { get; set; }
            public Roll CurrentRoll { get; set; }
            public int WpTire { get; set; }
        }
    }
}
