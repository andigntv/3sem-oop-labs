using BusinessLogic.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Extensions;

public static class DbSetExtensions
{
    public static async Task<T> GetEntityAsync<T>(this DbSet<T> set, Guid? id, CancellationToken cancellationToken)
        where T : class
    {
        if (id is null)
            return null;
        T? entity = await set.FindAsync(new object[] { id }, cancellationToken);

        if (entity is null)
            throw new NotFoundException("Entity not found");

        return entity;
    }
}