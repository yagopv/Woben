using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Woben.Domain.Model
{
    /// <summary>
    /// Features for the products
    /// </summary>
    public class Feature
    {
        /// <summary>
        /// Entity identity
        /// </summary>
        [Key]
        public int FeatureId { get; set; }

        /// <summary>
        /// Feature Name
        /// </summary>
        [Required]
        [StringLength(100)]        
        public string Name { get; set; }

        /// <summary>
        /// Feature Description
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Related product identity
        /// </summary>        
        public int ProductId { get; set; }

        /// <summary>
        /// Related product
        /// </summary>
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        /// <summary>
        /// Unique Identity for add/delete/update operations
        /// </summary>
        [Index(IsUnique=true)]
        public Guid Identity { get; set; }
    }
}