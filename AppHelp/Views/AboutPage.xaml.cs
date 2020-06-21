using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Plugin.Messaging;

namespace AppHelp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        public String locationTxt = "";
        public AboutPage()
        {
            LocationCache();
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey("TxtNumberTel"))
                this.txtNumber.Text = Application.Current.Properties["TxtNumberTel"].ToString();

            if (Application.Current.Properties.ContainsKey("TxtMessage"))
                this.txtMessage.Text = Application.Current.Properties["TxtMessage"].ToString();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        async void LocationCache()
        {
            //var locationTxt = "";
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);
                //var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    locationTxt = $"https://www.google.com/maps/@?api=1&map_action=map&center= {location.Latitude},{ location.Longitude}";
                    //locationTxt = location.ToString();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                locationTxt = "sem Loc 1";
                // Handle not supported on device exception
                //await DisplayAlert("Erro", "Não tem permissão para localização", "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                locationTxt = "sem Loc 2";
                // Handle not enabled on device exception
                //await DisplayAlert("Failed", "Não tem permissão para localização", "OK");
            }
            catch (PermissionException pEx)
            {
                locationTxt = "sem Loc 3";
                // Handle permission exception
                //await DisplayAlert("Failed", "Não tem permissão para localização", "OK");
            }
            catch (Exception ex)
            {
                locationTxt = "sem Loc 4";
                // Unable to get location
                //await DisplayAlert("Failed", "Não tem permissão para localização", "OK");
            }
        }
        async void btnSave_Clicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNumber.Text))
            {
                if (Application.Current.Properties.ContainsKey("TxtNumberTel"))
                    Application.Current.Properties["TxtNumberTel"] = txtNumber.Text;
                else
                    Application.Current.Properties.Add("TxtNumberTel", txtNumber.Text);
            }

            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                if (Application.Current.Properties.ContainsKey("TxtMessage"))
                    Application.Current.Properties["TxtMessage"] = txtMessage.Text;
                else
                    Application.Current.Properties.Add("TxtMessage", txtMessage.Text);
            }

            await DisplayAlert("Sucesso", "Configuração salva.", "OK");

        }

        public async Task SendSms(string messageText, string recipient)
        {
            /*try
            {
                var message = new SmsMessage(messageText, recipient);
                await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                await DisplayAlert("Failed", "Sms is not supported on this device.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failed", ex.Message, "OK");
            }*/
        }
    }
}