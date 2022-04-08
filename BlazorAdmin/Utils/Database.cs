using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace BlazorAdmin.Utils
{
    public class Database
    {

        public class Set
        {
            public string name;
            public int count;
            public Set(string name, int count)
            {
                this.name = name;
                this.count = count;
            }
        }

        public static async Task<List<Set>> CountSetsAsync(DbContext context, bool onlyAdminVisible = true)
        {
            List<Set> sets = new();
            foreach (var p in context.GetType().GetProperties())
            {
                var name = p.Name;
                var val = p.GetValue(context);
                int count;
                if (val is not null && val is IQueryable<object>)
                {
        
                    var dbset = (IQueryable<object>)val;
                    if(onlyAdminVisible)
                    {
                        var attr = dbset.ElementType.GetCustomAttribute(typeof(Annotations.AdminVisible)) as Annotations.AdminVisible;

                        if (attr is not null)
                        {
                            count = await dbset.CountAsync();
                            sets.Add(new Set(attr.name is not null ? attr.name : name, count));
                        }
                    }
                    else
                    {
                        count = await dbset.CountAsync();
                        sets.Add(new Set(name, count));
                    }
        
                }
        
        
            }
            return sets;
        }
        public static IQueryable<object> GetSetQueryable(DbContext context, string name)
        {
            foreach (var p in context.GetType().GetProperties())
            {
                if (p.Name == name)
                {
                    var val = p.GetValue(context);
                    if (val is IQueryable<object>)
                        return val as IQueryable<object>;
                    break;
                }
            }
            return null;
        }
        public static IQueryable<object> GetSetQueryable(DbContext context, Type modelType)
        {
            var prop = context.GetType().GetProperties().FirstOrDefault(_ => 
            (_.GetValue(context) as IQueryable).ElementType.Equals(modelType)
            );
            return prop.GetValue(context) as IQueryable<object>;
        }
        public static async Task<object> GetEntityByIdAsync(DbContext context, Type modelType, object modelId)
        {
            var pkeyType = context.Model.FindEntityType(modelType).FindPrimaryKey().GetKeyType();
            return await context.FindAsync(modelType, Convert.ChangeType(modelId, pkeyType));

        }
        public static async Task<Dictionary<PropertyInfo, List<object>>> GetNavigationSetsAsync(DbContext context, Type modelType)
        {
            Dictionary<PropertyInfo, List<object>> sets = new();
            var navs = context.Model.FindEntityType(modelType).GetNavigations();
            var props = modelType.GetProperties();
            foreach (var n in navs)
            {
                var set = GetSetQueryable(context, n.TargetEntityType.ClrType);
                if (set is not null)
                    sets.Add(props.First(_ => _.Name == n.Name), await set.ToListAsync());
            }
            return sets;
        }
        public static async Task RemoveEntityAsync(DbContext context, object entity)
        {
            context.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
