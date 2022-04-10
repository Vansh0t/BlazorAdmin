using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.EntityFrameworkCore;
using BlazorAdmin.Utils;

namespace BlazorAdmin.Pages
{
    public partial class AdminIndex : ComponentBase
    {
        [Inject]
        protected DbContext _ctx { get; set; }
        [Inject]
        protected IJSRuntime _js { get; set; }
        internal List<Database.Set> sets;
        private bool isInitDone;
        //[Parameter]
        //public string EndpointBase { get { return _adminService.Endpoint; } set { } }

        protected async override Task OnInitializedAsync()
        {
            sets = await Database.CountSetsAsync(_ctx);
            isInitDone = true;
        }
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {

            //if(!firstRender)
                try
                {
                    await _js.InvokeVoidAsync("setTableSortable");
                }
                catch
                {

                }
                
        }
    }
}
