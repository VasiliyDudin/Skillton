namespace ManageStaff.interfaces
{
    //Интерфейс для валидации значений
    internal interface IValidation
    {
        bool CheckName(string name);
        bool CheckEmail(string email);
        bool CheckDate(string str, out string date);
        bool CheckSalary(string str, out decimal salary);
    }
}
