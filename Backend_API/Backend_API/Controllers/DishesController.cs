using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend_API.Models;
using Backend_API.Models.EntityModels;
using Backend_API.Models.ResourceModels;
using Backend_API.Models.BindingModels;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Backend_API.Constants;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly DBContext _context;

        private readonly ApplicationSettings Appsettings;
        private readonly string URLPath;

        public DishesController(DBContext context, IOptions<ApplicationSettings> appsettings)
        {
            _context = context;
            Appsettings = appsettings.Value;
            string relativepath = Appsettings.ImageStoragePath;
            URLPath = Path.Combine(Appsettings.baseURL, relativepath);
        }

        // GET: api/Dishes/Stat
        [HttpGet("Stat")]
        public ActionResult<int> GetDishStat()
        {
            var dish_count =  _context.Dishes.Count();
            return dish_count;
        }

        // GET: api/Dishes - Get All Dishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DishResourceModel>>> GetDishes()
        {
            //Get all dishes from DB
            var dish_list = await _context.Dishes.Include(i => i.Images).Include(i => i.Ratings).OrderByDescending(d => d.DateCreated).ToListAsync();

            List<DishResourceModel> All_Dishes = new List<DishResourceModel>();

            foreach (var dish in dish_list)
            {
                if (!dish.IsDeleted)
                {

                    //Create Image Resource Models from Image Objects
                    List<ImageResourceModel> All_Images = new List<ImageResourceModel>();
                    foreach (var image in dish.Images)
                    {
                        if (!image.IsDeleted)
                        {

                            string imageurl = Path.Combine(URLPath, image.Path);
                            imageurl = imageurl.Replace("\\", "/");

                            ImageResourceModel image_ = new ImageResourceModel
                            {
                                ImageId = image.ImageId,
                                Name = image.Name,
                                Path = imageurl

                            };
                            All_Images.Add(image_);
                        }

                    };

                    decimal TotalRating = 0;
                    
                    //Create Review Resource Models from review objects
                    List<RatingResourceModel> All_Ratings = new List<RatingResourceModel>();
                    foreach (var rating in dish.Ratings)
                    {
                        if (!rating.IsDeleted)
                        {
                            TotalRating += rating.Rate;

                            RatingResourceModel rating_ = new RatingResourceModel
                            {
                                RatingId = rating.RatingId,
                                UserName = rating.UserName,
                                UserEmail =rating.UserEmail,
                                Rate = rating.Rate,
                                Review = rating.Review

                            };
                            All_Ratings.Add(rating_);
                        }

                    };

                    //Get shorter description with 50 chars
                    String description = dish.DishDescrition;
                    if (description.Length > 50) {
                        description = dish.DishDescrition.Substring(0, 50);
                    }

                    //Calculate Average rating from all ratings
                    decimal DishTotalRating = 0;
                    if (TotalRating != 0) {
                        DishTotalRating = TotalRating / All_Ratings.Count();
                    }

                    //Create Dish Resource Model
                    DishResourceModel dish_ = new DishResourceModel
                    {
                        DishId = dish.DishId,
                        DishName = dish.DishName,
                        DishDescription = description,
                        DishPrice = dish.DishPrice,
                        DishTotalRating = DishTotalRating,
                        DateCreated = dish.DateCreated,
                        DateModified = dish.DateModified,

                        Images = All_Images,
                        Ratings = All_Ratings

                    };
                    //Add to output array
                    All_Dishes.Add(dish_);
                }

            };

            
            return Ok(All_Dishes);

        }

        // GET: api/Dishes/5 - Get Dish by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<DishResourceModel>> GetDish(Guid id)
        {
            //Get Dish object for given ID from DB
            var dish = await _context.Dishes.Include(i => i.Images).Include(i => i.Ratings).Where(d=> d.DishId == id && !d.IsDeleted).FirstOrDefaultAsync();

            DishResourceModel dish_ = new DishResourceModel();

            if (dish == null)
            {
                return NotFound();
            }
            else {
                //Create Image Resource Models from Image Objects
                List<ImageResourceModel> All_Images = new List<ImageResourceModel>();
                foreach (var image in dish.Images)
                {
                    if (!image.IsDeleted)
                    {

                        string imageurl = Path.Combine(URLPath, image.Path);
                        imageurl = imageurl.Replace("\\", "/");

                        ImageResourceModel image_ = new ImageResourceModel
                        {
                            ImageId = image.ImageId,
                            Name = image.Name,
                            Path = imageurl

                        };
                        All_Images.Add(image_);
                    }

                };

                //Create Rating Resource Models from Rating Objects
                decimal TotalRating = 0;
                List<RatingResourceModel> All_Ratings = new List<RatingResourceModel>();
                foreach (var rating in dish.Ratings)
                {
                    if (!rating.IsDeleted)
                    {
                        TotalRating += rating.Rate;
                        RatingResourceModel rating_ = new RatingResourceModel
                        {
                            RatingId = rating.RatingId,
                            UserName = rating.UserName,
                            UserEmail = rating.UserEmail,
                            Rate = rating.Rate,
                            Review = rating.Review

                        };
                        All_Ratings.Add(rating_);
                    }

                };

                //Calculate Average rating from all ratings
                decimal DishTotalRating = 0;
                if (TotalRating != 0)
                {
                    DishTotalRating = TotalRating / All_Ratings.Count();
                }


                //Create Dish Resource Model
                dish_ = new DishResourceModel
                {
                    DishId = dish.DishId,
                    DishName = dish.DishName,
                    DishDescription = dish.DishDescrition,
                    DishPrice = dish.DishPrice,
                    DishTotalRating = DishTotalRating,
                    DateCreated = dish.DateCreated,
                    DateModified = dish.DateModified,

                    Images = All_Images,
                    Ratings = All_Ratings

                };

            }

            return dish_;
        }

        // PUT: api/Dishes/ - Update Dish
        [HttpPut]
        [Authorize(AuthenticationSchemes = JWTSettings.AuthScheme)]
        public async Task<IActionResult> PutDish(DishEditBindingModel dish_update)
        {
            //Authorization
            string usertype = User.Claims.First(c => c.Type == "Role").Value;
            if (usertype.Equals(EntityConstants.Role_SuperAdmin) || usertype.Equals(EntityConstants.Role_Admin))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //Get  Existing Dish
                var dish = await _context.Dishes.Include(i => i.Images).Include(i => i.Ratings).Where(d => d.DishId == dish_update.DishId && !d.IsDeleted).FirstOrDefaultAsync();

                List<Image> All_Images = new List<Image>();
                foreach (var id in dish_update.Images)
                {
                    var image = await _context.Images.Where(i => i.ImageId == id && !i.IsDeleted).FirstOrDefaultAsync();
                    All_Images.Add(image);

                };

                if (dish == null)
                {
                    return NotFound();
                }

                //Update values
                dish.DishName = dish_update.DishName;
                dish.DishDescrition = dish_update.DishDescription;
                dish.DishPrice = dish_update.DishPrice;
                dish.Images = All_Images;

                //Save to DB
                _context.Entry(dish).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish_update.DishId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok();
            }
            return Unauthorized();
        }

        // POST: api/Dishes - Create New Dish
        [HttpPost]
        [Authorize(AuthenticationSchemes = JWTSettings.AuthScheme)]
        public async Task<ActionResult<Dish>> PostDish(DishBindingModel dish)
        {
            //Authorization
            string usertype = User.Claims.First(c => c.Type == "Role").Value;
            if (usertype.Equals(EntityConstants.Role_SuperAdmin) || usertype.Equals(EntityConstants.Role_Admin))
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Get Image Objects from DB according to the ImageID array recieved
                List<Image> All_Images = new List<Image>();
                foreach (var imageId in dish.Images)
                {
                    if (imageId != null)
                    {
                        var imagedata = await _context.Images.FindAsync(imageId);
                        if (imagedata != null)
                        {
                            Image image_ = new Image
                            {
                                Name = imagedata.Name,
                                Path = imagedata.Path

                            };
                            All_Images.Add(image_);
                        }
                    }

                };

                //Create new Dish object
                Dish new_dish = new Dish()
                {
                    DishName = dish.DishName,
                    DishDescrition = dish.DishDescription,
                    DishPrice = dish.DishPrice,
                    Images = All_Images
                };

                //Add to DB
                _context.Dishes.Add(new_dish);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDish", new { id = new_dish.DishId }, dish);
            }
            return Unauthorized();
        }

        // DELETE: api/Dishes/5 - Delete Dish by ID
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JWTSettings.AuthScheme)]
        public async Task<ActionResult<Dish>> DeleteDish(Guid id)
        {
            //Authorization
            string usertype = User.Claims.First(c => c.Type == "Role").Value;
            if (usertype.Equals(EntityConstants.Role_SuperAdmin) || usertype.Equals(EntityConstants.Role_Admin))
            {
                //Get Existing Dish
                var dish = await _context.Dishes.FindAsync(id);
                if (dish == null)
                {
                    return NotFound();
                }

                //Change IsDeleted status to true
                dish.IsDeleted = true;

                //Save to DB
                _context.Entry(dish).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            return Unauthorized();
        }

        private bool DishExists(Guid id)
        {
            return _context.Dishes.Any(e => e.DishId == id);
        }
    }
}
