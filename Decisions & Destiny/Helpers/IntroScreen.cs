namespace Decisions___Destiny.Helpers
{
	public static class IntroScreen
	{
		public static void PlayIntro(string gameName)
		{
			Console.Clear();
			Console.CursorVisible = false;

			void TypeWrite(string text, int delay = 40)
			{
				foreach (var c in text)
				{
					Console.Write(c);
					Thread.Sleep(delay);
				}
				Console.WriteLine();
			}

			void Pause(int ms = 1000) => Thread.Sleep(ms);

			void WriteCentered(string text, int delay = 0)
			{
				int left = (Console.WindowWidth - text.Length) / 2;
				Console.SetCursorPosition(Math.Max(left, 0), Console.CursorTop);
				if (delay > 0)
					TypeWrite(text, delay);
				else
					Console.WriteLine(text);
			}

			void ClearWithDarkEffect(int steps = 10, int delay = 100)
			{
				for (int i = 0; i < steps; i++)
				{
					Console.Clear();
					Pause(delay);
				}
			}

			void Flicker(string text, int times = 3)
			{
				for (int i = 0; i < times; i++)
				{
					Console.Clear();
					Pause(150);
					WriteCentered(text);
					Pause(100);
				}
			}

			// --- Intro Text ---
			string[] introLines = new string[]
			{
				"Ein Spiel zwischen Licht und Schatten...",
				"Zwischen Wahrheit und Lüge...",
				"Zwischen Hoffnung...",
				"...und dem Unausweichlichen.",
				"",
				"Deine Entscheidungen formen das Schicksal.",
				"Aber jedes Schicksal hat seinen Preis..."
			};

			foreach (var line in introLines)
			{
				Console.WriteLine();
				WriteCentered(line, 40);
				Pause(800);
			}

			Pause(1200);

			// --- Flacker-Effekt / Übergang ---
			Flicker("  . . .  ");
			Pause(1000);
			ClearWithDarkEffect();

			// --- Vorbereitender Text zum Titel ---
			string[] leadUp = new string[]
			{
				"Was du jetzt erlebst,",
				"wird dich verfolgen.",
				"Jede Entscheidung zählt.",
				"",
				"Mach dich bereit..."
			};

			foreach (var line in leadUp)
			{
				Console.WriteLine();
				WriteCentered(line, 35);
				Pause(600);
			}

			Pause(1500);
			Console.Clear();

			// --- Epischer Titel Reveal ---
			var ascii = GenerateAsciiArt(gameName);
			foreach (var line in ascii)
			{
				WriteCentered(line);
				Pause(150);
			}

			Pause(2500);
			Console.Clear();
			Console.CursorVisible = true;
		}

		private static string[] GenerateAsciiArt(string title)
		{
			// Simple framed title, can be replaced by Figlet font or real ASCII later
			string border = new string('#', title.Length + 10);
			string padding = new string(' ', 4);
			return new string[]
			{
				"",
				border,
				$"##{padding}{title.ToUpper()}{padding}##",
				border,
				""
			};
		}
	}
}
