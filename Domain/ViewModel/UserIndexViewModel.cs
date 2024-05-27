using Domain.Models;

namespace Test1.Domain.ViewModel
{
    public class UserIndexViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
