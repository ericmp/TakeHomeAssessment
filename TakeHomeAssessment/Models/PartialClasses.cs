using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakeHomeAssessment.Models
{
    public class PartialClasses
    {
    }

    // Enum for the types of vehicles
    public enum VehicleType
    {
        Yellow = 0,
        Green = 1,
        FHV = 2
    }

    // Enum for the types of payments
    public enum PaymentType
    {
        CreditCard = 1,
        Cash = 2,
        NoCharge = 3,
        Dispute = 4,
        Unknown = 5,
        VoidedTrip = 6
    }

    // Enum for the column indices that make up the yellow taxi csv data file
    public enum YellowTaxiColumns
    {
        VendorId = 0,
        PickUpDateTime = 1,
        DropOffDateTime = 2,
        PassengerCount = 3,
        TripDistance = 4,
        RateCodeId = 5,
        StoreAndForwardFlag = 6,
        PickUpLocationId = 7,
        DropOffLocationId = 8,
        PaymentType = 9,
        FareAmount = 10,
        Extra = 11,
        MtaTax = 12,
        TipAmount = 13,
        TollsAmount = 14,
        ImprovementSurcharge = 15,
        TotalAmount = 16
    }

    // Enum for the column indices that make up the green taxi csv data file
    public enum GreenTaxiColumns
    {
        VendorId = 0,
        PickUpDateTime = 1,
        DropOffDateTime = 2,
        StoreAndForwardFlag = 3,
        RateCodeId = 4,
        PickUpLocationId = 5,
        DropOffLocationId = 6,
        PassengerCount = 7,
        TripDistance = 8,
        FareAmount = 9,
        Extra = 10,
        MtaTax = 11,
        TipAmount = 12,
        TollsAmount = 13,
        EHailFee = 14,
        ImprovementSurcharge = 15,
        TotalAmount = 16,
        PaymentType = 17,
        TripType = 18
    }

    // Enum for the column indices that make up the combined taxi csv data file
    public enum CombinedTaxiColumns
    {
        VendorId = 0,
        PickUpDateTime = 1,
        DropOffDateTime = 2,
        StoreAndForwardFlag = 3,
        RateCodeId = 4,
        PickUpLocationId = 5,
        DropOffLocationId = 6,
        PassengerCount = 7,
        TripDistance = 8,
        FareAmount = 9,
        Extra = 10,
        MtaTax = 11,
        TipAmount = 12,
        TollsAmount = 13,
        ImprovementSurcharge = 14,
        TotalAmount = 15,
        PaymentType = 16,
    }
}
