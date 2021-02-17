using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.Models.ViewModels
{
    public class CartViewModel
    {
        public OrderHeader OrderHeader { get; set; }
        public IList<Service> ServiceList { get; set; }
    }
}
