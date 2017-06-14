using System.Collections;
using System.Collections.Generic;
using GridGame;

[System.Serializable]
public class BoardAlert
{
    public struct CellDebug
    {
        public Vec2 cell;
        public string info;
    }

	public CellDebug[] cells;
    public string message;

    public BoardAlert(CellDebug[] cells, string message)
    {
        this.cells = cells;
        this.message = message;
    }
}
