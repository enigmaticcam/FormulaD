using FormulaD_Logic.Data;
using FormulaD_Logic.Data.Tracks;
using FormulaD_Logic.Logic;
using System;
using System.Windows.Forms;

namespace FormulaD_Win {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            Track track = new Monaco();
            Engine engine = new Engine();
            engine.Begin(track);
        }

        
    }
}
