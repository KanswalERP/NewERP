﻿using System.Collections.Generic;

namespace NtierMvc.Model.Admin
{
    public class RoleAssignEntity
    {
        public int ID { get; set; }
        public int SNo { get; set; }
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public string MainMenu { get; set; }
        public string SubMenu { get; set; }
        public string ReadWrite { get; set; }

        public string EmpId { get; set; }
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string Access { get; set; }

        public int totalcount { get; set; }

    }

    public class RoleAssignEntityDetails
    {
        public RoleAssignEntity enqEntity { get; set; }
        public List<RoleAssignEntity> lstEnqEntity { get; set; }
        public int totalcount { get; set; }

        public RoleAssignEntityDetails()
        {
            enqEntity = new RoleAssignEntity();
            lstEnqEntity = new List<RoleAssignEntity>();
        }
    }

}
