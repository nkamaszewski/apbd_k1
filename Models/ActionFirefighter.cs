using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_kolokwium1.Models
{
    public class ActionFirefighter
    {
        public int IdAction { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Byte NeedSpecialEquipment { get; set; }
    }
}
