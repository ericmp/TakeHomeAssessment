using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace TakeHomeAssessment.Data.Models.Transportation
{
    public class FHVTripData
    {
        [Column("0")]
        public string DispatchingBaseNumber { get; set; }

        [Column("1")]
        public DateTime PickedUpOn { get; set; }

        [Column("2")]
        public DateTime DroppedOffOn { get; set; }

        [Column("3")]
        public int PickUpLocationID { get; set; }

        [Column("4")]
        public int DropOffLocationID { get; set; }

        [Column("5")]
        public bool SharedRide { get; set; }
    }

    public class FHVTripFarePrediction
    {
        [ColumnName("Score")]
        public float TotalAmount { get; set; }
    }
}
