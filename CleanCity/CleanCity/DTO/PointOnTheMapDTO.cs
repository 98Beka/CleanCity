namespace CleanCity.DTO
{
    public class PointOnTheMapDTO
    {
        public long Id { get; set; }
        public bool IsCleaned { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string FIO { get; set; }
        public string Email { get; set; }
        public float Rating { get; set; }
        public string Address { get; set; }
        public double[] Position { get; set; }
        public List<string> FilesBase64 { get; set; }
    }
}
