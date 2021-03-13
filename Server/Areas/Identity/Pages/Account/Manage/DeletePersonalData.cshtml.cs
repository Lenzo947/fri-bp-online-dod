using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BP_OnlineDOD.Server.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("../Login");
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("../Login");
        }
    }
}
