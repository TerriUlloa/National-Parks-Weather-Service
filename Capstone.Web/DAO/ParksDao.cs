using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.DAO
{
    
    public class ParkDao : IParkDao
    {
        #region SQL strings  
        private string _singleParkSql = "SELECT * FROM park WHERE parkCode = @parkCode";

        private string _getAllParksSql = "SELECT * FROM park";

        private string _getWeather = "SELECT * FROM weather WHERE parkCode = @parkCode";

        private string _addSurvey = "INSERT INTO survey_result Values (@parkCode, @emailAddress, @state, @activityLevel)";
        private string _favoriteParks = "Select Count(s.parkCode) as Fav, p.parkName, s.parkCode " +
                                        "From survey_result s " +
                                        "Join park p ON s.parkCode = p.parkCode " +
                                        "Group by s.parkCode, p.parkName " +
                                        "Order by Fav desc";
        #endregion

        /// <summary>
        /// Member Variables
        /// </summary>
        private string _connString;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Connection String"></param>
        public ParkDao(string conn)
        {
            _connString = conn;
        }

        /// <summary>
        /// Gets a list of all the parks from database 
        /// </summary>
        /// <returns>list of parks</returns>
        public IList<Park> GetAllParks()
        {
            IList<Park> parks = new List<Park>();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(_getAllParksSql, conn);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Park p = ReadParks(reader);
                        parks.Add(p);
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
            return parks;
        }

        /// <summary>
        /// Gets a list of a selected park
        /// </summary>
        /// <param name="parkCode"></param>
        /// <returns>Single park list</returns>
        public Park GetSinglePark(string parkCode)
        {
            Park park = new Park();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(_singleParkSql, conn);

                    cmd.Parameters.AddWithValue("@parkCode", parkCode);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        park = ReadParks(reader);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return park;
        }

        /// <summary>
        /// Gets a 5 day forecast for individual park
        /// </summary>
        /// <param name="parkCode"></param>
        /// <returns>list of weather for all parks</returns>
        public List<Weather> GetWeather(string parkCode)
        {
            List<Weather> weather = new List<Weather>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(_getWeather, conn);

                cmd.Parameters.AddWithValue("@parkCode", parkCode);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var w = new Weather();
                    w.FiveDayForecastValue = Convert.ToInt32(reader["fiveDayForecastValue"]);
                    w.Forecast = Convert.ToString(reader["forecast"]);
                    w.High = Convert.ToInt32(reader["high"]);
                    w.Low = Convert.ToInt32(reader["low"]);
                    w.ParkCode = Convert.ToString(reader["parkCode"]);
                    weather.Add(w);
                }
            }
            return weather;
        }


        /// <summary>
        /// Adds a new survey to the database.
        /// </summary>
        /// <param name="survey"></param>
        /// <returns>true if successful</returns>
        public bool AddSurvey(DailySurvey survey)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(_addSurvey, conn);
                cmd.Parameters.AddWithValue("@parkCode", survey.ParkCode);
                cmd.Parameters.AddWithValue("@emailAddress", survey.EmailAddress);
                cmd.Parameters.AddWithValue("@state", survey.State);
                cmd.Parameters.AddWithValue("@activityLevel", survey.ActivityLevel);
                int rowsAffected = cmd.ExecuteNonQuery();

                return (rowsAffected > 0);

            }
            
        }

        /// <summary>
        /// Gets a list of all surveys for each park
        /// </summary>
        /// <returns>list of surveys</returns>
        public IList<FavoriteParks> GetFavoriteParks()
        {
            IList<FavoriteParks> surveys = new List<FavoriteParks>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(_favoriteParks, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var f = new FavoriteParks();

                    f.NumberOfSurveys = Convert.ToInt32(reader["fav"]);
                    f.ParkCode = Convert.ToString(reader["parkCode"]);
                    f.ParkName = Convert.ToString(reader["parkName"]);

                    surveys.Add(f);
                }
            }
            return surveys;
        }

        /// <summary>
        /// Sql reader method for adding park database info to parks list.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>single park</returns>
        private Park ReadParks(SqlDataReader reader)
        {
            var p = new Park();
            p.Acreage = Convert.ToInt32(reader["acreage"]);
            p.AnnualVisitorCount = Convert.ToInt32(reader["annualVisitorCount"]);
            p.Climate = Convert.ToString(reader["climate"]);
            p.ElevationInFeet = Convert.ToInt32(reader["elevationInFeet"]);
            p.EntryFee = Convert.ToDecimal(reader["entryFee"]);
            p.InspirationalQuote = Convert.ToString(reader["inspirationalQuote"]);
            p.InspirationalQuoteSource = Convert.ToString(reader["inspirationalQuoteSource"]);
            p.MilesOfTrail = Convert.ToDecimal(reader["milesOfTrail"]);
            p.NumberOfAnimalSpecies = Convert.ToInt32(reader["numberOfAnimalSpecies"]);
            p.NumberOfCampsites = Convert.ToInt32(reader["numberOfCampsites"]);
            p.ParkCode = Convert.ToString(reader["parkCode"]);
            p.ParkDescription = Convert.ToString(reader["parkDescription"]);
            p.ParkName = Convert.ToString(reader["parkName"]);
            p.State = Convert.ToString(reader["state"]);
            p.YearFounded = Convert.ToInt32(reader["yearFounded"]);

            return p;
        }

    }
}
