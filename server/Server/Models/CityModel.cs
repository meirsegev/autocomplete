using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class CityModel
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string SubCountry { get; set; }
        public int GeoId { get; set; }
    }
}
