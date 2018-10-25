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
		

		public static void BoundObject(SystemTargetName stn, int index,Vector pos ,bool charaDir,
										Vector total, Vector target , Vector bps,double acceleration)
		{

			double addY = 0;
			double radian = 0;

			switch (stn)
			{
				case SystemTargetName.Player:

					bps.X = target.X / 32 * BlockPerSecond();
					bps.Y = target.Y * bps.X / target.X;

					addY = acceleration * PlayerStatus.weight / (target.X / 2);

					radian = Math.Atan(bps.Y - addY / bps.X) * Math.PI / 180;

					if (!PlayerStatus.boundDirection)
					{

						if (total.Y < target.Y)
						{

							PlayerStatus.playerPos.X += bps.X;
							PlayerStatus.playerPos.Y -= bps.Y;

							PlayerStatus.knockBackTotalDis.X += Math.Sqrt(Math.Pow(bps.X, 2));
							PlayerStatus.knockBackTotalDis.Y += Math.Sqrt(Math.Pow(bps.Y, 2));

							PlayerStatus.knockBackBps.X += bps.X;
							PlayerStatus.knockBackBps.Y += bps.Y;

							PlayerStatus.knockBackCountNum++;

						}
						else
						{
							PlayerStatus.boundDirection = true;
							KeyController.keyControlLocking = false;
						}
					}
					else
					{

						if (total.Y > 0)
						{


							PlayerStatus.playerPos.X += bps.X;
							PlayerStatus.playerPos.Y += bps.Y;

							PlayerStatus.knockBackTotalDis.X += Math.Sqrt(Math.Pow(bps.X, 2));
							PlayerStatus.knockBackTotalDis.Y += Math.Sqrt(Math.Pow(bps.Y, 2));

							PlayerStatus.knockBackBps.X += bps.X;
							PlayerStatus.knockBackBps.Y += bps.Y;

							PlayerStatus.knockBackCountNum--;

						}
						else
						{
							PlayerStatus.boundDirection = false;
							PlayerStatus.isKnockBack = false;
						}
					}
					break;

				case SystemTargetName.Enemy:

					bps.X = target.X / 32 * BlockPerSecond();
					bps.Y = target.Y * bps.X / target.X;

					addY = acceleration * SpawnEnemy.lstEnemyData[index].weight / (target.X / 2);

					radian = Math.Atan(bps.Y - addY / bps.X) * Math.PI / 180;

					if (!SpawnEnemy.lstEnemyData[index].boundDirection)
					{

						if (total.Y < target.Y)
						{

							if (!charaDir)
							{
								if (!BlockCheck.BlockCheckLeft(pos.X-bps.X, pos.Y, SpawnEnemy.lstEnemyData[index].speed) &&
									pos.X - bps.X > 0)
								{
									SpawnEnemy.lstEnemyData[index].position.X -= bps.X;
								}
								
							}
							else
							{
								if (!BlockCheck.BlockCheckRight(pos.X+bps.X, pos.Y, SpawnEnemy.lstEnemyData[index].speed) &&
									pos.X + bps.X < 1024-SpawnEnemy.lstEnemyData[index].widthblock*32)
								{
									SpawnEnemy.lstEnemyData[index].position.X += bps.X;
								}
							}

							if (!BlockCheck.BlockCheckTop(pos.X, pos.Y-bps.Y, SpawnEnemy.lstEnemyData[index].jumpPower)&&
								pos.Y - bps.Y > 0)
							{
								SpawnEnemy.lstEnemyData[index].position.Y -= bps.Y;
							}

							

							SpawnEnemy.lstEnemyData[index].totalDistance.X += Math.Sqrt(Math.Pow(bps.X, 2));
							SpawnEnemy.lstEnemyData[index].totalDistance.Y += Math.Sqrt(Math.Pow(bps.Y, 2));

							SpawnEnemy.lstEnemyData[index].bps.X += bps.X;
							SpawnEnemy.lstEnemyData[index].bps.Y += bps.Y;

							SpawnEnemy.lstEnemyData[index].universalvalue++;

						}
						else
						{
							SpawnEnemy.lstEnemyData[index].boundDirection = true;
						}
					}
					else
					{

						if (total.Y > 0)
						{

							if (!charaDir)
							{
								if (!BlockCheck.BlockCheckLeft(pos.X-bps.X, pos.Y, SpawnEnemy.lstEnemyData[index].speed)&&
									pos.X - bps.X > 0)
								{
									SpawnEnemy.lstEnemyData[index].position.X -= bps.X;
								}

							}
							else
							{
								if (!BlockCheck.BlockCheckRight(pos.X+bps.X, pos.Y, SpawnEnemy.lstEnemyData[index].speed)&&
									pos.X + bps.X < 1024 - SpawnEnemy.lstEnemyData[index].widthblock * 32)
								{
									SpawnEnemy.lstEnemyData[index].position.X += bps.X;
								}
							}

							if (!BlockCheck.BlockCheckBottom(pos.X, pos.Y+bps.Y, SpawnEnemy.lstEnemyData[index].weight)&&
								pos.Y + bps.Y < 768 - SpawnEnemy.lstEnemyData[index].heightblock * 32)
							{
								SpawnEnemy.lstEnemyData[index].position.Y += bps.Y;
							}
								

							SpawnEnemy.lstEnemyData[index].totalDistance.X += Math.Sqrt(Math.Pow(bps.X, 2));
							SpawnEnemy.lstEnemyData[index].totalDistance.Y += Math.Sqrt(Math.Pow(bps.Y, 2));

							SpawnEnemy.lstEnemyData[index].bps.X += bps.X;
							SpawnEnemy.lstEnemyData[index].bps.Y += bps.Y;

							SpawnEnemy.lstEnemyData[index].universalvalue--;

						}
						else
						{
							SpawnEnemy.lstEnemyData[index].boundDirection = false;
							SpawnEnemy.lstEnemyData[index].isKnockBack = false;
						}
					}
					break;


				case SystemTargetName.Item:

					break;

				case SystemTargetName.Ui:

					break;

			}

		}

		public static bool FaceEachOther(double posX ,double attackerX)
		{
			return (attackerX < posX) ? true : false;
		}



	}
}
