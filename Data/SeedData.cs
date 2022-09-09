using FaksPrijave.Authorization;
using FaksPrijave.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

// dotnet aspnet-codegenerator razorpage -m Contact -dc ApplicationDbContext -outDir Pages\Contacts --referenceScriptLibraries
namespace FaksPrijave.Data
{
    public static class SeedData
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        #region snippet_Initialize
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@faksprijave.com");
                await EnsureRole(serviceProvider, adminID, Constants.ContactAdministratorsRole);

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@faksprijave.com");
                await EnsureRole(serviceProvider, managerID, Constants.ContactManagersRole);

                SeedDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        #endregion
        #region snippet1
        public static void SeedDB(ApplicationDbContext context, string adminID)
        {
            if(!context.Fakultet.Any())
            {
                context.Fakultet.AddRange(
                    new Fakultet
                    {
                        FakultetIme = "Tehničko Veleučilište Zagreb",
                        SlobodnoMjesta = 400
                    },
                    new Fakultet
                    {
                        FakultetIme = "Fakultet Elektrotehnike i Računarstva",
                        SlobodnoMjesta = 500
                    },
                    new Fakultet
                    {
                        FakultetIme = "Fakultet Filozofije i Religijskih Znanosti",
                        SlobodnoMjesta = 800
                    },
                    new Fakultet
                    {
                        FakultetIme = "Fakultet Prometnih Znanosti",
                        SlobodnoMjesta = 400
                    },
                    new Fakultet
                    {
                        FakultetIme = "Fakultet Strojarstva i Brodogradnje",
                        SlobodnoMjesta = 550
                    },
                    new Fakultet
                    {
                        FakultetIme = "Fakultet Kemijskog Inžinjerstva i Tehnologije",
                        SlobodnoMjesta = 180
                    },
                    new Fakultet
                    {
                        FakultetIme = "Fakultet Političkih Znanosti",
                        SlobodnoMjesta = 450
                    });
                context.SaveChanges();
            }
            if (!context.Prijava.Any() && context.Fakultet.Any())
            {
                context.Prijava.AddRange(
                new Prijava
                {
                    FakultetId = 1,
                    OwnerID = adminID,
                    BodoviPrijemni = 0,
                    BodoviSrednja = 0,
                    BodoviNatjecanja = 0,
                    UkupnoBodova = 0
                },
                new Prijava
                {
                    FakultetId = 2,
                    OwnerID = adminID,
                    BodoviPrijemni = 0,
                    BodoviSrednja = 0,
                    BodoviNatjecanja = 0,
                    UkupnoBodova = 0
                });
                context.SaveChanges();
            }
            #endregion
        }
    }
}
#pragma warning restore CS8602 // Dereference of a possibly null reference.
