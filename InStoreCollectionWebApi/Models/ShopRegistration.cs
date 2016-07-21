//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InStoreCollectionWebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ShopRegistration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public double Latitiude { get; set; }
        public double Longitude { get; set; }
        public int ShopId { get; set; }
        public string Status { get; set; }
    
        public virtual DeviceInfo DeviceInfo { get; set; }
        public virtual Shop Shop { get; set; }
    }
}