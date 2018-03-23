﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Alert.API.Models
{
    public partial class AlertSolution
    {
        public AlertSolution()
        {
            Alert = new HashSet<Alert>();
        }

        public Guid Id { get; set; }
        public string Text { get; set; }

        [JsonIgnore]
        public ICollection<Alert> Alert { get; set; }
    }
}
