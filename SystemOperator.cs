using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zitulmyth.Checking;
using Zitulmyth.Data;

namespace Zitulmyth
{
	public class SystemOperator
	{

		public static double BlockPerSecond()
		{
			double temp;
			temp = Math.Round(32 * (double)MainWindow.elapsedTime / 500,1,MidpointRounding.AwayFromZero);

			return temp;

		}
	}
}
