// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
namespace Filer2_UI_.ViewModels.Pages;

public partial class DashboardViewModel : ObservableObject
{
	[ObservableProperty]
	private int _counter = 0;

	[ObservableProperty]
	public string _addresStartText = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
	
	[ObservableProperty]
	public string _addresEndText = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

	[RelayCommand]
	private void OnCounterIncrement()
	{
		Counter++;
	}

	[RelayCommand]
	private void OnAddAddresStart()
	{
		AddresStartText = CompleteField();
	}

	[RelayCommand]
	private void OnAddAddresEnd()
	{
		AddresEndText = CompleteField();
	}

	private static string CompleteField()
	{
		try
		{
			using var dialog = new System.Windows.Forms.FolderBrowserDialog();
			System.Windows.Forms.DialogResult result = dialog.ShowDialog();
			return dialog.SelectedPath;
		}
		catch(Exception ex) { return ex.Message; }
	}
}
