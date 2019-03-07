using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TakeHomeAssessment.Data.Models.Transportation;
using TakeHomeAssessment.Models;
using TakeHomeAssessment.Repositories;

namespace TakeHomeAssessment.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TransportationController : ControllerBase
    {

        private readonly ITransportationRepository _transportationRepository;
        private readonly ITaxiZoneRepository _taxiZoneRepository;
        private const int StartEndZoneMinId = 1;
        private const int StartEndZoneMaxId = 265;

        public TransportationController(ITransportationRepository transportationRepository, ITaxiZoneRepository taxiZoneRepository) : base()
        {
            _transportationRepository = transportationRepository;
            _taxiZoneRepository = taxiZoneRepository;
        }

        [HttpGet("GetFarePredictions/{startZoneName}/{endZoneName}")]
        public ActionResult<TripData[]> GetFarePredictions(string startZoneName, string endZoneName, DateTime? departureTime = null, VehicleType? vehicleType = null, int passengerCount = 1, PaymentType? paymentType = null)
        {
            try
            {
                // Check if the passenger count isn't a positive number
                if (passengerCount <= 0)
                {
                    return StatusCode(500, "Passenger count can't be less than 1.");
                }

                // Verify the departure time isn't less than 2017 or greater than our year + 1
                if (departureTime.HasValue && (departureTime.Value.Year > DateTime.UtcNow.Year + 1 || departureTime.Value.Year < 2017))
                {
                    return StatusCode(500, "Departure time can't be less than 2017 or greater than one year from our current year.");
                }

                // Get the start and end zone IDs
                var startZone = _taxiZoneRepository.GetTaxiZone(startZoneName);
                var endZone = _taxiZoneRepository.GetTaxiZone(endZoneName);

                // Check if start zone was found
                if (startZone == null)
                {
                    return StatusCode(500, "Please enter a valid start zone.");
                }

                // Check if end zone was found
                if (endZone == null)
                {
                    return StatusCode(500, "Please enter a valid end zone.");
                }

                int startZoneId = startZone.LocationId;
                int endZoneId = endZone.LocationId;

                // If a vehicle type is supplied, only get the prediction for that type
                if (vehicleType.HasValue)
                {
                    return new TripData[] { _transportationRepository.GetPrediction(startZoneId, endZoneId, departureTime ?? DateTime.UtcNow, vehicleType.Value, passengerCount, (int?)paymentType ?? 1) };
                }
                // Get the prediction for all vehicle types
                else
                {
                    return _transportationRepository.GetPrediction(startZoneId, endZoneId, departureTime ?? DateTime.UtcNow, passengerCount, (int?)paymentType ?? 1);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("GetFarePredictionsById/{startZoneId}/{endZoneId}")]
        public ActionResult<TripData[]> GetFarePredictionsById(int startZoneId, int endZoneId, DateTime? departureTime = null, VehicleType? vehicleType = null, int passengerCount = 1, PaymentType? paymentType = null)
        {
            try
            {
                // Check if the start zone id is valid
                if (startZoneId < StartEndZoneMinId || startZoneId > StartEndZoneMaxId)
                {
                    return StatusCode(500, "Please enter a valid start zone id.");
                }

                // Check if the end zone id is valid
                if (endZoneId < StartEndZoneMinId || endZoneId > StartEndZoneMaxId)
                {
                    return StatusCode(500, "Please enter a valid end zone id.");
                }

                // Check if the passenger count isn't a positive number
                if (passengerCount <= 0)
                {
                    return StatusCode(500, "Passenger count can't be less than 1.");
                }

                // Verify the departure time isn't less than 2017 or greater than our year + 1
                if (departureTime.HasValue && (departureTime.Value.Year > DateTime.UtcNow.Year + 1 || departureTime.Value.Year < 2017))
                {
                    return StatusCode(500, "Departure time can't be less than 2017 or greater than one year from our current year.");
                }

                // If a vehicle type is supplied, only get the prediction for that type
                if (vehicleType.HasValue)
                {
                    return new TripData[] { _transportationRepository.GetPrediction(startZoneId, endZoneId, departureTime ?? DateTime.UtcNow, vehicleType.Value, passengerCount, (int?)paymentType ?? 1) };
                }
                // Get the prediction for all vehicle types
                else
                {
                    return _transportationRepository.GetPrediction(startZoneId, endZoneId, departureTime ?? DateTime.UtcNow, passengerCount, (int?)paymentType ?? 1);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("GetZones")]
        public ActionResult<List<TaxiZones>> GetZones()
        {
            try
            {
                return _taxiZoneRepository.List.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("GetZones/{boroughName}")]
        public ActionResult<List<TaxiZones>> GetZones(string boroughName)
        {
            try
            {
                return _taxiZoneRepository.ListZonesByBorough(boroughName).ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}