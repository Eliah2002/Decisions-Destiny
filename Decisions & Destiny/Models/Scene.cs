using Decisions___Destiny.Enums;

namespace Decisions___Destiny.Models
{
	/// <summary>
	/// Repräsentiert eine Szene im Spiel, inklusive Beschreibungstext und auswählbaren Entscheidungen.
	/// </summary>
	public class Scene
	{
		public string ID { get; set; } = string.Empty;

		public string Text { get; set; } = string.Empty;

		// Liste möglicher Entscheidungen, die von dieser Szene aus getroffen werden können.
		public List<Choice> Choices { get; set; } = new();

		/// <summary>
		/// Führt die Szene aus, zeigt Optionen an und verarbeitet Spielerentscheidung.
		/// </summary>
		/// <param name="game">Aktuelles Spielobjekt.</param>
		/// <returns>Gibt zurück, wie es mit dem Spiel weitergeht.</returns>
		public SceneResult StartScene(Game game)
		{
			List<MenuItem> choiceItems = new();

			// Für jede Entscheidung prüfen, ob alle nötigen Flags erfüllt sind
			foreach (var choice in Choices)
			{
				bool isAvailable = choice.RequiredFlags.All(flag => game.Flags.Contains(flag));

				if (isAvailable)
				{
					bool isFirstSelectable = choiceItems.Count == 0;

					choiceItems.Add(new MenuItem(isFirstSelectable, choice.Text, () =>
					{
						// Neue Flags setzen
						foreach (var flag in choice.SetFlags)
						{
							game.Flags.Add(flag);
						}

						// Szene wechseln
						game.CurrentSceneID = choice.NextSceneID;
					}));
				}
			}

			// Option für das Speichern / Zurück zum Hauptmenü hinzufügen
			choiceItems.Add(new MenuItem(isSafeMenuItem: true));

			// Falls keine Auswahl möglich ist – Spiel beenden
			if (choiceItems.Count == 1) // nur der Save-Button wurde hinzugefügt
			{
				choiceItems.Add(new MenuItem(false, "Kein gültiger Pfad... (Spielende)", () =>
				{
					Environment.Exit(0);
				}));
			}

			// Menü anzeigen
			var menu = new Menu(choiceItems);
			var selected = menu.RunScene(Text);

			// Aktion der Auswahl ausführen
			selected.Action?.Invoke();

			// Rückgabewert basierend auf Auswahl
			return selected.IsSafeMenuItem ? SceneResult.ExitToMainMenu : SceneResult.Continue;
		}
	}
}
