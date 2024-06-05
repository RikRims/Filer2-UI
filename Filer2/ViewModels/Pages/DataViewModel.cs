// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Filer2_UI.Services;
using Wpf.Ui.Controls;

namespace Filer2_UI.ViewModels.Pages;
public partial class DataViewModel : ObservableObject, INavigationAware
{
	private bool _isInitialized = false;

	[ObservableProperty]
	private string? _logText;

	public void OnNavigatedTo()
	{
		if(!_isInitialized)
			InitializeViewModel();
	}

	public void OnNavigatedFrom() { }

	private void InitializeViewModel()
	{
		LogText = Log.GetLog(DashboardViewModel.PathDirectoryLog);
		
		_isInitialized = true;
	}
}
