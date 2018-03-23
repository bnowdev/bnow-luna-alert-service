using System;
using System.Collections.Generic;

namespace Alert.API.Models
{
    public partial class Company
    {
        public Company()
        {
            MonitoredDevice = new HashSet<MonitoredDevice>();
            SystemAdministator = new HashSet<SystemAdministator>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<MonitoredDevice> MonitoredDevice { get; set; }
        public ICollection<SystemAdministator> SystemAdministator { get; set; }
    }
}
