using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Model
{
    public class Member : IDisposable
    {
	    public string Account { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public object Menus { get; set; }

        public Member (int id,string name, int roleId, string roleName, int departmentId, string departmentName, object menus)
        {
            this.Id = id;
            this.Name = name;
            this.RoleId = roleId;
            this.RoleName = roleName;
            this.DepartmentId = departmentId;
            this.DepartmentName = departmentName;
            this.Menus = menus;
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
