using CleanCity.Data;
using CleanCity.DTO;
using CleanCity.Models;
using System.Net;

namespace CleanCity.Services
{
    public class RatingService
    {
        private readonly DataContext _context;
        private float? maxPoint = null;
        public RatingService(DataContext context)
        {
            _context = context;
        }
        public bool AddLike(LikeDTO likeDTO, string ip)
        {
            var like = _context.Likes.FirstOrDefault(a => a.PointOnTheMapId == likeDTO.PointOnTheMapId && a.Ip == ip);

            if (like != null)
            {
                return false;
            }

            var newLike = new Like
            {
                PointOnTheMapId = likeDTO.PointOnTheMapId,
                Value = likeDTO.Value,
                Ip = ip
            };

            _context.Likes.Add(newLike);
            _context.SaveChanges();
            maxPoint = _context.Likes.GroupBy(a => new { a.PointOnTheMapId, a.Value }).Select(a => a.Select(b => b.Value).Sum()).Max();
            return true;
        }
        public bool DeleteLikes(long pointId)
        {
            var point = _context.PointOnTheMaps.FirstOrDefault(a => a.Id == pointId);

            if (point == null)
            {
                return false;
            }

            var likes = _context.Likes.Where(a => a.PointOnTheMapId == pointId);

            _context.Likes.RemoveRange(likes);
            _context.SaveChanges();
            maxPoint = _context.Likes.GroupBy(a => new { a.PointOnTheMapId, a.Value }).Select(a => a.Select(b => b.Value).Sum()).Max();
            return true;
        }
        public float GetRating(long pointId)
        {
            var point = _context.PointOnTheMaps.FirstOrDefault(a => a.Id == pointId);

            if (point == null)
            {
                return 0.0F;
            }

            if(maxPoint == null)
            {
               maxPoint = _context.Likes.GroupBy(a => new { a.PointOnTheMapId, a.Value}).Select(a => a.Select(b => b.Value).Sum()).Max();
            }

            var pointLikes = _context.Likes.Where(a => a.PointOnTheMapId == pointId).Select(a => a.Value).Sum();

            return pointLikes / maxPoint.Value;
        }
    }
}
