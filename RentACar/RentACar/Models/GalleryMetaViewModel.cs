using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.Models
{
    public class GalleryMetaViewModel
    {
        public int Identifier { get; set; }
        public string Title { get; set; }

        public string Make { get; set; }

        public int Year { get; set; }

        public string Transmission { get; set; }

        public decimal Fare1 { get; set; }

        public decimal Fare2 { get; set; }

        public decimal Fare3 { get; set; }

        public VehicleCategory Category { get; set; }

        public string ImagePath { get; set; }
    }
}