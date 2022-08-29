using BusinessObject;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RoleRepository : IRoleRepository
    {
        public async Task<Role> AddRoleAsync(Role newRole)
            => await RoleDAO.Instance.AddRoleAsync(newRole);

        public async Task DeleteRoleAsync(int roleId) => await RoleDAO.Instance.DeleteRoleAsync(roleId);

        public async Task<Role> GetRoleAsync(int roleId) => await RoleDAO.Instance.GetRoleAsync(roleId);

        public async Task<IEnumerable<Role>> GetRolesAsync() => await RoleDAO.Instance.GetRolesAsync();

        public async Task<Role> UpdateRoleAsync(Role updatedRole) => await RoleDAO.Instance.UpdateRoleAsync(updatedRole);
    }
}
