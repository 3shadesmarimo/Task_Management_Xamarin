using Project.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Project.Models;

namespace Project
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();


        }



        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            var email = txtUsername.Text;
            var password = txtPassword.Text;

            var result = await SendLoginRequestAsync(email, password);

            if (result != null)
            {
                // Handle successful login here
                await Navigation.PushAsync(new UserPage(email));
            }
            else
            {
                // Handle failed login here
                await DisplayAlert("Login Failed", "Invalid email or password, please try again.", "OK");
            }
        }
        

        private async Task<Dictionary<string, object>> SendLoginRequestAsync(string email, string password)
        {
            var httpClient = new HttpClient();

            var loginData = new Dictionary<string, string>
            {
                { "email", email },
                { "password", password }
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/users/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                if (responseDict.TryGetValue("token", out object tokenValue))
                {
                    Application.Current.Properties["token"] = tokenValue.ToString();
                    await Application.Current.SavePropertiesAsync(); // Save the token persistently
                }
                //lblResponse.Text = responseDict.ToString();
                return responseDict;
            }

            return null;
        }
    }
}
