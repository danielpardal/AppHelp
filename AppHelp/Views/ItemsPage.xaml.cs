﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//using AppHelp.Models;
using AppHelp.Views;
//using AppHelp.ViewModels;

using Xamarin.Essentials;
using System.Threading.Tasks;
using Plugin.Messaging;

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
            //BindingContext = viewModel = new ItemsViewModel();
        }

        async void btnSendSms_Clicked(object sender, System.EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("TxtNumberTel"))
            {
                var txtNumber = Application.Current.Properties["TxtNumberTel"].ToString();
                var txtMessage = "";
                
                if (Application.Current.Properties.ContainsKey("TxtMessage"))
                    txtMessage = Application.Current.Properties["TxtMessage"].ToString();
                
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
                        smsMessenger.SendSmsInBackground(txtNumber, txtMessage + locationTxt);
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}