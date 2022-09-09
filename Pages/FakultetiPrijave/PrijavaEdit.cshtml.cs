using FaksPrijave.Authorization;
using FaksPrijave.Data;
using FaksPrijave.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace FaksPrijave.Pages.FakultetiPrijave
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    #region snippet
    public class PrijavaEditModel : DI_BasePageModel
    {
        public PrijavaEditModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        [BindProperty]//Malo vidit kaj je tocno ovo!
        public Prijava Prijava { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Prijava? prijava = await Context.Prijava.FirstOrDefaultAsync(
                                                             p => p.PrijavaId == id);
            if (prijava == null)
            {
                return NotFound();
            }

            Prijava = prijava;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                      User, Prijava,
                                                      ContactOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Fetch Contact from DB to get OwnerID.
            var prijava = await Context
                .Prijava.AsNoTracking()//Mozda se koristi jer vec prati jedan Prijava model
                .FirstOrDefaultAsync(p => p.PrijavaId == id);

            if (prijava == null)
            {
                return NotFound();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                     User, prijava,
                                                     ContactOperations.Update);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Prijava.OwnerID = prijava.OwnerID;
            //Cini se da sam ovdje trebo napravit Prijava.FakultetId
            //Ali sam vec napravio na drugi nacin tako da sam u cshtml
            //Dodao hidden value

            Context.Attach(Prijava).State = EntityState.Modified;

            Prijava.UkupnoBodova = Prijava.BodoviNatjecanja + Prijava.BodoviSrednja + Prijava.BodoviPrijemni;
            await Context.SaveChangesAsync();

            return RedirectToPage("./Prijava", new { id = Prijava.FakultetId });
        }
    }
    #endregion
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}
