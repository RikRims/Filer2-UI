// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Filer2_UI_.Models;

namespace Filer2_UI_.ViewModels.Pages;

public partial class DashboardViewModel : ObservableObject
{

	#region форматы файлов что не стоит менять     https://open-file.ru/types/system/
	private static readonly List<string> _block = new(new[]
	{
		".url",

		".ini", ".lnk", ".0", ".000", ".1", ".2", ".208", ".2fs", ".3", ".386", ".3fs", ".4", ".5", ".6", ".7", ".73u", ".8", ".89u", ".8cu", ".8xu", ".adm", ".adml",".admx", ".adv", ".aml", ".ani", ".aos", ".asec", ".atahd", ".b83", ".b84", ".bashrc", ".bbfw", ".bcd", ".bin", ".bio", ".bk1", ".bk2", ".blf", ".bmk", ".bom", ".bud", ".c32",".cab", ".cap", ".cat", ".cdmp", ".cgz", ".chg", ".chk", ".chs", ".cht", ".ci", ".clb", ".cm0012", ".cm0013", ".cmo", ".cnt", ".cpi", ".cpl", ".cpq", ".cpr", ".crash", ".cur", ".dat", ".desklink", ".dev", ".dfu", ".diagcab", ".diagcfg", ".diagpkg", ".dic", ".diffbase", ".dimax", ".dit", ".dll", ".dlx", ".dmp", ".dock", ".drpm", ".drv", ".dss", ".dthumb", ".dub", ".dvd", ".dyc", ".ebd", ".edj", ".efi", ".efires", ".elf", ".emerald", ".escopy", ".etl", ".evt", ".evtx", ".ffa", ".ffl", ".ffo", ".ffx", ".fid", ".firm", ".fl1", ".flg", ".fota", ".fpbf", ".ftf", ".ftg", ".ftr", ".fts", ".fx", ".gmmp", ".grl", ".group", ".grp", ".h1s", ".hcd", ".hdmp", ".help", ".hhc", ".hhk", ".hiv", ".hlp", ".hpj", ".hsh", ".htt", ".hve", ".icl", ".icns", ".ico", ".idi", ".idx", ".ifw", ".im4p", ".ime", ".img3", ".inf_loc", ".ins", ".ion", ".ioplist", ".ipod", ".iptheme", ".its", ".ius", ".jetkey", ".job", ".jpn", ".kbd", ".kc", ".kdz", ".kext", ".key", ".kl", ".ko", ".kor", ".ks", ".kwi", ".lex", ".lfs", ".library-ms", ".lm", ".lnk", ".localized", ".lockfile", ".log1", ".log2", ".lpd", ".lpd", ".lst", ".manifest", ".mapimail", ".mbn", ".mbr", ".mdmp", ".me", ".mem", ".menu", ".mi4", ".mlc", ".mmv", ".mod", ".msc", ".msp", ".msstyle", ".msstyles", ".mtz", ".mui", ".mum", ".mun", ".mydocs", ".nb0", ".nbh", ".nfo", ".nls", ".nlt", ".nt", ".ntfs", ".odex", ".ozip", ".panic", ".pat", ".pck", ".pdr", ".pfx", ".pid", ".pit", ".pk2", ".plasmoid", ".pnf", ".pol", ".ppd", ".ppm", ".prefpane", ".prf", ".pro", ".profile", ".prop", ".prt", ".ps1", ".ps2", ".pwl", ".qky", ".qvm", ".rc1", ".rc2", ".rco", ".rcv", ".reg", ".reglnk", ".rfw", ".rmt", ".roku", ".rs", ".ruf", ".rvp", ".saver", ".sb", ".sbf", ".sbn", ".scap", ".scf", ".schemas", ".scr", ".sdb", ".sdt", ".sefw", ".self", ".service", ".sfcache", ".shd", ".shsh", ".shsh2", ".sin", ".so.0", ".spl", ".sprx", ".spx", ".sqm", ".str", ".swp", ".sys", ".ta", ".tbres", ".tco2", ".tdz", ".tha", ".theme", ".thumbnails", ".timer", ".trashes", ".trashinfo", ".trx_dll", ".uce", ".vdex", ".vga", ".vgd", ".vx_", ".vxd", ".wdf", ".wdgt", ".webpnp", ".wer", ".wgz", ".wlu", ".wph", ".wpx", ".xfb", ".xrm-ms"
	});
	#endregion


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
	private void OnToSelectAll()
	{
		foreach(var item in ListFiles)
		{
			if(!item.EnableExtension)
			{
				item.EnableExtension = true;
			}
		}
	}

	[RelayCommand]
    private void OnDeselectAll()
    {
        foreach(var item in ListFiles)
        {
            if(item.EnableExtension)
            {
                item.EnableExtension = false;
            }
        }
    }

    [RelayCommand]
	private void OnScanFiles()
	{
		var filteredFiles = Directory.GetFiles(AddresStartText, "*.*").Where(file => !_block.Any<string>((extension) => file.EndsWith(extension, StringComparison.CurrentCultureIgnoreCase))).Select(file => new Files
		{
			Name = $"{file[file.LastIndexOf("\\")..]}",
			StartAddres = file,
			CheckExtension = $"{ file[file.LastIndexOf(".")..] }",
			Img = Icon.ExtractAssociatedIcon(file)
		});

		ListFiles = new ObservableCollection<Files>(filteredFiles);
	}

	[RelayCommand]
	private void OnTransferFiles()
	{
		foreach(var item in ListFiles)
		{
			if(item.EnableExtension)
			{
				if(!Equals(AddresEndText))
				{
					File.Move(item.StartAddres, _addresEndText + item.Name);
				}
			}
		}
	}
	[RelayCommand]
	private void OnDeletedFiles()
	{
		foreach(var item in ListFiles)
		{
			if(item.EnableExtension)
			{
				if(item.StartAddres!=null)
				{
					File.Delete(item.StartAddres);
				}
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
