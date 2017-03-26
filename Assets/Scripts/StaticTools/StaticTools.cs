using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticTools
{
	public static class HandlerChecker
	{
		public static bool IsHandlerRegistred<T>(Action<T> prospectiveHandler, Action<T> handlerToCheck) where T : IConvertible
		{
			if (handlerToCheck != null)
			{
				foreach (Action<T> _delegate in handlerToCheck.GetInvocationList())
				{
					if (_delegate.Method.CallingConvention == prospectiveHandler.Method.CallingConvention)
						return true;
				}
			}
			return false;
		}

		public static bool IsHandlerRegistred(Action prospectiveHandler, Action handlerToCheck)
		{
			if (handlerToCheck != null)
			{
				foreach (Action _delegate in handlerToCheck.GetInvocationList())
				{
					if (_delegate.Method.CallingConvention == prospectiveHandler.Method.CallingConvention)
						return true;
				}
			}
			return false;
		}
	}

}
