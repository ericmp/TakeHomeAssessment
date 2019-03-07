This app is live at https://takehomeassessment.azurewebsites.net/.

Note: Only the GetFarePredictionsById API method will execute successfully if you clone the repository. I removed the database credentials from the appsettings.json config file and as such, the app can't do any borough lookups.

## New Technologies
- ML.NET and Machine Learning
- Swagger/Swashbuckle

---

## Features
- Predict the costs of traveling by taxi/ride-share between different zones within the boroughs of New York City. Predicted prices can be filtered on by type of vehicle, number of passengers, the date and time of the pickup, and the method of payment that will be used.
- Get a list of all taxi cab zones in New York City, or get a list of taxi zones for an individual borough.

---

## Development Process
- All data was cleaned using the data cleaner utility class to remove rows with empty values in the feature columns.
- The Distance Learning Model was created by combining all the pick up/drop off data from the yellow and green taxi cab datasets, and training the model with a linear regression model to predict distance between location IDs using the "PickUpLocationID", "DropOffLocationID", and "TripDistance" columns as features.
- The Transportation Learning Models was trained linear regression model and using the "PickedUpOn", "PickUpLocationID", "DropOffLocationID", "TripDistance", "PassengerCount", "TotalAmount", and "PaymentType" columns as the features. The Yellow/Green taxi learning models were trained utilizing their respective datasets. The FHV data files did not contain any pricing data, so I averaged the price out by combining the yellow and green taxi data files figuring that Uber/Lyft would be near competitive with those prices.
- I have a database in Azure that has the taxi zone lookup records, and I am using EF Core to access it with a DbContext.
- I went with the repository pattern to abstract out the interfaces of the data access layer which allows for easier unit testing when paired with dependency injection.
- I am using Swagger as the front end to display all of the API methods.
- The API Controller accesses the repository layer to access the learning models and taxi zone connection.

---

## Desired Features/Improvements
- Work on extrapolating more features for the transportation machine learning models, such as weekends vs. weekdays to improve the accuracy of the model.
- Rewrite the training login in Python using more mature libraries so that I can use a k-fold cross validation training algorithm instead of a linear regression algorithm. Trying to do k-fold in ML.NET resulted in a hard crash of the program.
- Hooking into the Uber API to get fare predictions to account for ride sharing prices, or utilizing any historic Ubert/Lyft pricing data in the learning model.
- Utilize the Taxi Zone shapefiles and QGIS to create a front end interface display various map related metrics and gives us a visual of where locations are.
- Integrate the Uber/Lyft API to hail rides from the application.
- Error handling/validation
- Moving the Machine Learning models into a cloud storage, instead of storing them as zip files.
- Clean up the learning model class implementation.
- Remove connection string from appsettings.json.
- Implement unit tests.
- Implement a ratelimiter as the zone database is currently being hosted on my Azure account using the pay-as-you-go method and a script could be written to hit the database constantly, running up my bill.

---

## After Hours
After the 3 hours were up, I decided to spend some extra time getting the app hosted on Azure so that a live demo could be utilized. Whilst doing this, I encountered an issue with my connection string. Thankfully, this gave me a chance to learn the Azure Application Insights toolset to help diagnose the issue.