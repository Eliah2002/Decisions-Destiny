namespace Decisions___Destiny.Models
{
	public class Choice
	{
		public string Text { get; set; } = String.Empty;
		public string NextSceneId { get; set; } = String.Empty;
		public List<string> RequiredFlags { get; set; } = new();
		public List<string> SetFlags { get; set; } = new();
	}
}
