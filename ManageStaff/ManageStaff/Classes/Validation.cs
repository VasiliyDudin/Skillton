using ManageStaff.interfaces;
using System.Text.RegularExpressions;

namespace ManageStaff.Classes
{
    //Класс для простой валидации значений
    internal class Validation : IValidation
    {
        //Проверяем корректность имени/фамилии
        public bool CheckName(string name)
        {
            bool result = true;

            if (string.IsNullOrWhiteSpace(name))
                result = false;

            return result;
        }

        //Проверяем корректность Email
        public bool CheckEmail(string email)
        {
            Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }

        //Проверяем корректность даты
        public bool CheckDate(string str, out DateTime dataBirth)
        {
            bool result = true;

            if (!DateTime.TryParse(str, out dataBirth))
                result = false;

            return result;
        }

        //Проверяем корректность введенной зарплаты
        public bool CheckSalary(string str, out decimal salary)
        {
            bool result = true;

            if (!decimal.TryParse(str, out salary))
                result = false;

            return result;
        }
    }
}
