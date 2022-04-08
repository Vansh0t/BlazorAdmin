using Microsoft.AspNetCore.Components;

namespace BlazorAdmin.Utils
{
    internal class Components
    {
        /// <summary>
        /// Renders simple component of type without parameters.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static RenderFragment RenderSimple(Type type) => builder =>
        {
            builder.OpenComponent(1, type);
            builder.CloseComponent();
        };
        internal static RenderFragment RenderCustomForm(Type type, bool isNewInstance, string error
            ) => builder =>
        {
            builder.OpenComponent(1, type);
            builder.AddAttribute(2, "IsNewInstance", isNewInstance);
            builder.AddAttribute(3, "Error", error);
            builder.CloseComponent();
        };
    }
}
