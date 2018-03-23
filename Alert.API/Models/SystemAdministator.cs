using System;
using System.Collections.Generic;

namespace Alert.API.Models
{
    public partial class SystemAdministator
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public Guid CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
