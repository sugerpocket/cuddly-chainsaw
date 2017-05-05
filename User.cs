using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cuddly_chainsaw.Models
{
    class User
    {
        private string username;
        private string password;
        private string id;
        private string nickname;
        private Boolean isTeacher;

        public User(string Username, string Password, string Nickname, Boolean IsTeacher)
        {
            username = Username;
            password = Password;
            nickname = Nickname;
            isTeacher = IsTeacher;
            id = Guid.NewGuid().ToString();//不知道要不要id并且不知道跟Homework一样的生成方式可不可以
        }
        
        public string getUsername()
        {
            return username;
        }

        public string getPassword()
        {
            return password;
        }

        public string getId()
        {
            return id;
        }

        public string getNickname()
        {
            return nickname;
        }

        public Boolean getIsTeacher()
        {
            return isTeacher;
        }

        public void setNickname(string Nickname)
        {
            nickname = Nickname;
        }

        public void setPassword(string Password)
        {
            password = Password;
        }
    }
}
