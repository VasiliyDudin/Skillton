using ManageStaff.interfaces;
using System.Configuration;
using System.Data.SqlClient;


namespace ManageStaff.Classes
{
    //Класс для выполнения CRUD операций с БД
    //Содержит соединения с БД
    //Реализует интерфейс IDisposable
    internal class Comands : IComands, IDisposable
    {
        string _connectionStr; //Строка подключения к БД
        static SqlConnection _connection; //Соединение с БД

        public Comands()
        {
            _connectionStr = ConfigurationManager.ConnectionStrings["connectToEmployeeDB"].ConnectionString;
        }

        //Метод для добавления нового сотрудника
        public bool AddEmployee(Employee empl)
        {
            bool result = false;

            using (_connection = new SqlConnection(_connectionStr))
            {
                string query = "INSERT INTO Employees (FirstName, LastName, Email, DateOfBirth, Salary) " +
                               "VALUES (@FirstName, @LastName, @Email, @DateOfBirth, @Salary)";

                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@FirstName", empl.FirstName);
                command.Parameters.AddWithValue("@LastName", empl.LastName);
                command.Parameters.AddWithValue("@Email", empl.Email);
                command.Parameters.AddWithValue("@DateOfBirth", empl.DateOfBirth);
                command.Parameters.AddWithValue("@Salary", empl.Salary);

                try
                {
                    _connection.Open();
                    result = command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                finally
                {
                    _connection.Close();
                }
            }

            return result;
        }

        //Метод для отображения всей информации по сотрудникам из БД
        public bool ShowStaff()
        {
            Console.WriteLine("\nСписок сотрудников:\n");

            using (_connection = new SqlConnection(_connectionStr))
            {
                string query = "SELECT EmployeeID,FirstName,LastName,Email,DateOfBirth,Salary FROM Employees ORDER BY EmployeeID";
                SqlCommand command = new SqlCommand(query, _connection);

                try
                {
                    _connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["EmployeeID"].ToString().Trim()} | {reader["FirstName"].ToString().Trim()} {reader["LastName"].ToString().Trim()}   " +
                                          $"{reader["Email"].ToString().Trim()} {reader["DateOfBirth"].ToString().Trim()}   {reader["Salary"].ToString().Trim()}");
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                    return false;
                }
                finally
                {
                    _connection.Close();
                }
            }

            return true;
        }

        //Метод для обновления конкретной информации по сотруднику
        public bool UpdateEmployee(string empId)
        {
            bool result = false;

            int id;
            while (!int.TryParse(empId, out id))
            {
                Console.Write("Неверный формат ID. Введите снова: ");
                empId = Console.ReadLine();
            }

            using (_connection = new SqlConnection(_connectionStr))
            {
                string query = "SELECT EmployeeID,FirstName,LastName,Email,DateOfBirth,Salary FROM Employees WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@EmployeeID", id);

                string[] values = new string[5]; //Массив для хранения значений по сотруднику
                int indx = 0;

                try
                {
                    _connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows) //Если из БД не вернулось ни одной строки выходим с соответствующим сообщением
                    {
                        Console.WriteLine("Сотрудник с таким ID не найден.");
                        return result;
                    }

                    while (reader.Read())
                    {
                        values[indx] = reader["FirstName"].ToString().Trim();
                        values[++indx] = reader["LastName"].ToString().Trim();
                        values[++indx] = reader["Email"].ToString().Trim();
                        values[++indx] = reader["DateOfBirth"].ToString().Trim();
                        values[++indx] = reader["Salary"].ToString().Trim();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                finally
                {
                    _connection.Close();
                }

                indx = 0;

                List<string> menuItems = new List<string>() //формируем пункты меню
                {
                    "Имя: " + values[indx],
                    "Фамилия: " + values[++indx],
                    "Эл. почта: " + values[++indx],
                    "Дата рождения (гггг-мм-дд): " + values[++indx],
                    "Зарплата: " + values[++indx]
                };

                ConsoleMenu menu = new ConsoleMenu("Обновление информации (Нажмите ESC для выхода в основное меню)", menuItems);
                Validation validator = new Validation(); //Для валидации вводимых значений

                query = "UPDATE Employees SET "; //формируем запрос к БД
                string input = string.Empty;
                bool isRun = true, isUpdate = false;

                while (isRun)
                {
                    int selectedIndex = menu.Run();

                    switch (selectedIndex)
                    {
                        case 0:
                            Console.WriteLine(values[selectedIndex]);
                            input = Console.ReadLine();
                            values[selectedIndex] = validator.CheckName(input) ? input : values[selectedIndex];
                            if (values[selectedIndex] != input)
                            {
                                Console.WriteLine("Имя не может быть пустым!");
                                Console.ReadKey(true);
                                break;
                            }
                            if (!query.Contains("FirstName")) //Что-бы не дублировать
                                query += "FirstName = @FirstName, ";
                            isUpdate = true;
                            break;
                        case 1:
                            Console.WriteLine(values[selectedIndex]);
                            input = Console.ReadLine();
                            values[selectedIndex] = validator.CheckName(input) ? input : values[selectedIndex];
                            if (values[selectedIndex] != input)
                            {
                                Console.WriteLine("Фамилия не может быть пустой!");
                                Console.ReadKey(true);
                                break;
                            }
                            if (!query.Contains("LastName"))
                                query += "LastName = @LastName, ";
                            isUpdate = true;
                            break;
                        case 2:
                            Console.WriteLine(values[selectedIndex]);
                            input = Console.ReadLine();
                            values[selectedIndex] = validator.CheckEmail(input) ? input : values[selectedIndex];
                            if (values[selectedIndex] != input)
                            {
                                Console.WriteLine("Email не валидный!");
                                Console.ReadKey(true);
                                break;
                            }
                            if (!query.Contains("Email"))
                                query += "Email = @Email, ";
                            isUpdate = true;
                            break;
                        case 3:
                            Console.WriteLine(values[selectedIndex]);
                            input = Console.ReadLine();
                            if (!validator.CheckDate(input, out values[selectedIndex]))
                            {
                                Console.WriteLine("Дата не валидная!");
                                Console.ReadKey(true);
                                break;
                            }
                            if (!query.Contains("DateOfBirth"))
                                query += "DateOfBirth = @DateOfBirth, ";
                            isUpdate = true;
                            break;
                        case 4:
                            Console.WriteLine(values[selectedIndex]);
                            input = Console.ReadLine();
                            values[selectedIndex] = validator.CheckSalary(input) ? input : values[selectedIndex];
                            if (values[selectedIndex] != input)
                            {
                                Console.WriteLine("Зарплата не валидная!");
                                Console.ReadKey(true);
                                break;
                            }
                            if (!query.Contains("Salary"))
                                query += "Salary = @Salary, ";
                            isUpdate = true;
                            break;
                        case -1: //При нажатии кнопки ESC
                            isRun = false;
                            break;
                    }
                }

                if (!isUpdate) //Если не установлено ни одного нового значения, выходим из меню без обновления БД
                    return result;

                query = query.TrimEnd(',', ' ') + " WHERE EmployeeID = @EmployeeID";

                command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@EmployeeID", id);
                if (query.Contains("FirstName")) //Проверяем есть ли в запросе данный параметр для записи его значения
                    command.Parameters.AddWithValue("@FirstName", values[0]);
                if (query.Contains("LastName"))
                    command.Parameters.AddWithValue("@LastName", values[1]);
                if (query.Contains("Email"))
                    command.Parameters.AddWithValue("@Email", values[2]);
                if (query.Contains("DateOfBirth"))
                    command.Parameters.AddWithValue("@DateOfBirth", values[3]);
                if (query.Contains("Salary"))
                    command.Parameters.AddWithValue("@Salary", decimal.Parse(values[4]));

                try
                {
                    _connection.Open();
                    result = command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                finally
                {
                    _connection.Close();
                }
            }

            return result;
        }

        //Метод для удаления сотрудника по ID
        public bool DeleteEmployee(string empId)
        {
            bool result = false;

            int id;
            while (!int.TryParse(empId, out id))
            {
                Console.Write("Неверный формат ID. Введите снова: ");
                empId = Console.ReadLine();
            }

            using (_connection = new SqlConnection(_connectionStr))
            {
                string query = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";
                SqlCommand command = new SqlCommand(query, _connection);
                command.Parameters.AddWithValue("@EmployeeID", id);

                try
                {
                    _connection.Open();
                    result = command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                finally
                {
                    _connection.Close();
                }
            }

            return result;
        }

        //Метод для освобождения ресурсов, вызывается при выборе пункта меню - "Выйдите из приложения"
        public void Dispose()
        {
            try
            {
                if (_connection != null)
                {
                    if (_connection.State != System.Data.ConnectionState.Closed)
                    {
                        _connection.Close();
                    }
                    _connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при завершении работы: {ex.Message}");
            }
        }
    }
}
