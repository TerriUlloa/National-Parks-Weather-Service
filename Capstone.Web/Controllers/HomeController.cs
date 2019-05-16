using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Capstone.Web.Models;
using SessionControllerData;
using Capstone.Web.DAO;
using Microsoft.AspNetCore.Http;


namespace Capstone.Web.Controllers
{
    public class HomeController : SessionController
    {
        private IParkDao _parkDao;

        public HomeController(IParkDao dao)
        {
            _parkDao = dao;
        }

        public IActionResult Index()
        {
            var parksList = _parkDao.GetAllParks();
            return View(parksList);
        }

        [HttpGet]
        public IActionResult Details(string parkCode)
        {

            var park = _parkDao.GetSinglePark(parkCode);
            park.Weather = _parkDao.GetWeather(parkCode);
            var TempType = SetTemperatureType();
            park.TemperatureType = TempType;
            return View("Details",park);
        }

        [HttpGet]
        public IActionResult DetailsToggleTemp(string parkCode)
        {
            var TempType = SetTemperatureType();
            if (TempType.Equals("Fahrenheit"))
            {
                SetSessionData("TempType", "Celsius");
            }
            else
            {
                SetSessionData("TempType", "Fahrenheit");
            }
            return Details(parkCode);
        }


        [HttpGet]
        public IActionResult Survey()
        {
            DailySurvey survey = new DailySurvey();
            return View("Survey",survey);
        }

        [HttpPost]
        public IActionResult Survey(DailySurvey survey)
        {
            IActionResult result = null;
            if (!ModelState.IsValid)
            {
                result = View("Survey", survey);
            }
            else
            {
                _parkDao.AddSurvey(survey);
                result = RedirectToAction("FavoriteParks");
            }
            return result;
        }


        public IActionResult FavoriteParks()
        {
            var favoriteParks = _parkDao.GetFavoriteParks();
            return View(favoriteParks);
        }
        private string SetTemperatureType()
        {
            string result = "Fahrenheit";
      
            if (GetSessionData<string>("TempType") == null)
            {
                SetSessionData("TempType", result);
            }
            else
            {
                result = GetSessionData<string>("TempType");
            }
            return result;
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
