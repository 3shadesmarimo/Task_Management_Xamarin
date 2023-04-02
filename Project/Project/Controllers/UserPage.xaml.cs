using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Project.Models;
using RestSharp;
using System.Collections.ObjectModel;

namespace Project.Controllers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPage : ContentPage
    {
        public ObservableCollection<UserModel> Users { get; set; } = new ObservableCollection<UserModel>();
        public UserPage(string userEmail)
        {
            InitializeComponent();
            UsersListView.ItemsSource = Users;
            LoadUsersAsync();
            WelcomeLabel.Text = $"Welcome {userEmail}!";
        }

        public class ApiResponse
        {
            [JsonProperty("allUsers")]
            public List<UserModel> AllUsers { get; set; }
        }


        public async Task GetAllUsersAsync()
        {
            string endpoint = "https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/users/all";
            //string accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJlbWFpbCI6InNkYUBnbWFpbC5jb20iLCJpZCI6IjY0MjFjNjNiYjNlNDViMzY1NzYyZGU3NSIsImV4cCI6MTY4MjUyNzE0MX0.1pNBmMTHTqbo9C58AJ2Rpeau99vGtHRWd6bgxne8hDY";

            if (Application.Current.Properties.TryGetValue("token", out object tokenValue))
            {
                string token = tokenValue.ToString();
                //lbl.Text = token;
                // Use the token as needed
                using (var httpClient = new HttpClient())
                {

                    httpClient.DefaultRequestHeaders.Add("x-access-token", token);
                    var response = await httpClient.GetAsync(endpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);
                        var allUsers = apiResponse.AllUsers;
                        foreach (var user in allUsers)
                        {
                            Users.Add(user);
                        }
                    }
                }
            }


        }


        private async Task LoadUsersAsync()
        {
            await GetAllUsersAsync();
            //This is for only displaying the users in the picker
            Application.Current.Properties["users"] = Users;
        }

        private async void btnLogOut_Clicked(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("token"))
            {
                Application.Current.Properties.Remove("token");
                await Application.Current.SavePropertiesAsync();

                await Navigation.PushAsync(new MainPage());
            }

        }

        private async void btnCreate_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new CreateTaskPage(Users));

        }

        private async void btnGetTasks_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateTasksCreatedBy());
        }

        private async void btnTasksAss_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TasksAssignedPage());
        }
    }
}