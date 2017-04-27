using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace cuddly_chainsaw.Models
{
    class Assignment
    {
        [JsonProperty]
        private string aid;
        [JsonProperty]
        private string title;
        [JsonProperty]
        private string description;
        
        DateTimeOffset deadline;

        public Assignment(string Title, string Description, DateTimeOffset Deadline)
        {
            title = Title;
            description = Description;
            deadline = Deadline;
            aid = Guid.NewGuid().ToString();
        }

        public string getTitle()
        {
            return title;
        }
        public string getDescription()
        {
            return description;
        }

        public string getHomeworkId()
        {
            return aid;
        }

        public void setTitle(string newTitle)
        {
            title = newTitle;
        }

        public void setDescription(string newDescription)
        {
            description = newDescription;
        }

        public void setDeadline(DateTimeOffset newDeadline)
        {
            deadline = newDeadline;
        }
    }
}
