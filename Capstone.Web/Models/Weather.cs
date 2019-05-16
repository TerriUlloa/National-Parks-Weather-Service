using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.Models
{
    public class Weather
    {
        public bool IsFarenheit { get; set; } = true;
        public string ParkCode { get; set; }
        public int FiveDayForecastValue { get; set; }
        public int Low { get; set; }
        public int High { get; set; }
        public string Forecast { get; set; }
        public string ForecastDerived
        {
            get
            {
                switch (Forecast)
                {
                    case "snow":
                        return "Make sure to bring snow shoes!";
                    case "rain":
                        return "Pack rain gear and waterproof shoes!";
                    case "thunderstorms":
                        return "Seek shelter and avoid hiking on exposed ridges!";
                    case "sunny":
                        return "Don't forget the sunblock!";
                    default:
                        return "";
                }
            }
        }
        public string TemperatureDerived
        {
            get
            {
                return GetTemperatureMessage(Low, High);
            }
        }
        private string GetTemperatureMessage(int low, int high)
        {
            string result = "";
            if(high > 75)
            {
                result = "Bring water. ";
            }
            if(Math.Abs(high - low) >= 20)
            {
                result += "Wear breathable layers. ";
            }
            if(low <= 20)
            {
                result += "WARNING, frigid temperatures today.";
            }
            return result;
        }

        public int CalculateCelsius(int temp, string tempType)
        {
            if (tempType.Equals("Celsius"))
            {
                return (int)((temp - 32) / 1.8);
            }
            return temp;
        }
    }
}
