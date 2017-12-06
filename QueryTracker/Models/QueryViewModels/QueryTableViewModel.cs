using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QueryTracker.Models.QueryViewModels
{
    public class QueryTableViewModel
    {
		[Display(Name = "Id")]
		public int QueryId { get; set; }

		[Display(Name = "Title")]
		public string QueryTitle { get; set; }

		[Display(Name = "Assigned to User")]
		public string UserName { get; set; }

	    public string UserId { get; set; }

		[Display(Name = "Status")]
		public string QueryStatus { get; set; }

	    [Display(Name = "Type")]
	    public string QueryType { get; set; }

	    [Display(Name = "Created Date")]
	    public string ReceivedDate { get; set; }
	}
}
