using System.Runtime;
using System.Runtime.ExceptionServices;

namespace ImperfectDatabaseApi.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public static User None => new User { Id = Guid.Empty, Name = string.Empty, Profile = new User.UserProfile() };
        public UserProfile Profile { get; set; }

        public class UserProfile
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime DateOfBirth { get; set; }
        }

        internal Author ToAuthor()
        {
            return new Author { UserId = Id, Name = this.Name, FullName = $"{Profile.FirstName} {Profile.LastName}" };
        }
    }
}