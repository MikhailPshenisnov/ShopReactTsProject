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

        if (userToUpdate is null) throw new Exception("Неизвестный пользователь!");

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
}