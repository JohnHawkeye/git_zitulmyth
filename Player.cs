﻿using System;
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

		public static Vector playerSize = new Vector(32, 64);//for pixel calculation
		public static int playerWidth = 1;	//block num
		public static int playerHeight = 2;
		public static int playerImageZindex = 7;
		public static bool playerDirection = true;  //f:left t:right
		public static int weight = 6;
		public static int moveSpeed = 2;
		public static int meleeSpeed = 2;
		public static bool isMainAttack = false;

		public static int jumpPower = 12;
		public static int jumpCount = 0;
		public static int jumpMaxHeight = 96;
		public static int jumpTotalLength = 0;
		public static bool jumping = false;
		public static int fallingEndure = 3;
		public static bool fallingStart = false;
		public static double fallingStartPoint = 0;
		public static bool isLadder = false;
		public static bool isSquat = false;


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
				if (BlockCheck.BlockCheckLeft(posX, posY, PlayerStatus.moveSpeed))
				{
					if (posX - PlayerStatus.moveSpeed > 0)
					{
						posX -= PlayerStatus.moveSpeed;
					}
					PlayerStatus.playerDirection = false;
				}

			}
			else

			if (KeyController.keyRight)
			{
				if (BlockCheck.BlockCheckRight(posX, posY, PlayerStatus.moveSpeed))
				{

					if (posX + PlayerStatus.moveSpeed < 992)
					{
						posX += PlayerStatus.moveSpeed;
					}
					PlayerStatus.playerDirection = true;

				}

			}

			
			if (KeyController.keyUp)
			{
				if (BlockCheck.BlockCheckLadder(posX, posY, PlayerStatus.moveSpeed))
				{
					if (posY - PlayerStatus.moveSpeed > 0)
					{
						posY -= PlayerStatus.moveSpeed;
					}
				}

			}

			if (KeyController.keyDown)
			{
				//ladder
				if ((BlockCheck.BlockCheckLadder(posX, posY, PlayerStatus.moveSpeed) ||
					BlockCheck.BlockCheckTopLadder(posX, posY, PlayerStatus.weight))&&
					!BlockCheck.BlockCheckGround(posX, posY, PlayerStatus.weight))
				{
					if (posY + PlayerStatus.moveSpeed < 768)
					{
						posY += PlayerStatus.moveSpeed;
					}
				}

				if (BlockCheck.BlockCheckOnPlat(posX, posY, PlayerStatus.weight,PlayerStatus.playerHeight))
				{
					if (posY + PlayerStatus.moveSpeed < 768)
					{
						posY += PlayerStatus.moveSpeed;
					}
				}

				if (!PlayerStatus.isLadder)
				{
					PlayerStatus.isSquat = true;

			

				}

			}
			else
			{
				PlayerStatus.isSquat = false;
			}

			//laddercheck
			if(!BlockCheck.BlockCheckLadder(posX, posY, PlayerStatus.moveSpeed)){
				PlayerStatus.isLadder = false;
				
			}
			else
			{
				PlayerStatus.isLadder = true;
				PlayerStatus.fallingStartPoint = posY;
			}


			//jump

			if (KeyController.keySpace && PlayerStatus.jumpCount == 0)
			{
				if (BlockCheck.BlockCheckTop(posX, posY, PlayerStatus.jumpPower)&&
					!BlockCheck.BlockCheckLadder(posX, posY, PlayerStatus.moveSpeed))
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

				if (PlayerStatus.isSquat)
				{
					if (!PlayerStatus.playerDirection)
					{
						ImageData.imgPlayer.Source = ImageData.cbPlayer[5];
					}
					else
					{
						ImageData.imgPlayer.Source = ImageData.cbPlayer[4];
					}
				}

				if (PlayerStatus.isLadder && (KeyController.keyUp || KeyController.keyDown))
				{
					ImageData.imgPlayer.Source = ImageData.cbPlayer[6];
				}

				
			}

			//itemget
			if (KeyController.keyA)
			{
				PlayerItemGetting(canvas);
			}

			//Attack
			if (KeyController.keyS)
			{

				SubWeapon.SubWeaponGenerate(canvas, posX, posY);

			}

			if (KeyController.keyD)
			{
				if(!PlayerStatus.isMainAttack)
				{
					MainWeapon.imgMainWeapon.Visibility = Visibility.Visible;
					PlayerStatus.isMainAttack = true;
				}
			}

			Canvas.SetLeft(ImageData.imgPlayer, posX);
			Canvas.SetTop(ImageData.imgPlayer, posY);
		}

		public static void FallingPlayer()
		{
			double posX = Canvas.GetLeft(ImageData.imgPlayer);
			double posY = Canvas.GetTop(ImageData.imgPlayer);

			if (!PlayerStatus.isLadder && !BlockCheck.BlockCheckTopLadder(posX,posY,PlayerStatus.weight))
			{
				if (BlockCheck.BlockCheckBottom(posX, posY, PlayerStatus.weight,PlayerStatus.playerHeight)&&
					!BlockCheck.BlockCheckOnPlat(posX, posY, PlayerStatus.weight, PlayerStatus.playerHeight))
				{
					if (posY + 32 < 23 * 32)
					{
						posY += PlayerStatus.weight;
					}

					if (!PlayerStatus.fallingStart)
					{
						PlayerStatus.fallingStartPoint = posY;
					}
					PlayerStatus.fallingStart = true;
				}
				else
				{
					if (PlayerStatus.fallingStart)
					{
						int block = 0;

						block = (int)(posY - PlayerStatus.fallingStartPoint)/32;

						if (block > PlayerStatus.fallingEndure)
						{
							Sound.seDamage.Play();
						}
					}
					PlayerStatus.fallingStart = false;
					PlayerStatus.jumpCount = 0;
				}

				Canvas.SetTop(ImageData.imgPlayer, posY);
			}
			
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

		public static void PlayerItemGetting(Canvas canvas)
		{

			if (!PlayerStatus.flagDamaged)
			{
				for (int i = 0; i < Item.lstItemData.Count; i++)
				{

					Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
					Vector size1 = new Vector(PlayerStatus.playerSize.X, PlayerStatus.playerSize.Y);

					Vector p2 = new Vector(Canvas.GetLeft(Item.lstItemData[i].imgItem),Canvas.GetTop(Item.lstItemData[i].imgItem));
					Vector size2 = new Vector(Item.lstItemData[i].width*32, Item.lstItemData[i].height*32);

					if (CollisionCheck.Collision(p1, p2, size1, size2))
					{

						if (!Sound.seStop)
						{
							Sound.seChannelA.Stop();
							Sound.seChannelA.Play();
							Sound.seStop = true;
						}

						if(Item.lstItemData[i].itemName == ItemName.TreeBranch)
						{
							MainWeapon.SetMainWeapon();
						}

						canvas.Children.Remove(Item.lstItemData[i].imgItem);
						Item.lstItemData.RemoveAt(i);

						break;

					}

				}
			}
		}

	}
}
