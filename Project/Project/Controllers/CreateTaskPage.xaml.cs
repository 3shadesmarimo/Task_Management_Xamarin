using Newtonsoft.Json;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Controllers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateTaskPage : ContentPage
    {
        private ObservableCollection<UserModel> Users { get; set; }

        public CreateTaskPage(ObservableCollection<UserModel> users)
        {
           
            InitializeComponent();

            Users = users;

            // Load user names into the Picker
            foreach (var user in Users)
            {
                AssignedToPicker.Items.Add(user.Name);
            }


        }

        private async void CreateTaskButton_Clicked(object sender, EventArgs e)
        {
            string description = TaskTitleEntry.Text;
            if (AssignedToPicker.SelectedIndex != -1)
            {
                string assignedToUid = Users[AssignedToPicker.SelectedIndex].UId;
                bool success = await CreateTaskAsync(description, assignedToUid);
                if (success)
                {
                    // Navigate back or display a success message
                    await DisplayAlert("Success", "Task created successfully!", "OK");
                    await Navigation.PopAsync();
                }
            }
            else
            {
                await DisplayAlert("Error", "Please select a user to assign the task to.", "OK");
            }
        }

        private async Task<bool> CreateTaskAsync(string description, string assignedToUid)
        {
            string endpoint = "https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks";

            if (Application.Current.Properties.TryGetValue("token", out object tokenValue))
            {
                string token = tokenValue.ToString();

                using (var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true }))
                {
                    httpClient.DefaultRequestHeaders.Add("x-access-token", token);

                    var taskData = new Dictionary<string, string>
                    {
                        { "description", description },
                        { "assignedToUid", assignedToUid }
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(taskData), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(endpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        // Handle successful task creation, e.g., display a success message or navigate back to a previous page
                        //await DisplayAlert("Success", "Task created successfully!", "OK");
                        return true;
                    }
                    else
                    {
                        // Handle unsuccessful task creation, e.g., display an error message
                        await DisplayAlert("Create Task Failed", "Invalid input, please try again.", "OK");
                        return false;
                    }
                }
            }
            else
            {
                // Handle case when the token is not found in the application properties
                await DisplayAlert("Error", "Token not found, please log in again.", "OK");
                return false;
            }
        }


    }
}