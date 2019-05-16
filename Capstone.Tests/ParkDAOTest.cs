
using Capstone.Web.DAO;
using Capstone.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class ParkDAOTest
    {
        private TransactionScope tran;      //<-- used to begin a transaction during initialize and rollback during cleanup
        private string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=NPGeek;Integrated Security=True";
        private int _myPark = 0;
        private int _myWeather = 0;


        // Set up the database before each test        
        [TestInitialize]
        public void Initialize()
        {
            
            // Initialize a new transaction scope. This automatically begins the transaction.
            tran = new TransactionScope();

            // Open a SqlConnection object using the active transaction
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd;

                //New Test Park
                cmd = new SqlCommand("INSERT INTO park Values ('ZZZ','Test Park', 'Ohio', 0, 0, 30, 0, 'Rainforest', 2019, 2, 'this is a quote', 'Me', 'this is a park', 0, 12)", conn);
                cmd.ExecuteNonQuery();
                //Get Park Count
                cmd = new SqlCommand("SELECT COUNT(*) FROM park;", conn);
                _myPark = (int)cmd.ExecuteScalar();
                //New Weather test
                cmd = new SqlCommand("INSERT INTO weather Values('ZZZ', 1, -60, -60, 'Ice')," +
                                                               "('ZZZ', 2, 100, 100, 'Fire')," +
                                                               "('ZZZ', 3,  65,  40, 'Nice')," +
                                                               "('ZZZ', 4,  66,  35, 'Nice')," +
                                                               "('ZZZ', 5,  90,  70, 'Hot');", conn);
                                                                _myWeather = cmd.ExecuteNonQuery();
            }

        }

        // Cleanup runs after every single test
        [TestCleanup]
        public void Cleanup()
        {
  
                tran.Dispose(); //<-- disposing the transaction without committing it means it will get rolled back
            
        }

        [TestMethod]
        public void GetParkTest()
        {
            //Arrange
            IParkDao parksDoa = new ParkDao(_connectionString);

            //Act
            IList<Park> parks = parksDoa.GetAllParks();

            //Assert
            Assert.AreEqual(_myPark, parks.Count);
            Assert.AreEqual("ZZZ", parks[parks.Count - 1].ParkCode);
        }

        [TestMethod]
        public void GetSingleParkTest()
        {
            //Arrange
            IParkDao parksDoa = new ParkDao(_connectionString);

            //Act
            Park testpark  = parksDoa.GetSinglePark("zzz");

            //Assert
            Assert.AreEqual("Test Park", testpark.ParkName);
        }

        [TestMethod]
        public void WeatherTest()
        {
            //Arrange
            IParkDao parksDoa = new ParkDao(_connectionString);

            //Act
            List<Weather> testweather = parksDoa.GetWeather("zzz");

            //Assert
            Assert.AreEqual(_myWeather, testweather.Count);
            Assert.AreEqual("Ice", testweather[0].Forecast);
            Assert.AreEqual("Fire", testweather[1].Forecast);
            Assert.AreEqual("Nice", testweather[2].Forecast);
            Assert.AreEqual("Nice", testweather[3].Forecast);
            Assert.AreEqual("Hot", testweather[4].Forecast);
        }





    }
}
