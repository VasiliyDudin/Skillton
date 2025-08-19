using ManageStaff.Classes;

namespace ManageStaff
{
    class Program
    {
        static async Task Main(string[] args)
        {
            MenuConfig menuConf = new MenuConfig();

            //Устанавливаем заголовок и пункты меню
            ConsoleMenu menu = new ConsoleMenu(menuConf.GetMenuTitle(), menuConf.GetMenuItems());
            Comands cmd = new Comands();
            await cmd.OpenConnectionAsync(); //Открываем соединение с БД

            bool isRun = true;
            while (isRun)
            {
                int selectedIndex = menu.Run(); //Запускаем меню

                switch (selectedIndex)
                {
                    case 0:
                        Employee emp = new Employee();
                        Console.WriteLine(await cmd.AddEmployeeAsync(emp.InputEmployee()) ? "Сотрудник успешно добавлен!" : "Ошибка при добавлении сотрудника!");
                        Console.ReadKey(true);
                        break;
                    case 1:
                        await cmd.ShowStaffAsync();
                        Console.ReadKey(true);
                        break;
                    case 2:
                        Console.Write("\nВведите ID сотрудника для обновления информации: ");
                        Console.WriteLine(await cmd.UpdateEmployeeAsync(Console.ReadLine()) ? "Данные сотрудника успешно обновлены!" : "Ошибка при обновлении данных!");
                        Console.ReadKey(true);
                        break;
                    case 3:
                        Console.Write("\nВведите ID сотрудника для удаления: ");
                        Console.WriteLine(await cmd.DeleteEmployeeAsync(Console.ReadLine()) ? "Сотрудник успешно удален!" : "Ошибка при удалении сотрудника!");
                        Console.ReadKey(true);
                        break;
                    case 4:
                        cmd.Dispose();
                        isRun = false;
                        break;
                }
            }

            Console.WriteLine("Приложение завершено. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}