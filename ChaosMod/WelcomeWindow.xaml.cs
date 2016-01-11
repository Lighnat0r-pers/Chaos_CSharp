using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace ChaosMod
{
    /// <summary>
    /// Interaction logic for WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : MetroWindow
    {
        public string IntroText => $"Welcome to Chaos% v{Settings.ProgramVersion}!";
        public string SeedInstruction => $"Enter a {Settings.SeedValidLength} digit seed below:";
        public int SeedLength => Settings.SeedValidLength;
        public bool? TimedEffectsEnabledDefault => Settings.timedEffectsEnabledDefault;
        public bool? StaticEffectsEnabledDefault => Settings.staticEffectsEnabledDefault;
        public bool? SanicModeEnabledDefault => Settings.sanicModeEnabledDefault;
        private List<Game> SupportedGames => Settings.SupportedGames;

        public WelcomeWindow()
        {
            InitializeComponent();
            Title = Settings.ProgramName;
            // TODO(Ligh): Handle difficulty options dynamically instead of hardcoding available options + default.
        }

        private void WelcomeWindow_Closed(object sender, EventArgs e)
        {
            Program.ShouldStop = true;
            Debug.WriteLine("Event WelcomeWindow_Closed fired");
        }

        /// <summary>
        /// Only allow numeric inputs into the seed input.
        /// </summary>
        private void seedInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            int iValue;

            if (Int32.TryParse(textBox.Text, out iValue) == false)
            {
                var textChange = e.Changes.ElementAt(0);
                textBox.Text = textBox.Text.Remove(textChange.Offset, textChange.AddedLength);
            }
        }

        private void advancedOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (advancedOptions.IsVisible)
            {
                advancedOptionsButton.Content = "Show advanced options";
                advancedOptions.Visibility = Visibility.Hidden;
                timedEffectsEnabled.IsChecked = TimedEffectsEnabledDefault;
                staticEffectsEnabled.IsChecked = StaticEffectsEnabledDefault;
                sanicModeEnabled.IsChecked = SanicModeEnabledDefault;
            }
            else
            {
                advancedOptionsButton.Content = "Hide advanced options";
                advancedOptions.Visibility = Visibility.Visible;
            }
        }

        private void sanicModeEnabled_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!sanicModeEnabled.IsEnabled)
            {
                sanicModeEnabled.IsChecked = false;
            }
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Game = gameSelect.SelectedItem as Game;

            if (Settings.Game == null)
            {
                throw new ArgumentNullException(nameof(Settings.Game), "No game selected.");
            }

            int seed;
            if (!int.TryParse(seedInput.Text, out seed))
            {
                Debug.WriteLine("Seed entered was not valid, set to default 0.");
            }
            Settings.Seed = seed;
            Settings.Random = new Random(seed);

            Settings.Difficulty = Settings.difficulties.Find(d => d.Name == (string)DifficultyPanel.Children.OfType<RadioButton>().First(r => (bool)r.IsChecked).Content);

            if (Settings.Difficulty.Equals(default(Difficulty)))
            {
                Debug.WriteLine("Invalid difficulty selected, set to default.");
                Settings.Difficulty = Settings.defaultDifficulty;
            }

            Settings.PermanentEffectsEnabled = (Settings.Seed == 0);
            Settings.StaticEffectsEnabled = (bool)staticEffectsEnabled.IsChecked;
            Settings.TimedEffectsEnabled = (bool)timedEffectsEnabled.IsChecked;
            Settings.SanicModeEnabled = (bool)sanicModeEnabled.IsChecked;

            Debug.WriteLine($"Game: {Settings.Game.Name}");
            Debug.WriteLine($"Seed: {Settings.Seed}");
            Debug.WriteLine($"Difficulty: {Settings.Difficulty.Name}");
            Debug.WriteLine($"Static Effects Enabled: {Settings.StaticEffectsEnabled}");
            Debug.WriteLine($"Permanent Effects Enabled: {Settings.PermanentEffectsEnabled}");
            Debug.WriteLine($"Timed Effects Enabled: {Settings.TimedEffectsEnabled}");
            Debug.WriteLine($"Sanic Mode Enabled: {Settings.SanicModeEnabled}");

            Closed -= new EventHandler(WelcomeWindow_Closed);
            Close();
        }

        private void WelcomeWindow1_Activated(object sender, EventArgs e)
        {
            gameSelect.ItemsSource = SupportedGames;
            gameSelect.DisplayMemberPath = "Name";
            gameSelect.SelectedIndex = 0;
        }
    }
}
