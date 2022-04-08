# BlazorAdmin
BlazorAdmin is an RCL (Razor Class Library) with pages and components for database create, read, update, delete operations. The main goal of the project is to support CRUD operations for generic models. This means a user doesn't have to use CRUD scaffolding or manually process each individual model, but instead can simple add one attribute [AdminVisible] to a model and be able to execute all CRUD operations.
## Installation
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
