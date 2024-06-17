// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
using System.Collections.ObjectModel;
using System.IO;
using Filer2_UI.Models;
using Filer2_UI.Services;
using Microsoft.VisualBasic.FileIO;
using MessageBox = System.Windows.Forms.MessageBox;


namespace Filer2_UI.ViewModels.Pages;

public partial class DashboardViewModel : ObservableObject
{
	#region Поля
	[ObservableProperty]
	private string _addresStartText = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

	[ObservableProperty]
	private string _addresEndText = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "\\Filer2\\", DateTime.Today.ToString().AsSpan(0, 10));

	public static string PathDirectoryLog = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "\\Filer2\\", 
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
        string? tempAddresStartText = CompleteField();
        if(tempAddresStartText != "")
        {
            AddresStartText = tempAddresStartText;
            Log.LogAdd(PathDirectoryLog, DateTime.Now.ToString() + " => Поменяли -AddresStartText- на : " + AddresStartText);
        }
        else
			Log.LogAdd(PathDirectoryLog, DateTime.Now.ToString() + " => Не поменяли -AddresStartText-");
    }

	// Установка конечной папки
	[RelayCommand]
	private void OnAddAddresEnd()
	{
		string? tempAddresEndText = CompleteField();
		if(tempAddresEndText != "")
		{
			AddresEndText = tempAddresEndText;
			Log.LogAdd(PathDirectoryLog, DateTime.Now.ToString() + " => Поменяли -AddresEndText- на : " + AddresEndText);
		}
        else
            Log.LogAdd(PathDirectoryLog, DateTime.Now.ToString() + " => Не поменяли -AddresEndText-");
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
		Log.LogAdd(PathDirectoryLog, DateTime.Now.ToString() + " => Запустили сканер, найдено: " + ListFiles.Count + " файлов.");

        var filesCheckBox = ServiceFiles.GetFilesInPath(AddresStartText).Select(x => new Extentions(x))
			.GroupBy(x => x.CheckExtension).Select(c => c.First());
        ListCeckboxs = new ObservableCollection<Extentions>(filesCheckBox);
        Log.LogAdd(PathDirectoryLog, DateTime.Now.ToString() + " => Запустили сканер, найдено: " + ListCeckboxs.Count + " расширений.");
    }

    // Обработчик перемещения файлов
    [RelayCommand]
	private void OnTransferFiles()
	{
		ImportCheckd();
		foreach(var item in ListFiles.Where(x => x.EnableExtension && x.StartAddres != null))
		{
			File.Move(item.StartAddres, AddresEndText + item.Name);
            Log.LogAdd(PathDirectoryLog, DateTime.Now.ToString() + " => Перемещение файла: **" + item.StartAddres + "** по пути **" + AddresEndText  + "** Успешно.");
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
            {
                DialogResult dialogResult = MessageBox.Show(
                "Точно, удалить файлы полностью? Если нет поменяйте настройку.",
                "Удаление!", MessageBoxButtons.YesNo);
                if(dialogResult == DialogResult.Yes)
                {
                    File.Delete(item.StartAddres);
                    Log.LogAdd(PathDirectoryLog, DateTime.Now.ToString() + " => Удаление файла: **" + item.StartAddres + "** С настройкой удаления - " + SettingsViewModel.GetSetting() + "** Успешно.");
                }
				else
                    Log.LogAdd(PathDirectoryLog, DateTime.Now.ToString() + " => Удаление файла: **" + item.StartAddres + "** С настройкой удаления - " + SettingsViewModel.GetSetting() + "** Отменено.");
            }
            else
            {
                FileSystem.DeleteFile(item.StartAddres, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                Log.LogAdd(PathDirectoryLog, DateTime.Now.ToString() + " => Удаление файла: **" + item.StartAddres + "** С настройкой удаления - " + SettingsViewModel.GetSetting() + "** Успешно.");
            }
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
		Directory.CreateDirectory(PathDirectoryLog);
		Log.LogCreate(PathDirectoryLog);
	}
    #endregion
}
