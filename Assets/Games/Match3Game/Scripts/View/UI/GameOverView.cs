using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Match3
{
	public class GameOverView : MonoBehaviour 
	{
		public GameObject container;
		public Text contentText;

		private bool success;

		void Awake()
		{
			Hide();
		}

		public void Hide()
		{
			container.SetActive(false);
		}

		public void Show(bool success)
		{
			this.success = success;

			if (success)
			{
				contentText.text = 	"success!";
			}
			else
			{
				contentText.text = "failed :(";
			}

			container.SetActive(true);
		}

		public void OnClickContinue()
		{
			
		}
	}
}