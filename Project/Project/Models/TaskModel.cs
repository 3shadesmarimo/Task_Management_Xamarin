using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Models
{
    public class TaskModel
    {
        [JsonProperty("taskUid")]
        public string TaskUid { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("createdByUid")]
        public string CreatedByUid { get; set; }

        [JsonProperty("createdByName")]
        public string CreatedByName { get; set; }

        [JsonProperty("assignedToUid")]
        public string AssignedToUid { get; set; }

        [JsonProperty("assignedToName")]
        public string AssignedToName { get; set; }

        [JsonProperty("done")]
        public bool Done { get; set; }
    }
}
