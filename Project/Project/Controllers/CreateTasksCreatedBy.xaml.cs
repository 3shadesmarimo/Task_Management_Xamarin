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
    public partial class CreateTasksCreatedBy : ContentPage
    {
        public ObservableCollection<TaskModel> TasksCreatedBy { get; set; } = new ObservableCollection<TaskModel>();
        public TaskModel SelectedTask { get; set; }


        public CreateTasksCreatedBy()
        {
            InitializeComponent();
            TasksCreatedByListView.ItemsSource = TasksCreatedBy;
            LoadTasksCreatedByAsync();
        }

        public class ApiResponse
        {
            [JsonProperty("allTasks")]
            public List<TaskModel> AllTasks { get; set; }
        }

        private async void LoadTasksCreatedByAsync()
        {
            string endpoint = "https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks/createdby";

            if (Application.Current.Properties.TryGetValue("token", out object tokenValue))
            {
                string token = tokenValue.ToString();

                using (var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true}))
                {
                    httpClient.DefaultRequestHeaders.Add("x-access-token", token);
                    var response = await httpClient.GetAsync(endpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);
                        var allTasks = apiResponse.AllTasks;

                        foreach (var task in allTasks)
                        {
                            TasksCreatedBy.Add(task);
                        }

                    }
                }

            }
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            if (SelectedTask != null)
            {
                bool success = await DeleteTaskAsync(SelectedTask.TaskUid);
                if (success)
                {
                    TasksCreatedBy.Remove(SelectedTask);
                    await DisplayAlert("Success", "Task deleted successfully!", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Please select a task to delete.", "OK");
            }
        }

        private void TasksCreatedByListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                SelectedTask = e.SelectedItem as TaskModel;
            }
        }


        private async Task<bool> DeleteTaskAsync(string taskUid)
        {
            string endpoint = $"https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks/{taskUid}";

            if (Application.Current.Properties.TryGetValue("token", out object tokenValue))
            {
                string token = tokenValue.ToString();

                using (var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true }))
                {
                    httpClient.DefaultRequestHeaders.Add("x-access-token", token);
                    var response = await httpClient.DeleteAsync(endpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        await DisplayAlert("Delete Task Failed", "Failed to delete the task, please try again.", "OK");
                        return false;
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "Token not found, please log in again.", "OK");
                return false;
            }
        }

    }
}