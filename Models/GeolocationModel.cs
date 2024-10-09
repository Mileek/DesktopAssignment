using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopAssignment.Models
{
    public class GeolocationModel
    {
        //Primary key - Naming convention
        public int Id { get; set; }

        //Selected fields from the response
        public string Ip { get; set; }
        public string Type { get; set; }
        public string ContinentName { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
