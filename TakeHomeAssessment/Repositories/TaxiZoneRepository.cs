using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TakeHomeAssessment.Data.Models.Transportation;
using TakeHomeAssessment.Models;

namespace TakeHomeAssessment.Repositories
{
    public interface ITaxiZoneRepository
    {
        IEnumerable<TaxiZones> List { get; }
        IEnumerable<TaxiZones> ListZonesByBorough(string borough);
        TaxiZones GetTaxiZone(string zone);
        TaxiZones GetTaxiZone(int id);
    }

    public class TaxiZoneRepository : ITaxiZoneRepository
    {
        private readonly NewYorkFaresPredictionDbContext _dbContext;

        public TaxiZoneRepository(NewYorkFaresPredictionDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Returns a list of all taxi zones.
        /// </summary>
        public IEnumerable<TaxiZones> List
        {
            get
            {
                return _dbContext.TaxiZones.AsEnumerable();
            }
        }

        /// <summary>
        /// Return a list of all taxi zones in the specified borough.
        /// </summary>
        /// <param name="borough">The New York City borough to filter on.</param>
        /// <returns></returns>
        public IEnumerable<TaxiZones> ListZonesByBorough(string borough)
        {
            return _dbContext.TaxiZones.Where(x => x.Borough.Equals(borough, StringComparison.InvariantCultureIgnoreCase)).AsEnumerable();
        }

        /// <summary>
        /// Get a specific taxi zone by its name.
        /// </summary>
        /// <param name="zone">The name of the taxi zone.</param>
        /// <returns></returns>
        public TaxiZones GetTaxiZone(string zone) {
            return _dbContext.TaxiZones.FirstOrDefault(x => x.Zone.Equals(zone, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Get a specific taxi zone by its ID.
        /// </summary>
        /// <param name="id">The ID of the taxi zone.</param>
        /// <returns></returns>
        public TaxiZones GetTaxiZone(int id)
        {
            return _dbContext.TaxiZones.FirstOrDefault(x => x.LocationId == id);
        }
    }
}
