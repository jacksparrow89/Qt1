using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryTracker.Models
{
    public class Query
    {
	    public int Id { get; set; }
		public string UserId { get; set; }
		public int QueryStatus { get; set; }
		public int QueryType { get; set; }
		public string InvoiceNumber { get; set; }
		public string QueryTitle { get; set; }
		public DateTime ReceivedDate { get; set; }
		public DateTime? CompletedDate { get; set; }
		public List<QueryHistory> QueryHistories { get; set; }
		public ApplicationUser User { get; set; }
	}
}
