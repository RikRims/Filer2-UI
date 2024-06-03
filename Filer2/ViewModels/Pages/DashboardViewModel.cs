// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
using System.Collections.ObjectModel;
using System.IO;
using Filer2_UI.Models;

namespace Filer2_UI.ViewModels.Pages;

public partial class DashboardViewModel : ObservableObject
{

	#region форматы файлов что не стоит менять     https://open-file.ru/types/system/
	private static readonly List<string> _block = new(new[]
	{
		".url",

		".ini", ".lnk", ".0", ".000", ".1", ".2", ".208", ".2fs", ".3", ".386", ".3fs", ".4", ".5", ".6", ".7", ".73u", ".8", ".89u", ".8cu", ".8xu", ".adm", ".adml",".admx", ".adv", ".aml", ".ani", ".aos", ".asec", ".atahd", ".b83", ".b84", ".bashrc", ".bbfw", ".bcd", ".bin", ".bio", ".bk1", ".bk2", ".blf", ".bmk", ".bom", ".bud", ".c32",".cab", ".cap", ".cat", ".cdmp", ".cgz", ".chg", ".chk", ".chs", ".cht", ".ci", ".clb", ".cm0012", ".cm0013", ".cmo", ".cnt", ".cpi", ".cpl", ".cpq", ".cpr", ".crash", ".cur", ".dat", ".desklink", ".dev", ".dfu", ".diagcab", ".diagcfg", ".diagpkg", ".dic", ".diffbase", ".dimax", ".dit", ".dll", ".dlx", ".dmp", ".dock", ".drpm", ".drv", ".dss", ".dthumb", ".dub", ".dvd", ".dyc", ".ebd", ".edj", ".efi", ".efires", ".elf", ".emerald", ".escopy", ".etl", ".evt", ".evtx", ".ffa", ".ffl", ".ffo", ".ffx", ".fid", ".firm", ".fl1", ".flg", ".fota", ".fpbf", ".ftf", ".ftg", ".ftr", ".fts", ".fx", ".gmmp", ".grl", ".group", ".grp", ".h1s", ".hcd", ".hdmp", ".help", ".hhc", ".hhk", ".hiv", ".hlp", ".hpj", ".hsh", ".htt", ".hve", ".icl", ".icns", ".ico", ".idi", ".idx", ".ifw", ".im4p", ".ime", ".img3", ".inf_loc", ".ins", ".ion", ".ioplist", ".ipod", ".iptheme", ".its", ".ius", ".jetkey", ".job", ".jpn", ".kbd", ".kc", ".kdz", ".kext", ".key", ".kl", ".ko", ".kor", ".ks", ".kwi", ".lex", ".lfs", ".library-ms", ".lm", ".lnk", ".localized", ".lockfile", ".log1", ".log2", ".lpd", ".lpd", ".lst", ".manifest", ".mapimail", ".mbn", ".mbr", ".mdmp", ".me", ".mem", ".menu", ".mi4", ".mlc", ".mmv", ".mod", ".msc", ".msp", ".msstyle", ".msstyles", ".mtz", ".mui", ".mum", ".mun", ".mydocs", ".nb0", ".nbh", ".nfo", ".nls", ".nlt", ".nt", ".ntfs", ".odex", ".ozip", ".panic", ".pat", ".pck", ".pdr", ".pfx", ".pid", ".pit", ".pk2", ".plasmoid", ".pnf", ".pol", ".ppd", ".ppm", ".prefpane", ".prf", ".pro", ".profile", ".prop", ".prt", ".ps1", ".ps2", ".pwl", ".qky", ".qvm", ".rc1", ".rc2", ".rco", ".rcv", ".reg", ".reglnk", ".rfw", ".rmt", ".roku", ".rs", ".ruf", ".rvp", ".saver", ".sb", ".sbf", ".sbn", ".scap", ".scf", ".schemas", ".scr", ".sdb", ".sdt", ".sefw", ".self", ".service", ".sfcache", ".shd", ".shsh", ".shsh2", ".sin", ".so.0", ".spl", ".sprx", ".spx", ".sqm", ".str", ".swp", ".sys", ".ta", ".tbres", ".tco2", ".tdz", ".tha", ".theme", ".thumbnails", ".timer", ".trashes", ".trashinfo", ".trx_dll", ".uce", ".vdex", ".vga", ".vgd", ".vx_", ".vxd", ".wdf", ".wdgt", ".webpnp", ".wer", ".wgz", ".wlu", ".wph", ".wpx", ".xfb", ".xrm-ms"
	});
	#endregion

	#region Поля
	[ObservableProperty]
	private string _addresStartText = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

	[ObservableProperty]
	private string _addresEndText = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "\\Filer2\\", DateTime.Today.ToString().AsSpan(0, 10));

	[ObservableProperty]
	private ObservableCollection<Files> _listFiles = new ObservableCollection<Files>();

	[ObservableProperty]
	private ObservableCollection<Extentions> _listCeckboxs = new ObservableCollection<Extentions>();
    #endregion

    //FileSystem.DeleteFile(@_file, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);

	#region Команды
	// Установка начальной папки
	[RelayCommand]
	private void OnAddAddresStart()
	{
		AddresStartText = CompleteField();
	}

	// Установка конечной папки
	[RelayCommand]
	private void OnAddAddresEnd()
	{
		AddresEndText = CompleteField();
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
		var filteredFiles = Directory.GetFiles(AddresStartText, "*.*").Where(file => !_block.Any<string>((extension) => file.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase))).Select(file => new Files
		{
			Name = $"{file[file.LastIndexOf("\\")..]}",
			StartAddres = file,
			CheckExtension = $"{file[file.LastIndexOf(".")..]}",
			Img = Icon.ExtractAssociatedIcon(file)
		});

		var filteredFilesCheckBox = Directory.GetFiles(AddresStartText, "*.*").Where(file => !_block.Any<string>((extension) => file.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase))).Select(file => new Extentions
		{
			CheckExtension = $"{file[file.LastIndexOf(".")..]}",
		}).GroupBy(x => x.CheckExtension).Select(c => c.First());

		ListCeckboxs = new ObservableCollection<Extentions>(filteredFilesCheckBox);
		ListFiles = new ObservableCollection<Files>(filteredFiles);

	}

	// Обработчик перемещения файлов
	[RelayCommand]
	private void OnTransferFiles()
	{
		ImportCheckd();
		foreach(var item in ListFiles.Where(x => x.EnableExtension && x.StartAddres != null))
		{
			File.Move(item.StartAddres, AddresEndText + item.Name);
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
			File.Delete(item.StartAddres);
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
		Directory.CreateDirectory(string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "\\Filer2\\", "Logs"));
	}
    #endregion
}
