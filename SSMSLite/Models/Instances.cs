using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public sealed class Instances
    {
        public static event EventHandler<InstanceEventArgs> InstanceAdded;
        public static event EventHandler<InstanceEventArgs> InstanceRemoved;

        static List<Instance> items;

        public static List<Instance> Items
        {
            get
            {
                if (items == null) items = new List<Instance>();
                return items;
            }
        }

        public static void Add(Instance instance)
        {
            if (instance.CheckConnection())
            {
                Items.Add(instance);
                InstanceAdded?.Invoke(null, new InstanceEventArgs(instance));
            }
            else
            {
                throw new Exception($"Połączenie z {instance.Name} nie jest możliwe");
            }
        }

        public static void Remove(Instance instance)
        {
            Items.Remove(instance);
            InstanceRemoved?.Invoke(null, new InstanceEventArgs(instance));
        }
    }

    public class InstanceEventArgs : EventArgs
    {
        public Instance Instance { get; set; }

        public InstanceEventArgs(Instance instance)
        {
            Instance = instance;
        }
    }
}
