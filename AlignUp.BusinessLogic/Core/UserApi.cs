using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using AlignUp.Domain.Model.User;
using AlignUp.Helper;
using System.Data.Entity.Infrastructure;

namespace AlignUp.BusinessLogic.Core
{
    public class UserApi
    {
        internal UserLoginResponseDTO UserLoginAction(UserLoginDTO data)
        {
            UserDbTable result;
            var validate = new EmailAddressAttribute();

            // Verificăm dacă credențialul este un email
            if (validate.IsValid(data.Username))
            {
                var pass = PasswordHelper.HashPassword(data.Password);
                using (var db = new UserContext())
                {
                    result = db.Users.FirstOrDefault(u => u.Email == data.Username && u.Password == pass);
                }

                if (result == null)
                {
                    return new UserLoginResponseDTO { Status = false, StatusMessage = "Username sau parolă incorectă" };
                }

                using (var context = new UserContext())
                {
                    result.LastIp = data.UserIp;
                    result.LastLogin = DateTime.Now;
                    context.Entry(result).State = EntityState.Modified;
                    context.SaveChanges();
                }

                var userInfo = new UserInfo
                {
                    Id = result.Id,
                    Username = result.Username,
                    Email = result.Email,
                    LastLogin = result.LastLogin ?? DateTime.Now,
                    LastIp = result.LastIp,
                    UserRole = result.UserRole ?? UserRole.Standard
                };

                return new UserLoginResponseDTO { Status = true, UserInfo = userInfo };
            }
            else
            {
                // Credențialul este un username
                var pass = PasswordHelper.HashPassword(data.Password);
                using (var db = new UserContext())
                {
                    result = db.Users.FirstOrDefault(u => u.Username == data.Username && u.Password == pass);
                }

                if (result == null)
                {
                    return new UserLoginResponseDTO { Status = false, StatusMessage = "Username sau parolă incorectă" };
                }

                using (var context = new UserContext())
                {
                    result.LastIp = data.UserIp;
                    result.LastLogin = DateTime.Now;
                    context.Entry(result).State = EntityState.Modified;
                    context.SaveChanges();
                }

                var userInfo = new UserInfo
                {
                    Id = result.I