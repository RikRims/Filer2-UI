// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
using System.Collections.ObjectModel;
using System.IO;
using Filer2_UI.Models;
using Filer2_UI.Services;
using Microsoft.VisualBasic.FileIO;

namespace Filer2_UI.ViewModels.Pages;

public partial class DashboardViewModel : ObservableObject
{
	#region Поля
	[ObservableProperty]
	private string _addresStartText = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

	[ObservableProperty]
	private string _addresEndText = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "\\Filer2\\", DateTime.Today.ToString().AsSpan(0, 10));

	private string _pathDirectoryLog = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "\\Filer2\\", 
		DateTime.Today.ToString().AsSpan(0, 10)) + "\\Logs";

    [ObservableProperty]
	private ObservableCollection<Files> _listFiles = new ObservableCollection<Files>();

	[ObservableProperty]
	private ObservableCollection<Extentions> _listCeckboxs = new ObservableCollection<Extentions>();
    #endregion

    #region Команды
	// Установка начальной папки
	[RelayCommand]
	private void OnAddAddresStart()
	{
		AddresStartText = CompleteField(); //TODO обработать возврат пустой строки
		Log.LogAdd(_pathDirectoryLog, DateTime.Now.ToString() + " => Поменяли -AddresStartText- на : " + AddresStartText);
	}

	// Установка конечной папки
	[RelayCommand]
	private void OnAddAddresEnd()
	{
		AddresEndText = CompleteField(); //TODO обработать возврат пустой строки
        Log.LogAdd(_pathDirectoryLog, DateTime.Now.ToString() + " => Поменяли -AddresEndText- на : " + AddresEndText);
    }

	// Обработчик выбора всех файлов
	[RelayCommand]
	private void OnToSelectAll()
	{
		foreach(var item in ListCeckboxs.Where(x => !x.EnableExtension))
		{
			item.EnableExtension = true;
		}
	}

	// Обработчик отмены выделения файлов
	[RelayCommand]
	private void OnDeselectAll()
	{
		foreach(var item in ListCeckboxs.Where(x => x.EnableExtension))
		{
			item.EnableExtension = false;
		}
	}

	// Обработчик сканировыания файлов в стартовой папке исключая файлы из списка _bloxk
	[RelayCommand]
	private void OnScanFiles()
	{
		var files = ServiceFiles.GetFilesInPath(AddresStartText).Select(x => new Files(x));
		ListFiles = new ObservableCollection<Files>(files);
        Log.LogAdd(_pathDirectoryLog, DateTime.Now.ToString() + " => Запустили сканер, найдено: " + ListFiles.Count + " файлов.");

        var filesCheckBox = ServiceFiles.GetFilesInPath(AddresStartText).Select(x => new Extentions(x))
			.GroupBy(x => x.CheckExtension).Select(c => c.First());
        ListCeckboxs = new ObservableCollection<Extentions>(filesCheckBox);
        Log.LogAdd(_pathDirectoryLog, DateTime.Now.ToString() + " => Запустили сканер, найдено: " + ListCeckboxs.Count + " расширений.");
    }

    // Обработчик перемещения файлов
    [RelayCommand]
	private void OnTransferFiles()
	{
		ImportCheckd();
		foreach(var item in ListFiles.Where(x => x.EnableExtension && x.StartAddres != null))
		{
			File.Move(item.StartAddres, AddresEndText + item.Name);
            Log.LogAdd(_pathDirectoryLog, DateTime.Now.ToString() + " => Перемещение файла: **" + item.StartAddres + "** по пути **" + AddresEndText  + "** Успешно.");
        }
        OnScanFiles();
	}

	// Обработчик удаления файлов
	[RelayCommand]
	private void OnDeletedFiles()
	{
		ImportCheckd();
		foreach(var item in ListFiles.Where(x => x.EnableExtension && x.StartAddres != null))
        {
            if(SettingsViewModel.GetSetting())
                File.Delete(item.StartAddres);
			else
                FileSystem.DeleteFile(item.StartAddres, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
            Log.LogAdd(_pathDirectoryLog, DateTime.Now.ToString() + " => Удаление файла: **" + item.StartAddres + "** С настройкой удаления - " + SettingsViewModel.GetSetting() + "** Успешно.");
        }
        OnScanFiles();
	}

	[RelayCommand]
	//Импорт галочки в соответствующие файлы
	private void ImportCheckd()
	{
		foreach(var item in ListCeckboxs.Where(x => x.EnableExtension))
		{
			foreach(var itemfile in ListFiles.Where(y => y.CheckExtension == item.CheckExtension))
			{ 
				itemfile.EnableExtension = item.EnableExtension;
			}
		}
	}
	
	//Вывод диалогового окна для выбора пути
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

	// Создание инфрастуктуры папок при запусе
	public void WorkPreraration()
	{
		Directory.CreateDirectory(AddresEndText);
		Directory.CreateDirectory(_pathDirectoryLog);
		Log.LogCreate(_pathDirectoryLog);
	}
    #endregion
}
