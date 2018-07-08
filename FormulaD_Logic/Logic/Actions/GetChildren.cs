using FormulaD_Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FormulaD_Logic.Logic.Actions {
    public class ActionGetChildren {
        private GridChainRef _gridChain;
        private DiceRef _dice;
        private GridRef _grid;
        private RollRef _rollRef = new RollRef();
        private Dictionary<int, List<RollTemplate>> _rolls = new Dictionary<int, List<RollTemplate>>();
        private Dictionary<int, Dictionary<int, HashSet<int>>> _seenBefore = new Dictionary<int, Dictionary<int, HashSet<int>>>();

        public ActionGetChildren(GridChainRef gridChain, DiceRef dice, GridRef grid) {
            _gridChain = gridChain;
            _dice = dice;
            _grid = grid;
        }

        public RollRef Perform() {
            AddInitialTwoMoveSets();
            AddRestOfMoveSets();
            return _rollRef;
        }

        private void AddInitialTwoMoveSets() {
            foreach (var grid in _grid.Enumerate) {
                if (_gridChain.ChainBySpotNumberFromContains(grid.SpotNumber)) {
                    foreach (var chain in _gridChain.GetChainBySpotNumberFrom(grid.SpotNumber)) {
                        List<int> overshootCount = new List<int>();
                        if (CheckForNewOvershootCount(chain)) {
                            overshootCount.Add(1);
                        }
                        AddRoll(new RollTemplate() {
                            Direction = chain.Direction,
                            DoesCrossFinish = _grid.GetBySpotNumber(chain.StopNumberTo).IsFinish && !_grid.GetBySpotNumber(chain.StopNumberFrom).IsFinish,
                            MoveCount = 1,
                            OvershootCount = overshootCount,
                            SpotStart = grid.SpotNumber,
                            SpotEnd = chain.StopNumberTo
                        });
                    }
                }
            }
        }

        private void AddRestOfMoveSets() {
            for (int moveCount = 2; moveCount <= _dice.HighestDie.DieMax; moveCount++) {
                if (_rolls.ContainsKey(moveCount - 1)) {
                    foreach (var roll in _rolls[moveCount - 1]) {
                        if (_gridChain.ChainBySpotNumberFromContains(roll.SpotEnd)) {
                            foreach (var chain in _gridChain.GetChainBySpotNumberFrom(roll.SpotEnd)) {
                                if (CanMoveDirection(roll.Direction, chain.Direction)) {
                                    List<int> overshootCount = new List<int>(roll.OvershootCount);
                                    if (CheckForNewOvershootCount(chain)) {
                                        overshootCount.Add(0);
                                    }
                                    IncrementOvershoot(overshootCount);
                                    if (!SeenBefore(roll.SpotStart, chain.StopNumberTo, moveCount) && CanMoveDirection(roll.Direction, chain.Direction)) {
                                        AddRoll(new RollTemplate() {
                                            Direction = CombineDirections(roll.Direction, chain.Direction),
                                            DoesCrossFinish = roll.DoesCrossFinish,
                                            OvershootCount = overshootCount,
                                            MoveCount = moveCount,
                                            SpotEnd = chain.StopNumberTo,
                                            SpotStart = roll.SpotStart
                                        });
                                    } else {
                                        CompareOvershoot(roll.SpotStart, chain.StopNumberTo, moveCount, overshootCount);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CompareOvershoot(int spotStart, int spotEnd, int moveCount, List<int> newOvershoot) {
            var oldOvershoot = _rollRef.RollsBySpotByDie(spotStart, moveCount).Where(x => x.EndSpot == spotEnd).First();
            bool replace = false;
            if (newOvershoot.Count > oldOvershoot.OvershootCount.Count) {
                replace = true;
            } else {
                for (int index = 0; index < newOvershoot.Count; index++) {
                    if (newOvershoot[index] < oldOvershoot.OvershootCount[index]) {
                        replace = true;
                        break;
                    }
                }
            }
            if (replace) {
                oldOvershoot.OvershootCount = newOvershoot;
            }
        }

        private bool CheckForNewOvershootCount(GridChain chain) {
            return _grid.GetBySpotNumber(chain.StopNumberFrom).IsTurn && !_grid.GetBySpotNumber(chain.StopNumberTo).IsTurn;
        }

        private void IncrementOvershoot(List<int> overshoot) {
            for (int index = 0; index < overshoot.Count; index++) {
                overshoot[index]++;
            }
        }

        private bool SeenBefore(int spotStart, int spotEnd, int moveCount) {
            return _seenBefore.ContainsKey(spotStart) && _seenBefore[spotStart].ContainsKey(spotEnd) && _seenBefore[spotStart][spotEnd].Contains(moveCount);
        }

        private bool CanMoveDirection(GridChain.enumDirectionType lastDirection, GridChain.enumDirectionType newDirection) {
            switch (lastDirection) {
                case GridChain.enumDirectionType.Inward:
                    return newDirection != GridChain.enumDirectionType.Outward;
                case GridChain.enumDirectionType.Outward:
                    return newDirection != GridChain.enumDirectionType.Inward;
                default:
                    return true;
            }
        }

        private GridChain.enumDirectionType CombineDirections(GridChain.enumDirectionType lastDirection, GridChain.enumDirectionType newDirection) {
            if (lastDirection != GridChain.enumDirectionType.Straight) {
                return lastDirection;
            } else {
                return newDirection;
            }
        }

        private void AddRoll(RollTemplate roll) {
            if (roll.SpotStart == roll.SpotEnd) {
                throw new Exception("Chain not correct. Cannot create a roll with the same point for start and end");
            }
            if (!_rolls.ContainsKey(roll.MoveCount)) {
                _rolls.Add(roll.MoveCount, new List<RollTemplate>());
            }
            _rolls[roll.MoveCount].Add(roll);
            if (!_seenBefore.ContainsKey(roll.SpotStart)) {
                _seenBefore.Add(roll.SpotStart, new Dictionary<int, HashSet<int>>());
            }
            if (!_seenBefore[roll.SpotStart].ContainsKey(roll.SpotEnd)) {
                _seenBefore[roll.SpotStart].Add(roll.SpotEnd, new HashSet<int>());
            }
            _seenBefore[roll.SpotStart][roll.SpotEnd].Add(roll.MoveCount);
            _rollRef.AddRoll(roll.SpotStart, roll.MoveCount, roll.SpotEnd, roll.OvershootCount);
        }

        private class RollTemplate {
            public bool DoesCrossFinish { get; set; }
            public int MoveCount { get; set; }
            public int SpotEnd { get; set; }
            public int SpotStart { get; set; }
            public GridChain.enumDirectionType Direction { get; set; }
            public List<int> OvershootCount { get; set; }
        }
    }
}
