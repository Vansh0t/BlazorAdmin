namespace BlazorAdmin.Annotations
{
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Class)]
    /// <summary>
    /// A model property with this attribute applied is shown on administration UI.
    /// </summary>
    public class AdminVisible:Attribute
    {
        public string name = null;
    }
}
