using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Windows.Phone.Speech.Synthesis;
using Windows.Phone.Speech.VoiceCommands;

namespace RepeatforCortana
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            BuildLocalizedApplicationBar();
        }

        private void BuildLocalizedApplicationBar()
        {
            // Set the page's ApplicationBar to a new instance of ApplicationBar.
            ApplicationBar = new ApplicationBar();

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem("help");
            ApplicationBar.MenuItems.Add(appBarMenuItem);
            appBarMenuItem.Click += appBarMenuItem_Click;

#if DEBUG
            ApplicationBarMenuItem appBarMenuTest = new ApplicationBarMenuItem("Test");
            ApplicationBar.MenuItems.Add(appBarMenuTest);
            appBarMenuTest.Click += appBarMenuTest_Click;
#endif
        }

        void appBarMenuTest_Click(object sender, EventArgs e)
        {
            CortanaOverlay("I am repeating what you said");
        }

        void appBarMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/TutorialPage.xaml?parameter=" + 0, UriKind.RelativeOrAbsolute));
        }

        private void CortanaOverlay(string message)
        {
            CortanaOverlayData data = new CortanaOverlayData("I heard you say:", message);
            CustomMessageBox CortanaOverlay = new CustomMessageBox()
            {
                ContentTemplate = (DataTemplate)this.Resources["CortanaOverlay"],
                LeftButtonContent = "Ok",
                RightButtonContent = "Cancel",
                IsFullScreen = false,
                Content = data
            };
            speech(message);
            CortanaOverlay.Show();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string voiceCommandName;

            if (NavigationContext.QueryString.TryGetValue("voiceCommandName", out voiceCommandName))
            {
                HandleVoiceCommand(voiceCommandName);
            }
            else
            {
                Task.Run(() => InstallVoiceCommands());
            }
        }

        private void HandleVoiceCommand(string voiceCommandName)
        {
            string result = null;
            NavigationContext.QueryString.TryGetValue("naturalLanguage", out result);

            CortanaOverlay(result);
        }

        private async void speech(string text)
        {
            try
            {
                if (!String.IsNullOrEmpty(text) && text != "...")
                {
                    SpeechSynthesizer talk = new SpeechSynthesizer();
                    await talk.SpeakTextAsync(text);
                }
            }
            catch (Exception exception)
            {
                throw new Exception("Error when trying to use TTS", exception);
            }
        }

        private async void InstallVoiceCommands()
        {
            const string VcdPath = "ms-appx:///VoiceDefinition.xml";
            try
            {
                Uri vcdUri = new Uri(VcdPath);
                await VoiceCommandService.InstallCommandSetsFromFileAsync(vcdUri);
            }
            catch (Exception vcdEx)
            {
                Debug.WriteLine(vcdEx.ToString());
            }
        }
    }
}
