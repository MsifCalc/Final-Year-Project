using System;

namespace MeltCalc.Controls.Internals
{
	public interface IRunner
	{
		void Run(Action<object> callback);
	}
}