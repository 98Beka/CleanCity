namespace CleanCity.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string ContentBase64String { get; set; }
        public string ContentType { get; set; }
        public long PointOnTheMapId { get; set; }
        public PointOnTheMap PointOnTheMap { get; set; }
    }
}
