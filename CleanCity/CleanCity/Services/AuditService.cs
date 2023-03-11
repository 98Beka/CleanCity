using CleanCity.Data;
namespace CleanCity.Services;

public class AuditService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuditService(DataContext context, IHttpContextAccessor httpContextAccessor) {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }
    public enum ActionType {
        Create = 0,
        Update = 1,
        Delete = 2
    }
    public void AuditCreate( ActionType actionType, string? entityName = null) {
        string createdBy = _httpContextAccessor.HttpContext.User.Identity.Name;
        //var jsonString = JsonConvert.SerializeObject(entity);

        //long Id = entity.Id;
        //if (string.IsNullOrEmpty(entityName))
        //    entityName = entity.GetType().Name;

        _context.SaveChanges();
    }
}
