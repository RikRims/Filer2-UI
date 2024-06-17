// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Reflection.Metadata;
using Wpf.Ui.Controls;
using static System.Reflection.Assembly;
using static Wpf.Ui.Appearance.Theme;
using static Wpf.Ui.Appearance.ThemeType;

namespace Filer2_UI.ViewModels.Pages;
public partial class SettingsViewModel : ObservableObject, INavigationAware
{
    #region Поля
    private bool _isInitialized = false;

    [ObservableProperty]
    private string _appVersion = String.Empty;

    [ObservableProperty]
    private string _appName = String.Empty;

    [ObservableProperty]
    private static bool _deleted = false;

    [ObservableProperty]
    private Wpf.Ui.Appearance.ThemeType _currentTheme = Unknown;

    [ObservableProperty]
    private bool _currentThemeBool = false;

    [ObservableProperty]
    private DateTime _countDeys = DateTime.Now;
   
    #endregion

    public void OnNavigatedTo()
	{
		if(!_isInitialized)
			InitializeViewModel();
	}

	public void OnNavigatedFrom() { }

	private void InitializeViewModel()
	{
		CurrentTheme = GetAppTheme();
		AppVersion = $"{GetAssemblyName()} - {GetAssemblyVersion()}";

		_isInitialized = true;
	}

	public static bool GetSetting() => _deleted;
	
	private string GetAssemblyVersion() => GetExecutingAssembly().GetName().Version?.ToString() ?? String.Empty;

	public static string GetAssemblyName() => GetExecutingAssembly().GetName().Name?.ToString() ?? String.Empty;

    [RelayCommand]
    private void SetCountDeys(string parametr)
    {
        if(int.TryParse(parametr, out int i))
        { 

        }
        else parametr = String.Empty;
    }

    [RelayCommand]
    private void OnChangeTheme()
    {
        switch(CurrentThemeBool)
        {
            case true:
                if(CurrentTheme == Light)
                    break;

                Apply(Light);
                CurrentTheme = Light;

                break;

            default:
                if(CurrentTheme == Dark)
                    break;

                Apply(Dark);
                CurrentTheme = Dark;

                break;
        }
    }
}
