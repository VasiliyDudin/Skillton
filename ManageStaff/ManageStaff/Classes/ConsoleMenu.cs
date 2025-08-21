namespace ManageStaff.Classes
{
    ///<summery>
    ///Класс для работы с меню
    ///</summery>
    internal class ConsoleMenu
    {
        List<string> _menuItems;
        int _index;
        string _title;

        public List<string> MenuItems
        {
            get => _menuItems;
            set => _menuItems = value;
        }
        public string Title
        {
            get => _title;
            set => _title = value;
        }

        public ConsoleMenu()
        {
            _index = 0;
        }

        public int Run()
        {
            ConsoleKey key;
            do
            {
                Console.Clear();
                DisplayMenu(); //Отображаем меню

                key = Console.ReadKey(true).Key;

                switch (key) //Установка значения индекса при нажатии соответствующих кнопок
                {
                    case ConsoleKey.UpArrow:
                        _index = _index == 0 ? _menuItems.Count - 1 : _index - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        _index = _index == _menuItems.Count - 1 ? 0 : _index + 1;
                        break;
                    case ConsoleKey.Escape:
                        return -1;
                }
            } while (key != ConsoleKey.Enter);

            return _index;
        }

        private void DisplayMenu()
        {
            Console.WriteLine(_title); //выводим заголовок
            Console.WriteLine("----------------------------");

            for (int i = 0; i < _menuItems.Count; i++)
            {
                if (i == _index) //установки для выбранного пункта меню
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("> ");
                }
                else
                {
                    Console.Write("  ");
                }

                Console.WriteLine(_menuItems[i]);
                Console.ResetColor();
            }

            Console.WriteLine("\nИспользуйте стрелки ↑ ↓ для навигации, Enter для выбора");
        }
    }
}
