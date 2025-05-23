namespace Decisions___Destiny.Models
{
	public class Scene
	{
		public string ID { get; set; } = String.Empty;
		public string Text { get; set; } = String.Empty;
		public List<Choice> Choices { get; set; } = new();

		public void StartScene(Game game)
		{
			List<MenuItem> choiceItems = new List<MenuItem>();
			for (int i = 0; i < Choices.Count; i++)
			{
				var choice = Choices[i];

				if (choice.RequiredFlags.All(flag => game.Flags.Contains(flag)))
				{
					bool isFirst = choiceItems.Count == 0;
					choiceItems.Add(new MenuItem(isFirst, choice.Text, () =>
					{
						foreach (var flag in choice.SetFlags)
						{
							game.Flags.Add(flag);
						}

						game.CurrentSceneID = choice.NextSceneID;
					}));
				}
			}

			if (choiceItems.Count == 0)
			{
				choiceItems.Add(new MenuItem(true, "Kein gültiger Pfad... (Spielende)", () => Environment.Exit(0)));
			}

			Menu choiceMenu = new Menu(choiceItems);
			MenuItem selected = choiceMenu.RunScene(Text);
			selected.Action();
		}
	}
}
