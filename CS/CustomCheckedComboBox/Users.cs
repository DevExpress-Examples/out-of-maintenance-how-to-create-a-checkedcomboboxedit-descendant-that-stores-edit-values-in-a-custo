using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CustomCheckedComboBox
{
    public class User
    {
        string name;
        string lang;
        public User(string name)
        {
            this.name = name;
            this.lang = "";
        }
        public string Lang
        {
            get { return lang; }
            set { lang = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }

        }
    }

    public class UsersList : ArrayList
    {
        public User GetUser(int index)
        { return this[index]; }

        public new User this[int index]
        {
            get { return base[index] as User; }
        }
    }
}
