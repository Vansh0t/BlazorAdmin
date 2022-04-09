
using BlazorAdmin.Annotations;
namespace BlazorAdmin.Examples.Data
{
    [AdminVisible]
    public class Image
    {
        public int Id { get; set; }
        [AdminVisible]
        public string Path { get; set; }
        [AdminVisible]
        public string Url { get; set; }

        public Image(string path, string url)
        {
            Path = path;
            Url = url;
        }
        public Image()
        {
        }
        public override string ToString()
        {
            return Url;
        }
    }
}
