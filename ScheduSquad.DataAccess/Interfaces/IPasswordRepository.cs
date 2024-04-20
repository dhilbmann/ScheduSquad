public interface IPasswordRepository {
    
    public void UpdatePassword(Guid memberId, string password, string salt);
    
    public string? GetPassword(Guid memberId);
    public string? GetSalt(Guid memberId);

}





