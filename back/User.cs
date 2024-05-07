using System.ComponentModel.DataAnnotations;

namespace back;

public class User
{
    [MaxLength(32)]
    public string username { get; set; }
        
    [MaxLength(32)]
    public string password { get; set; }
    
    [MaxLength(512)]
    public string cart { get; set; }
    
    [MaxLength(16)]
    public string end_date { get; set; }
}