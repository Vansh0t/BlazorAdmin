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
        protected DbContext Ctx { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
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
                await Ctx.AddAsync(Model);
                IsNewInstance = false;
            }

            await Ctx.SaveChangesAsync();
            NavigationManager.NavigateTo($"/admin/{SetName}", false);
        }
        private async Task DeleteAsync()
        {
            if (!IsNewInstance)
            {
                Ctx.Remove(Model);
                await Ctx.SaveChangesAsync();
                NavigationManager.NavigateTo($"/admin/{SetName}", true);
            }
        }
        private void OnCancelClick(MouseEventArgs e)
        {
            NavigationManager.NavigateTo($"/admin/{SetName}", true);
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
