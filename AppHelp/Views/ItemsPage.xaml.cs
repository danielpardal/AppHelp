using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Mail;

//using AppHelp.Models;
using AppHelp.Views;
//using AppHelp.ViewModels;

using Xamarin.Essentials;
using System.Threading.Tasks;
using Plugin.Messaging;
using Plugin.Permissions;

namespace AppHelp.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        //ItemsViewModel viewModel;
        public String locationTxt = "";

        public ItemsPage()
        {
            InitializeComponent();
            if (!Application.Current.Properties.ContainsKey("TxtNumberTel"))
            {
                DisplayAlert("ATENÇÃO", "Cadastre um número de telefone válido para solicitar ajuda.", "OK");
                Navigation.PushAsync(new AboutPage(), true);
            }
            else if (Application.Current.Properties.ContainsKey("bMsgOpen"))
            {
                if(Convert.ToBoolean(Application.Current.Properties["bMsgOpen"]))
                    EnviaSMSAuto();
            }
            //BindingContext = viewModel = new ItemsViewModel();
        }

        async void EnviaSMSAuto()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var dateNow = DateTime.Now;
                    var lat = location.Latitude.ToString().Replace(",", ".");
                    var lon = location.Longitude.ToString().Replace(",", ".");
                    locationTxt = $" Local: https://www.google.com/maps/?q={lat},{lon}, Altitude: { location.Altitude}, Data: { dateNow }";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro", "Não tem suporte a localização do dispositivo", "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Failed", "Não habilitada a localização do dispositivo", "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Failed", "Não tem permissão para acessar a localização do dispositivo", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failed", "Não foi possível acessar a localização do dispositivo.", "OK");
            }

            var smsMessenger = CrossMessaging.Current.SmsMessenger;

            if (smsMessenger.CanSendSmsInBackground)
            {
                try
                {
                    var txtNumberAux = Application.Current.Properties["TxtNumberTel"].ToString();
                    var txtMessageAux = "";

                    if (String.IsNullOrWhiteSpace(txtNumberAux))
                    {
                        await DisplayAlert("Erro", "Não foi possível realizar o envio. Número de Telefone inválido.", "OK");
                        return;
                    }

                    if (Application.Current.Properties.ContainsKey("TxtMessage"))
                        txtMessageAux = Application.Current.Properties["TxtMessage"].ToString();

                    //smsMessenger.SendSmsInBackground(Number.Text, Message.Text);
                    smsMessenger.SendSmsInBackground(txtNumberAux, txtMessageAux + locationTxt);
                    //await DisplayAlert("Sucesso", "Mensagem enviada.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Failed", "Falha no envio da mensagem.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Failed", "O dispositivo não pode enviar a mensagem.", "OK");
            }
            
        }

        async void SendSms_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<SmsPermission>();
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    status = await CrossPermissions.Current.RequestPermissionAsync<SmsPermission>();
                }

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Denied)
                {
                    await DisplayAlert("Erro", "A mensagem não poderá ser enviada sem permissão.", "OK");
                    return;
                } 
                else if(status == Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                {
                    await DisplayAlert("Erro", "A mensagem não poderá ser enviada sem permissão.", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Houve um problema com a solicitação. Tente novamente.", "OK");
                return;
                //Something went wrong
            }

            if (Application.Current.Properties.ContainsKey("TxtNumberTel"))
            {
                var txtNumberAux = Application.Current.Properties["TxtNumberTel"].ToString();
                var txtMessageAux = "";

                if (String.IsNullOrWhiteSpace(txtNumberAux))
                {
                    await DisplayAlert("Erro", "Não foi possível realizar o envio. Número de Telefone inválido.", "OK");
                    return;
                }

                if (Application.Current.Properties.ContainsKey("TxtMessage"))
                    txtMessageAux = Application.Current.Properties["TxtMessage"].ToString();
                
                try
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                    var location = await Geolocation.GetLocationAsync(request);
                    
                    if (location != null)
                    {
                        var dateNow = DateTime.Now;
                        var lat = location.Latitude.ToString().Replace(",", ".");
                        var lon = location.Longitude.ToString().Replace(",", ".");
                        locationTxt = $" Local: https://www.google.com/maps/?q={lat},{lon}, Altitude: { location.Altitude}, Data: { dateNow }";
                    }
                }
                catch (FeatureNotSupportedException fnsEx)
                {
                    await DisplayAlert("Erro", "Não tem suporte a localização do dispositivo", "OK");
                }
                catch (FeatureNotEnabledException fneEx)
                {
                    await DisplayAlert("Failed", "Não habilitada a localização do dispositivo", "OK");
                }
                catch (PermissionException pEx)
                {
                    await DisplayAlert("Failed", "Não tem permissão para acessar a localização do dispositivo", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Failed", "Não foi possível acessar a localização do dispositivo.", "OK");
                }

                var smsMessenger = CrossMessaging.Current.SmsMessenger;

                if (smsMessenger.CanSendSmsInBackground)
                {
                    try
                    {
                        //smsMessenger.SendSmsInBackground(Number.Text, Message.Text);
                        smsMessenger.SendSmsInBackground(txtNumberAux, txtMessageAux + locationTxt);
                        //await DisplayAlert("Sucesso", "Mensagem enviada.", "OK");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Failed", "Falha no envio da mensagem.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Failed", "O dispositivo não pode enviar a mensagem.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Erro", "Não há telefone cadastrado para o envio da mensagem.", "OK");
            }
        }

        async void Call(object sender, EventArgs e)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync<PhonePermission>();
                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    status = await CrossPermissions.Current.RequestPermissionAsync<PhonePermission>();
                }

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Denied)
                {
                    await DisplayAlert("Erro", "A ligação não poderá ser realizada sem permissão.", "OK");
                    return;
                }
                else if (status == Plugin.Permissions.Abstractions.PermissionStatus.Unknown)
                {
                    await DisplayAlert("Erro", "A ligação não poderá ser realizada sem permissão.", "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Houve um problema com a solicitação. Tente novamente.", "OK");
                //Something went wrong
            }
            var phoneDialer = CrossMessaging.Current.PhoneDialer;
            var txtNumberAux = "";
            if (Application.Current.Properties.ContainsKey("TxtNumberTel"))
                txtNumberAux = Application.Current.Properties["TxtNumberTel"].ToString();
            
            if (String.IsNullOrWhiteSpace(txtNumberAux))
            {
                await DisplayAlert("Erro", "Não foi possível realizar a ligação. Número de Telefone inválido.", "OK");
                return;
            }

            if (phoneDialer.CanMakePhoneCall && !String.IsNullOrWhiteSpace(txtNumberAux))
            {
                try
                {
                   phoneDialer.MakePhoneCall(txtNumberAux);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Failed", "Não foi possível realizar a ligação.", "OK");
                }

            }
            else
            {
                await DisplayAlert("Erro", "Não foi possível realizar a ligação. Verificar permissão de chamada do aplicativo.", "OK");
            }
        }

        async void Wathsapp(object sender, EventArgs e)
        {
            var txtNumberAux = Application.Current.Properties["TxtNumberTel"].ToString();
            var txtMessageAux = "";

            if (String.IsNullOrWhiteSpace(txtNumberAux))
            {
                await DisplayAlert("Erro", "Não foi possível enviar a mensagem. Número de Telefone inválido.", "OK");
                return;
            }
        
            if (Application.Current.Properties.ContainsKey("TxtMessage"))
                txtMessageAux = Application.Current.Properties["TxtMessage"].ToString();

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var dateNow = DateTime.Now;
                    var lat = location.Latitude.ToString().Replace(",", ".");
                    var lon = location.Longitude.ToString().Replace(",", ".");
                    locationTxt = $" Local: https://www.google.com/maps/?q={lat},{lon}, Altitude: { location.Altitude}, Data: { dateNow }";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro", "Não há suporte a localização do dispositivo", "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Failed", "Não está habilitada a localização do dispositivo", "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Failed", "Não tem permissão para acessar a localização do dispositivo", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failed", "Não foi possível acessar a localização do dispositivo.", "OK");
            }

            txtMessageAux = txtMessageAux + locationTxt;
            var txtNumberFmt = "+55" + txtNumberAux;
            
            try
            {
                Device.OpenUri(new Uri(String.Format("https://wa.me/{0}?text={1}", txtNumberFmt, txtMessageAux)));
            }                
            catch (Exception ex)
            {
                await DisplayAlert("Failed", "Não foi possível enviar a mensagem.", "OK");
            }
        }

        async void SendEmail(object sender, EventArgs e)
        {
            var txtMessageAux = "";

            if (!Application.Current.Properties.ContainsKey("TxtEmail"))
            {
                await DisplayAlert("Failed", "Não há endereço de email cadastrado.", "OK");
                return;
            }

            if (Application.Current.Properties.ContainsKey("TxtMessage"))
                txtMessageAux = Application.Current.Properties["TxtMessage"].ToString();

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var dateNow = DateTime.Now;
                    var lat = location.Latitude.ToString().Replace(",", ".");
                    var lon = location.Longitude.ToString().Replace(",", ".");
                    locationTxt = $" Local: https://www.google.com/maps/?q={lat},{lon}, Altitude: { location.Altitude}, Data: { dateNow }";
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro", "Não há suporte a localização do dispositivo", "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Failed", "Não está habilitada a localização do dispositivo", "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Failed", "Não tem permissão para acessar a localização do dispositivo", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failed", "Não foi possível acessar a localização do dispositivo.", "OK");
            }

            if ((Application.Current.Properties.ContainsKey("BMailAuto")) && (Convert.ToBoolean(Application.Current.Properties["BMailAuto"])))
            {
                var txtUserEmailSmtpAux = "";
                var txtPassEmailSmtpAux = "";

                var txtEmailAux = "";

                if (Application.Current.Properties.ContainsKey("TxtEmail"))
                    txtEmailAux = Application.Current.Properties["TxtEmail"].ToString();

                if (Application.Current.Properties.ContainsKey("TxtUserEmailSmtp"))
                {
                    txtUserEmailSmtpAux = Application.Current.Properties["TxtUserEmailSmtpAux"].ToString();
                    if (String.IsNullOrWhiteSpace(txtUserEmailSmtpAux))
                    {
                        await DisplayAlert("Erro", "Não há email gmail SMTP cadastrado.", "OK");
                        return;
                    }
                }
                else
                {
                    await DisplayAlert("Failed", "Não há email gmail SMTP cadastrado.", "OK");
                    return;
                }

                if (Application.Current.Properties.ContainsKey("TxtPassEmailSmtpAux"))
                {
                    txtPassEmailSmtpAux = Application.Current.Properties["TxtPassEmailSmtpAux"].ToString();
                    if (String.IsNullOrWhiteSpace(txtPassEmailSmtpAux))
                    {
                        await DisplayAlert("Erro", "Não há senha de email gmail SMTP cadastrado.", "OK");
                        return;
                    }
                }
                else
                {
                    await DisplayAlert("Failed", "Não há senha de email gmail SMTP cadastrado.", "OK");
                    return;
                }

                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress(txtUserEmailSmtpAux);
                    mail.To.Add(txtEmailAux);
                    mail.Subject = txtMessageAux;
                    mail.Body = locationTxt;

                    SmtpServer.Port = 587;
                    SmtpServer.Host = "smtp.gmail.com";
                    SmtpServer.EnableSsl = true;
                    SmtpServer.UseDefaultCredentials = false;
                    SmtpServer.Credentials = new System.Net.NetworkCredential(txtUserEmailSmtpAux, txtPassEmailSmtpAux);

                    SmtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Failed", "Não foi possível enviar a mensagem por email. " + ex.Message, "OK");
                }
            }
            else
            {
                try
                {

                    List<string> txtEmail = new List<string>();
                    if (Application.Current.Properties.ContainsKey("TxtEmail"))
                        txtEmail.Add(Application.Current.Properties["TxtEmail"].ToString());

                    var message = new EmailMessage
                    {
                        Subject = txtMessageAux,
                        Body = locationTxt,
                        To = txtEmail,
                        //Cc = ccRecipients,
                        //Bcc = bccRecipients
                    };
                    await Email.ComposeAsync(message);
                }
                catch (FeatureNotSupportedException fbsEx)
                {
                    await DisplayAlert("Failed", "Não há suporte a função no dispositivo. " + fbsEx.Message, "OK");
                    // Email is not supported on this device
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Failed", "Não foi possível enviar a mensagem por email. " + ex.Message, "OK");
                    // Some other exception occurred
                }
                //await DisplayAlert("Sucess", "Mensagem de ajuda enviada para o email cadastrado.", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}