using Server.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Tools
{
    public class CsvParser
    {
        public static List<CityModel> GetAllCities()
        {
            List<CityModel> values = File.ReadAllLines("world-cities.csv")
                                           .Skip(1)
                                           .Select(v => FromCsv(v))
                                           .Where(v => !string.IsNullOrEmpty(v.Name))
                                           .ToList();
            return values;
        }

        public static CityModel FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            if (values.Length != 4)
                return new CityModel() { Name = null };

            CityModel city = new CityModel();
            city.Name = values[0];
            city.Country = values[1];
            city.SubCountry = values[2];
            city.GeoId = Convert.ToInt32(values[3]);
            
            return city;
        }
    }
}
