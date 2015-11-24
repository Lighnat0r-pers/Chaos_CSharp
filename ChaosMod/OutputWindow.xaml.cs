using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using MahApps.Metro.Controls;

namespace ChaosMod
{
    /// <summary>
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class OutputWindow : MetroWindow
    {
        private DispatcherTimer dispatcherTimer;

        public string DifficultyText => $"{Settings.Difficulty.Name} difficulty, seed {Settings.Seed.ToString($"D{Settings.SeedValidLength}")}";
        public string StaticEffectsText => "Static Effects " + (Settings.StaticEffectsEnabled ? "Enabled" : "Disabled");
        public string PermanentEffectsText => "Permanent effect placeholder";
        public string TimedEffectsText => $"{Settings.CurrentEffect?.name ?? "No effect"} active";


        public OutputWindow()
        {
            InitializeComponent();
            Visibility = Visibility.Hidden;
            Title = Settings.ProgramName;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, Settings.DefaultWaitTime);
        }

        private void OutputWindow1_Activated(object sender, EventArgs e)
        {
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
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
