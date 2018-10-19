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
			temp = Math.Round(32 * (double)MainWindow.elapsedTime / 500, 2, MidpointRounding.AwayFromZero);

			return temp;

		}
		

		public static void BoundObject(int index, Vector pos , Vector total, Vector target , Vector bps)
		{
			bps.X = target.X / 32 * BlockPerSecond();
			bps.Y = target.Y * bps.X / target.X;
			
			double radian = Math.Atan(bps.Y/bps.X) * Math.PI / 180;

			if (total.Y < target.Y)
			{

				SpawnEnemy.lstEnemyData[index].position.X += bps.X;
				SpawnEnemy.lstEnemyData[index].position.Y -= bps.Y;

				SpawnEnemy.lstEnemyData[index].totalDistance.X += Math.Sqrt(Math.Pow(bps.X,2));
				SpawnEnemy.lstEnemyData[index].totalDistance.Y += Math.Sqrt(Math.Pow(bps.Y,2));

				Console.WriteLine(SpawnEnemy.lstEnemyData[index].totalDistance.Y);
			}
			else
			{
				SpawnEnemy.lstEnemyData[index].isKnockBack = false;
			}
		}
	}
}
