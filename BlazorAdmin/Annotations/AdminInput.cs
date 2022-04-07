
namespace BlazorAdmin.Annotations
{
    [AttributeUsage(AttributeTargets.Property)]
    /// <summary>
    /// Defines custom display component for model property.
    /// </summary>
    public class AdminInput:Attribute
    {
        public Type componentType;
        public AdminInput(Type componentType)
        {
            this.componentType = componentType;
        }
    }
}
