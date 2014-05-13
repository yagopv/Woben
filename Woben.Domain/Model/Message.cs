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
    /// Messages
    /// </summary>
    public class Message : AuditInfoBase
    {
        /// <summary>
        /// Entity identity
        /// </summary>
        [Key]
        public int MessageId { get; set; }

        /// <summary>
        /// Message Title
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// Message Body
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Related product identity
        /// </summary>
        public int? ProductId { get; set; }

        /// <summary>
        /// Related product
        /// </summary>
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
