using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Test1.Services.Service;

namespace Test1.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            try
            {
                var users = _userService.GetAllUsers();
                return View(users);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving users.");
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userService.AddUser(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return StatusCode(500, "An error occurred while creating the user.");
                }
            }

            return View(user);
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var user = _userService.GetUser(id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                return View(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the user for editing.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userService.UpdateUser(user);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return StatusCode(500, "An error occurred while updating the user.");
                }
            }

            return View(user);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var user = _userService.GetUser(id);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                return View(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving the user for deletion.");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _userService.DeleteUser(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }
    }
}
