using apbd_kolokwium1.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_kolokwium1.DAL
{
    public interface IActionDBService
    {
        public ActionAndFirefigthersDTO GetActionAndFirefighters(int id);

        public StatusDTO DeleteAction(int id);
    }
}
