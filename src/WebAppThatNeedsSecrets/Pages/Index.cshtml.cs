using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebAppThatNeedsSecrets.Pages
{
    public class IndexModel : PageModel
    {

        public IndexModel()
        {
        }
        
        public void OnGet()
        {
        }
    }
}
