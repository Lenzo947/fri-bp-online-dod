using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace BP_OnlineDOD.Server.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
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
