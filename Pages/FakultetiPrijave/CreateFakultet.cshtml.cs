#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#region snippet
using FaksPrijave.Authorization;
using FaksPrijave.Data;
using FaksPrijave.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FaksPrijave.Pages.FakultetiPrijave
{
    public class CreateFakultetModel : DI_BasePageModel
    {
        public CreateFakultetModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Fakultet Fakultet { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, Fakultet,
                                                        ContactOperations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Context.Fakultet.Add(Fakultet);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
#endregion
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.