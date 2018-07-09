using FormulaD_Logic.Data;
using FormulaD_Logic.Data.Tracks;
using FormulaD_Logic.Logic.Actions;

namespace FormulaD_Logic.Logic {
    public class BuildResults {
        private Track _track = new Monaco();
        private ResultRef _results = new ResultRef();
        private int _laps;
        

        public Result Perform(int laps) {
            _laps = laps;
            CalculateAllMoves();
            return null;
        }

        private void CalculateAllMoves() {
            ActionGetChildren action = new ActionGetChildren(_track.Chain, _track.Dice, _track.Grid);
            action.Perform();
        }

        
    }
}
