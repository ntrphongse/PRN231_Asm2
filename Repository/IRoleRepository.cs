using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<Role> GetRoleAsync(int roleId);
        Task<Role> AddRoleAsync(Role newRole);
        Task<Role> UpdateRoleAsync(Role updatedRole);
        Task DeleteRoleAsync(int roleId);
    }
}
