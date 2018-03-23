using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alert.API.Repositories.Helpers
{
    public enum FilterType { AND, OR };

    public class Filter
    {

        public string Field { get; set; }
        public string Operator { get; set; }
        public DateTime? DateValue { get; set; }
        public int? NumericValue { get; set; }
        public string StringValue { get; set;  }
        public FilterType Type { get; set; }

    }
}
