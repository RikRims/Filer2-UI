// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
using System;
using System.Collections.ObjectModel;
using System.IO;
using Filer2_UI.Models;

namespace Filer2_UI_.ViewModels.Pages;

public partial class DashboardViewModel : ObservableObject
{
	[ObservableProperty]
	public string _addresStartText = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
	
	[ObservableProperty]
	public string _addresEndText = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "\\Filer2\\", DateTime.Today.ToString().AsSpan(0, 10));

	[ObservableProperty]
	public ObservableCollection<Files> _listFiles = new ObservableCollection<Files>();

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

	[RelayCommand]
	private void OnScanFiles()
	{
		var files = Directory.EnumerateFiles(AddresStartText).Select(files => new Files 
		{ 
			StartAddres = files, 
			CheckExtension = files[files.LastIndexOf(".")..],
			Img = Icon.ExtractAssociatedIcon(files)
		});
		;

		ListFiles = new ObservableCollection<Files>(files);
	}

	[RelayCommand]
	private void OnDeletedFiles()
	{
        foreach (var item in ListFiles)
        {
			if(item.EnableExtension)
			{
				File.Delete(item.StartAddres);
			}
        }
    }

	private static string CompleteField()
	{
		try
		{
			using var dialog = new FolderBrowserDialog();
			DialogResult result = dialog.ShowDialog();
			return dialog.SelectedPath;
		}
		catch(Exception ex) { return ex.Message; }
	}

	public void WorkPreraration()
	{
		Directory.CreateDirectory(AddresEndText);
		Directory.CreateDirectory(string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "\\Filer2\\", "Logs"));
	}
}
