using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAPIDemo.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string Position { get; set; }
    }
}
