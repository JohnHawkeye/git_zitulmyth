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
	public class MainWeapon
	{
	}

	public class SubWeapon
	{
		//weapon parameters
		public static int subWeaponSpeed = 8;
		public static int subWeaponRange = 320;
		public static int subWeaponTotalDistance = 0;
		public static bool subWeaponDirection;
		public static int subWeaponZindex = 10;


		public static void SubWeaponGenerate(Canvas canvas, double posX,double posY)
		{
			if (ImageData.imgSubWeapon.Count == 0)
			{
				var _imgSubWeapon = new Image()
				{
					Source = ImageData.cbSubWeapon,
					Width = 32,
					Height = 32,
					Name = "sw0",
				};

				ImageData.imgSubWeapon.Add(_imgSubWeapon);
				canvas.Children.Add(ImageData.imgSubWeapon[0]);

				if (!PlayerStatus.playerDirection)
				{
					RotateTransform rt = new RotateTransform(180);
					rt.CenterX = 16;
					rt.CenterY = 16;
					ImageData.imgSubWeapon[0].RenderTransform = rt;

					subWeaponDirection = false;
				}
				else
				{
					subWeaponDirection = true;
				}

				Canvas.SetLeft(ImageData.imgSubWeapon[0], posX);
				Canvas.SetTop(ImageData.imgSubWeapon[0], posY);
				Canvas.SetZIndex(ImageData.imgSubWeapon[0], subWeaponZindex);
			}
		}

		public static void SubWeaponPosUpdate(Canvas canvas)
		{
			if (ImageData.imgSubWeapon.Count == 1)
			{
				double posX = Canvas.GetLeft(ImageData.imgSubWeapon[0]);
				double posY = Canvas.GetTop(ImageData.imgSubWeapon[0]);

				if (subWeaponTotalDistance < subWeaponRange)
				{
					if (!subWeaponDirection)
					{
						posX -= subWeaponSpeed;
					}
					else
					{
						posX += subWeaponSpeed;
					}

					subWeaponTotalDistance += subWeaponSpeed;
					Canvas.SetLeft(ImageData.imgSubWeapon[0], posX);
					Canvas.SetZIndex(ImageData.imgSubWeapon[0], subWeaponZindex);
				}
				else
				{
					subWeaponTotalDistance = 0;
					ImageData.imgSubWeapon[0].Name = "";
					canvas.Children.Remove(ImageData.imgSubWeapon[0]);
					ImageData.imgSubWeapon.Remove(ImageData.imgSubWeapon[0]);

				}
			}
		}

		public static void CollisionSubWeapon(Canvas canvas)
		{
			for (int i = 0; i < EnemyData.lstSpawnEnemy.Count; i++)
			{
				if (ImageData.imgSubWeapon.Count >= 1)
				{
					Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgSubWeapon[0]), Canvas.GetTop(ImageData.imgSubWeapon[0]));
					Vector size1 = new Vector(32, 32);

					Vector p2 = new Vector(Canvas.GetLeft(EnemyData.lstSpawnEnemy[i].imgEnemy), Canvas.GetTop(EnemyData.lstSpawnEnemy[i].imgEnemy));
					Vector size2 = new Vector(EnemyData.lstSpawnEnemy[i].enemySize.X, EnemyData.lstSpawnEnemy[i].enemySize.Y);

					if (CollisionCheck.Collision(p1, p2, size1, size2))
					{

						canvas.Children.Remove(ImageData.imgSubWeapon[0]);
						ImageData.imgSubWeapon.Remove(ImageData.imgSubWeapon[0]);

						if (!Sound.seStop)
						{
							Sound.seFog.Stop();
							Sound.seFog.Play();
							Sound.seStop = true;
						}

						EnemyData.lstSpawnEnemy[i].enemyHp -= 1;

						if (EnemyData.lstSpawnEnemy[i].enemyHp <= 0)
						{
							bool popOn = false;
							EnemyName name = EnemyName.Zigytu01;

							if (EnemyData.lstSpawnEnemy[i].deathEffect == EnemyDeathEffect.Pop)
							{
								if (GameTransition.gameTransition == GameTransitionType.StageDuring)
								{
									popOn = true;
								}

							}

							canvas.Children.Remove(EnemyData.lstSpawnEnemy[i].imgEnemy);
							EnemyData.lstSpawnEnemy.RemoveAt(i);
							GameTransition.numKillEnemy++;
							Console.WriteLine(GameTransition.numKillEnemy);

							if (popOn)
							{
								if (GameTransition.numKillEnemy < 10)
								{
									SpawnEnemy.SpawnSelect(canvas, name);
								}

							}
						}

						break;
					}
				}
			}

		}

	}
}
