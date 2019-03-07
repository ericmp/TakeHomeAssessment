using Microsoft.ML;
using Microsoft.ML.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TakeHomeAssessment.Data.Models.Learners;
using TakeHomeAssessment.Data.Models.Transportation;
using TakeHomeAssessment.Models;

namespace TakeHomeAssessment.Repositories
{
    public interface ITransportationRepository
    {
        TripData[] GetPrediction(int startZoneId, int endZoneId, DateTime departureTime, int passengerCount, int paymentType);
        TripData GetPrediction(int startZoneId, int endZoneId, DateTime departureTime, VehicleType vehicleType, int passengerCount, int paymentType);
    }

    public class TransportationRepository : ITransportationRepository
    {
        private readonly TransportationLearningModel _transportationYellowLearningModel;
        private readonly TransportationLearningModel _transportationGreenLearningModel;
        private readonly TransportationLearningModel _transportationFHVLearningModel;
        private readonly DistanceLearningModel _distanceLearningModel;
        private readonly ITransformer _locationDistanceModel;
        private ITransformer _taxiDistanceModel { get; set; }
        private ITransformer _taxiFareModel { get; set; }

        public TransportationRepository()
        {
            // Instantiate the distance learning model
            _distanceLearningModel = new DistanceLearningModel((int)CombinedTaxiColumns.PickUpLocationId, (int)CombinedTaxiColumns.DropOffLocationId, (int)CombinedTaxiColumns.TripDistance);

            // Instantiate the yellow taxi learning model
            _transportationYellowLearningModel = new TransportationLearningModel((int)YellowTaxiColumns.PickUpDateTime, (int)YellowTaxiColumns.PickUpLocationId, (int)YellowTaxiColumns.DropOffLocationId, (int)YellowTaxiColumns.PassengerCount, (int)YellowTaxiColumns.TripDistance, (int)YellowTaxiColumns.TotalAmount, (int)YellowTaxiColumns.PaymentType);

            // Instantiate the green taxi learning model
            _transportationGreenLearningModel = new TransportationLearningModel((int)GreenTaxiColumns.PickUpDateTime, (int)GreenTaxiColumns.PickUpLocationId, (int)GreenTaxiColumns.DropOffLocationId, (int)GreenTaxiColumns.PassengerCount, (int)GreenTaxiColumns.TripDistance, (int)GreenTaxiColumns.TotalAmount, (int)GreenTaxiColumns.PaymentType);

            // Instantiate the ride sharing vehicle learning model
            _transportationFHVLearningModel = new TransportationLearningModel((int)CombinedTaxiColumns.PickUpDateTime, (int)CombinedTaxiColumns.PickUpLocationId, (int)CombinedTaxiColumns.DropOffLocationId, (int)CombinedTaxiColumns.PassengerCount, (int)CombinedTaxiColumns.TripDistance, (int)CombinedTaxiColumns.TotalAmount, (int)CombinedTaxiColumns.PaymentType);

            // Load the location distance learning model from the zip file. If the file can't be found, retrain a model using all the taxi data 
            _locationDistanceModel = _distanceLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "LocationDistanceModel.zip")) ?? _distanceLearningModel.TrainDistanceLearningModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\Taxi"));
        }

        /// <summary>
        /// Predict the cost of traveling between the start and end zone using a taxi cab or ride share.
        /// </summary>
        /// <param name="startZoneId">The ID of the pick up zone.</param>
        /// <param name="endZoneId">The ID of the drop off zone.</param>
        /// <param name="departureTime">The date and time of the pick up.</param>
        /// <param name="passengerCount">How many passengers will be picked up.</param>
        /// <param name="paymentType">The type of payment that will be used to pay for the ride.</param>
        /// <returns>The trip cost prediction for a yellow taxi, green taxi, and ride share.</returns>
        public TripData[] GetPrediction(int startZoneId, int endZoneId, DateTime departureTime, int passengerCount, int paymentType)
        {
            // Instantiate a model to calculate the probable distance between the two zones
            var distanceData = new LocationDistanceData()
            {
                DropOffLocationID = startZoneId,
                PickUpLocationID = endZoneId,
                TripDistance = 0
            };

            // Instantiate the return variable
            TripData[] result = new TripData[3];
            // Store the types of vehicles
            string[] vehicleTypes = new string[3] { "Yellow", "Green", "FHV" };

            // Calculate the trip prediction cost for each type of vehicle
            for (int i = 0; i < result.Length; i++)
            {

                result[i] = new TripData()
                {
                    PickedUpOn = departureTime,
                    PickUpLocationID = startZoneId,
                    DropOffLocationID = endZoneId,
                    PassengerCount = passengerCount,
                    PaymentType = paymentType,
                    // Use the predicted distance as the trip distance
                    TripDistance = _distanceLearningModel.GetPrediction<LocationDistanceData, LocationDistancePrediction>(_locationDistanceModel, distanceData).TripDistance,
                    TotalAmount = 0,
                    VehicleType = vehicleTypes[i]
                };

                // Yyellow taxi
                if (i == 0)
                {
                    // Predict the fare
                    _taxiFareModel = _transportationYellowLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "YellowTaxiFareModel.zip")) ?? _transportationYellowLearningModel.TrainFarePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\YellowTaxi"));
                    // Predict the distance again now that the fare has been calculated
                    _taxiDistanceModel = _transportationYellowLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "YellowTaxiDistanceModel.zip")) ?? _transportationYellowLearningModel.TrainDistancePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\YellowTaxi"));
                }
                // Green taxi
                else if (i == 1)
                {
                    // Predict the fare
                    _taxiFareModel = _transportationGreenLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "GreenTaxiFareModel.zip")) ?? _transportationGreenLearningModel.TrainFarePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\GreenTaxi"));
                    // Predict the distance again now that the fare has been calculated
                    _taxiDistanceModel = _transportationGreenLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "GreenTaxiDistanceModel.zip")) ?? _transportationGreenLearningModel.TrainDistancePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\GreenTaxi"));
                }
                // Ride share
                else if (i == 2)
                {
                    // Predict the fare
                    _taxiFareModel = _transportationFHVLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "CombinedTaxiFareModel.zip")) ?? _transportationFHVLearningModel.TrainFarePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\Taxi"));
                    // Predict the distance again now that the fare has been calculated
                    _taxiDistanceModel = _transportationFHVLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "CombinedTaxiDistanceModel.zip")) ?? _transportationFHVLearningModel.TrainDistancePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\Taxi"));
                }
                
                // Store the results
                result[i].TotalAmount = _transportationGreenLearningModel.GetPrediction<TripData, TripFarePrediction>(_taxiFareModel, result[i]).TotalAmount;
                result[i].TripDistance = _transportationGreenLearningModel.GetPrediction<TripData, TripDistancePrediction>(_taxiDistanceModel, result[i]).TripDistance;
            }

            return result;
        }

        /// <summary>
        /// Predict the cost of traveling between the start and end zone using a specific vehicle type.
        /// </summary>
        /// <param name="startZoneId">The ID of the pick up zone.</param>
        /// <param name="endZoneId">The ID of the drop off zone.</param>
        /// <param name="departureTime">The date and time of the pick up.</param>
        /// <param name="vehicleType">The type of vehicle that you want to filter on for your trip.</param>
        /// <param name="passengerCount">How many passengers will be picked up.</param>
        /// <param name="paymentType">The type of payment that will be used to pay for the ride.</param>
        /// <returns>The trip cost prediction for the specified vehicle type.</returns>
        public TripData GetPrediction(int startZoneId, int endZoneId, DateTime departureTime, VehicleType vehicleType, int passengerCount, int paymentType)
        {
            // Instantiate a model to calculate the probable distance between the two zones
            var distanceData = new LocationDistanceData()
            {
                DropOffLocationID = startZoneId,
                PickUpLocationID = endZoneId,
                TripDistance = 0
            };

            // Store the types of vehicles
            string[] vehicleTypes = new string[3] { "Yellow", "Green", "FHV" };

            TripData result = new TripData()
            {
                PickedUpOn = departureTime,
                PickUpLocationID = startZoneId,
                DropOffLocationID = endZoneId,
                PassengerCount = passengerCount,
                PaymentType = paymentType,
                // Use the predicted distance as the trip distance
                TripDistance = _distanceLearningModel.GetPrediction<LocationDistanceData, LocationDistancePrediction>(_locationDistanceModel, distanceData).TripDistance,
                TotalAmount = 0,
                VehicleType = vehicleTypes[(int)vehicleType]
            };

            // Yellow taxi
            if (vehicleType == VehicleType.Yellow)
            {
                // Predict the fare
                _taxiFareModel = _transportationYellowLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "YellowTaxiFareModel.zip")) ?? _transportationYellowLearningModel.TrainFarePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\YellowTaxi"));
                // Predict the distance again now that the fare has been calculated
                _taxiDistanceModel = _transportationYellowLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "YellowTaxiDistanceModel.zip")) ?? _transportationYellowLearningModel.TrainDistancePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\YellowTaxi"));
            }
            // Green taxi
            else if (vehicleType == VehicleType.Green)
            {
                // Predict the fare
                _taxiFareModel = _transportationGreenLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "GreenTaxiFareModel.zip")) ?? _transportationGreenLearningModel.TrainFarePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\GreenTaxi"));
                // Predict the distance again now that the fare has been calculated
                _taxiDistanceModel = _transportationGreenLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "GreenTaxiDistanceModel.zip")) ?? _transportationGreenLearningModel.TrainDistancePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\GreenTaxi"));
            }
            // Ride share
            else if (vehicleType == VehicleType.FHV)
            {
                // Predict the fare
                _taxiFareModel = _transportationFHVLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "CombinedTaxiFareModel.zip")) ?? _transportationFHVLearningModel.TrainFarePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\Taxi"));
                // Predict the distance again now that the fare has been calculated
                _taxiDistanceModel = _transportationFHVLearningModel.LoadModelFromFile(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Models\TrainedModels", "CombinedTaxiDistanceModel.zip")) ?? _transportationFHVLearningModel.TrainDistancePredictionModel(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\Taxi"));
            }

            // Store the result
            result.TotalAmount = _transportationGreenLearningModel.GetPrediction<TripData, TripFarePrediction>(_taxiFareModel, result).TotalAmount;
            result.TripDistance = _transportationGreenLearningModel.GetPrediction<TripData, TripDistancePrediction>(_taxiDistanceModel, result).TripDistance;

            return result;
        }
    }
}
