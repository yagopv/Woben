using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

using Microsoft.AspNet.Identity.EntityFramework;
using Woben.Domain.Resources;

namespace Woben.Domain.Model
{
    /// <summary>
    ///  User Profile entity
    /// </summary>
    public class UserProfile : IdentityUser
    {
        [Required(ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public string FirstName { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof(DomainResources), ErrorMessageResourceName = "MaxLength")]
        public string Lastname { get; set; }
    }
}
