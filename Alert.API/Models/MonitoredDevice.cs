using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Alert.API.Models
{
    public partial class MonitoredDevice
    {
        public MonitoredDevice()
        {
            Alert = new HashSet<Alert>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CompanyId { get; set; }

        public Company Company { get; set; }

        [JsonIgnore]
        public ICollection<Alert> Alert { get; set; }
    }
}
