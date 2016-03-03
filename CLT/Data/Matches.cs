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
    
    public partial class Matches
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Matches()
        {
            this.MatchEvents = new HashSet<MatchEvents>();
        }
    
        public int ID { get; set; }
        public int Competition { get; set; }
        public Nullable<int> Round { get; set; }
        public int Team1Data { get; set; }
        public int Team2Data { get; set; }
        public System.DateTime Date { get; set; }
        public int Fixture { get; set; }
    
        public virtual Competitions Competitions { get; set; }
        public virtual TeamMatchData TeamMatchData { get; set; }
        public virtual TeamMatchData TeamMatchData1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MatchEvents> MatchEvents { get; set; }
        public virtual Fixtures Fixtures { get; set; }
    }
}
