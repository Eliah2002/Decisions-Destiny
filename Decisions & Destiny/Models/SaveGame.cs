using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions___Destiny.Models
{
	public class SaveGame
	{
		public string CurrentSceneID { get; set; } = string.Empty;
		public List<string> Flags { get; set; } = new();
	}
}
