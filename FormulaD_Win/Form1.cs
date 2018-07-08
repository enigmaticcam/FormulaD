using FormulaD_Logic.Logic;
using System;
using System.Windows.Forms;

namespace FormulaD_Win {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            BuildResults results = new BuildResults();
            results.Perform(2);
        }
    }
}
