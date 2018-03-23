using System;
using System.Collections.Generic;

namespace Alert.API.Models
{
    public partial class Alert
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid MonitoredDeviceId { get; set; }
        public DateTime TimeGenerated { get; set; }
        public string Description { get; set; }

        // changed to int from byte
        public int Priority { get; set; }

        // changed to int from byte
        public int Severity { get; set; }

        public string Source { get; set; }
        public Guid? AlertConclusionId { get; set; }
        public Guid? AlertExplanationId { get; set; }
        public Guid? AlertSolutionId { get; set; }

        public AlertConclusion AlertConclusion { get; set; }
        public AlertExplanation AlertExplanation { get; set; }
        public AlertSolution AlertSolution { get; set; }
        public MonitoredDevice MonitoredDevice { get; set; }
    }
}
