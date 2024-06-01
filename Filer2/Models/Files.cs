namespace Filer2_UI_.Models;
public class Files : ObservableObject
{
    // Имя файла со слешами в начале
    private string? _name;
    public string? Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    // Адрес расположения файла на начало работы
	private string? _startAddres;
    public string? StartAddres 
	{ 
		get => _startAddres; 
		set => SetProperty(ref _startAddres, value); 
	}

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

    // Иконка файла (по моему сейчас не работает)
    private Icon? _icon;
    public Icon? Img
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }
}
