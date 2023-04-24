using System.ComponentModel.DataAnnotations;

namespace Web_API_SED.Entities
{
    public class User
    {
        [Required(ErrorMessage = "Por favor, informe um username")]
        public string username { get; set; }

        [Required(ErrorMessage = "Por favor, informe um password")]
        public string password { get; set; }
    }
}
