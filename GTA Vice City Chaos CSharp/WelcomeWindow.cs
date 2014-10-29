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
    public partial class WelcomeWindow : Form
    {
        private bool showAdvancedOptions = false;

        public WelcomeWindow()
        {
            InitializeComponent();
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
            Debug.WriteLine("Event OnWelcomeWindowClose fired");
            if (e.CloseReason != CloseReason.UserClosing)
            {
                e.Cancel = false;
                return;
            }
            if (MessageBox.Show("Are you sure you want to exit the program?", "GTA Vice City Chaos%", MessageBoxButtons.YesNo) == DialogResult.No)
                e.Cancel = true;
        }

        /// <summary>
        /// Because the designer doesn't like variables in any of the control options and every time a control is 
        /// changed in the designer the code is updated and any manual changes reverted, we have to set the variables 
        /// we want to use in any of the controls after the code of the designer is executed, 
        /// that is just before the window is shown. Doing it like this kinda sucks, but it's the best I can do without ditching the designer.
        /// </summary>
        private void WelcomeWindow_Load(object sender, EventArgs e)
        {
            numericTextBoxSeed.MaxLength = Program.SEED_VALID_LENGTH;
            labelEnterSeed.Text = String.Format("Please enter a {0} digit seed below:", Program.SEED_VALID_LENGTH);
            labelWelcomeMessage.Text = String.Format("Welcome to Chaos% v{0}!", Program.PROGRAM_VERSION);
        }

        /// <summary>
        /// Toggle showing the advance options, i.e. the checkboxes below the Advanced Options button.
        /// If the advanced options are disabled, make sure all the checkboxes are at default.
        /// </summary>
        private void buttonShowAdvancedOptions_Click(object sender, EventArgs e)
        {
            if(showAdvancedOptions)
            {
                checkboxSanicModeEnabled.Hide();
                checkboxSanicModeEnabled.Checked = false;
                checkBoxStaticEffectsEnabled.Hide();
                checkBoxStaticEffectsEnabled.Checked = true;
                checkBoxTimedEffectsEnabled.Hide();
                checkBoxTimedEffectsEnabled.Checked = true;
            }
            else
            {
                checkboxSanicModeEnabled.Show();
                checkBoxStaticEffectsEnabled.Show();
                checkBoxTimedEffectsEnabled.Show();
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
                checkboxSanicModeEnabled.Checked = false;
            }
            else
            {
                checkboxSanicModeEnabled.Enabled = true;
            }
        }
    }
}
