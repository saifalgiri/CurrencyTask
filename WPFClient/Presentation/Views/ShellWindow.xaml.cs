using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Net.Http;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFClient.Applications.Views;

namespace WPFClient.Presentation.Views
{
    [Export(typeof(IShellView))]
    public partial class ShellWindow : Window, IShellView
    {
        string ServiceURL = ConfigurationManager.AppSettings["ServiceURL"].ToString();
        public ShellWindow()
        {
            InitializeComponent();
        }


        private async void Convert_button_Click(object sender, RoutedEventArgs e)
        {
            string message = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ServiceURL);
                    HttpResponseMessage response = client.GetAsync("GetData" + $"/{amount_input.Text}").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        message = await response.Content.ReadAsStringAsync();
                        text_result.Text = message.Trim('"');
                        text_error.Foreground = Brushes.Black;
                        text_result.Visibility = Visibility.Visible;
                        text_error.Visibility = Visibility.Hidden;
                    }
                    if (!response.IsSuccessStatusCode)
                    {
                        message = await response.Content.ReadAsStringAsync();
                        text_error.Text = message.Trim('"');
                        text_error.Foreground = Brushes.Red;
                        text_error.Visibility = Visibility.Visible;
                        text_result.Visibility = Visibility.Hidden;
                    }

                }

            }
            catch (FaultException ex)
            {
                text_result.Text = ex.Message.ToString();
                text_error.Foreground = Brushes.Red;
                text_error.Visibility = Visibility.Visible;
                text_result.Visibility = Visibility.Hidden;
            }

        }

        private void Amount_input_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^([0-9]{1,9})(?:(,)\d{0,2})?$");

            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }
    }
}
