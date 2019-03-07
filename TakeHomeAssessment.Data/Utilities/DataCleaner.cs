using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TakeHomeAssessment.Data.Utilities
{
    public static class DataCleaner
    {
        // Method to clean rows with empty feature values from the green taxi csv files
        public static void CleanGreenTaxiRecords()
        {
            foreach (var file in Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\GreenTaxi\"), "*.csv"))
            {
                // Create the IEnumerable data source  
                string[] lines = File.ReadAllLines(file);


                //IEnumerable<string> query = lines.Skip(2).Where(x => !x.Split(',').Any(cell => string.IsNullOrWhiteSpace(cell)));
                var query = lines.Skip(2).Select(x => x.Split(','));
                List<string> result = new List<string>();
                result.AddRange(lines.Take(2));

                foreach (var cell in query)
                {
                    for (var i = 0; i < cell.Count(); i++)
                    {
                        // Skip the ehail column, it is always blank
                        if (i == 14)
                        {
                            continue;
                        }

                        // Filter out cells with blank values or where any of the numerical data is negative for cleaner estimations
                        if (string.IsNullOrWhiteSpace(cell[i]) || i >= 7 && i < 18 && cell[i].Contains('-'))
                        {
                            break;
                        }

                        // If we've made it through, join the row back and insert it into the results list
                        if (i == 17)
                        {
                            result.Add(string.Join(",", cell));
                        }
                    }
                }

                File.WriteAllLines(file, result);
            }
        }

        // Method to clean rows with empty feature values from the yellow taxi csv files
        public static void CleanYellowTaxiRecords()
        {
            foreach (var file in Directory.GetFiles(Path.Combine(Environment.CurrentDirectory, @"..\TakeHomeAssessment.Data\Data\YellowTaxi\"), "*.csv"))
            {
                // Create the IEnumerable data source  
                string[] lines = File.ReadAllLines(file);

                // Filter out cells with blank values or where any of the numerical data is negative for cleaner estimations
                IEnumerable<string> query = lines.Skip(2).Where(x => !x.Split(',').Any(cell => string.IsNullOrWhiteSpace(cell)) && !x.Split(',').Skip(7).Take(10).Any(cell => cell.Contains('-')));


                List<string> result = new List<string>();
                // Add the header from the csv file
                result.AddRange(lines.Take(2));
                // Add the records that are valid
                result.AddRange(query);

                File.WriteAllLines(file, result);
            }
        }
    }
}
