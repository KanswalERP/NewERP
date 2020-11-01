﻿using NtierMvc.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtierMvc.Model.Vendor
{
    public class VendorEntity
    {
        public bool? IsActive { get; set; }
        public string ipAddress { get; set; }
        public string sessionid { get; set; }
        public string UserInitial { get; set; }
        public string UnitNo { get; set; }
        public string VendorId { get; set; }
        public string VendorTypeId { get; set; }
        public string VendorType { get; set; }
        public string VendorNatureId { get; set; }
        public string VendorNature { get; set; }
        public string FunctionAreaId { get; set; }
        public string FunctionArea { get; set; }
        public string VendorName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string tel1 { get; set; }
        public string tel2 { get; set; }
        public string mob1 { get; set; }
        public string mob2 { get; set; }
        public string fax { get; set; }
        public string email1 { get; set; }
        public string email2 { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string VendorInitial { get; set; }
        public int TotalCount { get; set; }

    }

    public class VendorEntityDetails
    {
        public VendorEntity vendEnt { get; set; }
        public List<VendorEntity> LstVendEnt { get; set; }
        public int totalcount { get; set; }
        public VendorEntityDetails()
        {
            vendEnt = new VendorEntity();
            LstVendEnt = new List<VendorEntity>();
        }
    }

}
