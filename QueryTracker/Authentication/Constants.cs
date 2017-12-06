using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryTracker.Authentication
{
	public static class PolicyConstants
	{
		public const string AdminOnlyPolicy = "Admin";
		public const string AdminEmployeePolicy = "Admin_Employee";
	}

	public static class ClaimsConstants
	{
		public const string Designation = "Designation";
	}
}
