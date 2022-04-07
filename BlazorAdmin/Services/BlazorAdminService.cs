namespace BlazorAdmin.Services
{
    public class BlazorAdminService
    {
        public string Endpoint { get; }
        public string Roles { get; }
        internal static BlazorAdminService main;
        public BlazorAdminService(string endpoint, string roles)
        {
            this.Endpoint = endpoint;
            this.Roles = roles;
            main = this;
        }
    }
}
