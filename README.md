# BlazorAdmin
<img style="width:40%; height:40%;" src="https://user-images.githubusercontent.com/35566242/162442880-f064506c-7300-4334-8874-3f9645694825.png" />

BlazorAdmin is an RCL (Razor Class Library) with pages and components for database create, read, update, delete operations. The main goal of the project is to support CRUD operations for generic models, to remove necessity of using CRUD scaffolding or manual processing of each individual model. BlazorAdmin takes a model and tries to find corresponding input for each of its properties. BlazorAdmin will index and display each model with ``[AdminVisible]`` attribute applied to the model class and one of its properties.

BlazorAdmin is designed to use with Blazor Server. Security and support for Blazor WebAssembly are not guaranteed.

## Features
1. Easy UI forms for CRUD operations on generic models.
2. Authorization using built-in .NET authorization flow.
3. Customization support for forms and inputs.
4. Sorting and search support thanks to [Tablesorter](https://github.com/Mottie/tablesorter).

## Setup
1. Add nuget to the project.
2. Add BlazorAdmin service. AddBlazorAdmin method accepts single string parameter of comma delimited user roles. Only user with the roles provided will be able to access 'blazoradmin' endpoint. Defaults to Admin.
```C#
using BlazorAdmin;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBlazorAdmin("Admin,Superuser");
```
3. Add BlazorAdmin assembly to the app router's AdditionalAssemblies.
```C#
<Router AppAssembly="@typeof(App).Assembly" AdditionalAssemblies = "new [] {typeof(BlazorAdmin.Pages.AdminIndex).Assembly}">
```
4. Apply ``[AdminVisible]`` attribute to a model class and at least one of its properties. Properties marked with this attribute will be visible on the entity list as column.
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
5. Add NavLink somewhere in your app pointing to blazoradmin endpoint.
```C#
<NavLink class="nav-link" href="blazoradmin">Admin</NavLink>
```
6. Optionally add [Tablesorter](https://github.com/Mottie/tablesorter) and [JQueryUI](https://jqueryui.com) plugins and their css to _Layout.cshtml <head> section in order to enable sorting and searching.
```html
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery.tablesorter/2.31.3/css/theme.jui.min.css" integrity="sha512-P8bbeO94Om6NRt8zty7v54b1LuwclWVqrufWMaZm/s+Bc+y8/fCL5iRk/yXtmZKA6FmB8G2ehSgVZXgPyJO1jQ==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jqueryui/1.12.1/themes/cupertino/jquery-ui.min.css" integrity="sha512-ug/p2fTnYRx/TfVgL8ejTWolaq93X+48/FLS9fKf7AiazbxHkSEENdzWkOxbjJO/X1grUPt9ERfBt21iLh2dxg==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js" integrity="sha512-894YE6QWD5I59HgZOGReFYm4dnWc1Qt5NtvYSaNcOP+u1T9qYdvdihz0PPSiiqn/+/3e7Jo4EaG7TubfWGUrMQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.tablesorter/2.31.3/js/jquery.tablesorter.min.js" integrity="sha512-qzgd5cYSZcosqpzpn7zF2ZId8f/8CHmFKZ8j7mU4OUXTNRd5g+ZHBPsgKEwoqxCtdQvExE5LprwwPAgoicguNg==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.tablesorter/2.31.3/js/jquery.tablesorter.widgets.min.js" integrity="sha512-dj/9K5GRIEZu+Igm9tC16XPOTz0RdPk9FGxfZxShWf65JJNU2TjbElGjuOo3EhwAJRPhJxwEJ5b+/Ouo+VqZdQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script>
        window.setTableSortable = () => {
            
            $(".blazoradmin-table").tablesorter({
                theme: 'jui',
                headerTemplate : '{content} {icon}',
                widgets : ['uitheme', 'filter'],
                widgetOptions: {
                    filter_hideFilters : true
                },
                headers: {
                   '.sort-ignore' : {
                     sorter: false
                   }
                },
                 
            });
        }   
    </script>    
```
## Authorization
BlazorAdmin uses built-in .NET ``[Authorize]`` attribute with roles defined during Setup to authorize users.
## Customization
Sometimes you can't rely on BlazorAdmin generic inputs or forms. For example, to edit ``IdentityUser`` or ``IdentityRole`` entities you must use ``UserManager`` and ``RoleManager``. To do this you can use custom inputs and forms. Individual input fields or entire forms can be overriden with custom blazor components.
### Custom Input
Your custom input component must derive from ``BlazorAdmin.Components.InputBase``. Model to edit will be passed to it as CascadingParameter. Apply ``[AdminInput(typeof(MyCustomInput))]`` to a property of a model class to use ``MyCustomInput`` as input for a model property. See [example](https://github.com/Vansh0t/BlazorAdmin/blob/master/BlazorAdmin.Examples/CustomComponents/ImageInput.razor).
### Custom Form
Your custom form component must derive from ``BlazorAdmin.Components.FormBase``. Model to edit will be passed to it as CascadingParameter. Apply ``[AdminForm(typeof(MyCustomForm))]`` to a model class to use ``MyCustomForm`` as edit form for a model. See [example](https://github.com/Vansh0t/BlazorAdmin/blob/master/BlazorAdmin.Examples/CustomComponents/IdentityUserForm.razor).
