using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Models;
using WebApp.Data;
using WebApp.EFModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersToCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataFunctions _dataFunctions;

        public UsersToCategoryController(ApplicationDbContext context, IDataFunctions dataFunctions)
        {
            _context = context;
            _dataFunctions = dataFunctions;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersForCategory(int categoryId)
        {
            UsersCategoryListModel usersCategoryListModel = new UsersCategoryListModel();

            var allUsers = await GetAllUsers();

            var selectedUsersForCategory = await GetSavedUsersForCategory(categoryId);

            usersCategoryListModel.Users = allUsers;

            usersCategoryListModel.UsersSelected = selectedUsersForCategory;

            return PartialView("_UsersListPartial", usersCategoryListModel);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSelectedUsers([Bind("CategoryId, UsersSelected")] UsersCategoryListModel usersToCategory)
        {
            List<UserCategory> usersSelectedForCategoryToAdd = null;

            if(usersToCategory.UsersSelected != null)
            {
                usersSelectedForCategoryToAdd = await GetUsersForCategoryToAdd(usersToCategory);
            }

            var usersSelectedForCategoryToDelete = await GetUsersForCategoryToDelete(usersToCategory.CategoryId);

            await _dataFunctions.UpdateUserCategoryEntityAsync(usersSelectedForCategoryToDelete, usersSelectedForCategoryToAdd);

            usersToCategory.Users = await GetAllUsers();

            return PartialView("_UsersListPartial", usersToCategory);
        }

        private async Task<List<UserModel>> GetAllUsers()
        {
            var allUsers = await (from user in _context.Users
                                  select new UserModel
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName
                                  }).ToListAsync();

            return allUsers;
        }

        private async Task<List<UserCategory>> GetUsersForCategoryToAdd(UsersCategoryListModel usersCategoryListModel)
        {
            var usersForCategoryToAdd = (from user in usersCategoryListModel.UsersSelected
                                         select new UserCategory
                                         {
                                             CategoryId = usersCategoryListModel.CategoryId,
                                             UserId = user.Id
                                         }).ToList();

            return await Task.FromResult(usersForCategoryToAdd);
        }

        private async Task<List<UserCategory>> GetUsersForCategoryToDelete(int categoryId)
        {
            var usersForCategoryToDelete = await (from userCat in _context.UserCategory
                                                  where userCat.CategoryId == categoryId
                                                  select new UserCategory
                                                  {
                                                      Id = userCat.Id,
                                                      UserId = userCat.UserId,
                                                      CategoryId = categoryId
                                                  }).ToListAsync();

            return usersForCategoryToDelete;
        }

        private async Task<List<UserModel>> GetSavedUsersForCategory(int categoryId)
        {
            var savedSelectedUsersForCategory = await (from user in _context.UserCategory
                                                       where user.CategoryId == categoryId
                                                       select new UserModel
                                                       {
                                                           Id = user.UserId
                                                       }).ToListAsync();

            return savedSelectedUsersForCategory;

        }
    }
}
