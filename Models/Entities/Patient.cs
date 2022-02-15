//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedicalLaboratoryNumber20WebAPI.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Patient()
        {
            this.Blood = new HashSet<Blood>();
        }
    
        public int PatientId { get; set; }
        public string PatientFullName { get; set; }
        public string PatientLogin { get; set; }
        public string PatientPassword { get; set; }
        public string Guid { get; set; }
        public string PatientEmail { get; set; }
        public string SecurityNumber { get; set; }
        public string EIN { get; set; }
        public int SocialTypeId { get; set; }
        public string PatientPhone { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public System.DateTime BirthDate { get; set; }
        public string IPv4 { get; set; }
        public string UserAgent { get; set; }
        public Nullable<int> InsuranceCompanyId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Blood> Blood { get; set; }
        public virtual InsuranceCompany InsuranceCompany { get; set; }
        public virtual PatientSocialType PatientSocialType { get; set; }
    }
}