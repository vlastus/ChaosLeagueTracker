//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CLT.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class TypeGroups
    {
        public int ID { get; set; }
        public int PlayerType { get; set; }
        public int SkillGroup { get; set; }
        public Nullable<int> IsDouble { get; set; }
    
        public virtual PlayerTypes PlayerTypes { get; set; }
    }
}