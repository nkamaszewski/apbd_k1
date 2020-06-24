using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apbd_kolokwium1.DAL;
using apbd_kolokwium1.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apbd_kolokwium1.Controllers
{
    [Route("api/actions")]
    [ApiController]
    public class ActionsController : ControllerBase
    {
        private IActionDBService _actionDBService;

        public ActionsController(IActionDBService actionDBService)
        {
            _actionDBService = actionDBService;
        }

        [HttpGet("{id}")]
        public IActionResult GetActionAndFirefighters(int id)
        {
            ActionAndFirefigthersDTO actionAndFirefigthersDTO = _actionDBService.GetActionAndFirefighters(id);

           if( actionAndFirefigthersDTO.Action == null)
            {
                return BadRequest("nie istnieje akcja o podanym id");
            }

            return Ok(actionAndFirefigthersDTO);
        }
    }
}