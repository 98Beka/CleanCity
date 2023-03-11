using System.ComponentModel.DataAnnotations.Schema;

namespace CleanCity.Models
{
    public class Like
    {
        public long Id { get; set; }
        public int Value { get; set; }
        public string Ip { get; set; }
        public long PointOnTheMapId { get; set; }
        [ForeignKey(nameof(PointOnTheMapId))]
        public PointOnTheMap PointOnTheMap { get; set; }
    }
}
