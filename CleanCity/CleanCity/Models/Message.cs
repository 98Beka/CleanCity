namespace CleanCity.Models {
    public class Message : BaseEntity {
        public long PointId { get; set; }
        public string Value { get; set; }
        public string UserEmail { get; set; }
    }
}
