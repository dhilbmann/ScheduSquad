namespace ScheduSquad.Models
{
    public interface IPersisitedEntityBase
    {
        public Guid Id { get; set; }

        public bool IsDeleted { get; set; }
    }

}