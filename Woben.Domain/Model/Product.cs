using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Represents a Post for a given Blog
    /// </summary>
    public class Product : AuditInfoComplete
    {
        /// <summary>
        /// Entity identity
        /// </summary>
        [Key]
        public int ProductId { get; set; }

        /// <summary>
        /// Title of this product
        /// </summary>
        [Display(ResourceType = typeof(DomainResources), Name = "Name")]
        [Required(ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public string Name { get; set; }

        /// <summary>
        /// The title accesible by url
        /// </summary>
        [StringLength(200, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public string UrlCodeReference { get; set; }

        /// <summary>
        /// Description for this product
        /// </summary>
        [Display(ResourceType = typeof(DomainResources), Name = "Description")]
        [StringLength(500, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public string Description { get; set; }

        /// <summary>
        /// Url for the image representing the post
        /// </summary>
        [StringLength(500, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Will store markdown text
        /// </summary>
        public string Markdown { get; set; }

        /// <summary>
        /// Will store the markdown converted to html for quicker rendering
        /// </summary>
        [DataType(DataType.Html)]
        public string Html { get; set; }

        /// <summary>
        /// If the product is published and visible to others or not
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Foreign CategoryId key
        /// </summary> 
        public int? CategoryId { get; set; }

        /// <summary>
        /// Related Category
        /// </summary>
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        /// <summary>
        /// Related Tags
        /// </summary>
        public  ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// Related Features
        /// </summary>
        public ICollection<Feature> Features { get; set; }

        /// <summary>
        /// Related Messages
        /// </summary>
        public ICollection<Notification> Notifications { get; set; }

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
