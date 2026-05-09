using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModel
{
    public class UploadImageViewModel
    {
        [Required(ErrorMessage = "Image inválida")]
        public string Base64Image { get; set; }
    }
}
