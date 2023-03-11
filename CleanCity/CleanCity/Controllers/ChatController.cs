using CleanCity.Data;
using CleanCity.Models;
using CleanCity.Models.ViewModels;
using CleanCity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CleanCity.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class ChatController : ControllerBase {
        private readonly DataContext _dataContext;
        public ChatController(DataContext dataContext) {
            _dataContext = dataContext;
        }
        [HttpPost("AddMessage")]
        [Authorize(Roles = RoleService.AdminRole)]
        public IActionResult AddMessage(MessageDto message) {
            var res = _dataContext.PointOnTheMaps.Where(x => x.Id == message.PointId).Any();
            if(res == null)
                return NotFound("Point wasn't found");
            var messageTmp = new Message {
                PointId = message.PointId,
                Value = message.Value
            
            };
            _dataContext.Messages.Add(messageTmp);
            _dataContext.SaveChanges();
            return Ok();
        }
        [HttpGet("GetMessages")]
        public IActionResult GetMessages(long pointId) {
            var res = _dataContext.Messages.Where(s => s.PointId == pointId).ToList();
            return Ok(res);
        }
    }
}
