using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class Park
    {
        public string ParkCode { get; set; }
        public string ParkName { get; set; }
        public string State { get; set; }
        public int Acreage { get; set; }
        public int ElevationInFeet { get; set; }
        public decimal MilesOfTrail { get; set; }
        public int NumberOfCampsites { get; set; }
        public string NumberOfCampsitesDerived
        {
            get
            {
                if (NumberOfCampsites == 0)
                {
                    return "N/A";
                }
                else
                {
                    return NumberOfCampsites.ToString("N0");
                }
            }
        }
        public string Climate { get; set; }
        public int YearFounded { get; set; }
        public int AnnualVisitorCount { get; set; }
        public string InspirationalQuote { get; set; }
        public string InspirationalQuoteSource { get; set; }
        public string ParkDescription { get; set; }
        public decimal EntryFee { get; set; }
        public string EntryFeeDerived
        {
            get
            {
                if (EntryFee == 0)
                {
                    return "FREE!!!";
                }
                else
                {
                    return EntryFee.ToString("C");
                }
            }
        }
        public int NumberOfAnimalSpecies { get; set; }
        public List<Weather> Weather { get; set; } = new List<Weather>();
        public string TemperatureType { get; set; } = "Fahrenheit";
    }
}
