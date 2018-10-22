using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SSMSLite
{
    public class Connection
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public bool SqlAuth { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Encrypted { get; set; }

        public Connection()
        {

        }

        public Instance getInstance()
        {
            return new Instance(Name, Server, SqlAuth, Username, Password, Encrypted);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
