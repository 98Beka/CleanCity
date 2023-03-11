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
        private readonly IHttpContextAccessor _contextAccessor;

        public ChatController(IHttpContextAccessor contextAccessor, DataContext dataContext) {
            _contextAccessor = contextAccessor;
            _dataContext = dataContext;
        }

        [HttpPost("AddMessage")]
        [Authorize(Roles = RoleService.UserRole)]
        public IActionResult AddMessage(MessageDto message) {
            var res = _dataContext.PointOnTheMaps.Where(x => x.Id == message.PointId).Any();
            if(res == null)
                return NotFound("Point wasn't found");
            var userEmail = _contextAccessor.HttpContext.User.Claims.First().Value;
            if (userEmail == null)
                return NotFound("User wasn't singed in");
            var messageTmp = new Message {
                PointId = message.PointId,
                Value = message.Value,
                UserEmail = userEmail
            };
            _dataContext.Messages.Add(messageTmp);
            _dataContext.SaveChanges();
            return Ok();
        }
        [HttpGet("GetMessages")]
        public IActionResult GetMessages(long pointId) {
            var res = _dataContext.Messages.Where(s => s.PointId == pointId).ToList();
            
            return Ok(res.Select(s => new {
                Value = s.Value,
                UserEmail = s.UserEmail,
                Name = _dataContext.Users.Where(a => a.Email == s.UserEmail).FirstOrDefault()?.UserName,
            }));
        }
    }
}
