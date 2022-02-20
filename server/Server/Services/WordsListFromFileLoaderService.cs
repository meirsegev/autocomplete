using Server.Interfaces;
using Server.Models;
using Server.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Services
{
    public class WordsListFromFileLoaderService : IWordsListLoaderService
    {
        public async Task<List<string>> LoadInitialWordsListAsync(CancellationToken ct)
        {
            // in real implementation this should be an asnyc read from db of
            // all city names and priority, here just from a file.
            var cities = await GetAllCitiesAsync(ct);
            return cities.Select(v => v.Name).ToList();
        }

        public async static Task<List<CityModel>> GetAllCitiesAsync(CancellationToken ct)
        {
            var lines = await File.ReadAllLinesAsync("world-cities.csv", ct);

            List<CityModel> values = lines
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
