using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Window
{
	public abstract class Window : MonoBehaviour
	{
		public virtual void OpenWindow(IInputWindowPatamerer windowParameter)
		{
			this.gameObject.SetActive(true);
		}

		public virtual void CloseWindow()
		{
			this.gameObject.SetActive(false);
		}

	}
}
