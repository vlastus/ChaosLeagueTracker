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
    
    public partial class TeamInducements
    {
        public int ID { get; set; }
        public int TeamMatchData { get; set; }
        public int Type { get; set; }
        public Nullable<int> Value { get; set; }
    
        public virtual TeamMatchData TeamMatchData1 { get; set; }
    }
}