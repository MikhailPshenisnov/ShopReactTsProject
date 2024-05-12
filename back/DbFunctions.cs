namespace back;

public static class DbFunctions
{
    public static int AddUser(string username, string password, string cart, DateOnly endDate)
    {
        using var dbContext = new ApplicationDbContext();

        var addedUser = dbContext.users.Add(new User()
            { username = username, password = password, cart = cart, end_date = endDate });
        dbContext.SaveChanges();

        return addedUser.Entity.id;
    }

    public static void MakeConstUser(int id, string username, string password)
    {
        using var dbContext = new ApplicationDbContext();

        var userToUpdate = (from user in dbContext.users
            where user.id == id
            select user).First();

        if (userToUpdate is null) throw new Exception("incorrect_user_id");

        userToUpdate.username = username;
        userToUpdate.password = password;
        userToUpdate.end_date = DateOnly.FromDateTime(DateTime.Now).AddYears(2);

        dbContext.SaveChanges();
    }

    public static void DeleteExpiredUsers()
    {
        if (DateChecker.IsReadyForNextCheck())
        {
            var curDate = DateOnly.FromDateTime(DateTime.Now);

            using var dbContext = new ApplicationDbContext();
            
            var usersToDelete = from user in dbContext.users select user;

            foreach (var user in usersToDelete)
            {
                if (DateChecker.IsDateExpired(user.end_date)) dbContext.users.Remove(user);
            }

            dbContext.SaveChanges();
        }
    }

    public static bool IsUsernameFree(string username)
    {
        if (username == "") return true;
        
        using var dbContext = new ApplicationDbContext();
        var users = from user in dbContext.users 
            where user.username == username 
            select user;

        if (users.Any()) return false;

        return true;
    }

    public static User? TryLoginUser(string username, string password)
    {
        using var dbContext = new ApplicationDbContext();
        var userList = from user in dbContext.users 
            where user.username == username && user.password == password 
            select user;
        return userList.Any() ? userList.First() : null;
    }

    public static void UpdateUserCart(int userId, string userCart)
    {
        using var dbContext = new ApplicationDbContext();
        var users = from user in dbContext.users
            where user.id == userId
            select user;
        if (!users.Any()) throw new Exception("incorrect_user_id");
        users.First().cart = userCart;
        dbContext.SaveChanges();
    }
}