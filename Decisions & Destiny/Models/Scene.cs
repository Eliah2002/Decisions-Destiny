namespace Decisions___Destiny.Models
{
	public class Scene
	{
		public string Id { get; set; } = String.Empty;
		public string Text { get; set; } = String.Empty;
		public List<Choice> Choices { get; set; } = new();

		public void StartScene()
		{
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine(Text);
			Console.WriteLine();

			foreach (Choice choice in Choices)
			{
				Console.WriteLine(choice.Text);
				Console.WriteLine();
			}
		}
	}
}
