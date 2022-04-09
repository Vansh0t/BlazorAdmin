using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
namespace BlazorAdmin.Pages
{
    using Utils;
    using Annotations;
    public partial class ModelIndex:ComponentBase
    {
        [Inject]
        protected DbContext _ctx { get; set; }
        [Inject]
        protected IJSRuntime _js { get; set; }
        [Parameter]
        public string SetName { get; set; }
        //[Parameter]
        //public string EndpointBase { get { return _adminService.Endpoint; } set { } }

        public Type modelType;
        public List<ModelProperty> modelProps = new();
        public List<ModelRow> rows = new();

        private IQueryable<object> set;

        public string Error { get; set; } = "";

        public bool ShowDeleteConfirm { get; set; }
        //public List<object> models = new();
        private object targetEntity;
        public class ModelProperty
        {
            public string name;
            public string displayName;
            public bool isVisible = false;
            public bool isPKey = false;
            
            public ModelProperty(string name, bool isVisible, bool isPKey = false, string displayName = null)
            {
                this.name = name;
                this.displayName = displayName;
                this.isPKey = isPKey;
                this.isVisible = isVisible;
            }
        }
        public class ModelRow
        {
            public string pkeyName = null;
            public string pkeyValue = null;
            public object entity = null;
            public Dictionary<string, string> data = new();
        }

        protected async override Task OnInitializedAsync()
        {
            await InitDbDataAsync(SetName, _ctx);
        }
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                try
                {
                    await _js.InvokeVoidAsync("setTableSortable");
                }
                catch
                {

                }
        }
        public async Task InitDbDataAsync(string setName, DbContext ctx)
        {
            set = Database.GetSetQueryable(ctx, setName);
            if (set is not null)
            {
                modelType = set.ElementType;
                if (modelType.GetAdminVisible() is not null)
                {
                    InitModelProps(ctx);
                    IncludeNavigations(ctx);
                    await InitRowsAsync();
                }
                else
                {
                    Error = $"{modelType} does not have [AdminVisible] attribute.";
                }
                if(modelProps.Count==0)
                {
                    Error = "No model properties set to display. Add [AdminVisible] attribute to the model's property.";
                }
            }
        }
        private void InitModelProps(DbContext ctx)
        {
            foreach (var p in modelType.GetProperties())
            {
                AdminVisible attr = p.GetAdminVisible();
                bool isPKey = ctx.IsPKey(p.Name, modelType);
                if (attr is not null)
                {
                    modelProps.Add(new ModelProperty(p.Name, true, isPKey, attr.name));
                }
                else
                {
                    modelProps.Add(new ModelProperty(p.Name, false, isPKey));
                }

            }
        }
        private void IncludeNavigations(DbContext ctx)
        {
            var navs = ctx.Model.FindEntityType(modelType).GetNavigations();
                List<string> includes = navs.Select(_ => _.Name).ToList();

                foreach (var i in includes)
                {
                    set = set.Include(i);
                }
        }
        private async Task InitRowsAsync()
        {
            var data = await set.ToArrayAsync();
            foreach (var d in data)
            {
                var propsInfo = d.GetType().GetProperties();
                var props = propsInfo.ToStringDict(d);
                var pkeyProp = modelProps.FirstOrDefault(_ => _.isPKey);
                if (pkeyProp is not null)
                {
                    var pkeyValue = propsInfo.GetPropertyValue(pkeyProp.name, d);
                    string pkeyStr = pkeyValue.ToStringOrNull();
                    rows.Add(new ModelRow { pkeyName = pkeyProp.name, pkeyValue = pkeyStr, data = props, entity=d  });
                }
                else
                {
                    rows.Add(new ModelRow { data = props, entity=d });
                }
            }
        }
        private async Task DeleteAsync()
        {
            _ctx.Remove(targetEntity);
            await _ctx.SaveChangesAsync();
            rows.Remove(rows.First(_ => _.entity == targetEntity));
            targetEntity = null;
            ShowDeleteConfirm = false;
            StateHasChanged();
        }
    }
}
