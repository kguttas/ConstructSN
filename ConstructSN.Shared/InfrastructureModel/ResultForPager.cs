using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructSN.Shared.InfrastructureModel
{
    public class ResultForPager<T>
    {
        public IEnumerable<T>? Items { get; set; }
        public long CountItems { get; set; }
    }
}
