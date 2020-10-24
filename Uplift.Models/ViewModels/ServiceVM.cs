using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.Models.ViewModels
{

    // ViewModels são view que são altamente especializadas para um controler.
    public class ServiceVM
    {
        public Service Service { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> FrequencyList { get; set; }
    }
}
