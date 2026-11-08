using Microsoft.AspNetCore.Mvc;
using MainHW.Models;
using MainHW.Models.DTO;
using MainHW.Services;
using static System.Net.Mime.MediaTypeNames;
using AutoMapper;
namespace MainHW.Controllers;

[ApiController]
[Route("api/[controller]")] // api/user
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public UserController(UserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }


    // реєстрація користувача

    [HttpPost("register")]
    public async Task<ActionResult<UserToken>> RegisterUserAsync([FromBody] UserRegisterRequest newUser)
    {
        try
        {
            var createdUser = await _userService.RegisterAsync(newUser);
            return Ok(new UserToken { Token = createdUser.Token });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // авторизація користувача

    [HttpPost("login")]
    public async Task<ActionResult<UserToken>> AuthenticateUserAsync([FromBody] UserLoginRequest loginUser)
    {
        var user = await _userService.AuthenticateAsync(loginUser.Email, loginUser.Password);
        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }
        return Ok(_mapper.Map<UserToken>(user));
    }

    // по ід користувача отримати дані користувача


    // оновлення користувача


    // перегляд всіх користувачів
    [HttpGet("all")]
    public async Task<ActionResult<ICollection<UserViewDto>>> GetAllUsersAsync()
    {
        var users = await _userService.GetAllAsync();
        var userDtos = users.Select(u => new UserViewDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            IsActive = u.IsActive,
            RoleId = u.RoleId
        }).ToList();
        return Ok(userDtos);
    }



}
