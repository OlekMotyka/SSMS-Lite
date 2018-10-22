using System;
using System.Collections.Generic;
using System.Linq;

namespace SSMSLite
{
    public class Folder
    {
        public Folder Parent { get; set; }
        public List<Folder> Objects { get; set; }

        public string Name { get; set; }

        public Folder(string name, Folder parent = null)
        {
            Name = name;
            Objects = new List<Folder>();
            Parent = parent;
        }

        public Folder getFolder(string name)
        {
            return Objects.FirstOrDefault(x => x.Name == name);
        }

        public Type ObjectsType { get => Objects.FirstOrDefault().GetType(); }


        public virtual void LoadData(Folder folder)
        {
            if (folder.Parent != null)
                folder.Parent.LoadData(folder);
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class DbObject : Folder
    {
        public Database Database { get; set; }
        public string Schema { get; set; }

        public DbObject(DbObject parent, string name) : base(name, parent)
        {
            Parent = parent;
            Database = parent?.Database;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Schema))
                return Name;
            else
                return Schema + "." + Name;
        }
    }
}
