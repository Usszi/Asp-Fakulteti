using FaksPrijave.Authorization;
using FaksPrijave.Data;
using FaksPrijave.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FaksPrijave.Pages.FakultetiPrijave
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    #region snippet
    public class PrijavaModel : DI_BasePageModel
    {
        public PrijavaModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager)
            : base(context, authorizationService, userManager)
        {
        }

        public Fakultet Fakultet { get; set; }

        public Prijava Prijava { get; set; }

        public IList<Prijava> PrijavaList { get; set; }

        public Boolean Prijavljen { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Fakultet? _fakultet = await Context.Fakultet.FirstOrDefaultAsync(f => f.FakultetId == id);
            if (_fakultet != null)
            {
                Fakultet = _fakultet;
            }

            #pragma warning disable CS8602
            var prijave = from p in Context.Prijava
                          where p.FakultetId == _fakultet.FakultetId
                          orderby p.UkupnoBodova descending
                          select p;
            #pragma warning restore CS8602

            var isAuthorized = User.IsInRole(Constants.ContactAdministratorsRole);

            var currentUserId = UserManager.GetUserId(User);

            Prijava? _prijava = await Context.Prijava.FirstOrDefaultAsync(pp => pp.OwnerID == currentUserId && pp.FakultetId == _fakultet.FakultetId);
            if (_prijava != null)
            {
                Prijavljen = true;
            }
            else {Prijavljen = false; }

            if (_prijava != null)
            {
                Prijava = _prijava;
            }



            PrijavaList = await prijave.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, string idtype, int fid)
            //fid je pointless kod prijava
        {
            if (idtype == "prijava")
            {

                Fakultet? _fakultet = await Context.Fakultet.FirstOrDefaultAsync(f => f.FakultetId == id);
                var currentUserId = UserManager.GetUserId(User);
                if (_fakultet != null) {
                    Context.Prijava.Add(
                        new Prijava
                        {
                            FakultetId = _fakultet.FakultetId,
                            OwnerID = currentUserId,
                            BodoviPrijemni = 0,
                            BodoviSrednja = 0,
                            BodoviNatjecanja = 0,
                            UkupnoBodova = 0
                        });
                    await Context.SaveChangesAsync();
                }
                else { RedirectToPage("../Error"); }

            } else if (idtype == "odjava") {

                Prijava? _prijava = await Context.Prijava.FirstOrDefaultAsync(p => p.PrijavaId == id);
                if (_prijava != null)
                {
                    Context.Prijava.Remove(_prijava);
                    await Context.SaveChangesAsync();
                }
                else { return RedirectToPage("../Error"); }
            }
            else { return NotFound(); }

            return RedirectToPage("./Prijava", new {id = fid });
        }
    }
    #endregion
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
