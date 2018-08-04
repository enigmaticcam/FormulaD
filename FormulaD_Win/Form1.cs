using FormulaD_Logic.Data;
using FormulaD_Logic.Data.Tracks;
using FormulaD_Logic.Logic;
using FormulaD_Logic.Logic.Actions;
using System;
using System.Windows.Forms;

namespace FormulaD_Win {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            Track track = new Monaco();
            var results = new ResultRef();
            results.Initialize(track.MaxLaps, track.MaxSpots, track.MaxTurnCount, 15, 8, 8, 8, 6);
            BuildResults buildResults = new BuildResults(
                rolls: GetRolls(track),
                results: results,
                track: track);
            buildResults.Perform(185);
        }

        private RollRef GetRolls(Track track) {
            ActionGenerateRolls action = new ActionGenerateRolls(track.Chain, track.Dice, track.Grid);
            return action.Perform();
        }
    }
}
