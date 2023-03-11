using CleanCity.Data;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace CleanCity.Services {
    public class RoleService {
        private readonly DataContext context;
        private readonly UserManager<IdentityUser> userManager;
        public const string AdminRole = "admin";
        public const string UserRole = "user";
        public const string RegistriesSenderRole = "registriesSender";
        private string adminRoleId;
        private string userRoleId;
        private string registriesSenderRoleId;
        public RoleService(DataContext context, UserManager<IdentityUser> userManager) {
            this.context = context;
            this.userManager = userManager;
            adminRoleId = context.Roles.Where(r => r.Name == AdminRole).Select(s => s.Id).FirstOrDefault() ?? string.Empty;
            userRoleId = context.Roles.Where(r => r.Name == UserRole).Select(s => s.Id).FirstOrDefault() ?? string.Empty;
            registriesSenderRoleId = context.Roles.Where(r => r.Name == RegistriesSenderRole).Select(s => s.Id).FirstOrDefault() ?? string.Empty;
        }

        public string GetRole(string id) {
            return context.UserRoles.Any(s => s.UserId == id && s.RoleId == adminRoleId) ? AdminRole :
                context.UserRoles.Any(s => s.UserId == id && s.RoleId == userRoleId) ? UserRole :
                context.UserRoles.Any(s => s.UserId == id && s.RoleId == registriesSenderRoleId) ? RegistriesSenderRole : "none role";
        }

        public async Task AddRolesAsync(IdentityUser identityUser, string role) {
            switch (role) {
                case UserRole:
                    await userManager.AddToRoleAsync(identityUser, UserRole);
                    return;
                case RegistriesSenderRole:
                    await userManager.AddToRoleAsync(identityUser, RegistriesSenderRole);
                    return;
                case AdminRole:
                    await userManager.AddToRoleAsync(identityUser, UserRole);
                    await userManager.AddToRoleAsync(identityUser, RegistriesSenderRole);
                    await userManager.AddToRoleAsync(identityUser, AdminRole);
                    return;
            }
        }
    }
}
