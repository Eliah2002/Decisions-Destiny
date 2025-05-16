public class MenuItem
{
	internal bool Selected { get; set; }
	internal string Title { get; set; }
	internal Action Action { get; set; }

	public MenuItem(string title)
	{
		Title = title;
	}

	public MenuItem(bool selected, string title) : this(title)
	{
		Selected = selected;
	}

	public MenuItem(bool selected, string title, Action action) : this(selected, title)
	{
		Action = action;
	}
}