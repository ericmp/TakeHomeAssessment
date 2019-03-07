using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace TakeHomeAssessment.Data.Models.Transportation
{
    public class LocationDistanceData
    {
        [Column("5")]
        public int PickUpLocationID { get; set; }

        [Column("6")]
        public int DropOffLocationID { get; set; }

        [Column("8")]
        public float TripDistance { get; set; }
    }

    public class LocationDistancePrediction
    {
        [ColumnName("Score")]
        public float TripDistance { get; set; }
    }
}
