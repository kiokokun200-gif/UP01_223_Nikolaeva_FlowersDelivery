using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UP01_223_Nikolaeva
{
    public static class PasswordChecker
    {
        public static bool CheckPassword(string password, out string message)
        {
            if (password.Length < 6)
            {
                message = "Пароль слишком короткий, должно быть минимум 6 символов!";
                return false;
            }
            bool containDigit = false;
            bool english = true;
            for (int i = 0; i < password.Length; i++)
            {
                if (char.IsDigit(password[i]))
                {
                    containDigit = true;

                }
                else if (!((password[i] >= 'A' && password[i] <= 'Z') || (password[i] >= 'a' && password[i] <= 'z')))
                {
                    english = false;
                }

            }
            if (!containDigit)
            {
                message = "Пароль должен содержать цифру";
                return false;
            }

            else if (!english)
            {
                message = "Пароль должен состоять из английских букв";
                return false;
            }
            else
            {
                message = "";
                return true;
            }
        }
    }
}
