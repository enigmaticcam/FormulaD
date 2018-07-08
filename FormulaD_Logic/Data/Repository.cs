using System;
using System.Collections.Generic;
using System.IO;

namespace FormulaD_Logic.Data {
    public class Repository {
        private string _rootDirectory;
        public string RootDirectory {
            get {
                if (_rootDirectory == null) {
                    _rootDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.Substring(8)) + "\\Data\\RawData\\";
                }
                return _rootDirectory;
            }
        }

        private DiceRef _dice;
        public DiceRef GetDice {
            get {
                if (_dice == null) {
                    LoadData();
                }
                return _dice;
            }
        }

        private GridRef _grid;
        public GridRef GetGrid {
            get {
                if (_grid == null) {
                    LoadData();
                }
                return _grid;
            }
        }

        private GridChainRef _gridChain;
        public GridChainRef GetGridChain {
            get {
                if (_gridChain == null) {
                    LoadData();
                }
                return _gridChain;
            }
        }

        public void LoadData() {
            LoadDice();
            LoadGrid();
            LoadGridChain();
        }

        private void LoadDice() {
            _dice = new DiceRef();
            using (var stream = new StreamReader(RootDirectory + "Dice.txt")) {
                do {
                    string line = stream.ReadLine();
                    string[] data = line.Split(',');
                    _dice.AddDie(new Die() {
                        DieId = Convert.ToInt32(data[0]),
                        DieMax = Convert.ToInt32(data[3]),
                        DieMin = Convert.ToInt32(data[2]),
                        DieNum = Convert.ToInt32(data[1]),
                    });
                } while (!stream.EndOfStream);
            };
        }

        private void LoadGrid() {
            _grid = new GridRef();
            using (var stream = new StreamReader(RootDirectory + "Grid.txt")) {
                do {
                    string line = stream.ReadLine();
                    string[] data = line.Split(',');
                    _grid.AddGrid(new Grid() {
                        GridId = Convert.ToInt32(data[0]),
                        IsFinish = StringToBoolean(data[3]),
                        IsStart = StringToBoolean(data[2]),
                        IsTurn = StringToBoolean(data[4]),
                        SpotNumber = Convert.ToInt32(data[1]),
                        TurnCount = Convert.ToInt32(data[5])
                    });
                } while (!stream.EndOfStream);
            }
        }

        private void LoadGridChain() {
            List<GridChain> gridChain = new List<GridChain>();
            using (var stream = new StreamReader(RootDirectory + "GridChain.txt")) {
                do {
                    string line = stream.ReadLine();
                    string[] data = line.Split(',');
                    gridChain.Add(new GridChain() {
                        Direction = (GridChain.enumDirectionType)(Convert.ToInt32(data[3])),
                        GridChainId = Convert.ToInt32(data[0]),
                        StopNumberFrom = Convert.ToInt32(data[1]),
                        StopNumberTo = Convert.ToInt32(data[2])
                    });
                } while (!stream.EndOfStream);
            }
            _gridChain = new GridChainRef(gridChain);
        }

        private bool StringToBoolean(string value) {
            return (value == "1" ? true : false);
        }
    }
}
