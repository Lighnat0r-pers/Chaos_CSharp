using System;
using System.Diagnostics;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ChaosMod
{
    /// <summary>
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class OutputWindow : MetroWindow
    {
        public string DifficultyText => $"{Settings.difficultyName} difficulty, seed {Settings.seed.ToString($"D{Settings.SEED_VALID_LENGTH}")}";
        public string StaticEffectsText => "Static Effects " + (Settings.staticEffectsEnabled ? "Enabled" : "Disabled");
        public string PermanentEffectsText => "Permanent effect placeholder";
        public string TimedEffectsText => "Timed effect placeholder";


        public OutputWindow()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
            Title = Settings.PROGRAM_NAME;
        }

        private void OutputWindow1_Activated(object sender, EventArgs e)
        {
            difficultyTextBlock.Text = DifficultyText;
            StaticEffectsTextBlock.Text = StaticEffectsText;
            PermanentEffectsTextBlock.Text = PermanentEffectsText;
            TimedEffectsTextBlock.Text = TimedEffectsText;
        }

        private void OutputWindow1_Closed(object sender, EventArgs e)
        {
            Debug.WriteLine("Event OutputWindow1_Closed fired");
            Program.shouldStop = true;
        }
    }
}
