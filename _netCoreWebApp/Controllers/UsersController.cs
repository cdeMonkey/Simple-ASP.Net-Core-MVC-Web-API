using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _netCoreWebApp.Context;
using _netCoreWebApp.Models;
using _netCoreWebApp.DAL;

namespace _netCoreWebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private IUserRepository _userRepository;

        public UsersController(AppDbContext context)
        {
            _context = context;
            _userRepository = new UserRepository(_context);
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userRepository.GetUsers();
                return Json(new { success = true, data = users });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(new { success = false, error = "Id is null." });
            }

            try
            {
                var usersModel = await _userRepository.GetDetails((int)id);

                if (usersModel == null)
                {
                    return NotFound(new { success = false, error = "User not found." });
                }

                return Json(new { success = true, data = usersModel });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,FirstName,MiddleName,LastName,Email,PhoneNO")] UsersModel usersModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _userRepository.CreateUser(usersModel);
                    _userRepository.SaveChanges();
                    return Json(new { success = true, message = "User created successfully." });
                }

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                                       .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                return Json(new { success = false, errors });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(new { success = false, error = "Id is null." });
            }

            try
            {
                var usersModel = await _userRepository.FindAsync((int)id);

                if (usersModel == null)
                {
                    return NotFound(new { success = false, error = "User not found." });
                }

                return Json(new { success = true, data = usersModel });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,Email,PhoneNO")] UsersModel usersModel)
        {
            try
            {
                if (id != usersModel.Id)
                {
                    return NotFound(new { success = false, error = "Id mismatch." });
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _userRepository.EditUser((int)id, usersModel);
                        _userRepository.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UsersModelExists(usersModel.Id))
                        {
                            return NotFound(new { success = false, error = "User not found." });
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return Json(new { success = true, message = "User updated successfully." });
                }

                var errors = ModelState.Where(x => x.Value.Errors.Any())
                                       .ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                return Json(new { success = false, errors });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(new { success = false, error = "Id is null." });
            }

            try
            {
                var usersModel = await _userRepository.DeleteUser((int)id);

                if (usersModel == null)
                {
                    return NotFound(new { success = false, error = "User not found." });
                }

                return Json(new { success = true, data = usersModel });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var user = await _userRepository.FindAsync(id);

                if (user == null)
                {
                    return NotFound(new { success = false, error = "User not found." });
                }

                _userRepository.DeleteUserConfirmed(user);
                _userRepository.SaveChanges();

                return Json(new { success = true, message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        private bool UsersModelExists(int id)
        {
            return _context.users.Any(e => e.Id == id);
        }
    }

}
