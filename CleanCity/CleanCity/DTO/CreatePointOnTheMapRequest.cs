namespace CleanCity.DTO
{
    public class CreatePointOnTheMapRequest
    {
        public string Phone { get; set; }
        public string Description { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public float Rating { get; set; }
        public double[] Address { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
