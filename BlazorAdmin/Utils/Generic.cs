using System.Reflection;
namespace BlazorAdmin.Utils
{
    public class Generic
    {
        public static void SetProperty(object obj, PropertyInfo prop, object value)
        {
            var trueType = StripNullable(prop.PropertyType);
            Console.WriteLine(value.ToString());
            //Try simple convert
            try
            {
                if (trueType.Equals(typeof(DateTime)))
                {
                    //npgsql throws error if DateTimeKind is unspecified
                    //need testing with other providers
                    var dt = DateTime.SpecifyKind(DateTime.Parse(value.ToString()), DateTimeKind.Utc);
                    prop.SetValue(obj, dt);
                }
                else if (trueType.Equals(typeof(DateTimeOffset)))
                {
                    //npgsql throws error if DateTimeOffset.Offset is anything other than 0
                    //need testing with other providers
                    var dto = DateTimeOffset.Parse(value.ToString());
                    prop.SetValue(obj, dto.ToOffset(TimeSpan.Zero));
                }
                else
                    prop.SetValue(obj, Convert.ChangeType(value, trueType));
            }
            //Try special cases like null
            catch (Exception e)
            {
                Type valType = value.GetType();
                if (value == null || (valType.Equals(typeof(string)) && value.ToString() == ""))
                {
                    prop.SetValue(obj, null);
                }
                else
                    throw new Exception("Unable to assign Property! " + e.ToString());

            }
        }
        public static void SetPropertyFromSet(object obj, PropertyInfo prop, List<object> set, int index)
        {
            if (index == -1)
                prop.SetValue(obj, null);
            else
                prop.SetValue(obj, set[index]);
        }
        public static void SetPropertyFromSet(object obj, PropertyInfo prop, List<object> set, int[] indexes)
        {
            var clearMethod = prop.PropertyType.GetMethod("Clear");
            var addMethod = prop.PropertyType.GetMethod("Add");
            if (clearMethod is null || addMethod is null)
            {
                throw new Exception($"Property {prop.Name} of type {prop.PropertyType} does not have Clear or Add methods. Property must implement ICollection<>.");
            }
            clearMethod.Invoke(prop.GetValue(obj), null);
            foreach (var i in indexes)
            {
                addMethod.Invoke(prop.GetValue(obj), new object[] { set[i] });
            }
        }
        public static Type StripNullable(Type t)
        {
            var uType = Nullable.GetUnderlyingType(t);
            if (uType is not null)
            {
                return uType;
            }
            return t;
        }
    }
}
