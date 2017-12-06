using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QueryTracker.Models;
using QueryTracker.Authentication;
using QueryTracker.ViewModel;

namespace QueryTracker.Controllers
{
	public class HomeController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public HomeController(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Index()
		{
			ActionResult result = View();
			var currectUserDesignation =
				(DesignationEnum)(int.Parse(User.Claims.First(c => c.Type == ClaimsConstants.Designation).Value));

			ApplicationUser user = _userManager.GetUserAsync(User).Result;
			UserViewModel userViewModel = new UserViewModel();

			userViewModel.UserName = user.Name;

			if (currectUserDesignation == DesignationEnum.Employee)
				result = RedirectToAction("EmployeeIndex", userViewModel);
			if (currectUserDesignation == DesignationEnum.Admin)
				result = RedirectToAction("AdminIndex",userViewModel);

			return result;
		}

		[Authorize(PolicyConstants.AdminOnlyPolicy)]
		public IActionResult AdminIndex()
		{
			return View();
		}

		[Authorize(PolicyConstants.AdminEmployeePolicy)]
		public IActionResult EmployeeIndex()
		{
			return View();
		}


		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
