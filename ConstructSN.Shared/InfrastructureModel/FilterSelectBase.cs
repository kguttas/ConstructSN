using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructSN.Shared.InfrastructureModel
{
    public class FilterSelectBase
    {
        public int? Id { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? KeySearch { get; set; }
        public bool bShowDisableRegisters { get; set; }
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
    }
}
