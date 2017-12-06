using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueryTracker.Authentication;
using QueryTracker.Models.QueryViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QueryTracker.Models;
using QueryTracker.Data;

namespace QueryTracker.Controllers
{
	public class QueryController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public QueryController(ApplicationDbContext context,
			UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
			_context = context;
		}

		[Authorize(PolicyConstants.AdminEmployeePolicy)]
		public async Task<IActionResult> CreateNewQuery()
		{
			LoadQueryType();
			await LoadEmployees();

			return View("CreateNewQuery", new CreateQueryViewModel());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(PolicyConstants.AdminEmployeePolicy)]
		public async Task<IActionResult> CreateNewQuery(CreateQueryViewModel createQueryViewModel)
		{
			if (ModelState.IsValid)
			{
				if (createQueryViewModel.QueryStatus == 4 && string.IsNullOrEmpty(createQueryViewModel.Invoice))
				{
					ModelState.AddModelError("", "Please add invoice / file number before marking the query as complete");
					LoadQueryType();
					await LoadEmployees();
					return View("CreateNewQuery", createQueryViewModel);
				}

				Query query = new Query
				{
					ReceivedDate = DateTime.Now,
					QueryStatus = 1,
					QueryType = createQueryViewModel.QueryType,
					QueryTitle = createQueryViewModel.QueryTitle,
					UserId = createQueryViewModel.UserId,
				};

				QueryHistory queryHistory = new QueryHistory
				{
					QueryStatus = 1,
					QueryType = createQueryViewModel.QueryType,
					Description = createQueryViewModel.Description,
					ChangeDate = DateTime.Now,
					UserId = createQueryViewModel.UserId,
				};

				query.QueryHistories = new List<QueryHistory> { queryHistory };

				_context.Add(query);
				await _context.SaveChangesAsync();
			}
			else
			{
				LoadQueryType();
				await LoadEmployees();
				return View("CreateNewQuery", createQueryViewModel);
			}
			if (_userManager.GetUserAsync(User).Result.Designation == 1000)
				return RedirectToAction("ViewAllQueries");
			else
				return RedirectToAction("ViewEmployeeQueries");
		}

		[Authorize(PolicyConstants.AdminEmployeePolicy)]
		public async Task<IActionResult> EditQuery(int id)
		{
			IEnumerable<Query> selectedQuery = _context.Query.Where(query => query.Id == id).Include(query => query.QueryHistories);
			CreateQueryViewModel createQueryViewModel = new CreateQueryViewModel
			{
				QueryId = selectedQuery.Single().Id,
				QueryTitle = selectedQuery.Single().QueryTitle,
				QueryType = selectedQuery.Single().QueryHistories.Last().QueryType,
				QueryStatus = selectedQuery.Single().QueryHistories.Last().QueryStatus,
				UserId = selectedQuery.Single().QueryHistories.Last().UserId,
				Description = selectedQuery.Single().QueryHistories.Last().Description
			};
			LoadQueryType();
			LoadQueryStatus();
			await LoadEmployees();
			return View("CreateNewQuery", createQueryViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(PolicyConstants.AdminEmployeePolicy)]
		public async Task<IActionResult> EditQuery(CreateQueryViewModel createQueryViewModel)
		{
			if (ModelState.IsValid)
			{
				if (createQueryViewModel.QueryStatus == 4 && string.IsNullOrEmpty(createQueryViewModel.Invoice))
				{
					ModelState.AddModelError("", "Please add invoice / file number before marking the query as complete");
					LoadQueryType();
					LoadQueryStatus();
					await LoadEmployees();
					return View("CreateNewQuery", createQueryViewModel);
				}

				IEnumerable<Query> selectedQuery = _context.Query.Where(query => query.Id == createQueryViewModel.QueryId).Include(query => query.QueryHistories);

				selectedQuery.Single().QueryTitle = createQueryViewModel.QueryTitle;

				QueryHistory queryHistory = new QueryHistory
				{
					QueryStatus = createQueryViewModel.QueryStatus,
					QueryType = createQueryViewModel.QueryType,
					Description = createQueryViewModel.Description,
					ChangeDate = DateTime.Now,
					UserId = createQueryViewModel.UserId
				};

				selectedQuery.Single().QueryHistories = new List<QueryHistory> { queryHistory };

				_context.Update(selectedQuery.Single());
				await _context.SaveChangesAsync();
			}
			else
				return RedirectToAction("EditQuery");

			if (_userManager.GetUserAsync(User).Result.Designation == 1000)
				return RedirectToAction("ViewAllQueries");
			else
				return RedirectToAction("ViewEmployeeQueries");
		}

		[Authorize(PolicyConstants.AdminOnlyPolicy)]
		public async Task<IActionResult> ViewAllQueries(string sortOrder, string searchString,string userName)
		{
			await LoadEmployees();
			ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
			ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
			ViewData["CurrentFilter"] = searchString;
			ViewData["UserName"] = userName;

			IEnumerable<Query> queries = await _context.Query.Include(query => query.QueryHistories).ToListAsync();

			if (!String.IsNullOrEmpty(searchString))
				queries = queries.Where(s => s.QueryTitle.Contains(searchString));

			switch (sortOrder)
			{
				case "name_desc":
					queries = queries.OrderByDescending(s => s.QueryTitle);
					break;
				case "Date":
					queries = queries.OrderBy(s => s.ReceivedDate);
					break;
				case "date_desc":
					queries = queries.OrderByDescending(s => s.ReceivedDate);
					break;
				default:
					queries = queries.OrderBy(s => s.QueryTitle);
					break;
			}

			List<QueryTableViewModel> queryTableViewModels = new List<QueryTableViewModel>();

			foreach (Query query in queries)
			{
				var lastQueryHistory = query.QueryHistories.Last();
				IEnumerable<QueryHistory> queryHistories = _context.QueryHistory.Where(item => item.Id == lastQueryHistory.Id).Include(item => item.User);
				QueryTableViewModel queryTableViewModel = new QueryTableViewModel
				{
					QueryId = query.Id,
					QueryType = ((QueryType)queryHistories.Single().QueryType).ToString(),
					QueryStatus = ((QueryStatus)queryHistories.Single().QueryStatus).ToString(),
					QueryTitle = query.QueryTitle,
					UserName = queryHistories.Single().User.Name,
					UserId = queryHistories.Single().UserId,
					ReceivedDate = query.ReceivedDate.ToShortDateString()
				};

				queryTableViewModels.Add(queryTableViewModel);
			}

			if (!String.IsNullOrEmpty(userName))
				queryTableViewModels = queryTableViewModels.Where(s => s.UserId == userName).ToList();

			return View(queryTableViewModels);
		}

		[Authorize(PolicyConstants.AdminEmployeePolicy)]
		public async Task<IActionResult> ViewEmployeeQueries()
		{
			string userId = _userManager.GetUserId(User);
			var queries = await _context.Query.Include(query => query.QueryHistories).ToListAsync();

			List<QueryTableViewModel> queryTableViewModels = new List<QueryTableViewModel>();

			foreach (Query query in queries)
			{
				var lastQueryHistory = query.QueryHistories.Last();
				IEnumerable<QueryHistory> queryHistories = _context.QueryHistory.Where(item => item.Id == lastQueryHistory.Id && item.UserId == userId).Include(item => item.User);
				if (queryHistories.Count() > 0)
				{
					QueryTableViewModel queryTableViewModel = new QueryTableViewModel
					{
						QueryId = query.Id,
						QueryType = ((QueryType)queryHistories.Single().QueryType).ToString(),
						QueryStatus = ((QueryStatus)queryHistories.Single().QueryStatus).ToString(),
						QueryTitle = query.QueryTitle,
						UserName = queryHistories.Single().User.Name,
						ReceivedDate = query.ReceivedDate.ToShortDateString()
					};

					queryTableViewModels.Add(queryTableViewModel);
				}
			}

			return View("ViewAllQueries", queryTableViewModels);
		}

		[Authorize(PolicyConstants.AdminEmployeePolicy)]
		public async Task<IActionResult> ViewQueryHistory(int id)
		{
			IEnumerable<QueryHistory> queryHistories = await _context.QueryHistory.Where(qh => qh.QueryId == id).Include(qh => qh.User).Include(qh => qh.Query).ToListAsync();
			List<QueryHistoryTableViewModel> queryHistoryTableViewModels = new List<QueryHistoryTableViewModel>();

			foreach (QueryHistory queryHistory in queryHistories)
			{
				QueryHistoryTableViewModel queryHistoryTableViewModel = new QueryHistoryTableViewModel
				{
					QueryTitle = queryHistory.Query.QueryTitle,
					QueryType = ((QueryType)queryHistory.QueryType).ToString(),
					QueryStatus = ((QueryStatus)queryHistory.QueryStatus).ToString(),
					UserName = queryHistory.User.Name,
					ChangeDate = queryHistory.ChangeDate.ToShortDateString(),
					Description = queryHistory.Description
				};

				queryHistoryTableViewModels.Add(queryHistoryTableViewModel);
			}
			return View(queryHistoryTableViewModels);
		}

		#region Private Methods

		private void LoadQueryType()
		{
			var queryType = new List<SelectListItem>();

			queryType.Add(new SelectListItem
			{
				Text = "Select",
				Value = ""
			});

			foreach (QueryType eVal in Enum.GetValues(typeof(QueryType)))
			{
				queryType.Add(new SelectListItem { Text = Enum.GetName(typeof(QueryType), eVal), Value = ((int)eVal).ToString() });
			}

			ViewBag.QueryType = queryType;
		}

		private void LoadQueryStatus()
		{
			var queryStatus = new List<SelectListItem>();

			queryStatus.Add(new SelectListItem
			{
				Text = "Select",
				Value = ""
			});

			foreach (QueryStatus eVal in Enum.GetValues(typeof(QueryStatus)))
			{
				queryStatus.Add(new SelectListItem { Text = Enum.GetName(typeof(QueryStatus), eVal), Value = ((int)eVal).ToString() });
			}

			ViewBag.QueryStatus = queryStatus;
		}

		private async Task LoadEmployees()
		{
			var employees = await _context.Users.Where(item => item.Designation != 1000).ToListAsync();

			var employeesList = new List<SelectListItem>();

			employeesList.Add(new SelectListItem
			{
				Text = "Select",
				Value = ""
			});

			foreach (ApplicationUser employee in employees)
			{
				employeesList.Add(new SelectListItem { Text = employee.Name, Value = employee.Id });
			}

			ViewBag.EmployeeList = employeesList;
		}

		#endregion
	}
}
