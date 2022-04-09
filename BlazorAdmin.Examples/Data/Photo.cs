using BlazorAdmin.Annotations;
namespace BlazorAdmin.Examples.Data
{
    [AdminVisible]
    public class Photo
    {
        public int Id { get; set; }
        [AdminVisible]
        public string Name { get; set; }
        [AdminVisible]
        public DateTime DateTaken { get; set; }
        [AdminInput(typeof(CustomComponents.ImageInput))]
        public Image Image { get; set; }
        public int? ImageId { get; set; }
    }
}
