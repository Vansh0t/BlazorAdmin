# BlazorAdmin
<img style="width:40%; height:40%;" src="https://user-images.githubusercontent.com/35566242/162442880-f064506c-7300-4334-8874-3f9645694825.png" />

BlazorAdmin is an RCL (Razor Class Library) with pages and components for database create, read, update, delete operations. The main goal of the project is to support CRUD operations for generic models. This means a user doesn't have to use CRUD scaffolding or manually process each individual model, but instead can simple add one attribute ``[AdminVisible]`` to a model and be able to execute all CRUD operations.
## Setup
1. Add nuget to the project
2. Add BlazorAdmin service. The first parameter defines endpoint for admin services, the second one defines comma delimited roles that user should have to be authorized. Defaults to Admin.
```C#
using BlazorAdmin;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBlazorAdmin("admin", "Admin");
```
3. Add BlazorAdmin assembly to the app router's AdditionalAssemblies
```C#
<Router AppAssembly="@typeof(App).Assembly" AdditionalAssemblies = "new [] {typeof(BlazorAdmin.Pages.AdminIndex).Assembly}">
```
4. Apply ``[AdminVisible]`` attribute to a model class and at least one of its properties. Properties marked with the attribute will be visible on the entities list as column
```C#
using BlazorAdmin.Annotations;
    [AdminVisible]
    public class Event
    {
        public int Id { get; set; } //Will not be displayed on the entities list
        [AdminVisible]
        public string Name { get; set; } = "New Event";
    }
```
5. Add NavLink somewhere in your app, pointing to endpoint you've set in step 2.
```C#
<NavLink class="nav-link" href="admin">Admin</NavLink>
```
## Authorization
BlazorAdmin uses built-in .NET ``[Authorize]`` attribute with roles defined during Setup to authorize users.
## Customization
Sometimes you can't rely on BlazorAdmin generic inputs or forms. For example, to edit ``IdentityUser`` or ``IdentityRole`` entities you must use ``UserManager`` and ``RoleManager``. To do this you can use custom inputs and forms. Individual input fields or entire forms can be overriden with custom blazor components.
### Custom Input
Custom input component must derive from BlazorAdmin.Components.InputBase. Model to edit will be passed to it as CascadingParameter. Apply ``[AdminInput(typeof(MyCustomInput))]`` to a property of a model class. Example.
### Custom Form
Custom form component must derive from BlazorAdmin.Components.FormBase. Model to edit will be passed to it as CascadingParameter. Apply ``[AdminForm(typeof(MyCustomForm))]`` to a model class. Example.
