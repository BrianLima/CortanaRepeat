using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace RepeatforCortana
{
    public partial class TutorialPage : PhoneApplicationPage
    {
        public TutorialPage()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WebBrowserTask task = new WebBrowserTask();
            task.Uri = new System.Uri("http://www.windowsphone.com/en-us/how-to/wp8/cortana/start-using-cortana", System.UriKind.RelativeOrAbsolute);
            task.Show();
        }
    }
}