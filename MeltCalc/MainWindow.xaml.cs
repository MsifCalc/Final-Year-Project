using System.ComponentModel;
using System.Threading.Tasks;
using MeltCalc.Providers;

namespace MeltCalc
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeDatabases();
			InitializeComponent();
			var asyncOp = AsyncOperationManager.CreateOperation(null);
			asyncOp.Post(OnOperationCompleted, null);
		}

		private void OnOperationCompleted(object state)
		{
		}

		private static void InitializeDatabases()
		{
			Task.Factory.StartNew(TableCache.Refresh);
		}
	}
}
