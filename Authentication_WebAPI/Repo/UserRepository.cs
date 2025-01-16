using Authentication_WebAPI.Context;
using Authentication_WebAPI.Models;
using TMS_WebAPI.IRepo;
using TMS_WebAPI.ViewModel;

namespace TMS_WebAPI.Repo
{
    public class UserRepository : IUserRepo
    {
        AppDBContext _dbContext;
        public UserRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User AddUser(User user)
        {
            user.IsActive = true;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }
        public bool DeleteUser(int UserId)
        {
            User obj = GetUserById(UserId);
            if (obj != null)
            {

                //_dbContext.Remove(obj);
                obj.IsActive = false;
                _dbContext.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateUser(int UserId, User user)
        {
            User obj = GetUserById(UserId);
            if (obj != null)
            {
                obj.UserName = user.UserName;
                obj.Password = user.Password;
                obj.RoleId = user.RoleId;
                obj.ManagerId = user.ManagerId;
                obj.Updated = user.Updated;
                obj.IsActive = user.IsActive;
                obj.UpdatedBy = user.UpdatedBy;
                obj.EmailId = user.EmailId;

                _dbContext.SaveChanges();
                return true;
            }
            else return false;

        }
        public User GetUserById(int id)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.UserId == id && x.IsActive == true);
            return user;
        }

        /// <summary>
        /// For Details view of User
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public UserRoleViewModel GetUserDetailsById(int UserId)
        {
            var role = (from x in _dbContext.Users
                        join y in _dbContext.Roles on x.RoleId equals y.RoleId
                        join manager in _dbContext.Users on x.ManagerId equals manager.UserId
                        into managerJoin
                        from m in managerJoin.DefaultIfEmpty()
                        where x.IsActive == true && x.UserId == UserId
                        select new UserRoleViewModel
                        {
                            RoleId = x.RoleId,
                            UserName = x.UserName,
                            Password = x.Password,
                            RoleName = y.RoleName,
                            UserId = x.UserId,
                            ManagerId = x.ManagerId,
                            ManagerName = m != null ? m.UserName : null,
                            EmailId = x.EmailId
                        }).FirstOrDefault();
            return role;
        }

        #region GetUsers
        /// <summary>
        /// For Index View of User
        /// </summary>
        /// <returns></returns>
        public List<UserRoleViewModel> GetUsers()
        {
            List<UserRoleViewModel> userRole = (from x in _dbContext.Users
                                                join y in _dbContext.Roles on x.RoleId equals y.RoleId
                                                join manager in _dbContext.Users on x.ManagerId equals manager.UserId
                                                into managerJoin
                                                from m in managerJoin.DefaultIfEmpty()
                                                where x.IsActive == true
                                                select new UserRoleViewModel
                                                {
                                                    RoleId = x.RoleId,
                                                    UserName = x.UserName,
                                                    Password = x.Password,
                                                    RoleName = y.RoleName,
                                                    UserId = x.UserId,
                                                    ManagerId = x.ManagerId,
                                                    ManagerName = m != null ? m.UserName : null,
                                                    EmailId = x.EmailId
                                                }).ToList();
            return userRole;
        }
        #endregion

        #region GetRoles
        /// <summary>
        /// For showing roles in the drop down
        /// </summary>
        /// <returns></returns>
        public List<Role> GetRoles()
        {
            List<Role> roles = (from x in _dbContext.Roles
                                where x.RoleId != 1
                                select new Role
                                {
                                    RoleId = x.RoleId,
                                    RoleName = x.RoleName
                                }).ToList();

            return roles;
        }
        #endregion

        /// <summary>
        /// For showing ManagerName
        /// </summary>
        /// <returns></returns>
        public List<UserRoleViewModel> GetManagerNames()
        {
            //Role Id : 2 => Managers
            List<UserRoleViewModel> managerInfo = (from x in _dbContext.Users
                                                   where x.IsActive == true && x.RoleId == 2
                                                   select new UserRoleViewModel
                                                   {
                                                       ManagerName = x.UserName,
                                                       ManagerId = x.UserId
                                                   }).ToList();
            return managerInfo;
        }

        /// <summary>
        /// To show managers name without the same user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<UserRoleViewModel> GetManagers(int id)
        {
            //Role id : 2=> Manager
            List<UserRoleViewModel> managerInfo = (from x in _dbContext.Users
                                                   where x.IsActive == true && x.RoleId == 2
                                                   && x.UserId != id
                                                   select new UserRoleViewModel
                                                   {
                                                       ManagerName = x.UserName,
                                                       ManagerId = x.UserId
                                                   }).ToList();
            return managerInfo;
        }
    }
}
