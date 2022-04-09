using Microsoft.AspNetCore.Identity;
using BlazorAdmin.Annotations;
namespace BlazorAdmin.Examples.Data
{
    //Inherit IdentityRole to apply attributes
    [AdminVisible]
    public class AppRole:IdentityRole
    {
        public override string Id => base.Id;
        [AdminVisible]
        public override string Name => base.Name;
       
        public AppRole(string roleName) : base(roleName)
        {

        }
        public AppRole():base()
        {
        }
    }
}
