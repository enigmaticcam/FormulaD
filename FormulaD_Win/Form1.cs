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
            BuildResults results = new BuildResults(
                rolls: GetRolls(track),
                results: new ResultRef(),
                track: track);
            var result = results.Perform(186, 2);
        }

        private RollRef GetRolls(Track track) {
            ActionGenerateRolls action = new ActionGenerateRolls(track.Chain, track.Dice, track.Grid);
            return action.Perform();
        }
    }
}
