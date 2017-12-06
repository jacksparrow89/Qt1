using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QueryTracker.Models.QueryViewModels
{
    public class CreateQueryViewModel
    {
		[Required]
	    [Display(Name = "Query Type")]
		public int QueryType { get; set; }

	    [Required]
	    [Display(Name = "Query Status")]
	    public int QueryStatus { get; set; }

		[Required]
		[Display(Name = "Title")]
		public string QueryTitle { get; set; }

	    [Required]
		public string Description { get; set; }

	    [Required]
		[Display(Name = "Assign to user")]
		public string UserId { get; set; }

	    [Display(Name = "Invoice/File numeber")]
	    public string Invoice { get; set; }

		public int? QueryId { get; set; }
    }
}
