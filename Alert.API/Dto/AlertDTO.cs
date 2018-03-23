using System;

namespace Alert.API.Dto
{
    public class AlertDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime TimeGenerated { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public int Severity { get; set; }

        public string Source { get; set; }

        public AlertConclusionDTO AlertConclusion { get; set; }

        public AlertExplanationDTO AlertExplanation { get; set; }

        public AlertSolutionDTO AlertSolution { get; set; }

        public MonitoredDeviceDTO MonitoredDevice { get; set; }

    }
}