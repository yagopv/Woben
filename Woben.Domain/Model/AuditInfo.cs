using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

using Woben.Domain.Resources;

namespace Woben.Domain.Model
{
    /// <summary>
    ///  Audit info for the different Entities
    /// </summary>
    public abstract class AuditInfoBase
    {
        /// <summary>
        /// ConcurrencyCheck
        /// </summary>        
        [ConcurrencyCheck]
        public int RowVersion { get; internal set; }
    }

    /// <summary>
    ///  Audit info for the different Entities
    /// </summary>
    public abstract class AuditInfoComplete : AuditInfoBase
    {
        /// <summary>
        /// Date the entity was created
        /// </summary>
        [Display(ResourceType = typeof(DomainResources), Name = "CreatedDate")]
        [DataType(DataType.DateTime, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "InvalidDate")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date the entity was updated
        /// </summary>
        [DataType(DataType.DateTime, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "InvalidDate")]
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        ///  User creating the entity
        /// </summary>
        [Index(IsUnique = false)]
        [StringLength(100, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]        
        public string CreatedBy { get; set; }

        /// <summary>
        /// User updating the entity
        /// </summary>
        [Index(IsUnique = false)]
        [Display(ResourceType = typeof(DomainResources), Name = "UpdatedBy")]
        [StringLength(100, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public string UpdatedBy { get; set; }
    }
}
