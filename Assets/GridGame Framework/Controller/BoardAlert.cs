using System.Collections;
using System.Collections.Generic;
using GridGame;

[System.Serializable]
public class BoardAlert
{
	public Vec2 from;
    public Vec2 to;
    public string context;

    public BoardAlert(Vec2 from, Vec2 to, string context)
    {
        this.from = from;
        this.to = to;
        this.context = context;
    }
}
