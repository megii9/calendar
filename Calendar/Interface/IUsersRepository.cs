using System.Collections.Generic;
using Calendar.Domain;

namespace Calendar.Interface
{
    public interface IUsersRepository
    {
        IEnumerable<string> GetEmailsByCategory(Category category);
    }
}
