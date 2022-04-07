using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorAdmin
{
    using Annotations;
    public static class Extensions
    {
        /// <summary>
        /// obj is null ? null : obj.ToString() shortcut.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static string ToStringOrNull(this object obj)
        {
            return obj is null ? null : obj.ToString();
        }
        /// <summary>
        /// Converts array of properties to Dictionary, where keys are property names and values property values of given object
        /// </summary>
        /// <param name="props"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static Dictionary<string, object> ToDict(this PropertyInfo[] props, object obj)
        {
            Dictionary<string, object> result = new();
            foreach(var p in props)
            {
                
                var val = p.GetValue(obj);
                //Console.WriteLine(val.Is()?val.GetType():"Null");
                Console.WriteLine(p.PropertyType);
                var name = p.Name;
                result.Add(name, val);
            }
            return result;
        }
        /// <summary>
        /// Converts array of properties to Dictionary, where keys are property names and values property values of given object.
        /// All values are converted to string via .ToString()
        /// </summary>
        /// <param name="props"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static Dictionary<string, string> ToStringDict(this PropertyInfo[] props, object obj)
        {
            Dictionary<string, string?> result = new();
            foreach (var p in props)
            {
                var val = p.GetValue(obj);
                var name = p.Name;
                result.Add(name, val.ToStringOrNull());
            }
            return result;
        }
        /// <summary>
        /// Converts array of properties to Dictionary with property type.
        /// All values are converted to string via .ToString()
        /// </summary>
        /// <param name="props"></param>
        /// <param name="obj"></param>
        /// <returns>Dictionary with property name as key and object[] where object[0]=value, object[1]=type</returns>
        internal static Dictionary<string, object[]> ToTypeDict(this PropertyInfo[] props, object obj)
        {
            Dictionary<string, object[]> result = new();
            
            foreach (var p in props)
            {
                var val = p.GetValue(obj);
                var name = p.Name;
                result.Add(name, new object[2] { val.ToStringOrNull(), p.PropertyType});
            }
            return result;
        }
        /// <summary>
        /// Returns property value of object by text name.
        /// </summary>
        /// <param name="props"></param>
        /// <param name="propName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static object GetPropertyValue(this PropertyInfo[] props, string propName, object obj)
        {
            var prop = props.FirstOrDefault(_ => _.Name == propName);
            if (prop is null)
                return null;
            var val = prop.GetValue(obj);
            return val;
        }
        internal static bool IsPKey(this DbContext ctx, string propName, Type modelType)
        {
            var eType = ctx.Model.FindEntityType(modelType);
            if (eType is null)
                return false;
            var pkey = eType.FindPrimaryKey();
            if (pkey is not null)
            {
                var pkName = pkey.Properties.FirstOrDefault(_=>_.Name==propName);
                if (pkName is not null)
                    return true;
            }
            return false;
        }

        internal static object Property(this object obj, string propName)
        {
            return obj.GetType().GetProperty(propName)?.GetValue(obj);
        }
        internal static string GetEntityName(this IReadOnlyList<IProperty> entityProps)
        {
            return entityProps.Select(_ => _.Name).SingleOrDefault();
        }
        internal static bool IsEnumerable(this Type type, bool ignoreString = true)
        {
            var isEnumerable = typeof(System.Collections.IEnumerable).IsAssignableFrom(type);
            var checkString = false;
            if (ignoreString)
                checkString = typeof(string).IsAssignableFrom(type);
            if (isEnumerable && !checkString)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Get AdminVisible Attribute from PropertyInfo.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns>AdminVisible or null</returns>
        internal static AdminVisible GetAdminVisible(this PropertyInfo prop) 
        {
            var obj = prop.GetCustomAttributes(typeof(AdminVisible), false).FirstOrDefault();
            if (obj is not null)
            {
                return obj as AdminVisible;
            }
            return null;
            
        }
        /// <summary>
        /// Get AdminVisible Attribute from Type.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns>AdminVisible or null</returns>
        internal static AdminVisible GetAdminVisible(this Type type)
        {
            return type.GetCustomAttribute(typeof(AdminVisible)) as AdminVisible;
        }
        public static IServiceCollection AddBlazorAdmin(this IServiceCollection services, string adminEndpoint, string roles = "Admin")
        {
            var adminService = new Services.BlazorAdminService(adminEndpoint, roles);
            services.AddSingleton(adminService);
            return services;
        }
    }
}
