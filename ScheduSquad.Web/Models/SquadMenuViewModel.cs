using System.ComponentModel.DataAnnotations;
using ScheduSquad.Models;

namespace ScheduSquad.Web.Models
{
    public class SquadMenuViewModel
    {
           public List<SquadMenuItem> UsersSquads { get; set; }
    }

    public class SquadMenuItem {
        public string SquadName { get; set; }
        public bool IsSquadmaster {get; set;}
        public string Id { get; set; }
    }
}