using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zitulmyth.Data;

namespace Zitulmyth.Checking
{
	public class BlockCheck
	{

		public static bool BlockCheckLeft(double posx, double posy, int speed)
		{
			int px = (int)posx;
			int py = (int)posy;

			int bkX = (px - speed) / 32;
			int bkY = py / 32;

			if (bkX >= 0)
			{
				if (StageData.indicateStage[bkY + 2, bkX] == 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		public static bool BlockCheckRight(double posx, double posy, int speed)
		{
			int px = (int)posx;
			int py = (int)posy;

			int bkX = (px + speed) / 32;
			int bkY = py / 32;

			if (bkX < 31)
			{

				if (StageData.indicateStage[bkY + 2, bkX + 1] == 0)
				{
					return true;
				}
				else
				{
					return false;
				}

			}
			else
			{
				return false;
			}
		}

		public static bool BlockCheckTop(double posx, double posy, int jumppower)
		{
			int px = (int)posx;
			int py = (int)posy;

			int bkX = px / 32;
			int bkY = (py - jumppower) / 32;

			if (bkY >= 0)
			{
				if (StageData.indicateStage[bkY, bkX] == 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		public static bool BlockCheckBottom(double posx, double posy, int weight)
		{
			int px = (int)posx;
			int py = (int)posy;

			int bkX = px / 32;
			int bkY = (py + weight + 32) / 32;

			if (bkY < 23)
			{

				if (StageData.indicateStage[bkY + 1, bkX] == 0 &&
					StageData.indicateStage[bkY + 1, bkX+1] == 0)
				{
					return true;
				}
				else
				{
					MainWindow.jumpCount = 0;
					return false;
				}

			}
			else
			{
				return false;
			}
		}
	}
}
