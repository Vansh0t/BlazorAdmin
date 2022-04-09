using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorAdmin.Components
{
    public abstract class InputBase: ComponentBase
    {
        /// <summary>
        /// Model instance passed by ModelEditor
        /// </summary>
        [CascadingParameter]
        protected EditContext Model { get; set; }
        /// <summary>
        /// Property of the Model being edited
        /// </summary>
        [Parameter]
        public virtual PropertyInfo Property { get; set; }
        /// <summary>
        /// Error message that could be displayed near input
        /// In custom input must be displayed manually
        /// </summary>
        [Parameter]
        public virtual string Error { get; set; }
    }
}
