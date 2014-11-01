namespace GTAVC_Chaos
{
    partial class WelcomeWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonShowAdvancedOptions = new System.Windows.Forms.Button();
            this.radioButtonDifficulty1 = new System.Windows.Forms.RadioButton();
            this.radioButtonDifficulty2 = new System.Windows.Forms.RadioButton();
            this.radioButtonDifficulty3 = new System.Windows.Forms.RadioButton();
            this.checkBoxStaticEffectsEnabled = new System.Windows.Forms.CheckBox();
            this.checkBoxTimedEffectsEnabled = new System.Windows.Forms.CheckBox();
            this.checkboxSanicModeEnabled = new System.Windows.Forms.CheckBox();
            this.labelWelcomeMessage = new System.Windows.Forms.Label();
            this.labelEnterSeed = new System.Windows.Forms.Label();
            this.labelDifficulty = new System.Windows.Forms.Label();
            this.labelQuicksave = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericTextBoxSeed = new GTAVC_Chaos.NumericTextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Location = new System.Drawing.Point(15, 287);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(75, 23);
            this.buttonConfirm.TabIndex = 0;
            this.buttonConfirm.TabStop = false;
            this.buttonConfirm.Text = "Confirm";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(110, 287);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.TabStop = false;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonShowAdvancedOptions
            // 
            this.buttonShowAdvancedOptions.Location = new System.Drawing.Point(15, 189);
            this.buttonShowAdvancedOptions.Name = "buttonShowAdvancedOptions";
            this.buttonShowAdvancedOptions.Size = new System.Drawing.Size(150, 23);
            this.buttonShowAdvancedOptions.TabIndex = 2;
            this.buttonShowAdvancedOptions.TabStop = false;
            this.buttonShowAdvancedOptions.Text = "Show advanced options";
            this.buttonShowAdvancedOptions.UseVisualStyleBackColor = true;
            this.buttonShowAdvancedOptions.Click += new System.EventHandler(this.buttonShowAdvancedOptions_Click);
            // 
            // radioButtonDifficulty1
            // 
            this.radioButtonDifficulty1.AutoSize = true;
            this.radioButtonDifficulty1.Location = new System.Drawing.Point(3, 3);
            this.radioButtonDifficulty1.Name = "radioButtonDifficulty1";
            this.radioButtonDifficulty1.Size = new System.Drawing.Size(48, 17);
            this.radioButtonDifficulty1.TabIndex = 4;
            this.radioButtonDifficulty1.Text = "Easy";
            this.radioButtonDifficulty1.UseVisualStyleBackColor = true;
            // 
            // radioButtonDifficulty2
            // 
            this.radioButtonDifficulty2.AutoSize = true;
            this.radioButtonDifficulty2.Checked = true;
            this.radioButtonDifficulty2.Location = new System.Drawing.Point(57, 3);
            this.radioButtonDifficulty2.Name = "radioButtonDifficulty2";
            this.radioButtonDifficulty2.Size = new System.Drawing.Size(62, 17);
            this.radioButtonDifficulty2.TabIndex = 5;
            this.radioButtonDifficulty2.TabStop = true;
            this.radioButtonDifficulty2.Text = "Medium";
            this.radioButtonDifficulty2.UseVisualStyleBackColor = true;
            // 
            // radioButtonDifficulty3
            // 
            this.radioButtonDifficulty3.AutoSize = true;
            this.radioButtonDifficulty3.Location = new System.Drawing.Point(125, 3);
            this.radioButtonDifficulty3.Name = "radioButtonDifficulty3";
            this.radioButtonDifficulty3.Size = new System.Drawing.Size(48, 17);
            this.radioButtonDifficulty3.TabIndex = 6;
            this.radioButtonDifficulty3.Text = "Hard";
            this.radioButtonDifficulty3.UseVisualStyleBackColor = true;
            // 
            // checkBoxStaticEffectsEnabled
            // 
            this.checkBoxStaticEffectsEnabled.AutoSize = true;
            this.checkBoxStaticEffectsEnabled.Checked = true;
            this.checkBoxStaticEffectsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxStaticEffectsEnabled.Location = new System.Drawing.Point(15, 218);
            this.checkBoxStaticEffectsEnabled.Name = "checkBoxStaticEffectsEnabled";
            this.checkBoxStaticEffectsEnabled.Size = new System.Drawing.Size(129, 17);
            this.checkBoxStaticEffectsEnabled.TabIndex = 12;
            this.checkBoxStaticEffectsEnabled.Text = "Static effects enabled";
            this.checkBoxStaticEffectsEnabled.UseVisualStyleBackColor = true;
            this.checkBoxStaticEffectsEnabled.Visible = false;
            // 
            // checkBoxTimedEffectsEnabled
            // 
            this.checkBoxTimedEffectsEnabled.AutoSize = true;
            this.checkBoxTimedEffectsEnabled.Checked = true;
            this.checkBoxTimedEffectsEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxTimedEffectsEnabled.Location = new System.Drawing.Point(15, 241);
            this.checkBoxTimedEffectsEnabled.Name = "checkBoxTimedEffectsEnabled";
            this.checkBoxTimedEffectsEnabled.Size = new System.Drawing.Size(131, 17);
            this.checkBoxTimedEffectsEnabled.TabIndex = 13;
            this.checkBoxTimedEffectsEnabled.Text = "Timed effects enabled";
            this.checkBoxTimedEffectsEnabled.UseVisualStyleBackColor = true;
            this.checkBoxTimedEffectsEnabled.Visible = false;
            this.checkBoxTimedEffectsEnabled.Click += new System.EventHandler(this.checkBoxTimedEffectsEnabled_Click);
            // 
            // checkboxSanicModeEnabled
            // 
            this.checkboxSanicModeEnabled.AutoSize = true;
            this.checkboxSanicModeEnabled.Location = new System.Drawing.Point(15, 264);
            this.checkboxSanicModeEnabled.Name = "checkboxSanicModeEnabled";
            this.checkboxSanicModeEnabled.Size = new System.Drawing.Size(123, 17);
            this.checkboxSanicModeEnabled.TabIndex = 14;
            this.checkboxSanicModeEnabled.Text = "Sånic mode enabled";
            this.checkboxSanicModeEnabled.UseVisualStyleBackColor = true;
            this.checkboxSanicModeEnabled.Visible = false;
            // 
            // labelWelcomeMessage
            // 
            this.labelWelcomeMessage.AutoSize = true;
            this.labelWelcomeMessage.Location = new System.Drawing.Point(12, 9);
            this.labelWelcomeMessage.Name = "labelWelcomeMessage";
            this.labelWelcomeMessage.Size = new System.Drawing.Size(132, 13);
            this.labelWelcomeMessage.TabIndex = 15;
            this.labelWelcomeMessage.Text = "Welcome to Chaos% v1.2!";
            // 
            // labelEnterSeed
            // 
            this.labelEnterSeed.AutoSize = true;
            this.labelEnterSeed.Location = new System.Drawing.Point(12, 53);
            this.labelEnterSeed.Name = "labelEnterSeed";
            this.labelEnterSeed.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelEnterSeed.Size = new System.Drawing.Size(160, 13);
            this.labelEnterSeed.TabIndex = 16;
            this.labelEnterSeed.Text = "Please enter a  digit seed below:";
            // 
            // labelDifficulty
            // 
            this.labelDifficulty.AutoSize = true;
            this.labelDifficulty.Location = new System.Drawing.Point(12, 92);
            this.labelDifficulty.Name = "labelDifficulty";
            this.labelDifficulty.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelDifficulty.Size = new System.Drawing.Size(50, 13);
            this.labelDifficulty.TabIndex = 17;
            this.labelDifficulty.Text = "Difficulty:";
            // 
            // labelQuicksave
            // 
            this.labelQuicksave.AutoSize = true;
            this.labelQuicksave.Location = new System.Drawing.Point(12, 134);
            this.labelQuicksave.MaximumSize = new System.Drawing.Size(200, 0);
            this.labelQuicksave.Name = "labelQuicksave";
            this.labelQuicksave.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelQuicksave.Size = new System.Drawing.Size(198, 52);
            this.labelQuicksave.TabIndex = 18;
            this.labelQuicksave.Text = "F5 will quicksave the game if not in a vehicle or on a mission. After a crash, pr" +
    "ess F5 in the main menu to restore the save.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButtonDifficulty1);
            this.panel1.Controls.Add(this.radioButtonDifficulty2);
            this.panel1.Controls.Add(this.radioButtonDifficulty3);
            this.panel1.Location = new System.Drawing.Point(12, 108);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 23);
            this.panel1.TabIndex = 19;
            // 
            // numericTextBoxSeed
            // 
            this.numericTextBoxSeed.AllowDecimalSeparator = false;
            this.numericTextBoxSeed.AllowNegativeSign = false;
            this.numericTextBoxSeed.AllowNumberGroupSeparator = false;
            this.numericTextBoxSeed.AllowSpace = false;
            this.numericTextBoxSeed.Location = new System.Drawing.Point(15, 69);
            this.numericTextBoxSeed.MaxLength = 8;
            this.numericTextBoxSeed.Name = "numericTextBoxSeed";
            this.numericTextBoxSeed.Size = new System.Drawing.Size(100, 20);
            this.numericTextBoxSeed.TabIndex = 9;
            // 
            // WelcomeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(237, 324);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelQuicksave);
            this.Controls.Add(this.labelDifficulty);
            this.Controls.Add(this.labelEnterSeed);
            this.Controls.Add(this.labelWelcomeMessage);
            this.Controls.Add(this.checkboxSanicModeEnabled);
            this.Controls.Add(this.checkBoxTimedEffectsEnabled);
            this.Controls.Add(this.checkBoxStaticEffectsEnabled);
            this.Controls.Add(this.numericTextBoxSeed);
            this.Controls.Add(this.buttonShowAdvancedOptions);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonConfirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "WelcomeWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Welcome to Chaos%";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WelcomeWindow_FormClosing);
            this.Load += new System.EventHandler(this.WelcomeWindow_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonShowAdvancedOptions;
        private System.Windows.Forms.RadioButton radioButtonDifficulty1;
        private System.Windows.Forms.RadioButton radioButtonDifficulty2;
        private System.Windows.Forms.RadioButton radioButtonDifficulty3;
        private System.Windows.Forms.CheckBox checkBoxStaticEffectsEnabled;
        private System.Windows.Forms.CheckBox checkBoxTimedEffectsEnabled;
        private System.Windows.Forms.CheckBox checkboxSanicModeEnabled;
        private System.Windows.Forms.Label labelWelcomeMessage;
        private System.Windows.Forms.Label labelEnterSeed;
        private System.Windows.Forms.Label labelDifficulty;
        private System.Windows.Forms.Label labelQuicksave;
        public NumericTextBox numericTextBoxSeed;
        private System.Windows.Forms.Panel panel1;
    }
}

