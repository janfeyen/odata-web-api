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
    
    public partial class StoreRegistration
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Status { get; set; }
    
        public virtual DeviceInfo DeviceInfo { get; set; }
        public virtual Store Store { get; set; }
    }
}