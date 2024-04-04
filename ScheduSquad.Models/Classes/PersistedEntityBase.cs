namespace ScheduSquad.Models
{
    public abstract class PersisitedEntityBase
    {
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }
    }

}