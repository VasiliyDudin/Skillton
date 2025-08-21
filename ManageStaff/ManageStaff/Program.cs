using ManageStaff.Classes;

namespace ManageStaff
{
    class Program
    {
        static void Main(string[] args)
        {
            MenuConfig menuConf = new MenuConfig();

            //Устанавливаем заголовок и пункты меню
            ConsoleMenu menu = new ConsoleMenu();
            menu.Title = menuConf.GetMenuTitle();
            menu.MenuItems = menuConf.GetMenuItems();
            
            Commands cmd = new Commands();

            bool isRun = true;
            while (isRun)
            {
                int selectedIndex = menu.Run(); //Запускаем меню

                switch (selectedIndex)
                {
                    case 0:
                        Employee emp = new Employee();
                        Console.WriteLine(cmd.AddEmployeeAsync().GetAwaiter().GetResult());
                        Console.ReadKey(true);
                        break;
                    case 1:
                        cmd.ShowStaffAsync();
                        Console.ReadKey(true);
                        break;
                    case 2:
                        Console.WriteLine(cmd.UpdateEmployeeAsync().GetAwaiter().GetResult());
                        Console.ReadKey(true);
                        break;
                    case 3:
                        Console.WriteLine(cmd.DeleteEmployeeAsync().GetAwaiter().GetResult());
                        Console.ReadKey(true);
                        break;
                    case 4:
                        cmd.Exit();
                        isRun = false;
                        break;
                }
            }

            Console.WriteLine("Приложение завершено. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}