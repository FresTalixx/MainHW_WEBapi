using MainHW.Models;
using MainHW.Services;
using Microsoft.AspNetCore.Mvc;

namespace MainHW.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadFileController(
        UploadFileService uploadFileService,
        UserService userService, 
        CategoryService categoryService
    ) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> UploadFileForBackground(IFormFile file)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
        {
            return Unauthorized("User not authenticated.");
        }
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var fileName = await uploadFileService.UploadFileAsync(file);
        await userService.UpdateBackgroundFileAsync(user, fileName);
        HttpContext.Items["User"] = user;
        return Ok(new { message = "File uploaded successfully.", fileName });
    }

}
