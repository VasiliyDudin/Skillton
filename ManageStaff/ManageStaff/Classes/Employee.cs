namespace ManageStaff.Classes
{
    //Класс описывающий сущность сотрудника и реализующий методы работы с ним
    internal class Employee
    {
        int _employeeID;
        string _firstName;
        string _lastName;
        string _email;
        DateTime _dateOfBirth;
        decimal _salary;

        public int EmployeeID
        {
            get => _employeeID;
            set => _employeeID = value;
        }

        public string FirstName
        {
            get => _firstName;
            set => _firstName = value;
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = value;
        }

        public string Email
        {
            get => _email;
            set => _email = value;
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set => _dateOfBirth = value;
        }

        public decimal Salary
        {
            get => _salary;
            set => _salary = value;
        }

        //Метод для ввода значений по сотруднику
        public Employee InputEmployee()
        {
            Employee emp = new Employee();
            Validation validator = new Validation(); //Для валидации вводимых значений
            string fname, lname, email;
            DateTime data; //Набор переменных соответствующий вводимым значениям
            decimal salary;

            Console.WriteLine("\nДобавление нового сотрудника:");

            Console.Write("Имя: ");
            fname = Console.ReadLine();
            while (!validator.CheckName(fname))
            {
                Console.Write("Имя не может быть пустым. Введите снова: ");
                fname = Console.ReadLine();
            }
            emp.FirstName = fname;

            Console.Write("Фамилия: ");
            lname = Console.ReadLine();
            while (!validator.CheckName(lname))
            {
                Console.Write("Фамилия не может быть пустой. Введите снова: ");
                lname = Console.ReadLine();
            }
            emp.LastName = lname;

            Console.Write("Эл. почта: ");
            email = Console.ReadLine();
            while (!validator.CheckEmail(email))
            {
                Console.Write("Email не валидный. Введите снова: ");
                email = Console.ReadLine();
            }
            emp.Email = email;

            Console.Write("Дата рождения (гггг-мм-дд): ");
            while (!validator.CheckDate(Console.ReadLine(), out data))
            {
                Console.Write("Неверный формат. Введите дату снова (гггг-мм-дд): ");
            }
            emp.DateOfBirth = data;

            Console.Write("Зарплата: ");
            while (!validator.CheckSalary(Console.ReadLine(), out salary))
            {
                Console.Write("Неверный формат. Введите зарплату снова: ");
            }
            emp.Salary = salary;

            return emp;
        }
    }
}
