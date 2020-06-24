using apbd_kolokwium1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_kolokwium1.DTO
{
    public class ActionAndFirefigthersDTO
    {
        public ActionFirefighter Action { get; set; }
        public IEnumerable<Firefighter> Firefighters { get; set; }
    }
}
