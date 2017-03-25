using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputWindowPatamerer
{
	T GetOriginWindowParameter<T>() where T : InputWindowParameter;
}
