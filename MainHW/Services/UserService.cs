using MainHW.Models;
using MainHW.Models.DTO;
using AutoMapper;
using MainHW.Models.Mapper;

namespace MainHW.Services;

public class UserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IRepository<User> _userRepository, IMapper mapper)
    {
        this._userRepository = _userRepository;
        this._mapper = mapper;
    }

    public async Task<ICollection<User>> GetAllAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User> RegisterAsync(UserRegisterRequest request)
    {
        var users = await _userRepository.GetAllAsync();
        // email unique
        if (users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }
        // use mapper to map UserRegisterRequest to User
        var newUser = _mapper.Map<User>(request);

        newUser.Token = Guid.NewGuid().ToString(); // Generate a unique token for the user
        newUser.IsActive = true; // Set the user as active by default
        newUser.RoleId = Roles.User; // Assign the default role (User)

        newUser.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        _userRepository.Add(newUser);
        await _userRepository.SaveChangesAsync();
        return newUser;
    }

    public async Task<User?> AuthenticateAsync(string email, string password)
    {
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        return user;
    }

    public async Task<User> CreateUserAsync(User newUser)
    {
        var users = await _userRepository.GetAllAsync();

        // email unique
        if (users.Any(u => u.Email.Equals(newUser.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }

        newUser.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        _userRepository.Add(newUser);
        await _userRepository.SaveChangesAsync();
        return newUser;
    }

    public async Task<User?> UpdateUserAsync(User updatedUser)
    {
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Id == updatedUser.Id);
        if (user == null)
        {
            return null;
        }
        // Check for email uniqueness if the email is being updated
        if (!user.Email.Equals(updatedUser.Email, StringComparison.OrdinalIgnoreCase) &&
            users.Any(u => u.Email.Equals(updatedUser.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidOperationException("A user with this email already exists.");
        }
        user = _mapper.Map(updatedUser, user);
        await _userRepository.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var users = await _userRepository.GetAllAsync();
        return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<User?> GetUserByTokenAsync(string token)
    {
        var users = await _userRepository.GetAllAsync();
        return users.FirstOrDefault(u => u.Token == token);
    }
}
