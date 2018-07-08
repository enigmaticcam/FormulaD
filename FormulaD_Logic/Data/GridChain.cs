using System.Collections.Generic;
using System.Linq;

namespace FormulaD_Logic.Data {
    public class GridChain {
        public enum enumDirectionType {
            Inward = 1,
            Outward,
            Straight
        }

        public int GridChainId { get; set; }
        public int StopNumberFrom { get; set; }
        public int StopNumberTo { get; set; }
        public enumDirectionType Direction { get; set; }
    }

    public class GridChainRef {
        private Dictionary<int, IEnumerable<GridChain>> _chainBySpotNumberFrom;

        public GridChainRef(IEnumerable<GridChain> gridChain) {
            var distinct = gridChain.Select(x => x.StopNumberFrom).Distinct();
            _chainBySpotNumberFrom = distinct.ToDictionary(x => x, x => gridChain.Where(y => y.StopNumberFrom == x));
        }

        public IEnumerable<GridChain> GetChainBySpotNumberFrom(int spotNumberFrom) {
            return _chainBySpotNumberFrom[spotNumberFrom];
        }

        public bool ChainBySpotNumberFromContains(int spotNumberFrom) {
            return _chainBySpotNumberFrom.ContainsKey(spotNumberFrom);
        }
    }
}
