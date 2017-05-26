using GridGame;

namespace Match3
{
	[System.Serializable]
	public class Field
	{
		public Vec2 position;
		public Gem Gem { get; private set; }
		public bool HasGem { get { return (Gem != null); } }

		public Field() { }

		public Field(Vec2 position)
		{
			this.position = position;
		}

		public void SetGem(Gem gem)
		{
			Gem = gem;

			if (Gem != null)
				Gem.Field = this;
		}
	}
}