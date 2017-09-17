using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FindMyDistance.Models
{
    public class Distance
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
    }

    public class GetDistance
    {
        public string StartLattitude { get; set; }
        public string StartLongitude { get; set; }
        public string EndLattitude { get; set; }
        public string EndLongitude { get; set; }
        public string DistanceBetween { get; set; }
    }
}
