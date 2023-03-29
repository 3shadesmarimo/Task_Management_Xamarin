using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Controllers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        private async void btnCreate_Clicked(object sender, EventArgs e)
        {
            var email = txtEmail.Text;
            var password = txtPassword.Text;
            var name = txtName.Text;

            var result = await SendCreateRequestAsync(email, password, name);

            if (result != null)
            {
                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Sign Up Failed", "Please Try Again", "Ok");
            }

        }

        public async Task<Dictionary<string, object>> SendCreateRequestAsync(string email, string password, string name)
        {
            var httpClient = new HttpClient();

            var signUpData = new Dictionary<string, string>
            {
                { "email", email },
                { "password", password },
                { "name", name }

            };

            var content = new StringContent(JsonConvert.SerializeObject(signUpData), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/users/signup", content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                return responseDict;
            }

            return null;

        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}