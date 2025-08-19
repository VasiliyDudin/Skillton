using ManageStaff.Classes;

namespace ManageStaff.interfaces
{
    //Интерфейс для реализации команд к БД
    internal interface IComands
    {
        public Task OpenConnectionAsync();
        public Task<bool> AddEmployeeAsync(Employee empl);
        public Task<bool> ShowStaffAsync();
        public Task<bool> UpdateEmployeeAsync(string id);
        public Task<bool> DeleteEmployeeAsync(string id);
    }
}
