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
    public class DistanceLearningModel : BaseRegressionLearningModel
    {
        public DistanceLearningModel() : base() { }

        public DistanceLearningModel(int pickUpLocationIdColumnIndex, int dropOffLocationIdColumnIndex, int tripDistanceColumnIndex) : base()
        {
            _textLoader = _mlContext.Data.CreateTextLoader(new TextLoader.Arguments()
            {
                Separators = new[] { ',' },
                HasHeader = true,
                Column = new[]
                {
                    new TextLoader.Column("PickUpLocationID", DataKind.I4, pickUpLocationIdColumnIndex),
                    new TextLoader.Column("DropOffLocationID", DataKind.I4, dropOffLocationIdColumnIndex),
                    new TextLoader.Column("TripDistance", DataKind.R4, tripDistanceColumnIndex),
                }
            });
        }

        public ITransformer TrainDistanceLearningModel(string dataPath)
        {
            if (_textLoader != null)
            {
                IDataView dataView = _textLoader.Read(Directory.GetFiles(dataPath, "*.csv"));

                var pipeline = _mlContext.Transforms.CopyColumns("Label", "TripDistance")
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("PickUpLocationID"))
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding("DropOffLocationID"))
                    .Append(_mlContext.Transforms.Concatenate("Features", "PickUpLocationID", "DropOffLocationID"))
                    .Append(_mlContext.Regression.Trainers.FastTree());

                // Train the model.
                var model = pipeline.Fit(dataView);
                return model;
            }

            return null;
        }

        public void SetTextLoader(
            int pickUpLocationIdColumnIndex,
            int dropOffLocationIdColumnIndex,
            int tripDistanceColumnIndex)
        {
            _textLoader = _mlContext.Data.CreateTextLoader(new TextLoader.Arguments()
            {
                Separators = new[] { ',' },
                HasHeader = true,
                Column = new[]
                {
                    new TextLoader.Column("PickUpLocationID", DataKind.I4, pickUpLocationIdColumnIndex),
                    new TextLoader.Column("DropOffLocationID", DataKind.I4, dropOffLocationIdColumnIndex),
                    new TextLoader.Column("TripDistance", DataKind.R4, tripDistanceColumnIndex)
                }
            });
        }
    }
}
