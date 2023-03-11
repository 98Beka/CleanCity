using CleanCity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanCity.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly DataContext _context;
        public MapController(DataContext context)
        {
            _context = context;
        }


    }
}
