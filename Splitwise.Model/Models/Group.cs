using Splitwise.Models.Enums;
using System.Collections.Generic;

namespace Splitwise.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GroupCategory Category { get; set; }
        public IEnumerable<User> Users { get; set; }
        public List<Expense> expenses { get; set; }
        public double CurrentBalance { get; set; }
    }
}