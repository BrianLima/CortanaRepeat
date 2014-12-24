using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Windows.Phone.Speech.Synthesis;
using Windows.Phone.Speech.VoiceCommands;
using Windows.Phone.Speech.Recognition;

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
        }

        void appBarMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CortanaOverlay(string message)
        {
            CortanaOverlayData data = new CortanaOverlayData("I heard you say:", message);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
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
        }

        private void HandleVoiceCommand(string voiceCommandName)
        {
            string result = null;
            NavigationContext.QueryString.TryGetValue("naturalLanguage", out result);

            speech(result);
        }

        private async void speech(string text)
        {
            try
            {
                if (!String.IsNullOrEmpty(text) && text != "...")
                {
                    SpeechSynthesizer talk = new SpeechSynthesizer();
                    await talk.SpeakTextAsync(text);
                    CortanaOverlay(text);
                }
                else
                {
                    CortanaOverlay(text);
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
