using Filer2_UI.Models;

namespace Filer2_UI.Models;
public class Files : Extentions
{
    // Имя файла со слешами в начале
    private string? _name;
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    // Адрес расположения файла на начало работы
    private string _startAddres = "";
    public string StartAddres
    {
        get => _startAddres;
        set => SetProperty(ref _startAddres, value);
    }
    
    // Иконка файла (по моему сейчас не работает)
    private Icon? _icon;
    public Icon? Img
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }
}
