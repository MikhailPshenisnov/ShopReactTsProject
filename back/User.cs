using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back;

public class User
{
    public User() {}
    public User(int id, string username, string password, string cart, DateOnly end_date)
    {
        this.id = id;
        this.username = username;
        this.password = password;
        this.cart = cart;
        this.end_date = end_date;

    }
    
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    
    [MaxLength(32)]
    public string username { get; set; }
    
    [MaxLength(32)]
    public string password { get; set; }
    
    [MaxLength(1024)]
    public string cart { get; set; }
    
    public DateOnly end_date { get; set; }
}