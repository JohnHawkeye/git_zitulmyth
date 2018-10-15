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
		public static Image imgMainWeapon = new Image();
		public static int width = 3;
		public static int height = 1;
		public static RotateTransform rtMainWeapon = new RotateTransform(270);
		private static Vector[] mainWeaponCollider = new Vector[2];

		public static int mainWeaponDamage = 1;


		public static Image[] imgColliderCheck = new Image[2];

		public static void InitMainWeapon(Canvas canvas)
		{
			var _imgMainWeapon = new Image
			{
				Source = ImageData.cbMainWeapon,
				Width = 96,
				Height = 32,

			};

			imgMainWeapon = _imgMainWeapon;
			imgMainWeapon.Visibility = Visibility.Hidden;
			canvas.Children.Add(imgMainWeapon);

			Canvas.SetLeft(imgMainWeapon, 0);
			Canvas.SetTop(imgMainWeapon, 0);
			Canvas.SetZIndex(imgMainWeapon, 9);

			var _imgap = new Image
			{
				Source = ImageData.cbItem[2],
				Width = 32,
				Height = 32,
			};

			imgColliderCheck[0] = _imgap;
			canvas.Children.Add(imgColliderCheck[0]);
			Canvas.SetLeft(imgColliderCheck[0], 0);
			Canvas.SetTop(imgColliderCheck[0], 0);
			Canvas.SetZIndex(imgColliderCheck[0], 9);

			var _imgap2 = new Image
			{
				Source = ImageData.cbItem[2],
				Width = 32,
				Height = 32,
			};
			imgColliderCheck[1] = _imgap2;
			canvas.Children.Add(imgColliderCheck[1]);
			Canvas.SetLeft(imgColliderCheck[1], 0);
			Canvas.SetTop(imgColliderCheck[1], 0);
			Canvas.SetZIndex(imgColliderCheck[1], 9);

		}

		public static void SetMainWeapon()
		{

			imgMainWeapon.Source = ImageData.cbMainWeapon;
			
			rtMainWeapon.CenterX = 16;
			rtMainWeapon.CenterY = 16;
			imgMainWeapon.RenderTransform = rtMainWeapon;
				
		}

		public static void MainWeaponAttack(Canvas canvas)
		{
			if (PlayerStatus.isMainAttack)
			{

				double posx = Canvas.GetLeft(ImageData.imgPlayer);
				double posy = Canvas.GetTop(ImageData.imgPlayer);

				Canvas.SetLeft(imgMainWeapon, posx);
				Canvas.SetTop(imgMainWeapon, posy);

				if (PlayerStatus.meleeDirection)
				{
					if (rtMainWeapon.Angle < 360)
					{

						double temp = PlayerStatus.meleeSpeed;


						rtMainWeapon.Angle += Math.Round(temp, 0);

						double radian = rtMainWeapon.Angle * Math.PI / 180;


						mainWeaponCollider[0] = new Vector(posx + 32 * Math.Cos(radian),
															posy + 32 * Math.Sin(radian));
						mainWeaponCollider[1] = new Vector(posx + 64 * Math.Cos(radian),
															posy + 64 * Math.Sin(radian));

						Canvas.SetLeft(imgColliderCheck[0], mainWeaponCollider[0].X);
						Canvas.SetTop(imgColliderCheck[0], mainWeaponCollider[0].Y);
						Canvas.SetLeft(imgColliderCheck[1], mainWeaponCollider[1].X);
						Canvas.SetTop(imgColliderCheck[1], mainWeaponCollider[1].Y);

					}
					else
					{

						rtMainWeapon.Angle = 270;
						imgMainWeapon.Visibility = Visibility.Hidden;
						PlayerStatus.isMainAttack = false;


					}
				}
				else
				{
					if (rtMainWeapon.Angle >= 180)
					{

						double temp = PlayerStatus.meleeSpeed;


						rtMainWeapon.Angle -= Math.Round(temp, 0);

						double radian = rtMainWeapon.Angle * Math.PI / 180;


						mainWeaponCollider[0] = new Vector(posx + 32 * Math.Cos(radian),
															posy + 32 * Math.Sin(radian));
						mainWeaponCollider[1] = new Vector(posx + 64 * Math.Cos(radian),
															posy + 64 * Math.Sin(radian));

						Canvas.SetLeft(imgColliderCheck[0], mainWeaponCollider[0].X);
						Canvas.SetTop(imgColliderCheck[0], mainWeaponCollider[0].Y);
						Canvas.SetLeft(imgColliderCheck[1], mainWeaponCollider[1].X);
						Canvas.SetTop(imgColliderCheck[1], mainWeaponCollider[1].Y);

					}
					else
					{

						rtMainWeapon.Angle = 270;
						imgMainWeapon.Visibility = Visibility.Hidden;
						PlayerStatus.isMainAttack = false;


					}
				}

				

			}

		}


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
			for (int i = 0; i < SpawnEnemy.lstEnemyData.Count; i++)
			{
				if (ImageData.imgSubWeapon.Count >= 1)
				{
					Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgSubWeapon[0]), Canvas.GetTop(ImageData.imgSubWeapon[0]));
					Vector size1 = new Vector(32, 32);

					Vector p2 = new Vector(Canvas.GetLeft(SpawnEnemy.lstEnemyData[i].imgEnemy), Canvas.GetTop(SpawnEnemy.lstEnemyData[i].imgEnemy));
					Vector size2 = new Vector(SpawnEnemy.lstEnemyData[i].pixSize.X, SpawnEnemy.lstEnemyData[i].pixSize.Y);

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

						SpawnEnemy.lstEnemyData[i].life -= 1;

						if (SpawnEnemy.lstEnemyData[i].life <= 0)
						{
							bool popOn = false;
							EnemyName name = EnemyName.Zigytu01;

							if (SpawnEnemy.lstEnemyData[i].deathEvent == EnemyDeathEvent.Pop)
							{
								if (GameTransition.gameTransition == GameTransitionType.StageDuring)
								{
									popOn = true;
								}

							}

							canvas.Children.Remove(SpawnEnemy.lstEnemyData[i].imgEnemy);
							SpawnEnemy.lstEnemyData.RemoveAt(i);
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
