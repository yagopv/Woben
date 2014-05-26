using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Woben.Domain.Resources;

namespace Woben.Domain.Model
{
    /// <summary>
    /// Tags for the products
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Entity identity
        /// </summary>
        [Key]
        public int TagId { get; set; }

        /// <summary>
        /// Tag Name
        /// </summary>
        [Index]
        [Display(ResourceType = typeof(ModelValidation), Name = "Name")]
        [Required(ErrorMessageResourceType = typeof(ModelValidation), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(ModelValidation), ErrorMessageResourceName = "MaxLength")]
        public string Name { get; set; }

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
        /// The category name accesible by url
        /// </summary>
        [Index(IsUnique = true)]
        [StringLength(100)]        
        public string UrlCodeReference { get; set; }

        /// <summary>
        /// Unique Identity for add/delete/update operations
        /// </summary>
        [Index(IsUnique = true)]
        public Guid Identity { get; set; }

        /// <summary>
        /// Create a url reference
        /// </summary>
        /// <param name="title">The string to convert</param>
        public void SetUrlReference()
        {
            if (String.IsNullOrEmpty(this.Name))
            {
                return;
            }

            char[] arr = this.Name.Where(c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))).ToArray();
            var urlcodereference = new string(arr);
            this.UrlCodeReference = urlcodereference.Trim().ToLower().Replace(" ", "-");
        }
    }
}
