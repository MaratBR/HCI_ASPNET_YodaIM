

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YodaIM.Models
{
	public enum Gender
	{
		Respect,
		Smooch
	}


	public class User : IdentityUser<Guid>
    {
        public string Alias { get; set; }

        public Gender? Gender { get; set; }

		// https://gist.github.com/theuntitled/7c70fff994993d7644f12d5bb0dc205f
		#region overrides

		public override string Email { get; set; }

		[JsonIgnore]
		public override bool EmailConfirmed { get; set; }

		[JsonIgnore]
		public override bool TwoFactorEnabled { get; set; }

		[JsonIgnore]
		public override string PhoneNumber { get; set; }

		[JsonIgnore]
		public override bool PhoneNumberConfirmed { get; set; }

		[JsonIgnore]
		public override string PasswordHash { get; set; }

		[JsonIgnore]
		public override string SecurityStamp { get; set; }

		[JsonIgnore]
		public override bool LockoutEnabled { get; set; }

		[JsonIgnore]
		public override DateTimeOffset? LockoutEnd { get; set; }

		[JsonIgnore]
		public override int AccessFailedCount { get; set; }

		#endregion
	}
}
