namespace ManageStaff.interfaces
{
    ///<summery>
    ///Интерфейс для получения информации из конфига
    ///</summery>
    internal interface IConfig
    {
        string? GetMenuTitle();
        List<string> GetMenuItems();
    }
}
