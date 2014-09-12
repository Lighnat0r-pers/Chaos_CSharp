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

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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

        private void buttonShowAdvancedOptions_Click(object sender, EventArgs e)
        {
            if(showAdvancedOptions)
            {
                checkboxSanicModeEnabled.Hide();
                checkBoxStaticEffectsEnabled.Hide();
                checkBoxTimedEffectsEnabled.Hide();
            }
            else
            {
                checkboxSanicModeEnabled.Show();
                checkBoxStaticEffectsEnabled.Show();
                checkBoxTimedEffectsEnabled.Show();
            }
            showAdvancedOptions = !showAdvancedOptions;
        }
    }
}
