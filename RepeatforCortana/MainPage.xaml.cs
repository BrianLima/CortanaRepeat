using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
//using CortanaRepeat.Resources;
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
        SpeechRecognizer speechRecognizer;

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

            // Create a new button and set the text value to the localized string from AppResources.
            ApplicationBarIconButton appBarButtonMic = new ApplicationBarIconButton(new Uri("/Assets/microphone.png", UriKind.Relative));
            appBarButtonMic.Text = "Speak";
            appBarButtonMic.Click += appBarButtonMic_Click;
            //ApplicationBar.Buttons.Add(appBarButtonMic);

            // Create a new menu item with the localized string from AppResources.
            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem("about");
            ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        async void appBarButtonMic_Click(object sender, EventArgs e)
        {
            speechRecognizer = new SpeechRecognizer();
            try
            {
                using (speechRecognizer)
                {
                    SpeechRecognitionResult result =
                    await speechRecognizer.RecognizeAsync();
                    if (speechRecognizer.RecognizeAsync().Status == Windows.Foundation.AsyncStatus.Completed)
                    {
                        this.Dispatcher.BeginInvoke((Action)(() => { status.Text = "You said:" + Environment.NewLine + result.Text; }));

                    }
                    else
                    {
                        //Error
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

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
                    this.Dispatcher.BeginInvoke((Action)(() => { status.Text = "You said:" + Environment.NewLine + text; }));
                }
                else
                {
                    this.Dispatcher.BeginInvoke((Action)(() => { status.Text = "Error contacting the servers" + Environment.NewLine + "Check your internet connection and try again."; }));
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
