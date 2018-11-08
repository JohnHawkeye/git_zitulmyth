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

	public enum EquipWeaponName
	{
		None = 0,
		TreeBranch,
	}

	public class MainWeapon
	{
		//mainweapon property
		public static Image imgMainWeapon = new Image();
		public static int width = 3;
		public static int height = 1;
		public static RotateTransform rtMainWeapon = new RotateTransform(270);
		private static List<Vector> lstMainWeaponCollider = new List<Vector>();

		public static int mainWeaponDamage = 1;


		public static void InitMainWeapon(Canvas canvas)
		{
			var _imgMainWeapon = new Image
			{
				Source = ImageData.cbEmpty,
				Width = 96,
				Height = 32,

			};

			imgMainWeapon = _imgMainWeapon;
			imgMainWeapon.Visibility = Visibility.Hidden;
			canvas.Children.Add(imgMainWeapon);

			Canvas.SetLeft(imgMainWeapon, 0);
			Canvas.SetTop(imgMainWeapon, 0);
			Canvas.SetZIndex(imgMainWeapon, 9);


		}

		public static void SetMainWeapon()
		{

			switch (PlayerStatus.equipWeapon)
			{
				case EquipWeaponName.None:
					width = 2; height = 1;
					break;

				case EquipWeaponName.TreeBranch:
					width = 3; height = 1;
					break;
				
			}

			for(int i = 0; i < width-1; i++)	//1hand ,collider for the length of weapon.
			{
				lstMainWeaponCollider.Add(new Vector(0,0));
			}
			

			imgMainWeapon.Source = ImageData.cbMainWeapon;
			
			rtMainWeapon.CenterX = 16;
			rtMainWeapon.CenterY = 16;
			imgMainWeapon.RenderTransform = rtMainWeapon;

		}

		public static void MainWeaponAttack(Canvas canvas)
		{

			double posx = Canvas.GetLeft(ImageData.imgPlayer);
			double posy = Canvas.GetTop(ImageData.imgPlayer);

			Canvas.SetLeft(imgMainWeapon, posx);
			Canvas.SetTop(imgMainWeapon, posy);

			if (PlayerStatus.meleeDirection)
			{
				if (rtMainWeapon.Angle < 360 + 45)
				{

					double temp = SystemOperator.BlockPerSecond() * PlayerStatus.meleeSpeed;


					rtMainWeapon.Angle += Math.Round(temp, 0);

					double radian = rtMainWeapon.Angle * Math.PI / 180;

					for(int i = 0; i < lstMainWeaponCollider.Count; i++)
					{
						lstMainWeaponCollider[i] = new Vector(posx + 32 * (i+1) * Math.Cos(radian),
																posy + 32 * (i+1) * Math.Sin(radian));

					}

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
				if (rtMainWeapon.Angle >= 180 - 45)
				{

					double temp = PlayerStatus.meleeSpeed;


					rtMainWeapon.Angle -= Math.Round(temp, 0);

					double radian = rtMainWeapon.Angle * Math.PI / 180;


					for (int i = 0; i < lstMainWeaponCollider.Count; i++)
					{
						lstMainWeaponCollider[i] = new Vector(posx + 32 * (i + 1) * Math.Cos(radian),
																posy + 32 * (i + 1) * Math.Sin(radian));

					}

				}
				else
				{

					rtMainWeapon.Angle = 270;
					imgMainWeapon.Visibility = Visibility.Hidden;
					PlayerStatus.isMainAttack = false;


				}
			}

		}

		public static void MainWeaponCollision(Canvas canvas)
		{

			for (int i = 0; i < lstMainWeaponCollider.Count; i++)
			{
				for (int j = 0; j < SpawnEnemy.lstEnemyData.Count; j++)
				{
					if (!SpawnEnemy.lstEnemyData[j].isDamage)
					{

						Vector p1 = new Vector(lstMainWeaponCollider[i].X, lstMainWeaponCollider[i].Y);
						Vector size1 = new Vector(32, 32);

						Vector p2 = new Vector(SpawnEnemy.lstEnemyData[j].position.X, SpawnEnemy.lstEnemyData[j].position.Y);
						Vector size2 = new Vector(SpawnEnemy.lstEnemyData[j].pixSize.X, SpawnEnemy.lstEnemyData[j].pixSize.Y);

						if (CollisionCheck.Collision(p1, p2, size1, size2)&&
							SpawnEnemy.lstEnemyData[j].state != EnemyState.Death)
						{

							SpawnEnemy.lstEnemyData[j].isDamage = true;
							SpawnEnemy.lstEnemyData[j].totalInvincibleTime = 0;

							SpawnEnemy.lstEnemyData[j].coefficient = 0;
							SpawnEnemy.lstEnemyData[j].totalDistance = new Vector(0, 0);
							SpawnEnemy.lstEnemyData[j].bps = new Vector(0, 0);
							SpawnEnemy.lstEnemyData[j].targetDistance = new Vector(32, 32);
							SpawnEnemy.lstEnemyData[j].isKnockBack = true;
							

							if (!Sound.seStop)
							{
								Sound.SoundEffectSelector(SeName.Shock);
								Sound.SoundEffectPlayer(SeName.Shock);

								Sound.seStop = true;
							}

							SpawnEnemy.lstEnemyData[j].life -= 1;

							if (SpawnEnemy.lstEnemyData[j].life <= 0)
							{
								SpawnEnemy.lstEnemyData[j].isWaiting = false;
								SpawnEnemy.lstEnemyData[j].state = EnemyState.Death;

								SpawnEnemy.EnemyDeathItemDrop(canvas, SpawnEnemy.lstEnemyData[j].name, SpawnEnemy.lstEnemyData[j].position);
							}

						}

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
							Sound.SoundEffectSelector(SeName.Fog);
							Sound.SoundEffectPlayer(SeName.Fog);

							Sound.seStop = true;
						}

						SpawnEnemy.lstEnemyData[i].life -= 1;

						if (SpawnEnemy.lstEnemyData[i].life <= 0)
						{
							bool popOn = false;
							EnemyName name = EnemyName.Zigitu01;

							if (SpawnEnemy.lstEnemyData[i].deathEvent == EnemyDeathEvent.Pop)
							{
								if (GameTransition.gameTransition == GameTransitionType.StageDuring)
								{
									popOn = true;
								}

							}

							canvas.Children.Remove(SpawnEnemy.lstEnemyData[i].imgEnemy);
							SpawnEnemy.lstEnemyData.RemoveAt(i);
							StageManager.numKillEnemy++;
							Console.WriteLine(StageManager.numKillEnemy);

							if (popOn)
							{
								if (StageManager.numKillEnemy < 10)
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
