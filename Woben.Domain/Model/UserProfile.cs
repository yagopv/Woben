using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

using Microsoft.AspNet.Identity.EntityFramework;

namespace Woben.Domain.Model
{
    /// <summary>
    ///  User Profile entity
    /// </summary>
    public class UserProfile : IdentityUser
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string Lastname { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
