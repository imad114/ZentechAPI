﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Zentech.Services;
using ZentechAPI.Models;

namespace ZentechAPI.controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidesController : ControllerBase
    {
        private readonly SlidesService _slideService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SlidesController"/> class.
        /// </summary>
        /// <param name="slideService">The service used for managing slides.</param>
        public SlidesController(SlidesService slideService)
        {
            _slideService = slideService;
        }

        /// <summary>
        /// Retrieves all slides.
        /// </summary>
        /// <returns>A list of all slides.</returns>
        [HttpGet]
        public IActionResult GetAllSlides()
        {
            try
            {
                var slides = _slideService.GetAllSlides();
                return Ok(slides);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a slide by its ID.
        /// </summary>
        /// <param name="id">The ID of the slide.</param>
        /// <returns>The requested slide.</returns>
        [HttpGet("{id}")]
        public IActionResult GetSlideById(int id)
        {
            try
            {
                var slide = _slideService.GetSlideById(id);
                if (slide == null)
                {
                    return NotFound("Slide not found.");
                }
                return Ok(slide);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new slide.
        /// </summary>
        /// <param name="Slide">The slide data.</param>
        /// <returns>The created slide.</returns>
        [HttpPost]
        public async Task<IActionResult> AddSlide([FromForm] Slide slide)
        {
            try
            {
                if (slide == null)
                {
                    return BadRequest("Slide data is null.");
                }

                var createdBy = "admin";
                slide.CreatedBy = createdBy;
                var slideId = _slideService.AddSlide(slide);
                slide.SlideID = slideId;

                if (slide.Picture!= null && slide.Picture.Length > 0)
                {

                     await UploadSlidePhoto(slide.SlideID, slide.Picture);

                }

                   

                return CreatedAtAction(nameof(GetSlideById), new { id = slideId }, slide);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing slide.
        /// </summary>
        /// <param name="id">The ID of the slide to update.</param>
        /// <param name="slide">The updated slide data.</param>
        [HttpPut("{id}")]
        public IActionResult UpdateSlide(int id, [FromBody] Slide slide)
        {
            try
            {
                if (slide == null)
                {
                    return BadRequest("Slide data is null.");
                }

                if (id != slide.SlideID)
                {
                    return BadRequest("Slide ID mismatch.");
                }

                var updatedBy = "admin";
                slide.UpdatedBy = updatedBy;
                var success = _slideService.UpdateSlide(slide);

                if (!success)
                {
                    return NotFound("Slide not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("{productId}/upload-photoSlide")]
        [SwaggerResponse(StatusCodes.Status201Created, "Photo uploaded successfully", typeof(object))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid file upload", typeof(object))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while uploading the photo", typeof(object))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadSlidePhoto(int slideID, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { Message = "Invalid file upload." });
                }

             
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { Message = "Invalid file type. Only JPG and PNG files are allowed." });
                }

                if (file.Length > 10 * 1024 * 1024)
                {
                    return BadRequest(new { Message = "File size exceeds the maximum limit of 10MB." });
                }

                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/Slides");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var photoUrl = $"/uploads/Slides/{fileName}";
                _slideService.UpdateSlidePicture(slideID, photoUrl);

                return CreatedAtAction("UploadSlidePhoto", new { slideID, photoUrl }, new { Message = "Photo uploaded successfully.", Url = photoUrl });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logger here)
                return StatusCode(500, new { Message = "An error occurred while uploading the photo.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a slide by its ID.
        /// </summary>
        /// <param name="id">The ID of the slide to delete.</param>
        [HttpDelete("{id}")]
        public IActionResult DeleteSlide(int id)
        {
            try
            {
                _slideService.DeleteSlide(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
