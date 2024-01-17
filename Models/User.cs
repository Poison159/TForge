/// <summary>
/// User object
/// </summary>
public class User
{
    /// <summary>
    /// User primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets user email
    /// </summary>
    public string Email { get; set; } = "";
    /// <summary>
    /// Gets or sets passwordHash
    /// </summary>
    public string PasswordHash { get; set; } = "";
}
