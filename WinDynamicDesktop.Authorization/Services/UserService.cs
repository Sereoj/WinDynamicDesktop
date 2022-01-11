﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinDynamicDesktop.Authorization.Models;
using WinDynamicDesktop.Core.Models;
using WinDynamicDesktop.Core.Services;

namespace WinDynamicDesktop.Authorization.Services
{
    public class UserService
    {
        private static string token;
        public static Task<string> GetLoginAsync(string email, string password)
        {
            var items = RequestRouter<string, Login>.PostAsync("login", new Login() { email = email, password = password });
            return items;
        }

        public static Task<string> GetLoginWithTokenAsync()
        {
            var items = RequestRouter<string>.GetAsync("user", null, null);
            return items;
        }

        public static Task<string> GetRegisterAsync(string name, string email, string password, string password_confirmation)
        {
            var items = RequestRouter<string, Register>.PostAsync("register", new Register() { name = name, email = email, password = password, password_confirmation = password_confirmation });
            return items;
        }
        public static string GetToken()
        {
            return token;
        }
        public static string ValidateRegister(JObject objects)
        {
            return Validate(objects);
        }
        public static string ValidateLogin(JObject objects)
        {
            if (objects["auth.failed"] != null)
            {
                return objects["auth.failed"].ToString();
            }

            return Validate(objects);
        }

        public static string ValidateWithToken(JObject objects)
        {
            if (objects["message"] != null)
            {
                return objects["message"].ToString();
            }
            if(objects["id"] != null)
            {
                return "success";
            }
            return null;
        }
        private static string Validate(JObject objects)
        {
            if (objects["token"] != null)
            {
                return token = objects["token"].ToString();
            }

            string msg = null;

            if (objects is JObject)
            {
                foreach (var item in objects)
                {
                    msg += item.Value[0] + " ";
                }
            }
            return msg;
        }
    }
}
