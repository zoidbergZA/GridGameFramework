namespace GridGame
{
	public partial struct Vec2
	{
		public static Vec2 zero = new Vec2(0, 0);
		public static Vec2 invalid = new Vec2(-1, -1);
		public static Vec2 up = new Vec2(0, 1);
		public static Vec2 down = new Vec2(0, -1);
		public static Vec2 left = new Vec2(-1, 0);
		public static Vec2 right = new Vec2(1, 0);

		public int x;
		public int y;

		public Vec2(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public bool IsValidPosition(IGrid grid)
		{
			Vec2 gridSize = grid.GetDimensions();

			if (this.x < 0 || this.x >= gridSize.x)
				return false;
			if (this.y < 0 || this.y >= gridSize.y)
				return false;
			return true;
		}

		public static Vec2 operator +(Vec2 a, Vec2 b)
		{
			return new Vec2(a.x + b.x, a.y + b.y);
		}

		public static Vec2 operator -(Vec2 a, Vec2 b)
		{
			return new Vec2(a.x - b.x, a.y - b.y);
		}

		public static Vec2 operator *(Vec2 a, int scalar)
		{
			return new Vec2(a.x * scalar, a.y * scalar);
		}

		public static bool operator ==(Vec2 a, Vec2 b)
		{
			return a.x == b.x && a.y == b.y;
		}

		public static bool operator !=(Vec2 a, Vec2 b)
		{
			return a.x != b.x || a.y != b.y;
		}

		public override bool Equals (object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			
			Vec2 other = (Vec2)obj;
			return this == other;
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }
	}	
}