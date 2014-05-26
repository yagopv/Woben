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
    /// Categories for the products
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Entity identity
        /// </summary>
        [Key]
        public int CategoryId { get; set; }

        /// <summary>
        /// Category Name
        /// </summary>
        [Index(IsUnique=true)]
        [Display(ResourceType = typeof(DomainResources), Name = "Name")]
        [Required(ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public string Name { get; set; }

        /// <summary>
        /// Category description
        /// </summary>
        [Display(ResourceType = typeof(DomainResources), Name = "Description")]
        [StringLength(1000, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }

        /// <summary>
        /// The category name accesible by url
        /// </summary>
        [Index(IsUnique=true)]
        [StringLength(100, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public string UrlCodeReference { get; set; }

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
