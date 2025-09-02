using ManageStaff.Classes;

namespace ManageStaff
{
    class Program
    {
        static void Main(string[] args)
        {
            Commands cmd = new Commands();
        
            try
            {
                var task = Task.Run(async () => await Start(cmd));
                Task.WaitAll(task);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        
            Console.WriteLine("Приложение завершено. Нажмите любую клавишу...");
            Console.ReadKey();
        }
        
        private static async Task Start(Commands cmd)
        {
            MenuConfig menuConf = new MenuConfig();
        
            //Устанавливаем заголовок и пункты меню
            ConsoleMenu menu = new ConsoleMenu();
            menu.Title = menuConf.GetMenuTitle();
            menu.MenuItems = menuConf.GetMenuItems();
        
            bool isRun = true;
        
            do
            {
                int selectedIndex = menu.Run(); //Запускаем меню
        
                switch (selectedIndex)
                {
                    case 0:
                        Employee emp = new Employee();
                        Console.WriteLine(await cmd.AddEmployeeAsync());
                        break;
                    case 1:
                        await cmd.ShowStaffAsync();
                        break;
                    case 2:
                        Console.WriteLine(await cmd.UpdateEmployeeAsync());
                        break;
                    case 3:
                        Console.WriteLine(await cmd.DeleteEmployeeAsync());
                        break;
                    case 4:
                        cmd.Exit();
                        isRun = false;
                        break;
                }
        
                if (isRun)
                    Console.ReadKey(true);
        
            } while (isRun);
        }
    }
}
