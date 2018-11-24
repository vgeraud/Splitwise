using Splitwise.Models.Enums;
using System.Collections.Generic;

namespace Splitwise.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Currency Currency { get; set; }
        public List<User> Friends { get; set; }
        public IEnumerable<Expense> Expenses { get; set; }
    }
}