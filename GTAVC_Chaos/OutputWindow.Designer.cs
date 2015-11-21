namespace GTAVC_Chaos
{
    partial class OutputWindow
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
            if (disposing)
            {
                components?.Dispose();
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
            this.difficultySeedLabel = new System.Windows.Forms.Label();
            this.StaticEffectsLabel = new System.Windows.Forms.Label();
            this.PermanentEffectsActiveLabel = new System.Windows.Forms.Label();
            this.TimedEffectActiveLabel = new System.Windows.Forms.Label();
            this.TimedEffectLabel = new System.Windows.Forms.Label();
            this.PermanentEffectsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // difficultySeedLabel
            // 
            this.difficultySeedLabel.AutoSize = true;
            this.difficultySeedLabel.BackColor = System.Drawing.SystemColors.Control;
            this.difficultySeedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.difficultySeedLabel.Location = new System.Drawing.Point(13, 13);
            this.difficultySeedLabel.Name = "difficultySeedLabel";
            this.difficultySeedLabel.Size = new System.Drawing.Size(141, 13);
            this.difficultySeedLabel.TabIndex = 0;
            this.difficultySeedLabel.Text = "Medium difficulty, seed 1234";
            // 
            // StaticEffectsLabel
            // 
            this.StaticEffectsLabel.AutoSize = true;
            this.StaticEffectsLabel.Location = new System.Drawing.Point(13, 48);
            this.StaticEffectsLabel.Name = "StaticEffectsLabel";
            this.StaticEffectsLabel.Size = new System.Drawing.Size(112, 13);
            this.StaticEffectsLabel.TabIndex = 1;
            this.StaticEffectsLabel.Text = "Static Effects Enabled";
            // 
            // PermanentEffectsActiveLabel
            // 
            this.PermanentEffectsActiveLabel.AutoSize = true;
            this.PermanentEffectsActiveLabel.Location = new System.Drawing.Point(13, 75);
            this.PermanentEffectsActiveLabel.Name = "PermanentEffectsActiveLabel";
            this.PermanentEffectsActiveLabel.Size = new System.Drawing.Size(130, 13);
            this.PermanentEffectsActiveLabel.TabIndex = 2;
            this.PermanentEffectsActiveLabel.Text = "Permanent Effects Active:";
            // 
            // TimedEffectActiveLabel
            // 
            this.TimedEffectActiveLabel.AutoSize = true;
            this.TimedEffectActiveLabel.Location = new System.Drawing.Point(12, 189);
            this.TimedEffectActiveLabel.Name = "TimedEffectActiveLabel";
            this.TimedEffectActiveLabel.Size = new System.Drawing.Size(103, 13);
            this.TimedEffectActiveLabel.TabIndex = 3;
            this.TimedEffectActiveLabel.Text = "Timed Effect Active:";
            // 
            // TimedEffectLabel
            // 
            this.TimedEffectLabel.AutoSize = true;
            this.TimedEffectLabel.Location = new System.Drawing.Point(13, 212);
            this.TimedEffectLabel.Name = "TimedEffectLabel";
            this.TimedEffectLabel.Size = new System.Drawing.Size(72, 13);
            this.TimedEffectLabel.TabIndex = 4;
            this.TimedEffectLabel.Text = "[effect_name]";
            // 
            // PermanentEffectsLabel
            // 
            this.PermanentEffectsLabel.AutoSize = true;
            this.PermanentEffectsLabel.Location = new System.Drawing.Point(16, 92);
            this.PermanentEffectsLabel.Name = "PermanentEffectsLabel";
            this.PermanentEffectsLabel.Size = new System.Drawing.Size(102, 52);
            this.PermanentEffectsLabel.TabIndex = 5;
            this.PermanentEffectsLabel.Text = "[permanent_effect1]\r\n[permanent_effect2]\r\n[permanent_effect3]\r\n[permanent_effect4" +
    "]";
            // 
            // OutputWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 262);
            this.Controls.Add(this.PermanentEffectsLabel);
            this.Controls.Add(this.TimedEffectLabel);
            this.Controls.Add(this.TimedEffectActiveLabel);
            this.Controls.Add(this.PermanentEffectsActiveLabel);
            this.Controls.Add(this.StaticEffectsLabel);
            this.Controls.Add(this.difficultySeedLabel);
            this.Name = "OutputWindow";
            this.Text = "Chaos% Output";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OutputWindow_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OutputWindow_FormClosed);
            this.Load += new System.EventHandler(this.OutputWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label difficultySeedLabel;
        private System.Windows.Forms.Label StaticEffectsLabel;
        private System.Windows.Forms.Label PermanentEffectsActiveLabel;
        private System.Windows.Forms.Label TimedEffectActiveLabel;
        private System.Windows.Forms.Label TimedEffectLabel;
        private System.Windows.Forms.Label PermanentEffectsLabel;
    }
}