namespace Filer2_UI_.Models;
public class Files : ObservableObject
{
	private string? _startAddres;
    public string? StartAddres 
	{ 
		get => _startAddres; 
		set => SetProperty(ref _startAddres, value); 
	}

	private string? _checkExtension;
    public string? CheckExtension 
	{ 
		get => _checkExtension; 
		set => SetProperty(ref _checkExtension, value); 
	}

    private bool _extension = false;
	public bool EnableExtension
    {
        get => _extension;
        set => SetProperty(ref _extension, value);
    }

    private Icon? _icon;
    public Icon? Img
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }
}
