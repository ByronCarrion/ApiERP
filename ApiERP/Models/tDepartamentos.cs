//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiERP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tDepartamentos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tDepartamentos()
        {
            this.tDepartamentos1 = new HashSet<tDepartamentos>();
        }
    
        public string idDepartamento { get; set; }
        public string NombreDepartamento { get; set; }
        public string idDepartamentoPadre { get; set; }
        public string UsuarioAvisoRecepcion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tDepartamentos> tDepartamentos1 { get; set; }
        public virtual tDepartamentos tDepartamentos2 { get; set; }
    }
}
