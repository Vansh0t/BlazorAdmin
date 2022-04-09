using Microsoft.AspNetCore.Identity;
using BlazorAdmin.Annotations;
namespace BlazorAdmin.Examples.Data
{
    //Inherit IdentityUser to apply attributes
    [AdminVisible]
    [AdminForm(typeof(CustomComponents.IdentityUserForm))]
    public class AppUser:IdentityUser
    {
        [AdminVisible(name = "Name")]
        public override string UserName => base.UserName;
        [AdminVisible]
        public override string Email => base.Email;
        [AdminVisible(name = "Is Email Confirmed")]
        public override bool EmailConfirmed => base.EmailConfirmed;
    }
}
