public class MenuItem
{
	internal bool Selected { get; set; }
	internal string Title { get; set; }
	internal Action? Action { get; set; }

	public MenuItem(bool selected, string title, Action action)
	{
		Selected = selected;
		Title = title;
		Action = action;
	}
}