using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
namespace BlazorAdmin.Components
{
    using Utils;
    partial class GenericInput:GenericInputBase
    {
        [Inject]
        protected NavigationManager _nav { get; set; }
        private const int MAX_FILE_SIZE = 10485760; //~10mb 
        private readonly Dictionary<Type, Dictionary<string, object>> InputMap = new()
        {
            { typeof(bool), QuickDict("type", "checkbox") },
            { typeof(bool?), QuickDict("type", "checkbox") },

            { typeof(string), QuickDict("type", "text") },

            { typeof(DateTime), QuickDict("type", "datetime-local", "step", ".1") },
            { typeof(DateTimeOffset), QuickDict("type", "datetime-local", "step", ".1") },
            { typeof(DateTime?), QuickDict("type", "datetime-local", "step", ".1") },
            { typeof(DateTimeOffset?), QuickDict("type", "datetime-local", "step", ".1") },

            { typeof(int), QuickDict("type", "number") },
            { typeof(long), QuickDict("type", "number") },
            { typeof(short), QuickDict("type", "number") },
            { typeof(float), QuickDict("type", "number", "step", "any") },
            { typeof(decimal), QuickDict("type", "number", "step", "any") },
            { typeof(double), QuickDict("type", "number", "step", "any") },

            { typeof(int?), QuickDict("type", "number") },
            { typeof(long?), QuickDict("type", "number") },
            { typeof(short?), QuickDict("type", "number") },
            { typeof(float?), QuickDict("type", "number", "step", "any") },
            { typeof(decimal?), QuickDict("type", "number", "step", "any") },
            { typeof(double?), QuickDict("type", "number", "step", "any") },

            { typeof(uint), QuickDict("type", "number", "min", 0) },
            { typeof(ulong), QuickDict("type", "number", "min", 0) },
            { typeof(ushort), QuickDict("type", "number", "min", 0) },

            { typeof(uint?), QuickDict("type", "number", "min", 0) },
            { typeof(ulong?), QuickDict("type", "number", "min", 0) },
            { typeof(ushort?), QuickDict("type", "number", "min", 0) },


        };
        private static Dictionary<string, object> QuickDict(params object[] kvp)
        {
            if (kvp.Length <= 0 || kvp.Length % 2 != 0)
            {
                throw new Exception("To make dictionary parameters Length must be even and greater than 0.");
            }
            Dictionary<string, object> dict = new();
            for (int i = 0; i < kvp.Length; i += 2)
            {
                dict.Add(kvp[i].ToString(), kvp[i + 1]);
            }
            return dict;
        }
        private async Task OnFileUploaded(InputFileChangeEventArgs e)
        {
            Error = "";
            if (e.File.Size > MAX_FILE_SIZE)
            {
                Error = "File is too big";
                return;
            }
            using (MemoryStream ms = new())
            {
                await e.File.OpenReadStream(MAX_FILE_SIZE).CopyToAsync(ms);
                byte[] bytes = ms.ToArray();
                SetProperty(bytes);
            }

            //testEntityInstance.File = 
        }
        private void SetProperty(object value)
        {
            try
            {
                Generic.SetProperty(Model.Model, Property, value);
            }
            catch (Exception e)
            {
                Error = e.ToString();
            }

        }
        private void SetPropertyFromEntitySet(object value)
        {
            var vType = value.GetType();
            //Single
            if (vType.Equals(typeof(string)))
            {
                int index = int.Parse(value.ToString());
                try
                {
                    Generic.SetPropertyFromSet(Model.Model, Property, EntitySet, index);
                }
                catch (Exception e)
                {
                    Error = e.ToString();
                }
            }
            //Multiple
            else if (vType.Equals(typeof(string[])))
            {
                int[] indexes = Array.ConvertAll(value as string[], int.Parse);
                try
                {
                    Generic.SetPropertyFromSet(Model.Model, Property, EntitySet, indexes);
                }
                catch (Exception e)
                {
                    Error = e.ToString();
                }
            }
            else
            {
                Error = "Unknown value type provided " + vType;
            }

        }
    }
}
