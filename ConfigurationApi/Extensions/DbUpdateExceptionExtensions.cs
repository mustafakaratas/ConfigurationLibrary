using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ConfigurationApi.Extensions;

public static class DbUpdateExceptionExtensions
{
    public static bool IsUniqueIndexException(this DbUpdateException exception)
    {
        SqlException innerException = null;
        Exception temp = exception;

        while (innerException == null && temp != null)
        {
            innerException = temp.InnerException as SqlException;
            temp = temp.InnerException;
        }

        return innerException is { Number: 2601 };
    }
}