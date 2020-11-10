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
using Microsoft.AspNetCore.Authorization;
using Backend_API.Constants;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly DBContext _context;

        public RatingsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Ratings/Stat
        [HttpGet("Stat")]
        public ActionResult<int> GetRatingStat()
        {
            var dish_count = _context.Ratings.Count();
            return dish_count;
        }

        // GET: api/Ratings/5
        //Get Ratings for given dish id
        [HttpGet("{dish_id}")]

        public async Task<ActionResult<IEnumerable<RatingResourceModel>>> GetRating(Guid dish_id)
        {
            //Get ratings for given Dish ID
            var ratings = await _context.Ratings.Where(r => r.DishId == dish_id && !r.IsDeleted).ToListAsync<Rating>();

            if (ratings == null)
            {
                return NotFound();
            }

            if (ratings.Count() == 0) {
                return NotFound();
            }
            //Convert to Resource Models
            List<RatingResourceModel> All_Ratings = new List<RatingResourceModel>();
            foreach (var rating in ratings)
            {
  
                    RatingResourceModel rating_ = new RatingResourceModel
                    {
                        RatingId = rating.RatingId,
                        UserName = rating.UserName,
                        UserEmail = rating.UserEmail,
                        Rate = rating.Rate,
                        Review = rating.Review

                    };
                    All_Ratings.Add(rating_);

            };
            return All_Ratings;
        }

        

        // POST: api/Ratings - Create new Rating
        [HttpPost]
        public async Task<ActionResult<Rating>> PostRating(RatingBindingModel rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Rating new_rating = new Rating() {
                DishId =rating.DishId,
                UserName = rating.UserName,
                UserEmail =rating.UserEmail,
                Rate = rating.Rate,
                Review = rating.Review
            };
            _context.Ratings.Add(new_rating);
            await _context.SaveChangesAsync();

            return Ok(new_rating);
        }

        // DELETE: api/Ratings/5 - Delete a Rating by ID
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JWTSettings.AuthScheme)]
        public async Task<ActionResult> DeleteRating(Guid id)
        {
            //Authorization
            string usertype = User.Claims.First(c => c.Type == "Role").Value;
            if (usertype.Equals(EntityConstants.Role_SuperAdmin) || usertype.Equals(EntityConstants.Role_Admin))
            {
                var rating = await _context.Ratings.FindAsync(id);
                if (rating == null)
                {
                    return NotFound();
                }

                rating.IsDeleted = true;
                _context.Entry(rating).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RatingExists(rating.RatingId))
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

        private bool RatingExists(Guid id)
        {
            return _context.Ratings.Any(e => e.RatingId == id);
        }
    }
}
