using System;

namespace Alert.API.Dto
{
    public class MonitoredDeviceDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid CompanyId { get; set; }
    }
}
