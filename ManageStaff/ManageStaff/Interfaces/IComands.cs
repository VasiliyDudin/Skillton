using ManageStaff.Classes;

namespace ManageStaff.interfaces
{
    //Интерфейс для реализации команд к БД
    internal interface IComands
    {
        public bool AddEmployee(Employee empl);
        public bool ShowStaff();
        public bool UpdateEmployee(string id);
        public bool DeleteEmployee(string id);
    }
}
