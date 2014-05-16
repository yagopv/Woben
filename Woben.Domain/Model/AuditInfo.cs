using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date the entity was updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        ///  User creating the entity
        /// </summary>
        [StringLength(100)]
        [Index(IsUnique=false)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// User updating the entity
        /// </summary>
        [StringLength(100)]
        [Index(IsUnique = false)]
        public string UpdatedBy { get; set; }
    }
}
