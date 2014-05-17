﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Woben.Domain.Model
{
    /// <summary>
    /// Notification class
    /// </summary>
    public class Notification : AuditInfoComplete
    {
        /// <summary>
        /// Entity identity
        /// </summary>
        [Key]
        public int NotificationId { get; set; }

        /// <summary>
        /// Notification Body
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Teléfono de contacto
        /// </summary>
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Best moment of the day to call 
        ///  - I = That´s the same for me
        ///  - M = Mornings
        ///  - T = Afternoons
        /// </summary>
        [StringLength(1)]
        public string BestTimeToCall { get; set; }

        /// <summary>
        /// Related product identity
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Related product
        /// </summary>
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
