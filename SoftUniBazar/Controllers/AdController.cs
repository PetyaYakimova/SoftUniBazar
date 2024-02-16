using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using SoftUniBazar.Models;
using System.Security.Claims;
using static SoftUniBazar.Data.DataConstants;

namespace SoftUniBazar.Controllers
{
	[Authorize]
	public class AdController : Controller
	{
		private readonly BazarDbContext data;

		public AdController(BazarDbContext context)
		{
			this.data = context;
		}

		[HttpGet]
		public async Task<IActionResult> All()
		{
			var model = await data.Ads
				.AsNoTracking()
				.Select(a => new AdViewModel
				{
					Id = a.Id,
					Name = a.Name,
					ImageUrl = a.ImageUrl,
					CreatedOn = a.CreatedOn.ToString(DateTimeFormat),
					Category = a.Category.Name,
					Description = a.Description,
					Price = a.Price,
					Owner = a.Owner.UserName
				})
				.ToListAsync();

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Cart()
		{
			string userId = GetUserId();

			var model = await data.AdsBuyers
				.AsNoTracking()
				.Where(ab=>ab.BuyerId==userId)
				.Include(ab=>ab.Ad)
				.Select(a => new AdViewModel
				{
					Id = a.AdId,
					Name = a.Ad.Name,
					ImageUrl = a.Ad.ImageUrl,
					CreatedOn = a.Ad.CreatedOn.ToString(DateTimeFormat),
					Category = a.Ad.Category.Name,
					Description = a.Ad.Description,
					Price = a.Ad.Price,
					Owner = a.Ad.Owner.UserName
				})
				.ToListAsync();

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var model = new AdFormViewModel();
			model.Categories = await GetCategories();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AdFormViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Categories = await GetCategories();

				return View(model);
			}

			var entity = new Ad()
			{
				Name = model.Name,
				Description = model.Description,
				ImageUrl = model.ImageUrl,
				Price = model.Price,
				CategoryId = model.CategoryId,
				CreatedOn = DateTime.Now,
				OwnerId = GetUserId()
			};

			await data.Ads.AddAsync(entity);
			await data.SaveChangesAsync();

			return RedirectToAction(nameof(All));
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var ad = await data.Ads
				.FindAsync(id);

			if (ad == null)
			{
				return BadRequest();
			}

			if (ad.OwnerId != GetUserId())
			{
				return Unauthorized();
			}

			var model = new AdFormViewModel()
			{
				Name = ad.Name,
				Description = ad.Description,
				ImageUrl = ad.ImageUrl,
				Price = ad.Price,
				CategoryId = ad.CategoryId,
				Categories = await GetCategories()
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(AdFormViewModel model, int id)
		{
			var ad = await data.Ads
				.FindAsync(id);

			if (ad == null)
			{
				return BadRequest();
			}

			if (ad.OwnerId != GetUserId())
			{
				return Unauthorized();
			}

			if (!ModelState.IsValid)
			{
				model.Categories = await GetCategories();

				return View(model);
			}

			ad.Name = model.Name;
			ad.Description = model.Description;
			ad.ImageUrl = model.ImageUrl;
			ad.Price = model.Price;
			ad.CategoryId = model.CategoryId;

			await data.SaveChangesAsync();

			return RedirectToAction(nameof(All));
		}

		[HttpPost]
		public async Task<IActionResult> AddToCart(int id)
		{
			var ad = await data.Ads
				.Where(a=>a.Id==id)
				.Include(a=>a.AdsBuyers)
				.FirstOrDefaultAsync();

			if (ad == null)
			{
				return BadRequest();
			}

			string userId = GetUserId();

			//You cannot add your own ads to your cart
			if (ad.OwnerId == userId)
			{
				return BadRequest();
			}

			if (ad.AdsBuyers.Any(ab => ab.BuyerId == userId))
			{
				return RedirectToAction(nameof(All));
			}

			ad.AdsBuyers.Add(new AdBuyer()
			{
				BuyerId = userId,
				AdId = id
			});

			await data.SaveChangesAsync();

			return RedirectToAction(nameof(Cart));
		}

		[HttpPost]
		public async Task<IActionResult> RemoveFromCart(int id)
		{
			var ad = await data.Ads
				.Where(a => a.Id == id)
				.Include(a => a.AdsBuyers)
				.FirstOrDefaultAsync();

			if (ad == null)
			{
				return BadRequest();
			}

			string userId = GetUserId();
			var adBuyer = ad.AdsBuyers.FirstOrDefault(ab=>ab.BuyerId == userId);

			if (adBuyer==null)
			{
				return BadRequest();
			}

			ad.AdsBuyers.Remove(adBuyer);
			await data.SaveChangesAsync();

			return RedirectToAction(nameof(All));
		}


		private string GetUserId()
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
		}

		private async Task<List<CategoryViewModel>> GetCategories()
		{
			return await data.Categories
				.AsNoTracking()
				.Select(c => new CategoryViewModel
				{
					Id = c.Id,
					Name = c.Name
				})
				.ToListAsync();			
		}
	}
}
