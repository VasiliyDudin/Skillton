using ManageStaff.interfaces;
using System.Configuration;
using System.Collections.Specialized;

namespace ManageStaff.Classes
{
    ///<summery>
    ///Класс для получения предустановленных значений меню из конфига
    ///</summery>
    internal class MenuConfig : IConfig
    {
        const Char _splitter = ';';
        NameValueCollection? _config;

        public MenuConfig()
        {
            _config = (NameValueCollection)ConfigurationManager.GetSection("menuSettings");
        }
        public string? GetMenuTitle()
        {
            return _config != null
            ? _config["MenuTitle"]
            : "Меню приложения"; //Если в конфиге отсутствует ключ - MenuTitle
        }

        public List<string> GetMenuItems()
        {
            List<string> items = new List<string>();
            if (_config != null) //Добавляем пункты меню из конфига
            {
                string menus = _config["MenuItems"];
                if (!string.IsNullOrWhiteSpace(menus))
                {
                    items.AddRange(menus.Split(_splitter));
                }
            }
            else //Если в конфиге отсутствует ключ - MenuItems, формируем пункты по умолчанию
            {
                items.AddRange(new[] {
                "Добавить нового сотрудника",
                "Посмотреть всех сотрудников",
                "Обновить информацию о сотруднике",
                "Удалить сотрудника",
                "Выйти из приложения"
                });
            }

            return items;
        }
    }
}
