using System.Collections.Generic;

namespace FormulaD_Logic.Data {
    public class Grid {
        public int SpotNumber { get; set; }
        public bool IsStart { get; set; }
        public bool IsFinish { get; set; }
        public bool IsTurn { get; set; }
        public int TurnCount { get; set; }
    }

    public class GridRef {
        private List<Grid> _grids = new List<Grid>();
        private List<Grid> _gridStart = new List<Grid>();
        private List<Grid> _gridFinish = new List<Grid>();
        private Dictionary<int, Grid> _gridBySpotNumber = new Dictionary<int, Grid>();

        public void AddGrid(Grid grid) {
            _grids.Add(grid);
            _gridBySpotNumber.Add(grid.SpotNumber, grid);
            if (grid.IsStart) {
                _gridStart.Add(grid);
            }
            if (grid.IsFinish) {
                _gridFinish.Add(grid);
            }
        }

        public IEnumerable<Grid> GridStart {
            get { return _gridStart; }
        }

        public IEnumerable<Grid> GridFinish {
            get { return _gridFinish; }
        }

        public Grid GetBySpotNumber(int spotNumber) {
            return _gridBySpotNumber[spotNumber];
        }

        public IEnumerable<Grid> Enumerate {
            get { return _grids; }
        }
    }
}
