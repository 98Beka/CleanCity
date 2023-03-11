namespace CleanCity.Models
{
    public class PointOnTheMap: BaseEntity
    {
        public bool IsPublish { get; set; }
        public bool IsCleaned { get; set; }
        public bool IsRequestCleaned { get; set; }
        public bool IsRequestDelete { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
