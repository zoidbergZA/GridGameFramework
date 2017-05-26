namespace Match3
{
	public enum GemColor
	{
		None = 0,
		Pink = 1,
		Blue = 2,
		Orange = 3,
		Yellow = 4,
		Lila = 5
	}

	public class Gem
	{
		public GemColor color;

		public Field Field { get; set; }

		public Gem(GemColor color)
		{
			this.color = color;
		}
	}
}