using ManageStaff.Classes;

namespace ManageStaff.interfaces
{
    ///<summery>
    ///Интерфейс для реализации команд к БД
    ///</summery>
    internal interface IComands
    {
        public Task OpenConnectionDbAsync();
        public Task<bool> AddEmployeeAsync(Employee empl);
        public Task<bool> ShowStaffAsync();
        public Task<bool?> UpdateEmployeeAsync(string id);
        public Task<bool> DeleteEmployeeAsync(string id);
    }
}
