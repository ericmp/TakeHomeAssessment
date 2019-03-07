using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace TakeHomeAssessment.Data.Models.Transportation
{
    public class TripData
    {

        [Column("1")]
        public DateTime PickedUpOn { get; set; }

        [Column("5")]
        public int PickUpLocationID { get; set; }

        [Column("6")]
        public int DropOffLocationID { get; set; }

        [Column("7")]
        public int PassengerCount { get; set; }

        [Column("8")]
        public float TripDistance { get; set; }

        [Column("16")]
        public float TotalAmount { get; set; }

        [Column("17")]
        public int PaymentType { get; set; }

        public string VehicleType { get; set; }
    }

    public class TripFarePrediction
    {
        [ColumnName("Score")]
        public float TotalAmount { get; set; }
    }

    public class TripDistancePrediction
    {
        [ColumnName("Score")]
        public float TripDistance { get; set; }
    }
}
