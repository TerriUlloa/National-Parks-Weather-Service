using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.DAO
{
    public interface IParkDao
    {
        /// <summary>
        /// gets a list of all parks
        /// </summary>
        /// <returns></returns>
        IList<Park> GetAllParks();

        /// <summary>
        /// gets a single park for detail display
        /// </summary>
        /// <param name="parkCode"></param>
        /// <returns></returns>
        Park GetSinglePark(string parkCode);

        /// <summary>
        /// gets the weather forecast for five days
        /// </summary>
        /// <param name="parkCode"></param>
        /// <returns></returns>
        List<Weather> GetWeather(string parkCode);

        /// <summary>
        /// returns true if survey was submmitted
        /// </summary>
        /// <param name="survey"></param>
        /// <returns></returns>
         bool AddSurvey(DailySurvey survey);


        /// <summary>
        /// returns a list of all favorite parks
        /// </summary>
        /// <returns></returns>
        IList<FavoriteParks> GetFavoriteParks();
    }
}
