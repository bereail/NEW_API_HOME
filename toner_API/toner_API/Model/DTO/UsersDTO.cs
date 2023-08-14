using toner_API.Models;

namespace toner_API.Model.DTO
{
    public class UsersDTO : Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public int IdRol { get; set; }
    }
}
