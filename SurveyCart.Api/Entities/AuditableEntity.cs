namespace SurveyCart.Api.Entities
{
    public class AuditableEntity
    {
        public string CreatedById { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? UpdatedById { get; set; } = string.Empty;
        public DateTime? UpdatedOn { get; set; } = DateTime.UtcNow;
        public User CreatedBy { get; set; } = default!;
        public User? UdateddBy { get; set; } = default!;
    }
}
