using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filer2_UI.Models;
public class Files
{
	public string? StartAddres { get; set; }
	
	public string? CheckExtension { get; set; }

	public bool EnableExtension { get; set; } = false;

	public Icon? Img {  get; set; }
}
