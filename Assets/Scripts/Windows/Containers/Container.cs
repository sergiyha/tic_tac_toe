using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour, IContainer
{
	public virtual void Close()
	{
		this.gameObject.SetActive(false);
	}

	public virtual void Open()
	{
		this.gameObject.SetActive(true);
	}
}
