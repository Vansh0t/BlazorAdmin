using Microsoft.AspNetCore.Components;

namespace BlazorAdmin.Components
{
    public abstract class FormBase:ComponentBase
    {
        /// <summary>
        /// Whether this instance should be new entity in database
        /// </summary>
        [Parameter]
        public bool IsNewInstance { get; set; }
        /// <summary>
        /// Model instance passed by ModelEditor
        /// </summary>
        [CascadingParameter]
        public object Model { get; set; }
        /// <summary>
        /// Error message that could be displayed somewhere in form
        /// Could be set by ModelEditor if Model initialization failed
        /// </summary>
        [Parameter]
        public string Error { get; set; } = "";
    }
}
