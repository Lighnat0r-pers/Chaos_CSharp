using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;

namespace GTAVC_Chaos
{
    public partial class WelcomeWindow : Form
    {
        private bool showAdvancedOptions = false;

        public WelcomeWindow()
        {
            InitializeComponent();
            Show();
        }

        /// <summary>
        /// Close the program when clicking the close button.
        /// </summary>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// On registering the program closing, show a window asking the user to confirm exiting the program.
        /// This really isnt necessary for the welcome window, but we'll leave the code in for now to use later for the output window.
        /// </summary>
        private void WelcomeWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine("Event WelcomeWindow_FormClosing fired");
            if (e.CloseReason != CloseReason.UserClosing)
            {
                e.Cancel = false;
                return;
            }
            if (MessageBox.Show("Are you sure you want to exit the program?", "GTA Vice City Chaos%", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
            Program._shouldStop = true;
        }

        private void WelcomeWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Debug.WriteLine("Event WelcomeWindow_FormClosed fired");
        }

        /// <summary>
        /// Because the designer doesn't like variables in any of the control options and every time a control is 
        /// changed in the designer the code is updated and any manual changes reverted, we have to set the variables 
        /// we want to use in any of the controls after the code of the designer is executed, 
        /// that is just before the window is shown. Doing it like this kinda sucks, but it's the best I can do without ditching the designer.
        /// </summary>
        private void WelcomeWindow_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.SunriseIcon;
            numericTextBoxSeed.MaxLength = Settings.SEED_VALID_LENGTH;
            labelEnterSeed.Text = String.Format("Please enter a {0} digit seed below:", Settings.SEED_VALID_LENGTH);
            labelWelcomeMessage.Text = String.Format("Welcome to Chaos% v{0:f}!", Settings.PROGRAM_VERSION);
        }

        /// <summary>
        /// Toggle showing the advance options, i.e. the checkboxes below the Advanced Options button.
        /// If the advanced options are disabled, make sure all the checkboxes are at default.
        /// </summary>
        private void buttonShowAdvancedOptions_Click(object sender, EventArgs e)
        {
            if(showAdvancedOptions)
            {
                checkBoxTimedEffectsEnabled.Hide();
                checkBoxTimedEffectsEnabled.Checked = Settings.timedEffectsEnabledDefault;
                checkBoxStaticEffectsEnabled.Hide();
                checkBoxStaticEffectsEnabled.Checked = Settings.staticEffectsEnabledDefault;
                checkboxSanicModeEnabled.Hide();
                checkboxSanicModeEnabled.Checked = Settings.sanicModeEnabledDefault;
            }
            else
            {
                checkBoxTimedEffectsEnabled.Show();
                checkBoxStaticEffectsEnabled.Show();
                checkboxSanicModeEnabled.Show();
            }
            showAdvancedOptions = !showAdvancedOptions;
        }

        /// <summary>
        /// When the timed effects are set to be turned off, disable the sanic mode checkbox since 
        /// that does nothing without timed effects. Re-enable the sanic mode checkbox when the timed effects are turned back on.
        /// </summary>
        private void checkBoxTimedEffectsEnabled_Click(object sender, EventArgs e)
        {
            if(!checkBoxTimedEffectsEnabled.Checked)
            {
                checkboxSanicModeEnabled.Enabled = false;
                checkboxSanicModeEnabled.Checked = Settings.sanicModeEnabledDefault;
            }
            else
            {
                checkboxSanicModeEnabled.Enabled = true;
            }
        }

        /// <summary>
        /// When the confirm button is clicked, save the settings set and close the form without 
        /// triggering the exit confirmation.
        /// </summary>
        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            bool isValidSeed = uint.TryParse(numericTextBoxSeed.Text, out Settings.seed);
            if (!isValidSeed)
            {
                Debug.WriteLine("Seed entered was not valid, set to default 0");
            }

            RadioButton checkedButton = panel1.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            bool isValidDifficulty = Settings.difficultiesArray.TryGetValue(checkedButton.Text, out Settings.difficulty);
            if (!isValidDifficulty)
            {
                Settings.difficulty = Settings.difficultyDefault;
            }

            Settings.staticEffectsEnabled = checkBoxStaticEffectsEnabled.Checked;
            Settings.timedEffectsEnabled = checkBoxTimedEffectsEnabled.Checked;
            Settings.sanicModeEnabled = checkboxSanicModeEnabled.Checked;

            Debug.WriteLine(String.Format("Seed: {0}", Settings.seed));
            Debug.WriteLine(String.Concat("Difficulty: ", checkedButton.Text));
            Debug.WriteLine(String.Format("Static Effects Enabled: {0}", Settings.staticEffectsEnabled));
            Debug.WriteLine(String.Format("Timed Effects Enabled: {0}", Settings.timedEffectsEnabled));
            Debug.WriteLine(String.Format("Sanic Mode Enabled: {0}", Settings.sanicModeEnabled));

            this.FormClosing -= new System.Windows.Forms.FormClosingEventHandler(this.WelcomeWindow_FormClosing);
            this.FormClosed -= new System.Windows.Forms.FormClosedEventHandler(this.WelcomeWindow_FormClosed);
            this.Close();
            Program.currentForm = null;
        }
    }
}
