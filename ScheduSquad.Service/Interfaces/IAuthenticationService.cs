using ScheduSquad.Models;

namespace ScheduSquad.Service
{

    public interface ILoginAuthenticationService
    {
        public bool CheckPassword(string password, Guid memberId);

        public void UpdatePassword(string password, Guid memberId);

    }
}





