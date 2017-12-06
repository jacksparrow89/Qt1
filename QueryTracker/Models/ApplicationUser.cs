using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace QueryTracker.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
	    public string Name { get; set; }
	    public int Designation { get; set; }
	}

	public enum DesignationEnum
	{
		Admin = 1000,
		Employee = 500,
	}

	public enum QueryStatus
	{
		Received = 1,
		[Display(Name = "Pending With Client")]
		PendingWithClient = 2,
		[Display(Name = "Pending With User")]
		PendingWithUser = 3,
		Completed = 4,
		ReOpened = 5,
	}

	public enum QueryType
	{
		[Display(Name ="Railway")]
		Railway = 10,
		[Display(Name = "Domestic air ticket")]
		DomesticAirTicket = 11,
		[Display(Name = "International air ticket")]
		InternationalAirTicket = 12,
		[Display(Name = "Domestic hotel only")]
		DomesticHotel = 13,
		[Display(Name = "International hotel only")]
		IntenationHotel = 14,
		[Display(Name = "Domestic package")]
		DomesticPackage = 15,
		[Display(Name = "International package")]
		InternationalPackage = 16,
		Visa = 17,
		Passport = 18,
		Forex = 19,
		[Display(Name = "Travel Insurance")]
		TravelInsurance = 20
	}
}
