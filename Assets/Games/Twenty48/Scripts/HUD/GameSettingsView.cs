using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Twenty48
{
	public class GameSettingsView : MonoBehaviour 
	{
		public Twenty48Game game;
		public Slider animSpeedSlider;

		public void OnAnimSpeedChange(float val)
		{
			game.animSpeed = animSpeedSlider.value;
		}
	}
}
