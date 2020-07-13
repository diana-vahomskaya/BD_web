using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workers;

namespace Workers.Models
{
    public class PlaceViewModel
    {
        public List<WorkersModel> workers { get; set; }
        public SelectList Place { get; set; }
        public string WorkersPlace { get; set; }
        public string SearchString { get; set; }
    }
}
