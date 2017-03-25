using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputWindowParameter : IInputWindowPatamerer
{
	public T GetOriginWindowParameter<T>() where T : InputWindowParameter
	{
		return this as T;
	}
}
