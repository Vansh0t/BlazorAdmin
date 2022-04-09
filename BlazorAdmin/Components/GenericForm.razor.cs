using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace BlazorAdmin.Components
{
    
    partial class GenericForm:FormBase
    {
        [Inject]
        protected DbContext _ctx { get; set; }
        [Inject]
        protected NavigationManager _nMgr { get; set; }
        [Parameter]
        public PropertyInfo[] Props { get; set; }
        [Parameter]
        public Dictionary<PropertyInfo, List<object>> SubSets { get; set; } = new();
        [Parameter]
        public string SetName { get; set; }

        private bool showDeleteConfirm;
        private async Task HandleValidAsync(EditContext editContext)
        {
            if (IsNewInstance)
            {
                await _ctx.AddAsync(Model);
                IsNewInstance = false;
            }

            await _ctx.SaveChangesAsync();
            _nMgr.NavigateTo($"/blazoradmin/{SetName}", false);
        }
        private async Task DeleteAsync()
        {
            if (!IsNewInstance)
            {
                _ctx.Remove(Model);
                await _ctx.SaveChangesAsync();
                _nMgr.NavigateTo($"/blazoradmin/{SetName}", true);
            }
        }
        private void OnCancelClick(MouseEventArgs e)
        {
            _nMgr.NavigateTo($"/blazoradmin/{SetName}", true);
        }
        private void OnDeleteClick(MouseEventArgs e)
        {
            showDeleteConfirm = true;
        }

        //private async Task InitModelAsync()
        //{
        //    var props = Model.Model.GetType().GetProperties();
        //    subSets = await Database.GetNavigationSetsAsync(Ctx, modelType);
        //}
    }
}
