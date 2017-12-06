using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryTracker.Models
{
    public class QueryHistory
	{
	    public int Id { get; set; }
		public int QueryId { get; set; }
		public Query Query { get; set; }
		public int QueryStatus { get; set; }
		public int QueryType { get; set; }
		public string Description { get; set; }
		public DateTime ChangeDate { get; set; }
		public string UserId { get; set; }
		public ApplicationUser User { get; set; }
	}
}
