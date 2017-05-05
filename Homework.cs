using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuddly_chainsaw.Models
{
    class Homework
    {
        private string HomeworkId;
        private string title;
        private string description;
        DateTimeOffset deadline;

        public Homework(string Title, string Description, DateTimeOffset Deadline)
        {
            title = Title;
            description = Description;
            deadline = Deadline;
            HomeworkId = Guid.NewGuid().ToString();
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
            return HomeworkId;
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
