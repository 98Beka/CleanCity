using AutoMapper;
using CleanCity.Data;
using CleanCity.DTO;
using CleanCity.Helpers;
using CleanCity.Models;
using CleanCity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Principal;

namespace CleanCity.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly RatingService _ratingService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MapController(DataContext context, IMapper mapper, IWebHostEnvironment appEnvironment, RatingService ratingService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _appEnvironment = appEnvironment;
            _ratingService = ratingService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("PointsOnTheMaps")]
        public async Task<IEnumerable<PointOnTheMapDTO>> GetAll()
        {
            //var points = await _context.PointOnTheMaps.Where(a => a.IsPublish).Include(a => a.Photos).ToListAsync();
            var points = await _context.PointOnTheMaps.Include(a => a.Photos).ToListAsync();
            var res = new List<PointOnTheMapDTO>();

            foreach (var point in points)
            {
                var tempPoint = _mapper.Map<PointOnTheMapDTO>(point);
                tempPoint.Rating = _ratingService.GetRating(tempPoint.Id);
                tempPoint.FilesBase64 = new List<string>();

                foreach (var photo in point.Photos)
                {
                    tempPoint.FilesBase64.Add(photo.ContentBase64String);
                }

                res.Add(tempPoint);
            }
            return res;
        }
        [HttpPost("Create")]
        public ActionResult Create([FromForm] CreatePointOnTheMapRequest pointOnTheMapDto)
        {
            var pointOnTheMap = new PointOnTheMap
            {
                Phone = pointOnTheMapDto.Phone,
                Description = pointOnTheMapDto.Description,
                FIO = pointOnTheMapDto.FIO,
                Email = pointOnTheMapDto.Email,
                Address = pointOnTheMapDto.Address,
                Latitude = pointOnTheMapDto.Position[0],
                Longitude = pointOnTheMapDto.Position[1],
                CreatedAt = DateTime.Now,
            };

            if (pointOnTheMapDto.Photos != null && pointOnTheMapDto.Photos.Count > 0)
            {
                try
                {
                    var photos = new List<Photo>();

                    foreach (var photo in pointOnTheMapDto.Photos)
                    {
                        var tempPhoto = new Photo()
                        {
                            ContentType = photo.ContentType,
                        };

                        // путь к файлу
                        string path = "/" + photo.FileName;
                        // сохраняем файл в каталог wwwroot
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            photo.CopyTo(fileStream);
                        }
                        String fileBase64String = "";
                        using (FileStream reader = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Open))
                        {
                            byte[] buffer = new byte[reader.Length];
                            reader.Read(buffer, 0, (int)reader.Length);
                            fileBase64String = Convert.ToBase64String(buffer);
                        }
                        tempPhoto.ContentBase64String = fileBase64String;
                        // удаляем файл с каталога wwwroot
                        System.IO.File.Delete(_appEnvironment.WebRootPath + path);

                        photos.Add(tempPhoto);
                    }
                    pointOnTheMap.Photos = photos;
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            _context.PointOnTheMaps.Add(pointOnTheMap);
            _context.SaveChanges();

            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            var userEmail = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault()?.Value;

            var likeDTO = new LikeDTO
            {
                PointOnTheMapId = pointOnTheMap.Id,
                Value = pointOnTheMapDto.RatingValue
            };
            
            if(_ratingService.AddLike(likeDTO, ip, userEmail))
            {
                return Ok();
            }

            return BadRequest("Like Exeption");
        }
        [HttpDelete("Delete")]
        public ActionResult Delete(long Id)
        {
            var point = _context.PointOnTheMaps.Include(a => a.Photos).FirstOrDefault(a => a.Id == Id);

            if (point == null)
            {
                return NotFound();
            }
            _ratingService.DeleteLikes(Id);
            _context.PointOnTheMaps.Remove(point);
            _context.SaveChanges();
            
            return Ok();
        }
        [HttpPost("Like")]
        public ActionResult Like(LikeDTO likeDTO)
        {
            var point = _context.PointOnTheMaps.Include(a => a.Photos).FirstOrDefault(a => a.Id == likeDTO.PointOnTheMapId);

            if (point == null)
            {
                return NotFound();
            }

            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            
            var userEmail = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault()?.Value;

            var newLikeDTO = new LikeDTO
            {
                PointOnTheMapId = likeDTO.PointOnTheMapId,
                Value = likeDTO.Value
            };

            if (!_ratingService.AddLike(likeDTO, ip, userEmail))
            {
                return BadRequest("Already liked");
            }
            return Ok();
        }
        [HttpGet("UnPublishedPointsOnTheMaps")]
        [Authorize(Roles = RoleService.AdminRole)]
        public async Task<IEnumerable<PointOnTheMapDTO>> GetAllUnPublished()
        {
            var points = await _context.PointOnTheMaps.Where(a => !a.IsPublish).Include(a => a.Photos).ToListAsync();
            var res = new List<PointOnTheMapDTO>();

            foreach (var point in points)
            {
                var tempPoint = _mapper.Map<PointOnTheMapDTO>(point);
                tempPoint.Rating = _ratingService.GetRating(tempPoint.Id);
                tempPoint.FilesBase64 = new List<string>();

                foreach (var photo in point.Photos)
                {
                    tempPoint.FilesBase64.Add(photo.ContentBase64String);
                }

                res.Add(tempPoint);
            }
            return res;
        }
        [HttpPost("Approve")]
        [Authorize(Roles = RoleService.AdminRole)]
        public ActionResult Approve(long pointId, Constants.Action action)
        {
            var point = _context.PointOnTheMaps.Include(a => a.Photos).FirstOrDefault(a => a.Id == pointId);

            if (point == null)
            {
                return NotFound();
            }

            if (action == Constants.Action.Add)
            {
                point.IsPublish = true;
                _context.PointOnTheMaps.Update(point);
                _context.SaveChanges();

                return Ok();
            }

            _context.PointOnTheMaps.Remove(point);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPost("Deny")]
        [Authorize(Roles = RoleService.AdminRole)]
        public ActionResult Deny(long pointId, Constants.Action action)
        {
            var point = _context.PointOnTheMaps.Include(a => a.Photos).FirstOrDefault(a => a.Id == pointId);

            if (point == null)
            {
                return NotFound();
            }

            if (action == Constants.Action.Add)
            {
                _context.PointOnTheMaps.Remove(point);
                _context.SaveChanges();

                return Ok();
            }
            return Ok();
        }
    }
}
