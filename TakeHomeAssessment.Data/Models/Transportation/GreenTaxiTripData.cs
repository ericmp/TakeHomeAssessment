using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace TakeHomeAssessment.Data.Models.Transportation
{
    public class GreenTaxiTripData
    {
        [Column("0")]
        public int VendorID { get; set; }

        [Column("1")]
        public DateTime PickedUpOn { get; set; }

        [Column("2")]
        public DateTime DroppedOffOn { get; set; }

        [Column("3")]
        public string StoreAndForward { get; set; }

        [Column("4")]
        public int RateCodeID { get; set; }

        [Column("5")]
        public int PickUpLocationID { get; set; }

        [Column("6")]
        public int DropOffLocationID { get; set; }

        [Column("7")]
        public int PassengerCount { get; set; }

        [Column("8")]
        public float TripDistance { get; set; }

        [Column("9")]
        public float FareAmount { get; set; }

        [Column("10")]
        public float Extra { get; set; }

        [Column("11")]
        public float MTATax { get; set; }

        [Column("12")]
        public float TipAmount { get; set; }

        [Column("13")]
        public float TollsAmount { get; set; }

        [Column("14")]
        public float EHailFee { get; set; }

        [Column("15")]
        public float ImprovementSurcharge { get; set; }

        [Column("16")]
        public float TotalAmount { get; set; }

        [Column("17")]
        public int PaymentType { get; set; }

        [Column("18")]
        public int TripType { get; set; }
    }

    public class GreenTaxiTripFarePrediction
    {
        [ColumnName("Score")]
        public float TotalAmount { get; set; }
    }
}
