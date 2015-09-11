using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace GTAVC_Chaos
{
    public partial class OutputWindow : Form
    {
        public OutputWindow()
        {
            InitializeComponent();
            Show();
        }

        /// <summary>
        /// On registering the program closing, show a window asking the user to confirm exiting the program.
        /// </summary>
        private void OutputWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine("Event OutputWindow_FormClosing fired");
            if (e.CloseReason != CloseReason.UserClosing)
            {
                e.Cancel = false;
                return;
            }
            if (MessageBox.Show("Are you sure you want to exit the program?", "Chaos%", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
            Program.shouldStop = true;
        }

        private void OutputWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.shouldStop = true;
            Debug.WriteLine("Event OutputWindow_FormClosed fired");
        }

        private void OutputWindow_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.SunriseIcon;

            difficultySeedLabel.Text = String.Format("{0} difficulty, seed {1:D" + Settings.SEED_VALID_LENGTH + "}", Settings.difficultyName, Settings.seed);
            StaticEffectsLabel.Text = "Static Effects " + (Settings.staticEffectsEnabled ? "Enabled" : "Disabled");
            PermanentEffectsLabel.Text = "Placeholder";
            TimedEffectLabel.Text = "Timed effect placeholder";
        }

        
    }
}
