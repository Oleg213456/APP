//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CurcovaaGolovin.DataBaseControler
{
    using System;
    using System.Collections.Generic;
    
    public partial class Doctor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Doctor()
        {
            this.Inpatient = new HashSet<Inpatient>();
        }
    
        public int IDDoctor { get; set; }
        public string NameDoctor { get; set; }
        public string SurnameDoctor { get; set; }
        public string MiddleNameDoctor { get; set; }
        public System.DateTime DateOfBirthDoctor { get; set; }
        public string PhoneDoctor { get; set; }
        public string AddressDoctor { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inpatient> Inpatient { get; set; }
    }
}
