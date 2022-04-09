namespace BlazorAdmin.Services
{
    public class BlazorAdminService
    {
        //@page directive does not allow setting dynamic endpoints without significant problems
        //public string Endpoint { get; }
        public string Roles { get; }
        internal static BlazorAdminService main;
        public BlazorAdminService(string roles)
        {
            //this.Endpoint = endpoint;
            this.Roles = roles;
            main = this;
        }
    }
}
