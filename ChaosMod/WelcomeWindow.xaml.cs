using System;
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
        public string IntroText => $"Welcome to Chaos% v{Settings.PROGRAM_VERSION}!";
        public string SeedInstruction => $"Please enter a {Settings.SEED_VALID_LENGTH} digit seed below:";
        public int SeedLength => Settings.SEED_VALID_LENGTH;
        public bool? TimedEffectsEnabledDefault => Settings.timedEffectsEnabledDefault;
        public bool? StaticEffectsEnabledDefault => Settings.staticEffectsEnabledDefault;
        public bool? SanicModeEnabledDefault => Settings.sanicModeEnabledDefault;

        public WelcomeWindow()
        {
            InitializeComponent();
            Title = Settings.PROGRAM_NAME;

            // TODO(Ligh): Handle difficulty options dynamically instead of hardcoding available options + default.
        }

        private void WelcomeWindow_Closed(object sender, EventArgs e)
        {
            Program.shouldStop = true;
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
                advancedOptions.Visibility = Visibility.Hidden;
                timedEffectsEnabled.IsChecked = TimedEffectsEnabledDefault;
                staticEffectsEnabled.IsChecked = StaticEffectsEnabledDefault;
                sanicModeEnabled.IsChecked = SanicModeEnabledDefault;
            }
            else
            {
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
            if (!int.TryParse(seedInput.Text, out Settings.seed))
            {
                Debug.WriteLine("Seed entered was not valid, set to default 0.");
            }

            Settings.difficultyName = (string)difficultyGrid.Children.OfType<RadioButton>().First(r => (bool)r.IsChecked).Content;

            if (!Settings.difficultiesArray.TryGetValue(Settings.difficultyName, out Settings.difficulty))
            {
                Debug.WriteLine("Invalid difficulty selected, set to default.");
                Settings.difficulty = Settings.defaultDifficulty;
            }

            Settings.permanentEffectsEnabled = (Settings.seed == 0);
            Settings.staticEffectsEnabled = (bool)staticEffectsEnabled.IsChecked;
            Settings.timedEffectsEnabled = (bool)timedEffectsEnabled.IsChecked;
            Settings.sanicModeEnabled = (bool)sanicModeEnabled.IsChecked;

            Debug.WriteLine($"Seed: {Settings.seed}");
            Debug.WriteLine($"Difficulty: {Settings.difficultyName}");
            Debug.WriteLine($"Static Effects Enabled: {Settings.staticEffectsEnabled}");
            Debug.WriteLine($"Permanent Effects Enabled: {Settings.permanentEffectsEnabled}");
            Debug.WriteLine($"Timed Effects Enabled: {Settings.timedEffectsEnabled}");
            Debug.WriteLine($"Sanic Mode Enabled: {Settings.sanicModeEnabled}");

            Closed -= new EventHandler(WelcomeWindow_Closed);
            Close();
        }
    }
}
