using Newtonsoft.Json;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Controllers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksAssignedPage : ContentPage
    {
        public ObservableCollection<TaskModel> TasksAssignedBy { get; set; } = new ObservableCollection<TaskModel>();

        public TaskModel SelectedTasksAssigned { get; set; }
        private TaskModel selectedTask;
        private static readonly HttpMethod PatchMethod = new HttpMethod("PATCH");

        public TasksAssignedPage()
        {
            InitializeComponent();
            TaskAssignedListView.ItemsSource = TasksAssignedBy;
            LoadTasksAssignedByAsync();
        }

        public class ApiResponse
        {
            [JsonProperty("allTasks")]
            public List<TaskModel> AllTasks { get; set; }
        }

        private async void LoadTasksAssignedByAsync()
        {
            string endpont = "https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks/assignedto";

            if(Application.Current.Properties.TryGetValue("token", out object tokenvalue))
            {
                string token = tokenvalue.ToString();

                using(var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true }))
                {
                    httpClient.DefaultRequestHeaders.Add("x-access-token", token);
                    var response = await httpClient.GetAsync(endpont);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(content);
                        var allTasks = apiResponse.AllTasks;

                        foreach (var task in allTasks )
                        {
                            TasksAssignedBy.Add(task);
                        }
                    }
                }
            }
        }

        private void TaskAssignedListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem!= null)
            {
                SelectedTasksAssigned = e.SelectedItem as TaskModel;
            }
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var switchControl = sender as Xamarin.Forms.Switch;
            selectedTask = switchControl.BindingContext as TaskModel;
            selectedTask.Done = e.Value;
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            if (selectedTask != null)
            {
                bool done = selectedTask.Done;
                bool success = await UpdateTaskStatusAsync(selectedTask.TaskUid, done);

                if (success)
                {
                    await DisplayAlert("Success", "Task status updated successfully!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to update task status.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "No task selected.", "OK");
            }
        }

        private async Task<bool> UpdateTaskStatusAsync(string taskUid, bool done)
        {
            string endpoint = $"https://taskmanager-project-fall2022-zmoya.ondigitalocean.app/v1/tasks/{taskUid}";

            if (Application.Current.Properties.TryGetValue("token", out object tokenValue))
            {
                string token = tokenValue.ToString();

                using (var httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = true }))
                {
                    httpClient.DefaultRequestHeaders.Add("x-access-token", token);

                    var taskData = new Dictionary<string, bool>
                    {
                        { "done", done }
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(taskData), Encoding.UTF8, "application/json");
                    var request = new HttpRequestMessage(PatchMethod, endpoint)
                    {
                        Content = content
                    };

                    var response = await httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine($"Failed to update task status. StatusCode: {response.StatusCode}, Error: {errorContent}");
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