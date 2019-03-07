using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Core.Data;
using Microsoft.ML.Data;

namespace TakeHomeAssessment.Data.Models.Learners
{
    public class TransportationLearningModel : BaseRegressionLearningModel
    {
        public TransportationLearningModel() : base() { }

        public TransportationLearningModel(int pickedUpOnColumnIndex,
           int pickUpLocationIdColumnIndex,
           int dropOffLocationIdColumnIndex,
           int passengerCountColumnIndex,
           int tripDistanceColumnIndex,
           int totalAmountColumnIndex,
           int paymentTypeColumnIndex) : base()
        {
            _textLoader = _mlContext.Data.CreateTextLoader(new TextLoader.Arguments()
            {
                Separators = new[] { ',' },
                HasHeader = true,
                Column = new[]
                {
                    new TextLoader.Column("PickedUpOn", DataKind.DateTime, pickedUpOnColumnIndex),
                    new TextLoader.Column("PickUpLocationID", DataKind.I4, pickUpLocationIdColumnIndex),
                    new TextLoader.Column("DropOffLocationID", DataKind.I4, dropOffLocationIdColumnIndex),
                    new TextLoader.Column("TripDistance", DataKind.R4, tripDistanceColumnIndex),
                    new TextLoader.Column("PassengerCount", DataKind.I4, passengerCountColumnIndex),
                    new TextLoader.Column("TotalAmount", DataKind.R4, totalAmountColumnIndex),
                    new TextLoader.Column("PaymentType", DataKind.I4, paymentTypeColumnIndex)
                }
            });
        }

        public ITransformer TrainFarePredictionModel(string dataPath)
        {
            if (_textLoader != null)
            {
                IDataView dataView = _textLoader.Read(Directory.GetFiles(dataPath, "*.csv"));

                var pipeline = _mlContext.Transforms.CopyColumns("Label", "TotalAmount")
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("PickedUpOn"))
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("PickUpLocationID"))
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("DropOffLocationID"))
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("PassengerCount"))
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("PaymentType"))
                    .Append(_mlContext.Transforms.Concatenate("Features", "PickedUpOn", "PickUpLocationID", "DropOffLocationID", "PassengerCount", "TripDistance", "PaymentType"))
                    .Append(_mlContext.Regression.Trainers.FastTree());

                // Train the model.
                var model = pipeline.Fit(dataView);

                return model;
            }

            return null;
        }

        public ITransformer TrainDistancePredictionModel(string dataPath)
        {
            if (_textLoader != null)
            {
                IDataView dataView = _textLoader.Read(Directory.GetFiles(dataPath, "*.csv"));

                var pipeline = _mlContext.Transforms.CopyColumns("Label", "TripDistance")
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("PickedUpOn"))
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("PickUpLocationID"))
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("DropOffLocationID"))
                    .Append(_mlContext.Transforms.Concatenate("Features", "PickedUpOn", "PickUpLocationID", "DropOffLocationID", "TotalAmount"))
                    .Append(_mlContext.Regression.Trainers.FastTree());

                // Train the model.
                var model = pipeline.Fit(dataView);

                return model;
            }

            return null;
        }

        public void SetTextLoader(
            int pickedUpOnColumnIndex,
            int pickUpLocationIdColumnIndex,
            int dropOffLocationIdColumnIndex,
            int passengerCountColumnIndex,
            int tripDistanceColumnIndex,
            int totalAmountColumnIndex,
            int paymentTypeColumnIndex)
        {
            _textLoader = _mlContext.Data.CreateTextLoader(new TextLoader.Arguments()
            {
                Separators = new[] { ',' },
                HasHeader = true,
                Column = new[]
                {
                    new TextLoader.Column("PickedUpOn", DataKind.DateTime, pickedUpOnColumnIndex),
                    new TextLoader.Column("PickUpLocationID", DataKind.I4, pickUpLocationIdColumnIndex),
                    new TextLoader.Column("DropOffLocationID", DataKind.I4, dropOffLocationIdColumnIndex),
                    new TextLoader.Column("TripDistance", DataKind.R4, tripDistanceColumnIndex),
                    new TextLoader.Column("PassengerCount", DataKind.I4, passengerCountColumnIndex),
                    new TextLoader.Column("TotalAmount", DataKind.R4, totalAmountColumnIndex),
                    new TextLoader.Column("PaymentType", DataKind.I4, paymentTypeColumnIndex)
                }
            });
        }
    }
}
