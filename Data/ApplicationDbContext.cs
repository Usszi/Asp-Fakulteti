using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FaksPrijave.Models;

namespace FaksPrijave.Data;

public class ApplicationDbContext : IdentityDbContext
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        : base(options)
    {
    }
    public DbSet<Fakultet> Fakultet { get; set; }
    public DbSet<Prijava> Prijava { get; set; }
}
