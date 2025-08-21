using ManageStaff.Classes;
using ManageStaff.interfaces;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace ManageStaff.DataBase
{
    ///<summery>
    ///Класс для выполнения CRUD операций с БД
    ///Содержит соединения с БД
    ///Реализует интерфейс IDisposable
    ///</summery>
    internal class DbCommand : IComands, IDisposable
    {
        string _connectionStr; //Строка подключения к БД
        readonly SqlConnection _connection; //Соединение с БД

        #region наименование столбцов БД
        const string _id = "EmployeeID";
        const string _firstName = "FirstName";
        const string _lastName = "LastName";
        const string _email = "Email";
        const string _date = "DateOfBirth";
        const string _salary = "Salary";
        #endregion

        public DbCommand()
        {
            _connectionStr = ConfigurationManager.ConnectionStrings["connectToEmployeeDB"].ConnectionString;
            if (!string.IsNullOrWhiteSpace(_connectionStr))
                _connection = new SqlConnection(_connectionStr);
        }

        ///<summery>
        ///Метод для открытия соединения с БД
        ///</summery>
        public async Task OpenConnectionDbAsync()
        {
            if (_connection != null && _connection.State != ConnectionState.Open)
            {
                await _connection.OpenAsync();
            }
        }

        ///<summery>
        ///Метод для добавления нового сотрудника
        ///</summery>
        public async Task<bool> AddEmployeeAsync(Employee empl)
        {
            bool result = false;

            if (empl == null)
                return result;

            string query = $"INSERT INTO Employees ({_firstName}, {_lastName}, {_email}, {_date}, {_salary}) " +
                            "VALUES (@FirstName, @LastName, @Email, @DateOfBirth, @Salary)";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@FirstName", empl.FirstName);
                command.Parameters.AddWithValue("@LastName", empl.LastName);
                command.Parameters.AddWithValue("@Email", empl.Email);
                command.Parameters.AddWithValue("@DateOfBirth", empl.DateOfBirth);
                command.Parameters.AddWithValue("@Salary", empl.Salary);

                try
                {
                    result = await command.ExecuteNonQueryAsync() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            return result;
        }

        ///<summery>
        ///Метод для отображения всей информации по сотрудникам из БД
        ///</summery>
        public async Task<bool> ShowStaffAsync()
        {
            bool result = false;
            string query = $"SELECT {_id},{_firstName},{_lastName},{_email},{_date},{_salary} FROM Employees ORDER BY {_id}";

            Console.WriteLine("\nСписок сотрудников:\n");

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                try
                {
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"{reader[_id].ToString().Trim()} | {reader[_firstName].ToString().Trim()} {reader[_lastName].ToString().Trim()}   " +
                                          $"{reader[_email].ToString().Trim()} {reader[_date].ToString().Trim()}   {reader[_salary].ToString().Trim()}");
                    }

                    await reader.CloseAsync();
                    result = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            return result;
        }

        ///<summery>
        ///Метод для обновления конкретной информации по сотруднику
        ///</summery>
        public async Task<bool?> UpdateEmployeeAsync(string empId)
        {
            bool? result = false;
            int id;

            while (!int.TryParse(empId, out id))
            {
                Console.Write("Неверный формат ID. Введите снова: ");
                empId = Console.ReadLine();
            }

            string query = $"SELECT {_id},{_firstName},{_lastName},{_email},{_date},{_salary} FROM Employees WHERE {_id} = @EmployeeID";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@EmployeeID", id);

                string[] values = new string[5]; //Массив для хранения значений по сотруднику
                int indx = 0;

                try
                {
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (!reader.HasRows) //Если из БД не вернулось ни одной строки выходим с соответствующим сообщением
                    {
                        Console.WriteLine("Сотрудник с таким ID не найден.");
                        await reader.CloseAsync();
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        values[indx] = reader[_firstName].ToString().Trim();
                        values[++indx] = reader[_lastName].ToString().Trim();
                        values[++indx] = reader[_email].ToString().Trim();
                        values[++indx] = reader[_date].ToString().Trim();
                        values[++indx] = reader[_salary].ToString().Trim();
                    }

                    await reader.CloseAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                indx = 0;

                ConsoleMenu menu = new ConsoleMenu();//"Обновление информации (Нажмите ESC для выхода в основное меню)", menuItems);
                menu.Title = "Обновление информации (Нажмите ESC для выхода в основное меню)";
                menu.MenuItems = CreateMenuItems(values.ToList());
                Validation validator = new Validation(); //Для валидации вводимых значений

                query = "UPDATE Employees SET "; //Формируем запрос к БД
                string input = string.Empty; //Новое значение введенное пользователем из консоли
                bool isRun = true, isUpdate = false;

                while (isRun)
                {
                    int selectedIndex = menu.Run();

                    if (selectedIndex >= 0)
                    {
                        Console.WriteLine(values[selectedIndex]);
                        input = Console.ReadLine();
                    }

                    switch (selectedIndex)
                    {
                        case 0:
                            values[selectedIndex] = validator.CheckName(input) ? input : values[selectedIndex];
                            if (values[selectedIndex] != input)
                            {
                                Console.WriteLine("Имя не может быть пустым!");
                                Console.ReadKey(true);
                                break;
                            }
                            if (!query.Contains(_firstName)) //Что-бы не дублировать
                                query += $"{_firstName} = @FirstName, ";
                            isUpdate = true;
                            break;
                        case 1:
                            values[selectedIndex] = validator.CheckName(input) ? input : values[selectedIndex];
                            if (values[selectedIndex] != input)
                            {
                                Console.WriteLine("Фамилия не может быть пустой!");
                                Console.ReadKey(true);
                                break;
                            }
                            if (!query.Contains(_lastName))
                                query += $"{_lastName} = @LastName, ";
                            isUpdate = true;
                            break;
                        case 2:
                            values[selectedIndex] = validator.CheckEmail(input) ? input : values[selectedIndex];
                            if (values[selectedIndex] != input)
                            {
                                Console.WriteLine("Email не валидный!");
                                Console.ReadKey(true);
                                break;
                            }
                            if (!query.Contains(_email))
                                query += $"{_email} = @Email, ";
                            isUpdate = true;
                            break;
                        case 3:
                            values[selectedIndex] = validator.CheckDate(input, out _) ? input : values[selectedIndex];
                            if (values[selectedIndex] != input)
                            {
                                Console.WriteLine("Дата не валидная!");
                                Console.ReadKey(true);
                                break;
                            }
                            if (!query.Contains(_date))
                                query += $"{_date} = @DateOfBirth, ";
                            isUpdate = true;
                            break;
                        case 4:
                            values[selectedIndex] = validator.CheckSalary(input, out _) ? input : values[selectedIndex];
                            if (values[selectedIndex] != input)
                            {
                                Console.WriteLine("Зарплата не валидная!");
                                Console.ReadKey(true);
                                break;
                            }
                            if (!query.Contains(_salary))
                                query += $"{_salary} = @Salary, ";
                            isUpdate = true;
                            break;
                        case -1: //При нажатии кнопки ESC
                            isRun = false;
                            break;
                    }

                    menu.MenuItems = CreateMenuItems(values.ToList());
                }

                if (!isUpdate) //Если не установлено ни одного нового значения, выходим из меню без обновления БД
                    return null;

                Console.WriteLine("Для выхода без сохранения изменений, введите - N");
                if (Console.ReadKey(true).Key == ConsoleKey.N)
                    return null;

                query = query.TrimEnd(',', ' ') + $" WHERE {_id} = @EmployeeID";

                SqlCommand command2 = new SqlCommand(query, _connection);
                command2.Parameters.AddWithValue("@EmployeeID", id);
                if (query.Contains(_firstName)) //Проверяем есть ли в запросе данный параметр для записи его значения
                    command2.Parameters.AddWithValue("@FirstName", values[0]);
                if (query.Contains(_lastName))
                    command2.Parameters.AddWithValue("@LastName", values[1]);
                if (query.Contains(_email))
                    command2.Parameters.AddWithValue("@Email", values[2]);
                if (query.Contains(_date))
                    command2.Parameters.AddWithValue("@DateOfBirth", values[3]);
                if (query.Contains(_salary))
                    command2.Parameters.AddWithValue("@Salary", decimal.Parse(values[4]));

                try
                {
                    result = await command2.ExecuteNonQueryAsync() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            return result;
        }

        ///<summery>
        ///Метод для удаления сотрудника по ID
        ///</summery>
        public async Task<bool> DeleteEmployeeAsync(string empId)
        {
            bool result = false;

            int id;
            while (!int.TryParse(empId, out id))
            {
                Console.Write("Неверный формат ID. Введите снова: ");
                empId = Console.ReadLine();
            }

            string query = $"DELETE FROM Employees WHERE {_id} = @EmployeeID";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@EmployeeID", id);

                try
                {
                    result = await command.ExecuteNonQueryAsync() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            return result;
        }

        ///<summery>
        ///Метод для освобождения ресурсов, вызывается при выборе пункта меню - "Выйдите из приложения"
        ///</summery>
        public void Dispose()
        {
            try
            {
                if (_connection != null)
                {
                    if (_connection.State != ConnectionState.Closed)
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

        /// <summary>
        /// Формируем пункты меню
        /// </summary>
        List<string> CreateMenuItems(List<string> items, int indx = 0)
        {
            return new List<string>()
            {
                "Имя: " + items[indx],
                "Фамилия: " + items[++indx],
                "Эл. почта: " + items[++indx],
                "Дата рождения (гггг-мм-дд): " + items[++indx],
                "Зарплата: " + items[++indx]
            };
        }
    }
}
