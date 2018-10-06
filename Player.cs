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
	public class PlayerStatus
	{

		//player parameters
		public static int playerMaxHp = 3;
		public static int playerNowHp;
		public static int playerMaxMana = 5;
		public static int playerNowMana;
		public static int damageInvincibleTotal = 0;
		public static int damageInvincible = 60;
		public static bool flagDamaged = false;

		public static Vector playerSize = new Vector(32, 64);
		public static int playerImageZindex = 5;
		public static bool playerDirection = true;  //f:left t:right
		public static int weight = 2;
		public static int speed = 2;
		public static int jumpPower = 8;
		public static int jumpCount = 0;
		public static int jumpMaxHeight = 64;
		public static int jumpTotalLength = 0;
		public static bool jumping = false;


		public static void PlayerStatusUpdate()
		{
			for (int i = playerMaxHp - 1; i >= 0; i--)
			{
				if (i > playerNowHp - 1)
				{
					ImageData.imgLife[i].Source = ImageData.cbLife[1];
				}
				else
				{
					ImageData.imgLife[i].Source = ImageData.cbLife[0];
				}

			}
		}
	}

	public class PlayerBehavior
	{

		public static void MovePlayer(Canvas canvas)
		{
			double posX = Canvas.GetLeft(ImageData.imgPlayer);
			double posY = Canvas.GetTop(ImageData.imgPlayer);
			
			if (KeyController.keyLeft)
			{
				if (BlockCheck.BlockCheckLeft(posX, posY, PlayerStatus.speed))
				{
					if (posX - PlayerStatus.speed > 0)
					{
						posX -= PlayerStatus.speed;
					}
					PlayerStatus.playerDirection = false;
				}

			}
			else

			if (KeyController.keyRight)
			{
				if (BlockCheck.BlockCheckRight(posX, posY, PlayerStatus.speed))
				{

					if (posX + PlayerStatus.speed < 1024)
					{
						posX += PlayerStatus.speed;
					}
					PlayerStatus.playerDirection = true;

				}

			}

			
			if (KeyController.keyUp)
			{
				if (BlockCheck.BlockCheckLadder(posX, posY, PlayerStatus.speed))
				{
					if (posY - PlayerStatus.speed > 0)
					{
						posY -= PlayerStatus.speed;
					}
				}
			}

			if (KeyController.keyDown)
			{
				//ladder
				if ((BlockCheck.BlockCheckLadder(posX, posY, PlayerStatus.speed)||
					BlockCheck.BlockCheckTopLadder(posX, posY, PlayerStatus.weight))&&
					!BlockCheck.BlockCheckGround(posX, posY, PlayerStatus.weight))
				{
					if (posY + PlayerStatus.speed <768)
					{
						posY += PlayerStatus.speed;
					}
				}

				if (BlockCheck.BlockCheckOnPlat(posX, posY, PlayerStatus.weight))
				{
					if (posY + PlayerStatus.speed < 768)
					{
						posY += PlayerStatus.speed;
					}
				}
				
			}
			else
			{

			}

			//jump

			if (KeyController.keySpace && PlayerStatus.jumpCount == 0)
			{
				if (BlockCheck.BlockCheckTop(posX, posY, PlayerStatus.jumpPower)&&
					!BlockCheck.BlockCheckLadder(posX, posY, PlayerStatus.speed))
				{
					if (posY - PlayerStatus.jumpPower > 0)
					{
						PlayerStatus.jumpCount++;

						PlayerStatus.jumping = true;

					}
				}
			}

			if (PlayerStatus.jumping)
			{
				if (PlayerStatus.jumpTotalLength < PlayerStatus.jumpMaxHeight)
				{
					posY -= PlayerStatus.jumpPower;
					PlayerStatus.jumpTotalLength += PlayerStatus.jumpPower;
				}
				else
				{
					PlayerStatus.jumping = false;
					PlayerStatus.jumpTotalLength = 0;
				}
			}

			if (GameTransition.gameTransition == GameTransitionType.StageDuring)
			{
				if (!PlayerStatus.playerDirection)
				{
					ImageData.imgPlayer.Source = ImageData.cbPlayer[1];
				}
				else
				{
					ImageData.imgPlayer.Source = ImageData.cbPlayer[0];
				}
			}


			//Attack
			if (KeyController.keyD)
			{

				SubWeapon.SubWeaponGenerate(canvas, posX, posY);

			}

			Canvas.SetLeft(ImageData.imgPlayer, posX);
			Canvas.SetTop(ImageData.imgPlayer, posY);
		}

		public static void FallingPlayer()
		{

			double posX = Canvas.GetLeft(ImageData.imgPlayer);
			double posY = Canvas.GetTop(ImageData.imgPlayer);


			if (BlockCheck.BlockCheckBottom(posX, posY, PlayerStatus.weight))
			{
				if (posY + 32 < 23 * 32)
				{
					posY += PlayerStatus.weight;
				}

			}

			Canvas.SetTop(ImageData.imgPlayer, posY);
		}

		public static void DamageInvinsibleTimer()
		{
			if (PlayerStatus.damageInvincibleTotal < PlayerStatus.damageInvincible)
			{
				PlayerStatus.damageInvincibleTotal++;
			}
			else
			{
				PlayerStatus.damageInvincibleTotal = 0;
				PlayerStatus.flagDamaged = false;
			}
		}

		public static void CollisionPtoE()
		{

			if (!PlayerStatus.flagDamaged)
			{
				for (int i = 0; i < EnemyData.lstSpawnEnemy.Count; i++)
				{

					Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
					Vector size1 = new Vector(PlayerStatus.playerSize.X, PlayerStatus.playerSize.Y);

					Vector p2 = new Vector(Canvas.GetLeft(EnemyData.lstSpawnEnemy[i].imgEnemy),
											Canvas.GetTop(EnemyData.lstSpawnEnemy[i].imgEnemy));
					Vector size2 = new Vector(EnemyData.lstSpawnEnemy[i].enemySize.X, EnemyData.lstSpawnEnemy[i].enemySize.Y);

					if (CollisionCheck.Collision(p1, p2, size1, size2))
					{

						if (!Sound.seStop)
						{
							Sound.seDamage.Stop();
							Sound.seDamage.Play();
							Sound.seStop = true;
						}

						if (PlayerStatus.playerNowHp > 0)
						{
							PlayerStatus.playerNowHp -= EnemyData.lstSpawnEnemy[i].enemyOfePower;
						}

						PlayerStatus.flagDamaged = true;
						PlayerStatus.damageInvincibleTotal = 0;
						Console.WriteLine("Break");
						break;

					}

				}
			}
		}
	}
}
