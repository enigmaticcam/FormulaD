using FormulaD_Logic.Data;
using FormulaD_Logic.Logic.Actions;
using System.Collections.Generic;
using System.Linq;

namespace FormulaD_Logic.Logic {
    public class BuildResults {
        private Repository _repository = new Repository();
        private ResultRef _results = new ResultRef();
        private int _laps;
        

        public Result Perform(int laps) {
            _laps = laps;
            CalculateAllMoves();
            return null;
        }

        private void CalculateAllMoves() {
            ActionGetChildren action = new ActionGetChildren(_repository.GetGridChain, _repository.GetDice, _repository.GetGrid);
            action.Perform();
        }

        
    }
}
