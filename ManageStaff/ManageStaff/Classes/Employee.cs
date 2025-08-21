namespace ManageStaff.Classes
{
    ///<summery>
    ///Класс описывающий сущность сотрудника и реализующий методы работы с ним
    ///</summery>
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

        ///<summery>
        ///Метод для ввода значений по сотруднику
        ///</summery>
        public Employee InputEmployee()
        {
            Validation validator = new Validation(); //Для валидации вводимых значений
            string fname, lname, email; //Набор переменных соответствующий вводимым значениям
            DateTime data;
            decimal salary;

            Console.WriteLine("\nДобавление нового сотрудника:");

            Console.Write("Имя: ");
            fname = Console.ReadLine();
            while (!validator.CheckName(fname))
            {
                Console.Write("Имя не может быть пустым. Введите снова: ");
                fname = Console.ReadLine();
            }
            FirstName = fname;

            Console.Write("Фамилия: ");
            lname = Console.ReadLine();
            while (!validator.CheckName(lname))
            {
                Console.Write("Фамилия не может быть пустой. Введите снова: ");
                lname = Console.ReadLine();
            }
            LastName = lname;

            Console.Write("Эл. почта: ");
            email = Console.ReadLine();
            while (!validator.CheckEmail(email))
            {
                Console.Write("Email не валидный. Введите снова: ");
                email = Console.ReadLine();
            }
            Email = email;

            Console.Write("Дата рождения (гггг-мм-дд): ");
            while (!validator.CheckDate(Console.ReadLine(), out data))
            {
                Console.Write("Неверный формат. Введите дату снова (гггг-мм-дд): ");
            }
            DateOfBirth = data;

            Console.Write("Зарплата: ");
            while (!validator.CheckSalary(Console.ReadLine(), out salary))
            {
                Console.Write("Неверный формат. Введите зарплату снова: ");
            }
            Salary = salary;

            return this;
        }
    }
}
