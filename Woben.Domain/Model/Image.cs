using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public String Name { get; set; }

        /// <summary>
        /// Url
        /// </summary>        
        public int Url { get; set; }

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
