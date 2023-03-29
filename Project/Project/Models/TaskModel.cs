using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Models
{
    public class TaskModel
    {
        public TaskModel(string taskUid, string assignedToName, string assignedToUid, string createdByName, string createdByUid, string description, bool done)
        {
            this.taskUid = taskUid;
            this.assignedToName = assignedToName;
            this.assignedToUid = assignedToUid;
            this.createdByName = createdByName;
            this.createdByUid = createdByUid;
            this.description = description;
            this.done = done;
        }

        public string taskUid { get; set; }

        public string assignedToName { get; set; }
        
        public string assignedToUid { get; set; }

        public string createdByName { get; set; }

        public string createdByUid { get; set; }

        public string description { get; set; }

        public bool done { get; set; }
    }
}
