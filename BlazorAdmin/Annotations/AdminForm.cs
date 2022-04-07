namespace BlazorAdmin.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    /// <summary>
    /// Defines custom display component for model property.
    /// </summary>
    public class AdminForm : Attribute
    {
        public Type componentType;
        public AdminForm(Type componentType)
        {
            this.componentType = componentType;
        }
    }
}
