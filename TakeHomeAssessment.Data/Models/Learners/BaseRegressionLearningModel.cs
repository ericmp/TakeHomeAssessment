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
    public abstract class BaseRegressionLearningModel
    {
        public readonly MLContext _mlContext;
        public TextLoader _textLoader { get; protected set; }

        protected BaseRegressionLearningModel()
        {
            _mlContext = new MLContext();
        }

        public virtual RegressionMetrics EvaluateModel(ITransformer model, IDataView dataView)
        {
            // Split the data 90:10 into train and test sets
            var (trainData, testData) = _mlContext.Regression.TrainTestSplit(dataView, testFraction: 0.1);

            // Evaluate on the test data
            var metrics = _mlContext.Regression.Evaluate(model.Transform(testData));

            return metrics;
        }

        public virtual RegressionMetrics EvaluateModel(ITransformer model, string testDataPath)
        {
            IDataView dataView = _textLoader.Read(testDataPath);
            var predictions = model.Transform(dataView);
            var metrics = _mlContext.Regression.Evaluate(predictions);
            return metrics;
        }

        public virtual TDest GetPrediction<TSrc, TDest>(ITransformer model, TSrc sample)
            where TSrc : class
            where TDest : class, new()
        {
            var predictionFunction = model.CreatePredictionEngine<TSrc, TDest>(_mlContext);

            var prediction = predictionFunction.Predict(sample);

            return prediction;
        }

        public virtual TDest GetPrediction<TSrc, TDest>(TSrc sample, string modelPath)
            where TSrc : class
            where TDest : class, new()
        {
            ITransformer loadedModel;
            using (var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                loadedModel = _mlContext.Model.Load(stream);
            }

            var predictionFunction = loadedModel.CreatePredictionEngine<TSrc, TDest>(_mlContext);

            var prediction = predictionFunction.Predict(sample);

            return prediction;
        }

        public void SaveModelAsFile(ITransformer model, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                _mlContext.Model.Save(model, fileStream);
            }
        }

        public virtual ITransformer LoadModelFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return _mlContext.Model.Load(stream);
                }
            }

            return null;
        }
    }
}
