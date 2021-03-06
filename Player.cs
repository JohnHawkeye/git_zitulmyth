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
using static Zitulmyth.PlayerStatus;

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
		public static EquipWeaponName equipWeapon = EquipWeaponName.None;

		public static Vector playerSize = new Vector(32, 64);//for pixel calculation
		public static Vector playerPos;
		public static bool playerDirection = true;  //f:left t:right
		public static int weight = 160;
		public static int moveSpeed = 128;
		public static bool isMove = false;
		public static bool isGround = false;
		public static int meleeSpeed = 256;
		public static bool meleeDirection = false;
		public static bool isMainAttack = false;
		public static Vector knockBackTotalDis;
		public static Vector knockBackBps;
		public static Vector knockBackTargetDis;
		public static double knockBackCountNum;
		public static double coefficient;
		public static bool isKnockBack;
		public static bool boundDirectionX = false;
		public static bool boundDirectionY = false;

		public static int jumpPower = 160;
		public static int jumpCount = 0;
		public static int jumpMaxHeight = 64;
		public static double jumpTotalLength = 0;
		public static bool jumping = false;
		public static int fallingEndure = 3;
		public static bool fallingStart = false;
		public static double fallingStartPoint = 0;
		public static bool isPlat = false;
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
	

			if (KeyController.keyLeft)
			{
				if (!BlockCheck.BlockCheckLeft(playerPos.X, playerPos.Y, (int)playerSize.Y, moveSpeed) 
					&& !ObjectChecker.obstacleLeft)
				{
					if (playerPos.X - SystemOperator.PixelPerSecond(moveSpeed) > 0)
					{
						playerPos.X -= SystemOperator.PixelPerSecond(moveSpeed);
						SystemOperator.moveCommonAmountX = moveSpeed;
					}
					playerDirection = false;
				}

			}

			if (KeyController.keyRight)
			{
				if (!BlockCheck.BlockCheckRight(playerPos.X, playerPos.Y, (int)playerSize.X, (int)playerSize.Y, moveSpeed)
					&& !ObjectChecker.obstacleRight)
				{

					if (playerPos.X + SystemOperator.PixelPerSecond(moveSpeed) < 992)
					{
						playerPos.X += SystemOperator.PixelPerSecond(moveSpeed);
						SystemOperator.moveCommonAmountX = moveSpeed;
					}
					playerDirection = true;

				}

			}

			if (KeyController.keyLeft || KeyController.keyRight)
			{
				isMove = true;
			}
			else
			{
				isMove = false;
			}


			if (KeyController.keyUp && !TalkCommander.isTalk
				&& !ObjectChecker.obstacleUp)
			{
				if (isLadder)
				{
					if (playerPos.Y - SystemOperator.PixelPerSecond(moveSpeed) > 0)
					{
						playerPos.Y -= SystemOperator.PixelPerSecond(moveSpeed);
						SystemOperator.moveCommonAmountY = moveSpeed;
					}
				}

			}

			if (KeyController.keyDown && !TalkCommander.isTalk
				&& !ObjectChecker.obstacleDown)
			{
				if (isLadder)
				{
					if (playerPos.Y + SystemOperator.PixelPerSecond(moveSpeed) < 768)
					{
						playerPos.Y += SystemOperator.PixelPerSecond(moveSpeed);
						SystemOperator.moveCommonAmountY = moveSpeed;
					}
				}

				if (isPlat)
				{
					if (playerPos.Y + SystemOperator.PixelPerSecond(moveSpeed) < 768)
					{
						playerPos.Y += SystemOperator.PixelPerSecond(moveSpeed);
						SystemOperator.moveCommonAmountY = moveSpeed;
					}
				}

				if (!isLadder)
				{
					isSquat = true;

				}

			}
			else
			{
				isSquat = false;
			}

			//jump
			if (KeyController.keySpace && jumpCount == 0)
			{

				if (playerPos.Y - jumpPower > 0)
				{
					jumpCount++;

					jumping = true;
				}

			}

			if (jumping)
			{
				if (jumpTotalLength < jumpMaxHeight &&
					!BlockCheck.BlockCheckTop(playerPos.X, playerPos.Y, (int)playerSize.X, jumpPower)
					&& !ObjectChecker.obstacleUp)
				{
					playerPos.Y -= SystemOperator.PixelPerSecond(jumpPower);
					SystemOperator.moveCommonAmountY = SystemOperator.PixelPerSecond(jumpPower);

					jumpTotalLength += SystemOperator.PixelPerSecond(jumpPower);

				}
				else
				{
					jumping = false;
					jumpTotalLength = 0;
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
				SubWeapon.SubWeaponGenerate(canvas, playerPos.X, playerPos.Y);

			}

			if (KeyController.keyD)
			{
				if (!isMainAttack)
				{
					MainWeapon.imgMainWeapon.Visibility = Visibility.Visible;
					meleeDirection = playerDirection;
					isMainAttack = true;
				}
			}

			if (KeyController.keyE)
			{
				if (ObjectChecker.isTrigger && !TalkCommander.isTalk)
				{
					ObjectBehavior.OnTriggerReactEvent();
				}
			}

			if (isKnockBack)
			{
				SystemOperator.BoundObject(ref playerPos, boundDirectionX, ref knockBackTotalDis, knockBackTargetDis,
					ref knockBackBps, ref coefficient, ref boundDirectionY,
					weight, moveSpeed, jumpPower, playerSize, ref isKnockBack);
			}

			//image change
			if (GameTransition.gameTransition == GameTransitionType.StageDuring)
			{
				if (KeyController.keyLeft || KeyController.keyRight)
				{
					if (!playerDirection)
					{
						ImageData.imgPlayer.Source = ImageData.ImageSourceSelector(CategoryName.Player, StageData.lstDbPlayer.spriteMoveL);
					}
					else
					{
						ImageData.imgPlayer.Source = ImageData.ImageSourceSelector(CategoryName.Player, StageData.lstDbPlayer.spriteMoveR);
					}
				}
				else
				{
					if (!playerDirection)
					{
						ImageData.imgPlayer.Source = ImageData.ImageSourceSelector(CategoryName.Player, StageData.lstDbPlayer.spriteIdleL);
					}
					else
					{
						ImageData.imgPlayer.Source = ImageData.ImageSourceSelector(CategoryName.Player, StageData.lstDbPlayer.spriteIdleR);
					}
				}


				if (isSquat)
				{
					if (!playerDirection)
					{
						ImageData.imgPlayer.Source = ImageData.ImageSourceSelector(CategoryName.Player, StageData.lstDbPlayer.spriteSquatL);
					}
					else
					{
						ImageData.imgPlayer.Source = ImageData.ImageSourceSelector(CategoryName.Player, StageData.lstDbPlayer.spriteSquatR);
					}
				}

				if (jumping)
				{
					if (!playerDirection)
					{
						ImageData.imgPlayer.Source = ImageData.ImageSourceSelector(CategoryName.Player, StageData.lstDbPlayer.spriteJumpL);
					}
					else
					{
						ImageData.imgPlayer.Source = ImageData.ImageSourceSelector(CategoryName.Player, StageData.lstDbPlayer.spriteJumpR);
					}
				}

				if (isLadder && (KeyController.keyUp || KeyController.keyDown))
				{
					ImageData.imgPlayer.Source = ImageData.ImageSourceSelector(CategoryName.Player, StageData.lstDbPlayer.spriteLadder);
				}
			}

			Canvas.SetLeft(ImageData.imgPlayer, playerPos.X);
			Canvas.SetTop(ImageData.imgPlayer, playerPos.Y);
		}

		public static void FallingPlayer()
		{

			if (!isLadder && !isPlat && !jumping)
			{

				if (!BlockCheck.BlockCheckBottom(playerPos.X, playerPos.Y, (int)playerSize.X, (int)playerSize.Y, weight))
				{
					if (!isKnockBack)
					{
						playerPos.Y += SystemOperator.PixelPerSecond(weight);
						SystemOperator.moveCommonAmountY = SystemOperator.PixelPerSecond(weight);
					}

					if (!fallingStart)
					{
						fallingStartPoint = playerPos.Y;
					}

					fallingStart = true;
					isGround = false;
				}
				else
				{
					if (!isGround)
						playerPos.Y = Math.Floor( playerPos.Y+SystemOperator.PixelPerSecond(weight) / 32 ) - 1;

					if (fallingStart)
					{
						int block = 0;

						block = (int)(playerPos.Y - fallingStartPoint) / 32;

						if (block > fallingEndure)
						{
							Sound.SoundEffectSelector(SeName.Player_Damage);

							Sound.SoundEffectPlayer(SeName.Player_Damage);
						}
					}

					fallingStart = false;
					isGround = true;
					jumpCount = 0;
				}

			}

			Canvas.SetTop(ImageData.imgPlayer, playerPos.Y);

		}

		public static void DamageInvinsibleTimer()
		{
			if (damageInvincibleTotal < damageInvincible)
			{
				damageInvincibleTotal++;
			}
			else
			{
				ImageData.imgPlayer.Opacity = 1;
				damageInvincibleTotal = 0;
				flagDamaged = false;
			}
		}

		public static void CollisionPtoE()
		{

			if (!flagDamaged)
			{
				for (int i = 0; i < SpawnEnemy.lstEnemyData.Count; i++)
				{

					Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
					Vector size1 = new Vector(playerSize.X, playerSize.Y);

					Vector p2 = new Vector(Canvas.GetLeft(SpawnEnemy.lstEnemyData[i].imgEnemy),
											Canvas.GetTop(SpawnEnemy.lstEnemyData[i].imgEnemy));
					Vector size2 = new Vector(SpawnEnemy.lstEnemyData[i].size.X, SpawnEnemy.lstEnemyData[i].size.Y);

					if (CollisionCheck.Collision(p1, p2, size1, size2))
					{

						if (!Sound.seStop)
						{
							Sound.SoundEffectSelector(SeName.Player_Damage);
							Sound.SoundEffectPlayer(SeName.Player_Damage);

							Sound.seStop = true;
						}

						if (!isKnockBack)
						{
							//playerPos = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
							boundDirectionX = SystemOperator.FaceEachOther(playerPos.X, SpawnEnemy.lstEnemyData[i].position.X);

							knockBackTotalDis = new Vector(0, 0);
							knockBackBps = new Vector(0, 0);
							coefficient = 0;
							boundDirectionY = false;
							knockBackTargetDis = new Vector(64, 64);

							isKnockBack = true;

						}



						if (playerNowHp > 0)
						{
							playerNowHp -= SpawnEnemy.lstEnemyData[i].ofepower;
							ImageData.imgPlayer.Opacity = 0.6;
						}

						flagDamaged = true;
						damageInvincibleTotal = 0;
						Console.WriteLine("Break");
						break;

					}

				}
			}
		}

		public static void PlayerItemGetting(Canvas canvas)
		{

			if (!flagDamaged)
			{
				for (int i = 0; i < Item.lstItemData.Count; i++)
				{

					Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
					Vector size1 = new Vector(playerSize.X, playerSize.Y);

					Vector p2 = new Vector(Canvas.GetLeft(Item.lstItemData[i].imgItem), Canvas.GetTop(Item.lstItemData[i].imgItem));
					Vector size2 = new Vector(Item.lstItemData[i].size.X, Item.lstItemData[i].size.Y);

					if (CollisionCheck.Collision(p1, p2, size1, size2))
					{

						if (!Sound.seStop)
						{
							Sound.SoundEffectSelector(SeName.Item_Get);
							Sound.SoundEffectPlayer(SeName.Item_Get);

							Sound.seStop = true;
						}

						ItemGetSelector(i);

						canvas.Children.Remove(Item.lstItemData[i].imgItem);
						Item.lstItemData.RemoveAt(i);

						break;

					}

				}
			}
		}

		private static void ItemGetSelector(int index)
		{

			playerNowHp += Item.lstItemData[index].nowLife;
			playerNowMana += Item.lstItemData[index].nowMana;

			if(Item.lstItemData[index].attribute == ItemAttribute.Equipment)
			{
				equipWeapon = EquipWeaponName.TreeBranch;
				MainWeapon.SetMainWeapon();
			}

		}

	}
}
