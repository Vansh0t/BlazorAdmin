using System;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using BlazorAdmin.Utils;

namespace BlazorAdmin.Pages
{
    public partial class AdminIndex : ComponentBase
    {
        [Inject]
        protected DbContext Ctx { get; set; }
        internal List<Database.Set> sets;
        [Parameter]
        public string EndpointBase { get { return _adminService.Endpoint; } set { } }

        protected async override Task OnInitializedAsync()
        {
            sets = await Database.CountSetsAsync(Ctx);
        }
    }
}
