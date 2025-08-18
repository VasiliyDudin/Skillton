namespace ManageStaff.interfaces
{
    //Интерфейс для получения информации из конфига
    internal interface IConfig
    {
        string? GetMenuTitle();
        List<string> GetMenuItems();
    }
}
