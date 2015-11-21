using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

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

        private void WelcomeWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.shouldStop = true;
            Debug.WriteLine("Event WelcomeWindow_FormClosed fired");
        }

        /// <summary>
        /// Because the designer doesn't like variables in any of the control options and every time a control is 
        /// changed in the designer the code is updated and any manual changes reverted, we have to set the variables 
        /// we want to use in any of the controls after the code of the designer is executed, 
        /// that is just before the window is shown.
        /// </summary>
        private void WelcomeWindow_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.SunriseIcon;
            numericTextBoxSeed.MaxLength = Settings.SEED_VALID_LENGTH;
            labelEnterSeed.Text = $"Please enter a {Settings.SEED_VALID_LENGTH} digit seed below:";
            labelWelcomeMessage.Text = $"Welcome to Chaos% v{Settings.PROGRAM_VERSION:f}!";
        }

        /// <summary>
        /// Toggle showing the advance options, i.e. the checkboxes below the Advanced Options button.
        /// If the advanced options are disabled, make sure all the checkboxes are at default.
        /// </summary>
        private void buttonShowAdvancedOptions_Click(object sender, EventArgs e)
        {
            if (showAdvancedOptions)
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
            if (!checkBoxTimedEffectsEnabled.Checked)
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
            bool isValidSeed = int.TryParse(numericTextBoxSeed.Text, out Settings.seed);
            if (!isValidSeed)
            {
                Debug.WriteLine("Seed entered was not valid, set to default 0");
            }

            RadioButton checkedButton = panel1.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            Settings.difficultyName = checkedButton.Text;
            bool isValidDifficulty = Settings.difficultiesArray.TryGetValue(checkedButton.Text, out Settings.difficulty);
            if (!isValidDifficulty)
            {
                Settings.difficulty = Settings.defaultDifficulty;
            }

            Settings.permanentEffectsEnabled = (Settings.seed == 0);
            Settings.staticEffectsEnabled = checkBoxStaticEffectsEnabled.Checked;
            Settings.timedEffectsEnabled = checkBoxTimedEffectsEnabled.Checked;
            Settings.sanicModeEnabled = checkboxSanicModeEnabled.Checked;

            Debug.WriteLine($"Seed: {Settings.seed}");
            Debug.WriteLine($"Difficulty: {Settings.difficultyName}");
            Debug.WriteLine($"Static Effects Enabled: {Settings.staticEffectsEnabled}");
            Debug.WriteLine($"Permanent Effects Enabled: {Settings.permanentEffectsEnabled}");
            Debug.WriteLine($"Timed Effects Enabled: {Settings.timedEffectsEnabled}");
            Debug.WriteLine($"Sanic Mode Enabled: {Settings.sanicModeEnabled}");

            FormClosed -= new FormClosedEventHandler(WelcomeWindow_FormClosed);
            Close();
        }
    }
}
