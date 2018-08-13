using FormulaD_Logic.Data;
using FormulaD_Logic.Logic.Actions;
using System.Linq;

namespace FormulaD_Logic.Logic {
    public class Engine {
        public void Begin(Track  track) {
            PerformTest(track);
            //PerformFull(track);
        }

        private void PerformTest(Track track) {
            var results = GetResults(track);
            ActionBuildResults action = new ActionBuildResults(
                rolls: GetRolls(track),
                results: results,
                track: track);
            action.Perform(351, 2);
            bool stophere = true;
        }

        private ResultRef GetResults(Track track) {
            var results = new ResultRef();
            results.Initialize(track.MaxLaps, track.MaxSpots, track.MaxTurnCount, 15, 8, 8, 8, 6);
            return results;
        }

        private RollRef GetRolls(Track track) {
            ActionGenerateRolls action = new ActionGenerateRolls(track.Chain, track.Dice, track.Grid);
            return action.Perform();
        }
    }
}
