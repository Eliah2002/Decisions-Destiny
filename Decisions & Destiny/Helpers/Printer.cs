using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decisions___Destiny.Helpers
{
	public static class Printer
	{
		public static void PrintError(string message)
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
			Console.ReadKey(true);
		}
	}
}
