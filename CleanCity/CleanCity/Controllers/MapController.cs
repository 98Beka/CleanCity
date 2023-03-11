using AutoMapper;
using CleanCity.Data;
using CleanCity.DTO;
using CleanCity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public MapController(DataContext context, IMapper mapper, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _appEnvironment = appEnvironment;
        }

        [HttpGet("PointsOnTheMaps")]
        public async Task<IEnumerable<PointOnTheMapDTO>> GetAll()
        {
            var points = await _context.PointOnTheMaps.Include(a => a.Photos).ToListAsync();
            var res = new List<PointOnTheMapDTO>();

            foreach (var point in points)
            {
                var tempPoint = _mapper.Map<PointOnTheMapDTO>(point);
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
                Rating = pointOnTheMapDto.Rating,
                Latitude = pointOnTheMapDto.Address[0],
                Longitude = pointOnTheMapDto.Address[1],
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
            return Ok();
        }
        [HttpDelete("Delete")]
        public ActionResult Delete(long Id)
        {
            var point = _context.PointOnTheMaps.Include(a => a.Photos).FirstOrDefault(a => a.Id == Id);

            if (point == null)
            {
                return NotFound();
            }

            _context.PointOnTheMaps.Remove(point);
            _context.SaveChanges();
            return Ok();
        }
    }
}
