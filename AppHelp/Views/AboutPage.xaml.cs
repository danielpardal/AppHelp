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
            //LocationCache();
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("bMsgOpen"))
                this.bMsgOpen.IsToggled = Convert.ToBoolean(Application.Current.Properties["bMsgOpen"]);

            if (Application.Current.Properties.ContainsKey("TxtNumberTel"))
                this.txtNumberTel1.Text = Application.Current.Properties["TxtNumberTel"].ToString();

            if (Application.Current.Properties.ContainsKey("TxtMessage"))
                this.txtMessage1.Text = Application.Current.Properties["TxtMessage"].ToString();

            if (Application.Current.Properties.ContainsKey("TxtEmail"))
                this.txtEmail1.Text = Application.Current.Properties["TxtEmail"].ToString();

            if (Application.Current.Properties.ContainsKey("TxtName"))
                this.txtName1.Text = Application.Current.Properties["TxtName"].ToString();

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
        async void Save_Clicked(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtNumberTel1.Text))
            {
                if (Application.Current.Properties.ContainsKey("TxtNumberTel"))
                    Application.Current.Properties["TxtNumberTel"] = this.txtNumberTel1.Text;
                else
                    Application.Current.Properties.Add("TxtNumberTel", this.txtNumberTel1.Text);
            }
            else
            {
                await DisplayAlert("Failed", "Número de Telefone inválido. Informação obrigatória. Não será possível solicitar ajuda.", "OK");
                return;
            }

            if (Application.Current.Properties.ContainsKey("BMailAuto"))
                Application.Current.Properties["BMailAuto"] = this.bMailAuto.IsToggled;
            else
                Application.Current.Properties.Add("BMailAuto", this.bMailAuto.IsToggled);

            if (!string.IsNullOrEmpty(txtUserEmailSmtp.Text))
            {
                if (Application.Current.Properties.ContainsKey("TxtUserEmailSmtp"))
                    Application.Current.Properties["TxtUserEmailSmtp"] = this.txtUserEmailSmtp.Text;
                else
                    Application.Current.Properties.Add("TxtUserEmailSmtp", this.txtUserEmailSmtp.Text);
            }

            if (!string.IsNullOrEmpty(txtPassEmailSmtp.Text))
            {
                if (Application.Current.Properties.ContainsKey("TxtPassEmailSmtp1"))
                    Application.Current.Properties["TxtPassEmailSmtp1"] = this.txtPassEmailSmtp.Text;
                else
                    Application.Current.Properties.Add("TxtPassEmailSmtp1", this.txtPassEmailSmtp.Text);
            }

            if (Application.Current.Properties.ContainsKey("bMsgOpen"))
                Application.Current.Properties["bMsgOpen"] = this.bMsgOpen.IsToggled;
            else
                Application.Current.Properties.Add("bMsgOpen", this.bMsgOpen.IsToggled);

            if (!string.IsNullOrEmpty(txtName1.Text))
            {
                if (Application.Current.Properties.ContainsKey("TxtName"))
                    Application.Current.Properties["TxtName"] = this.txtName1.Text;
                else
                    Application.Current.Properties.Add("TxtName", this.txtName1.Text);
            }

            if (!string.IsNullOrEmpty(txtMessage1.Text))
            {
                if (Application.Current.Properties.ContainsKey("TxtMessage"))
                    Application.Current.Properties["TxtMessage"] = this.txtMessage1.Text;
                else
                    Application.Current.Properties.Add("TxtMessage", this.txtMessage1.Text);
            }

            if (!string.IsNullOrEmpty(this.txtEmail1.Text))
            {
                if (Application.Current.Properties.ContainsKey("TxtEmail"))
                    Application.Current.Properties["TxtEmail"] = this.txtEmail1.Text;
                else
                    Application.Current.Properties.Add("TxtEmail", this.txtEmail1.Text);
            }

            await DisplayAlert("Sucesso", "Configuração salva.", "OK");

        }

    }
}