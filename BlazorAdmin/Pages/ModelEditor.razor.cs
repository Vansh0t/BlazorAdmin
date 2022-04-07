using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BlazorAdmin.Pages
{
    using Utils;
    public partial class ModelEditor:ComponentBase
    {
        [Parameter]
        public string SetName { get; set; }
        [Parameter]
        public string ModelId { get; set; }
        [Inject]
        protected DbContext Ctx { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        public object Model { get; set; }

        private Dictionary<PropertyInfo, List<object>> subSets = new();

        private bool isNewInstance = false;

        private object dbSet;

        private Type modelType;

        private PropertyInfo[] props;

        //Make sure model init is completed so we don't run into DbContext concurrency issues
        private bool isModelInitialized;

        [Parameter]
        public string EndpointBase { get { return _adminService.Endpoint; } set { } }
        public class InputMap
        {
            public Type inputType;
            public HashSet<Type> supportedTypes;

            public InputMap(Type inputType, HashSet<Type> supportedTypes)
            {
                this.inputType = inputType;
                this.supportedTypes = supportedTypes;
            }
        }

        public string Error { get; set; } = "";
        protected async override Task OnInitializedAsync()
        {
            dbSet = Database.GetSetQueryable(Ctx, SetName);
            var query = dbSet as IQueryable<object>;
            if (dbSet is null)
            {
                Error = $"No DbSet found in {Ctx.GetType()} with name {SetName}.";
                return;
            }
            
            modelType = query.ElementType;
            if(modelType.GetAdminVisible() is null)
            {
                Error = $"{modelType} does not have [AdminVisible] attribute.";
                return;
            }
            if (ModelId is not null)
            {
                Model = await Database.GetEntityByIdAsync(Ctx, modelType, ModelId);
            }
            else
            {
                try
                {
                    Model = Activator.CreateInstance(modelType);
                    isNewInstance = true;
                }
                catch (MissingMethodException)
                {
                    Error = $"No supported constructor found for {modelType}. Model must have a parameterless constructor.";
                    return;
                }            
            }
            
            if(Model is null)
            {
                Error = $"No model with primary key {ModelId} found in {SetName}.";
                return;
            }
            await InitModelAsync();
        }
        private async Task InitModelAsync()
        {
            props = Model.GetType().GetProperties();
            subSets = await Database.GetNavigationSetsAsync(Ctx, modelType);
            isModelInitialized = true;
        }
        
    }
    
    
}
