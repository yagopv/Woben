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
        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// The title accesible by url
        /// </summary>
        [StringLength(200)]
        public string UrlCodeReference { get; set; }

        /// <summary>
        /// Description for this product
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Url for the image representing the post
        /// </summary>
        [Required]
        [StringLength(500)]
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
        [Required]
        public bool IsPublished { get; set; }


        /// <summary>
        /// Foreign CategoryId key
        /// </summary> 
        [DataMember]
        public int CategoryId { get; set; }

        /// <summary>
        /// Related Category
        /// </summary>
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        /// <summary>
        /// Related Tags
        /// </summary>
        [DataMember]
        public  ICollection<Tag> Tags { get; set; }

        /// <summary>
        /// Create a url reference
        /// </summary>
        /// <param name="title">The string to convert</param>
        public void SetUrlReference()
        {
            if (String.IsNullOrEmpty(this.Title))
            {
                return;
            }

            char[] arr = this.Title.Where(c => (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))).ToArray();
            var urlcodereference = new string(arr);
            this.UrlCodeReference = urlcodereference.Trim().ToLower().Replace(" ", "-");
        }
    }
}
