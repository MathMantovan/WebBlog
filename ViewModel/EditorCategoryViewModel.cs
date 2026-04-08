using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModel
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O Nome é obrigatorio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O Slug é obrigatorio")]
        public string Slug { get; set; }
    }
}
