public interface IUserService
{
    List<UserDTO> GetAllUsers();
    UserDTO GetUserById(int id);
    void AddUser(UserDTO userDto);
    void UpdateUser(UserDTO userDto);
    void DeleteUser(int id);
}
