namespace Decisions___Destiny.Models
{
	public class Scene
	{
		public string Id { get; set; } = String.Empty;
        public string Text { get; set; } = String.Empty;
		public List<Choice> Choices { get; set; } = new();
	}
}
