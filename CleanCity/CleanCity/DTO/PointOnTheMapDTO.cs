namespace CleanCity.DTO
{
    public class PointOnTheMapDTO
    {
        public string Phone { get; set; }
        public string Description { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public float Rating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<PhotoDTO> Photos { get; set; }
    }
}
