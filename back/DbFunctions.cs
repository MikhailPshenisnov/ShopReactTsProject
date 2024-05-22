namespace back;

public static class DbFunctions
{
    public static int AddUser(string username, string password, string cart, DateOnly endDate)
    {
        using var dbContext = new ApplicationDbContext();

        var addedUser = dbContext.users.Add(new User()
            { username = username, password = password, cart = cart, end_date = endDate });
        dbContext.SaveChanges();

        Console.WriteLine(addedUser.Entity.id + "я добавиль");

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

    public static List<int> DeleteExpiredUsers()
    {
        var deletedUsers = new List<int>();

        if (DateChecker.IsReadyForNextCheck())
        {
            using var dbContext = new ApplicationDbContext();

            var usersToDelete = from user in dbContext.users select user;

            foreach (var user in usersToDelete)
            {
                if (DateChecker.IsDateExpired(user.end_date))
                {
                    deletedUsers.Add(user.id);
                    dbContext.users.Remove(user);
                }
            }

            dbContext.SaveChanges();
        }

        return deletedUsers;
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

        var usr = (from user in dbContext.users
            where user.id == userId
            select user).Single();

        if (usr is null) throw new Exception("incorrect_user_id");

        usr.cart = userCart;
        dbContext.users.Update(usr);
        dbContext.SaveChanges();
    }

    public static string CombineCart(string cartOne, string cartTwo)
    {
        if (cartOne == "")
        {
            return cartTwo == "" ? "" : cartTwo;
        }

        if (cartTwo == "")
        {
            return cartOne == "" ? "" : cartOne;
        }

        var c1 = cartOne.Split(";").Select(productId => Convert.ToInt32(productId)).ToList();
        var c2 = cartTwo.Split(";").Select(productId => Convert.ToInt32(productId)).ToList();

        foreach (var productId in c2.Where(productId => !c1.Contains(productId)))
        {
            c1.Add(productId);
        }

        c1.Sort();
        return string.Join(';', c1);
    }

    public static bool IsUserExist(int id)
    {
        using var dbContext = new ApplicationDbContext();

        var tmpUsers = from user in dbContext.users
            where user.id == id
            select user;

        if (!tmpUsers.Any()) return false;

        var tmpUser = tmpUsers.First();

        if (tmpUser is null) return false;

        return true;
    }

    public static string UpdateUserDate(int id)
    {
        using var dbContext = new ApplicationDbContext();

        var usersToUpdate = from user in dbContext.users
            where user.id == id
            select user;

        if (!usersToUpdate.Any()) throw new Exception("incorrect_user_id");

        var userToUpdate = usersToUpdate.First();

        if (userToUpdate is null) throw new Exception("incorrect_user_id");

        if (userToUpdate.username == "")
        {
            userToUpdate.end_date = DateOnly.FromDateTime(DateTime.Now).AddDays(3);
        }
        else
        {
            userToUpdate.end_date = DateOnly.FromDateTime(DateTime.Now).AddYears(2);
        }

        dbContext.SaveChanges();
        return "ok";
    }
}