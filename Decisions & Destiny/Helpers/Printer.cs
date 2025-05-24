namespace Decisions___Destiny.Helpers
{
	public static class Printer
	{
		public static void ShowHeader(string title)
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.DarkMagenta;

			string border = new string('═', title.Length + 4);
			Console.WriteLine($"╔{border}╗");
			Console.WriteLine($"║  {title}  ║");
			Console.WriteLine($"╚{border}╝");

			Console.ResetColor();
			Console.WriteLine();
		}

		public static void PrintWithDelay(string text, bool hasPrintedOnce)
		{
			if (hasPrintedOnce)
			{
				Console.WriteLine(text);
				return;
			}

			foreach (char c in text)
			{
				Console.Write(c);

				// Check for Enter during typing
				if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
				{
					Console.Write(text.Substring(text.IndexOf(c) + 1));
					break;
				}

				Thread.Sleep(20); // delay in ms
			}

			Console.WriteLine();
		}



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
