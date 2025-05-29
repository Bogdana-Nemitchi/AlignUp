namespace eUseControl.BusinessLogic.Core
{
    /*public class AdminApi
    {
        public List<UserDbTable> GetAllUsers()
        {
            using (var db = new UserContext())
            {
                return db.Users.ToList();
            }
        }

        public UserDbTable GetUserById(int id)
        {
            using (var db = new UserContext())
            {
                return db.Users.Find(id);
            }
        }

        public void AddUser(UserDbTable user)
        {
            using (var db = new UserContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public void UpdateUser(UserDbTable user)
        {
            using (var db = new UserContext())
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            using (var db = new UserContext())
            {
                var user = db.Users.Find(id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
            }
        }

        public List<UserDbTable> GetUsersByRole(UserRole role)
        {
            using (var db = new UserContext())
            {
                return db.Users.Where(u => u.UserRole == role).ToList();
            }
        }

        public bool ChangeUserRole(int userId, UserRole newRole)
        {
            using (var db = new UserContext())
            {
                var user = db.Users.Find(userId);
                if (user != null)
                {
                    user.UserRole = newRole;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public int GetTotalUserCount()
        {
            using (var db = new UserContext())
            {
                return db.Users.Count();
            }
        }

        public List<UserDbTable> GetRecentUsers(int count)
        {
            using (var db = new UserContext())
            {
                return db.Users
                    .OrderByDescending(u => u.LastLogin)
                    .Take(count)
                    .ToList();
            }
        }
    }+*/
}