using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Match3
{
	public static class RandomHelper
	{
		public static T RandomEnum<T>() where T : struct
		{  
			Type type = typeof(T);
			Array values = Enum.GetValues(type);
			
			object value = values.GetValue(UnityEngine.Random.Range(0, values.Length));
			return (T)Convert.ChangeType(value, type);
		}
	}
}
