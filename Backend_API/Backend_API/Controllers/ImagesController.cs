using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend_API.Models;
using Backend_API.Models.EntityModels;
using Backend_API.Models.BindingModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.IO;
using Backend_API.Models.ResourceModels;
using Microsoft.AspNetCore.Authorization;
using Backend_API.Constants;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
       
        private readonly DBContext _context;

        private readonly ApplicationSettings Appsettings;
        private readonly IHostingEnvironment HostingEnvironment;
        private readonly string UploadPath;
        private readonly string URLPath;

        public ImagesController(DBContext context, IOptions<ApplicationSettings> appsettings, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            Appsettings = appsettings.Value;
            HostingEnvironment = hostingEnvironment;

            string relativepath = Appsettings.ImageStoragePath;
            UploadPath = Path.Combine(HostingEnvironment.WebRootPath, relativepath);
            URLPath = Path.Combine(Appsettings.baseURL, relativepath);
        }

     

        // GET: api/Images/5 - Get Image by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageResourceModel>> GetImage(Guid id)
        {
            //Get Image from DB
            var image = await _context.Images.Where(i =>i.ImageId == id && !i.IsDeleted).FirstOrDefaultAsync();

            if (image == null)
            {
                return NotFound();
            }

            //Build image file url 
            string imageurl = Path.Combine(URLPath, image.Path);
            imageurl = imageurl.Replace("\\", "/");

            //Create Image Resource Model
            ImageResourceModel image_ = new ImageResourceModel
            {
                ImageId = image.ImageId,
                Name = image.Name,
                Path = imageurl

            };
            return image_;
        }

        

        // POST: api/Images
        //Upload images to server
        [HttpPost]
        [Authorize(AuthenticationSchemes = JWTSettings.AuthScheme)]
        public async Task<ActionResult<ImageResourceModel>> PostImage([FromForm] ImageBindingModel imageModel)
        {

            //Authorization
            string usertype = User.Claims.First(c => c.Type == "Role").Value;
            if (usertype.Equals(EntityConstants.Role_SuperAdmin) || usertype.Equals(EntityConstants.Role_Admin))
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string uniqueName = null;
                if (imageModel.Image != null)
                {

                    //Check the availability of file sstoring folder
                    bool exists = System.IO.Directory.Exists(UploadPath);

                    if (!exists) { System.IO.Directory.CreateDirectory(UploadPath); }

                    //Create unique file name
                    uniqueName = Guid.NewGuid().ToString() + "_" + imageModel.Image.FileName;
                    string filepath = Path.Combine(UploadPath, uniqueName);

                    //Upload
                    imageModel.Image.CopyTo(new FileStream(filepath, FileMode.Create));

                    //Create new Image Object
                    Image new_image = new Image()
                    {
                        Name = imageModel.Name,
                        Path = uniqueName
                    };
                    //Add to DB
                    _context.Images.Add(new_image);
                    await _context.SaveChangesAsync();

                    string imageurl = Path.Combine(URLPath, new_image.Path);
                    imageurl = imageurl.Replace("\\", "/");

                    ImageResourceModel image_ = new ImageResourceModel
                    {
                        ImageId = new_image.ImageId,
                        Name = new_image.Name,
                        Path = imageurl

                    };
                    return Ok(image_);
                }


                return BadRequest();
            }
            return Unauthorized();
        }

        // DELETE: api/Images/5 -Delete Image By ID
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JWTSettings.AuthScheme)]
        public async Task<ActionResult<Image>> DeleteImage(Guid id)
        {
            //Authorization
            string usertype = User.Claims.First(c => c.Type == "Role").Value;
            if (usertype.Equals(EntityConstants.Role_SuperAdmin) || usertype.Equals(EntityConstants.Role_Admin))
            {
                //Get Image from DB
                var image = await _context.Images.FindAsync(id);
                if (image == null)
                {
                    return NotFound();
                }

                //Change isDeleted status
                image.IsDeleted = true;
                //Update DB
                _context.Entry(image).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(id))
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

        private bool ImageExists(Guid id)
        {
            return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
