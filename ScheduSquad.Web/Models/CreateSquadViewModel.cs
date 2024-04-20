using System.ComponentModel.DataAnnotations;

namespace ScheduSquad.Web.Models
{
    public class CreateSquadViewModel
    {
        [Required(ErrorMessage = "Squad Name is required.")]
        public string SquadName { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }
    }
}