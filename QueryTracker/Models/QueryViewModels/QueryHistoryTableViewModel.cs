using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QueryTracker.Models.QueryViewModels
{
    public class QueryHistoryTableViewModel
	{
		[Display(Name = "Title")]
		public string QueryTitle { get; set; }

		[Display(Name = "Assigned to User")]
		public string UserName { get; set; }

		[Display(Name = "Status")]
		public string QueryStatus { get; set; }

	    [Display(Name = "Type")]
	    public string QueryType { get; set; }

		[Display(Name = "Description")]
		public string Description { get; set; }

		[Display(Name = "Status Change Date")]
	    public string ChangeDate { get; set; }
	}
}
