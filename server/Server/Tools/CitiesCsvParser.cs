using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Tools
{
    public class City
    {
        public string Name;
        public string Country;
        public string SubCountry;
        public int GetNameId;

        public static List<City> GetAllCities()
        {
            List<City> values = File.ReadAllLines("world-cities.csv")
                                           .Skip(1)
                                           .Select(v => City.FromCsv(v))
                                           .Where(v => !String.IsNullOrEmpty(v.Name))
                                           .ToList();
            return values;
        }

        public static City FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            if (values.Length != 4)
                return new City() { Name = null };

            City city = new City();
            city.Name = values[0];
            city.Country = values[1];
            city.SubCountry = values[2];
            city.GetNameId = Convert.ToInt32(values[3]);
            
            return city;
        }
    }
}
