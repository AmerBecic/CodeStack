using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Data;
using WebApp.EFModels;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryItemDetailsModel> categoryItemDetailsModels = null;
            IEnumerable<CategoryItemsByCategoryModel> categoryItemsByCategoryModel = null;

            CategoryDetailsModel categoryDetailsModel = new CategoryDetailsModel();

            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);

                if(user != null)
                {
                    categoryItemDetailsModels = await GetCategoryItemDetailsForUser(user.Id);

                    categoryItemsByCategoryModel = GroupCategoryItemsByCategory(categoryItemDetailsModels);

                    categoryDetailsModel.CategoryItemsByCategoryModels = categoryItemsByCategoryModel;
                }
            }
            else
            {
                var categories = await GetCategoriesWithContent();

                categoryDetailsModel.Categories = categories;
            }

            return View(categoryDetailsModel);
        }


        private async Task<List<Category>> GetCategoriesWithContent()
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
        private IEnumerable<CategoryItemsByCategoryModel> GroupCategoryItemsByCategory(IEnumerable<CategoryItemDetailsModel> categoryItemDetailsModels)
        {
            var groupedItems = from item in categoryItemDetailsModels
                               group item by item.CategoryId into g
                               select new CategoryItemsByCategoryModel
                               {
                                   CategoryId = g.Key,
                                   Title = g.Select(c => c.CategoryTitle).FirstOrDefault(),
                                   CategoryItems = g
                               };

            return groupedItems;

        }

        private async Task<IEnumerable<CategoryItemDetailsModel>>  GetCategoryItemDetailsForUser(string userId)
        {
            var allCatItems = await (from catItem in _context.CategoryItem
                                     join category in _context.Category
                                     on catItem.CategoryId equals category.Id
                                     join mediaType in _context.MediaType
                                     on catItem.MediaTypeId equals mediaType.Id
                                     join userCat in _context.UserCategory
                                     on category.Id equals userCat.CategoryId
                                     where userId == userCat.UserId
                                  select new CategoryItemDetailsModel
                                  {
                                      CategoryId = category.Id,
                                      CategoryTitle = category.Title,
                                      CategoryItemId = catItem.Id,
                                      CategoryItemTitle = catItem.Title,
                                      CategoryItemDescription = catItem.Description,
                                      MediaImagePath = mediaType.ThumbnailImagePath
                                  }).ToListAsync();

            return allCatItems;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
