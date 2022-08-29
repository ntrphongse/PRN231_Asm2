using BusinessObject;
using eBookStoreLibrary;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RoleDAO
    {
        private static RoleDAO instance = null;
        private static readonly object instanceLock = new object();

        private RoleDAO()
        {

        }

        public static RoleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoleDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            var db = new eStoreContext();
            return await db.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleAsync(int roleId)
        {
            var db = new eStoreContext();
            return await db.Roles.FindAsync(roleId);
        }

        public async Task<Role> AddRoleAsync(Role newRole)
        {
            CheckRole(newRole);
            var db = new eStoreContext();
            await db.Roles.AddAsync(newRole);
            await db.SaveChangesAsync();

            return newRole;
        }

        public async Task<Role> UpdateRoleAsync(Role updatedRole)
        {
            if (await GetRoleAsync(updatedRole.RoleId) == null)
            {
                throw new ApplicationException("Role does not exist!!");
            }
            CheckRole(updatedRole);
            var db = new eStoreContext();
            db.Roles.Update(updatedRole);
            await db.SaveChangesAsync();
            return updatedRole;
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            Role deletedRole = await GetRoleAsync(roleId);
            if (deletedRole == null)
            {
                throw new ApplicationException("Role does not exist!!");
            }
            var db = new eStoreContext();
            db.Roles.Remove(deletedRole);
            await db.SaveChangesAsync();
        }

        private void CheckRole(Role role)
        {
            role.RoleDesc.StringValidate(
                allowEmpty: false,
                emptyErrorMessage: "Role Description is required!!",
                minLength: 2,
                minLengthErrorMessage: "Role Description must have at least 2 characters!!",
                maxLength: 50,
                maxLengthErrorMessage: "Role Description is limited to 50 characters!!"
                );
        }
    }
}
