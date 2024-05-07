namespace back;

public static class DbFunctions
{
    public static void AddUser(string username, string password, string cart, string end_date)
    {
        using (var dbContext = new ApplicationDbContext())
        {
            dbContext.users.Add(new User()
                { username = username, password = password, cart = cart, end_date = end_date });
            dbContext.SaveChanges();
            
            var data = from user in dbContext.users
                select user;
            foreach (var user in data)
            {
                Console.WriteLine(user.username + " " + user.password);
            }
        }
    }
    // public static void MakeConstUser(){}
    // public static void DeleteOldTemporaryUsers(){}
    
}