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
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Category description
        /// </summary>
        [StringLength(1000)]        
        public string Description { get; set; }

        /// <summary>
        /// The category name accesible by url
        /// </summary>
        [StringLength(100)]
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
