

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YodaIM.Helpers;

namespace YodaIM.Models
{
	public class User : IdentityUser<int>
	{
		[DefaultValue(0)]
		public byte Gender { get; set; } = 0;

		public string Status { get; set; }

		[JsonIgnore]
		public virtual List<UserRoom> Rooms { get; set; }

		// https://gist.github.com/theuntitled/7c70fff994993d7644f12d5bb0dc205f
		#region overrides

		[Required] [JsonIgnore]
		public override string Email { get; set; }

		[JsonIgnore] [Required]
		public override bool EmailConfirmed { get; set; }

		[JsonIgnore] [Required]
		public override bool TwoFactorEnabled { get; set; }

		[JsonIgnore]
		public override string PhoneNumber { get; set; }

		[JsonIgnore] [Required]
		public override bool PhoneNumberConfirmed { get; set; }

		[JsonIgnore] [Required]
		public override string PasswordHash { get; set; }

		[JsonIgnore] [Required]
		public override string SecurityStamp { get; set; }

		[JsonIgnore] [Required]
		public override bool LockoutEnabled { get; set; }

		[JsonIgnore]
		public override DateTimeOffset? LockoutEnd { get; set; }

		[JsonIgnore] [Required]
		public override int AccessFailedCount { get; set; }

		#endregion
	}
}
