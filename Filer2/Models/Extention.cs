namespace Filer2_UI.Models;

public partial class Extentions : ObservableObject
{
    // Расширение файла
    private string? _checkExtension;
    public string? CheckExtension
    {
        get => _checkExtension;
        set => SetProperty(ref _checkExtension, value);
    }

    // Признак выбора файла для работы
    private bool _extension = false;
    public bool EnableExtension
    {
        get => _extension;
        set => SetProperty(ref _extension, value);
    }
}

