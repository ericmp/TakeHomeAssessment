using System;
using Microsoft.ML.Data;

namespace TakeHomeAssessment.Data.Models.Transportation
{
    public class YellowTaxiTripData
    {

        [Column("0")]
        public int VendorID { get; set; }

        [Column("1")]
        public DateTime PickedUpOn { get; set; }

        [Column("2")]
        public DateTime DroppedOffOn { get; set; }

        [Column("3")]
        public int PassengerCount { get; set; }

        [Column("4")]
        public float TripDistance { get; set; }

        [Column("5")]
        public int RateCodeID { get; set; }

        [Column("6")]
        public string StoreAndForward { get; set; }

        [Column("7")]
        public int PickUpLocationID { get; set; }

        [Column("8")]
        public int DropOffLocationID { get; set; }

        [Column("9")]
        public int PaymentType { get; set; }

        [Column("10")]
        public float FareAmount { get; set; }

        [Column("11")]
        public float Extra { get; set; }

        [Column("12")]
        public float MTATax { get; set; }

        [Column("13")]
        public float TipAmount { get; set; }

        [Column("14")]
        public float TollsAmount { get; set; }

        [Column("15")]
        public float ImprovementSurcharge { get; set; }

        [Column("16")]
        public float TotalAmount { get; set; }
    }

    public class YellowTaxiTripFarePrediction
    {
        [ColumnName("Score")]
        public float TotalAmount { get; set; }
    }
}
