using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GridGame
{
	public class Board
	{
		public Vec2 Size { get; private set; }
		public Dictionary<int, object> layers = new Dictionary<int, object>();

		public Board(Vec2 size)
		{
			Size = size;
		}
		
		public int AddLayer<T>(BoardLayer<T> layer)
		{
			var id = layers.Count;
			layers.Add(id, layer);

			return id;
		}

		public BoardLayer<T> GetLayer<T>(int id)
		{
			return layers[id] as BoardLayer<T>;
		}
	}
}
