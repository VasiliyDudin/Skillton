using ManageStaff.interfaces;
using System.Text.RegularExpressions;

namespace ManageStaff.Classes
{
    ///<summary>
    ///Класс для простой валидации значений
    ///</summery>
    internal class Validation : IValidation
    {
        ///<summary>
        ///Проверяем корректность имени/фамилии
        ///</summery>
        public bool CheckName(string name)
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(name))
                result = false;

            return result;
        }

        ///<summery>
        ///Проверяем корректность Email
        ///</summery>
        public bool CheckEmail(string email)
        {
            Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }

        ///<summery>
        ///Проверяем корректность даты
        ///</summery>
        public bool CheckDate(string str, out DateTime dataBirth)
        {
            bool result = true;

            if (!DateTime.TryParse(str, out dataBirth))
                result = false;

            return result;
        }

        ///<summery>
        ///Проверяем корректность введенной зарплаты
        ///</summery>
        public bool CheckSalary(string str, out decimal salary)
        {
            bool result = true;

            if (!decimal.TryParse(str, out salary))
                result = false;

            return result;
        }
    }
}
