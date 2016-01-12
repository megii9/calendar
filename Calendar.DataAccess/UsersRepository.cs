using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Calendar.Domain;
using Calendar.Interface;
using Dapper;

namespace Calendar.DataAccess
{
    public class UsersRepository : IUsersRepository
    {
        private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Calendar.DB;Integrated Security=True;";
        private string selectByCategoryIdStatement = @"
SELECT Email FROM Users
INNER JOIN UsersCategories ON Users.Id = UsersCategories.UserId
WHERE UsersCategories.CategoryId = @categoryId";

        public IEnumerable<string> GetEmailsByCategory(Category category)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query(selectByCategoryIdStatement, new {categoryId = category.Id})
                    .Select(ResultToEmail);
            }
        }

        private string ResultToEmail(dynamic result)
        {
            return result.Email;
        }
    }
}
