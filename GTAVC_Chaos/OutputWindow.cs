using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GTAVC_Chaos
{
    public partial class OutputWindow : Form
    {
        public OutputWindow()
        {
            InitializeComponent();
            Show();
        }

        private void OutputWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine("Event OutputWindow_FormClosing fired");

            if (e.CloseReason == CloseReason.UserClosing && Program.game.IsRunning && MessageBox.Show("Are you sure you want to exit the program?", Settings.PROGRAM_NAME, MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void OutputWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Debug.WriteLine("Event OutputWindow_FormClosed fired");

            Program.shouldStop = true;
        }

        private void OutputWindow_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.SunriseIcon;

            difficultySeedLabel.Text = $"{Settings.difficultyName} difficulty, seed {Settings.seed.ToString($"D{Settings.SEED_VALID_LENGTH}")}";
            StaticEffectsLabel.Text = "Static Effects " + (Settings.staticEffectsEnabled ? "Enabled" : "Disabled");
            PermanentEffectsLabel.Text = "Placeholder";
            TimedEffectLabel.Text = "Timed effect placeholder";
        }
    }
}
