using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.EFModels;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class CategoriesToUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataFunctions _dataFunctions;

        public CategoriesToUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDataFunctions dataFunctions)
        {
            _context = context;
            _userManager = userManager;
            _dataFunctions = dataFunctions;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            CategoriesToUserModel categoriesToUserModel = new CategoriesToUserModel();

            var userId = _userManager.GetUserAsync(User).Result?.Id;

            categoriesToUserModel.Categories = await GetAllCategoriesWithContent();

            categoriesToUserModel.CategoriesSelected = await GetCategoriesCurrentlySavedForUser(userId);

            categoriesToUserModel.UserId = userId;

            return View(categoriesToUserModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string[] categoriesSelected)
        {
            var userId = _userManager.GetUserAsync(User).Result?.Id;

            List<UserCategory> userCategoriesToDelete = await GetCategoriesToDeleteForUser(userId);

            List<UserCategory> userCategoriesToAdd = GetCategoriesToAddForUser(categoriesSelected, userId);

            await _dataFunctions.UpdateUserCategoryEntityAsync(userCategoriesToDelete, userCategoriesToAdd);

            return RedirectToAction("Index", "Home");

        }
        private async Task<List<Category>> GetAllCategoriesWithContent()
        {
            var categoriesWithContent = await (from cat in _context.Category
                                               join catItem in _context.CategoryItem
                                               on cat.Id equals catItem.CategoryId
                                               join content in _context.Content
                                               on catItem.Id equals content.CategoryItem.Id
                                               select new Category
                                               {
                                                   Id = cat.Id,
                                                   Title = cat.Title,
                                                   Description = cat.Description,
                                                   ThumbnailImagePath = cat.ThumbnailImagePath
                                               }).Distinct().ToListAsync();

            return categoriesWithContent;
        }

        private async Task<List<Category>> GetCategoriesCurrentlySavedForUser(string userId)
        {
            var categoriesOfUser = await (from userCat in _context.UserCategory
                                    where userCat.UserId == userId
                                    select new Category
                                    {
                                        Id = userCat.CategoryId
                                    }).ToListAsync();

            return categoriesOfUser;
        }

        private async Task<List<UserCategory>> GetCategoriesToDeleteForUser(string userId)
        {
            var categoriesToDelete = await (from userCat in _context.UserCategory
                                            where userCat.UserId == userId
                                            select new UserCategory
                                            {
                                                Id = userCat.Id,
                                                CategoryId = userCat.CategoryId,
                                                UserId = userId 
                                            }).ToListAsync();

            return categoriesToDelete;
        }

        private List<UserCategory> GetCategoriesToAddForUser(string[] categoriesSelected, string userId)
        {
            var categoriesToAdd = (from categoryId in categoriesSelected
                                   select new UserCategory
                                   {
                                       UserId = userId,
                                       CategoryId = int.Parse(categoryId)
                                   }).ToList();

            return categoriesToAdd;
        }
    }
}
