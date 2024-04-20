using ScheduSquad.Models;
using System.Configuration.Assemblies;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;


namespace ScheduSquad.Models
{
    public class Squad : IPersisitedEntityBase
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public List<Member> Members { get; set; }

        private string _name;
        public string Name { get { return _name; } }

        private string _description;
        public string Description { get { return _description; } }

        private string _location;
        public string Location { get { return _location; } }

        private Member _squadMaster;
        public Member SquadMaster { get { return _squadMaster; } }


        public Squad() {
            _name = String.Empty;
            _description = String.Empty;
            _location = String.Empty;
            _squadMaster = new Member();
            Members = new List<Member>();

        }

        public Squad(Guid id, Member squadMaster, string name, string description, string location) 
        {
            Id = id;
            _squadMaster = squadMaster;
            _name = name;
            _description = description;
            _location = location;
            Members = new List<Member>();
        }

        public Squad(Member squadMaster, string name, string description, string location) : this(Guid.NewGuid(), squadMaster, name, description, location) {}
               
        public List<string> GetAllAvailabilityDescriptions()
        {
            return new List<string>();
        }

        public void AddMember(Member member)
        {
            // check to see if the member exists; if they don't, add member, else throw an exception
            if (!Members.Exists(x => x.Id == member.Id)) Members.Add(member);
            else
                throw new Exception("Unable to add.  Member already exists in the squad.");
        }

        public void RemoveMember(Member member)
        {
            if (Members.Exists(x => x.Id == member.Id)) Members.Remove(member);
            else
                throw new Exception("Unable to remove.  Member doesn't exist in the squad.");
        }
    }
}
