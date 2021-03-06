﻿using System;
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
    /// Messages
    /// </summary>
    public class Message : AuditInfoComplete
    {
        /// <summary>
        /// Entity identity
        /// </summary>
        [Key]
        public int MessageId { get; set; }

        /// <summary>
        /// Message User Name
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public string Name { get; set; }

        /// <summary>
        /// Message User Email
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// Teléfono de contacto
        /// </summary>
        [StringLength(20, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Message Body
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "Required")]
        [MinLength(5, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MinLength")]
        public string Text { get; set; }
    }
}
