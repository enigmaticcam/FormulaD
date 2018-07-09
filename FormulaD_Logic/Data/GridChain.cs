using System.Collections.Generic;

namespace FormulaD_Logic.Data {
    public class GridChain {
        public enum enumDirectionType {
            Inward = 1,
            Outward,
            Straight
        }

        public int StopNumberFrom { get; set; }
        public int StopNumberTo { get; set; }
        public enumDirectionType Direction { get; set; }
    }

    public class GridChainRef {
        private Dictionary<int, List<GridChain>> _chainBySpotNumberFrom = new Dictionary<int, List<GridChain>>();
        private List<GridChain> _chain = new List<GridChain>();

        public IEnumerable<GridChain> Enumerate {
            get { return _chain; }
        }

        public IEnumerable<GridChain> GetChainBySpotNumberFrom(int spotNumberFrom) {
            return _chainBySpotNumberFrom[spotNumberFrom];
        }

        public bool ChainBySpotNumberFromContains(int spotNumberFrom) {
            return _chainBySpotNumberFrom.ContainsKey(spotNumberFrom);
        }

        public void AddGridChain(GridChain chain) {
            if (!_chainBySpotNumberFrom.ContainsKey(chain.StopNumberFrom)) {
                _chainBySpotNumberFrom.Add(chain.StopNumberFrom, new List<GridChain>());
            }
            _chainBySpotNumberFrom[chain.StopNumberFrom].Add(chain);
            _chain.Add(chain);
        }
    }
}
