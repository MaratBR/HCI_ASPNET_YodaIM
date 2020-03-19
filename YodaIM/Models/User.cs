

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace YodaIM.Models
{
    [Table("users")]
    public class User : IdentityUser<int>
    {
        public string Alias { get; set; }
    }
}
