using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorAdmin.Components
{
    public abstract class GenericInputBase: ComponentBase
    {
        [CascadingParameter]
        protected EditContext Model { get; set; }
        [Parameter]
        public virtual PropertyInfo Property { get; set; }
        [Parameter]
        public virtual List<object> EntitySet { get; set; }
        [Parameter]
        public virtual string Class { get; set; }
        [Parameter]
        public virtual string Error { get; set; }
    }
}
