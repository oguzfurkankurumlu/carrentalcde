public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }




    public List<UserDTO> GetAllUsers()
    {
        var users = _userRepository.GetAllUsers();
        return users.Select(u => new UserDTO
        {
            UserId = u.UserId,
            Email = u.Email,
            FirstName = u.FirstName,
            LastName = u.LastName,
            City = u.City,
            Role = u.Role
        }).ToList();
    }

    public UserDTO GetUserById(int id)
    {
        var user = _userRepository.GetUserById(id);
        if (user != null)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Role = user.Role
            };
        }
        return null;
    }

    public void AddUser(UserDTO userDto)
    {
        var user = new User
        {
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            City = userDto.City,
            Role = userDto.Role,
            Password = userDto.Password,

        };

        _userRepository.AddUser(user);
    }

    public void UpdateUser(UserDTO userDto)
    {
        Console.WriteLine("UpdateUser metodu çağrıldı!"); // Terminalde görmek için

        var existingUser = _userRepository.GetUserById(userDto.UserId);
        if (existingUser != null)
        {
            Console.WriteLine("Kullanıcı bulundu: " + existingUser.UserId);

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.Email = userDto.Email;
            existingUser.City = userDto.City;
            existingUser.Role = userDto.Role;

            _userRepository.UpdateUser(existingUser);
            Console.WriteLine("Kullanıcı güncellendi.");
        }
        else
        {
            Console.WriteLine("HATA: Kullanıcı bulunamadı!");
        }
    }

    public void DeleteUser(int id)
    {
        _userRepository.DeleteUser(id);
    }

}
