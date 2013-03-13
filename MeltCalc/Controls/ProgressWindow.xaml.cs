using System;
using System.ComponentModel;
using System.Threading.Tasks;
using MeltCalc.Chemistry;
using MeltCalc.Controls.Internals;

namespace MeltCalc.Controls
{
	/// <summary>
	/// Interaction logic for ProgressWindow.xaml
	/// </summary>
	public partial class ProgressWindow : IUpdater
	{
		private IRunner _runner;

		public ProgressWindow()
		{
			InitializeComponent();
			Topmost = true;
			ShowInTaskbar = false;
		}

		public void Run(IRunner runner)
		{
			_runner = runner;
			_progress.Minimum = 0;
			// TODO: Maximum must be inited in more generic way.
			_progress.Maximum = Params.IterationNumber;
			_progress.Value = 0;

			var ao = AsyncOperationManager.CreateOperation(null);
			Task.Factory.StartNew(InternalRun, ao);

			ShowDialog();
			Close();
		}

		public void DoProgress(object value)
		{
			_progress.Value = Convert.ToDouble(value);
		}

		public void Close(object state)
		{
			Close();
		}

		private void InternalRun(object state)
		{
			var ao = (AsyncOperation)state;

			_runner.Run(x => ao.Post(DoProgress, x));
			ao.Post(Close, null);
			ao.OperationCompleted();
		}
	}
}