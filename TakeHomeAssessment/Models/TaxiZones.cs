using System;
using System.Collections.Generic;

namespace TakeHomeAssessment.Models
{
    public partial class TaxiZones
    {
        public int LocationId { get; set; }
        public string Borough { get; set; }
        public string Zone { get; set; }
        public string ServiceZone { get; set; }
    }
}
