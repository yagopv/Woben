using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woben.Domain.Resources;

namespace Woben.Domain.Model
{
    /// <summary>
    /// Images
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Entity identity
        /// </summary>
        [Key]
        public int ImageId { get; set; }

        /// <summary>
        /// Image file Name
        /// </summary>
        [StringLength(300, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public String Name { get; set; }

        /// <summary>
        /// Url
        /// </summary>   
        [StringLength(400, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public String Url { get; set; }

        /// <summary>
        /// Related Products
        /// </summary>
        public ICollection<Product> Products { get; set; }

        /// <summary>
        /// Unique Identity for add/delete/update operations
        /// </summary>
        [Index(IsUnique = true)]
        public Guid Identity { get; set; }
    }
}
