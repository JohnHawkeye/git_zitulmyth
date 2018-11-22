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

	public enum SystemTargetName
	{
		Player,
		Enemy,
		Item,
		Ui,
	}


	public class SystemOperator
	{

		public static int timeSeed = Environment.TickCount;
		public static Random randomNum;

		public static double BlockPerSecond()
		{
			double temp;
			temp = Math.Round(32 * (double)MainWindow.elapsedTime / 500, 2, MidpointRounding.AwayFromZero);

			return temp;

		}
		

		public static void BoundObject(ref Vector pos ,bool charaDir,ref Vector total, Vector target , ref Vector bps,
										ref double coefficient, ref bool boundDir,int weight,int speed,int jumppower,
										int width,int height,ref bool isKnockBack)
		{

			double addY = 0;
			double radian = 0;

			

			bps.X = target.X / 32 * BlockPerSecond() * 2;
			bps.Y = target.Y * bps.X / target.X;

			addY = coefficient * weight / (target.X / 2)*0.102;

			radian = Math.Atan(bps.Y - addY / bps.X) * Math.PI / 180;

			if (!boundDir)
			{

				if (total.Y < target.Y)
				{

					if (!charaDir)
					{
						if (!BlockCheck.BlockCheckLeft(pos.X - bps.X, pos.Y, speed) &&
							pos.X - bps.X > 0)
						{
							pos.X -= bps.X;
						}

					}
					else
					{
						if (!BlockCheck.BlockCheckRight(pos.X + bps.X, pos.Y, speed) &&
							pos.X + bps.X < 1024 - width * 32)
						{
							pos.X += bps.X;
						}
					}

					if (!BlockCheck.BlockCheckTop(pos.X, pos.Y - bps.Y,jumppower) &&
						pos.Y - bps.Y > 0)
					{
						pos.Y -= bps.Y;
					}



					total.X += Math.Sqrt(Math.Pow(bps.X, 2));
					total.Y += Math.Sqrt(Math.Pow(bps.Y, 2));

					bps.X += bps.X;
					bps.Y += bps.Y;

					coefficient++;

				}
				else
				{
					boundDir = true;
					total.Y = target.Y;
				}
			}
			else
			{

				if (total.Y > 0)
				{

					if (!charaDir)
					{
						if (!BlockCheck.BlockCheckLeft(pos.X - bps.X, pos.Y, speed) &&
							pos.X - bps.X > 0)
						{
							pos.X -= bps.X;
						}

					}
					else
					{
						if (!BlockCheck.BlockCheckRight(pos.X + bps.X, pos.Y, speed) &&
							pos.X + bps.X < 1024 - width * 32)
						{
							pos.X += bps.X;
						}
					}

					if (!BlockCheck.BlockCheckBottom(pos.X, pos.Y + bps.Y + height * 32,weight) &&
						pos.Y + bps.Y < 768 - height * 32)
					{
						pos.Y += bps.Y;
					}

					total.X += Math.Sqrt(Math.Pow(bps.X, 2));
					total.Y -= Math.Sqrt(Math.Pow(bps.Y, 2));

					bps.X += bps.X;
					bps.Y += bps.Y;

					coefficient--;

				}
				else
				{
					boundDir = false;
					isKnockBack = false;
				}
			}
		}

		public static bool FaceEachOther(double posX ,double attackerX)
		{
			return (attackerX < posX) ? true : false;
		}

		public static Vector FromCodeToBlocks(Point point)
		{
			Vector blockPos = new Vector();

			blockPos.X = (int)Math.Ceiling(point.X / 32);
			blockPos.Y = (int)Math.Ceiling(point.Y / 32);

			if (blockPos.X == 0) blockPos.X = 1;
			if (blockPos.Y == 0) blockPos.Y = 1;
			if (blockPos.X >= 32) blockPos.X = 32;
			if (blockPos.Y >= 24) blockPos.Y = 24;

			return blockPos ;
		}

	}
}
