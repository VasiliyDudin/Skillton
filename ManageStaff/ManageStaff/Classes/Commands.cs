using ManageStaff.DataBase;

namespace ManageStaff.Classes
{
    internal class Commands
    {
        bool _result;
        DbCommand _cmd;
        public Commands()
        {
            _cmd = new DbCommand();
            OpenConnectionAsync();
        }

        ///<summery>
        ///Метод для открытия соединения с БД
        ///</summery>
        async Task OpenConnectionAsync()
        {
            await _cmd.OpenConnectionDbAsync();
        }

        ///<summery>
        ///Метод для добавления сотрудника
        ///</summery>
        public async Task<string> AddEmployeeAsync()
        {
            Employee emp = new Employee();

            _result = await _cmd.AddEmployeeAsync(emp.InputEmployee());
            return _result ? "Сотрудник успешно добавлен!" : "Ошибка при добавлении сотрудника!";
        }

        ///<summery>
        ///Метод для удаления сотрудника
        ///</summery>
        public async Task<string> DeleteEmployeeAsync()
        {
            Console.Write("\nВведите ID сотрудника для удаления: ");

            _result = await _cmd.DeleteEmployeeAsync(Console.ReadLine());
            return _result ? "Сотрудник успешно удален!" : "Ошибка при удалении сотрудника!";
        }

        ///<summery>
        ///Метод для обновления информации по сотруднику
        ///</summery>
        public async Task ShowStaffAsync()
        {
            await _cmd.ShowStaffAsync();
        }

        ///<summery>
        ///Метод для обновления информации по сотруднику
        ///</summery>
        public async Task<string> UpdateEmployeeAsync()
        {
            bool? result;
            Console.Write("\nВведите ID сотрудника для обновления информации: ");

            result = await _cmd.UpdateEmployeeAsync(Console.ReadLine());

            return result == null ? "Данные сотрудника не обновлены!" : (result == true ? "Данные сотрудника успешно обновлены!" : "Ошибка при обновлении данных!");
        }

        ///<summery>
        ///Метод для обработки команды - "Выйдите из приложения"
        ///</summery>
        public void Exit()
        {
            _cmd.Dispose();
        }
    }
}
