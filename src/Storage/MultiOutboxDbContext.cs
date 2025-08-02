using Microsoft.EntityFrameworkCore;

namespace Storage;

public class MultiOutboxDbContext : DbContext
{
    public MultiOutboxDbContext(DbContextOptions<MultiOutboxDbContext> options)
        : base(options) { }
}
