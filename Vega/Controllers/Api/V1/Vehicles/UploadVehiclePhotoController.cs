using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Vega.Core;
using Vega.Core.Domain;
using Vega.Resources.V1;

namespace Vega.Controllers.Api.V1.Vehicles;

[Route("/api/v1/vehicles/{id:int}/photos")]
public class UploadVehiclePhotoController : BaseController
{
    private readonly IHostEnvironment _host;
    private readonly PhotoSettings _photoSettings;

    public UploadVehiclePhotoController(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IHostEnvironment host,
        IOptionsSnapshot<PhotoSettings> options
    ) : base(unitOfWork, mapper)
    {
        _host = host;
        _photoSettings = options.Value;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadPhoto(
        [FromRoute] int id,
        [FromForm] IFormFile? file)
    {
        var vehicle = await UnitOfWork.Vehicles.GetAsync(id);

        if (vehicle == null)
        {
            return NotFound();
        }

        if (null == file) return BadRequest("Null file");
        if (file.Length < 1) return BadRequest("Empty file");
        if (file.Length > _photoSettings.MaxBytes) return BadRequest(
            "file size should be 10mb maximum");
        if (!_photoSettings.IsSupported(file.FileName))
        {
            return BadRequest($"File type should be either ({string.Join(", ", _photoSettings.AcceptedFileTypes)})");
        }

        try
        {
            var uploadDir = Path.Combine(_host.ContentRootPath, "wwwroot", "uploads");
            Directory.CreateDirectory(uploadDir);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadDir, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var photo = new VehiclePhoto() { Name = fileName, Vehicle = vehicle };
            vehicle.Photos.Add(photo);
            await UnitOfWork.CompleteAsync();

            return Ok(Mapper.Map<VehiclePhoto, PhotoResource>(photo));
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Error = "Failed to upload file",
                ex.Message
            });
        }
    }
}
