using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zitulmyth.Checking;
using Zitulmyth.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static Zitulmyth.SpawnEnemy;

namespace Zitulmyth
{
	public enum EnemyState
	{
		Spawn,
		Idle,
		Normal,
		Active,
		Death,
	}

	public class EnemyBehavior
	{
		public static void EnemyAction()
		{

			for (int i = 0; i < lstEnemyData.Count; i++)
			{

				EnemyTriggerAreaSetting(i); //DebugView

				if (lstEnemyData[i].isDamage)
				{
					EnemyInvincibleTimeCount(i);

					if (lstEnemyData[i].isKnockBack)
					{

						SystemOperator.BoundObject(ref lstEnemyData[i].position, SystemOperator.FaceEachOther(lstEnemyData[i].position.X, PlayerStatus.playerPos.X),
							ref lstEnemyData[i].totalDistance, lstEnemyData[i].targetDistance, ref lstEnemyData[i].bps,
							ref lstEnemyData[i].coefficient, ref lstEnemyData[i].boundDirection,
							lstEnemyData[i].weight, lstEnemyData[i].speed, lstEnemyData[i].jumpPower,
							lstEnemyData[i].size, ref lstEnemyData[i].isKnockBack);
					}
				}

				if (!lstEnemyData[i].isKnockBack)
				{
					EnemyFalling(i, lstEnemyData[i].direction, lstEnemyData[i].position,
									lstEnemyData[i].size, lstEnemyData[i].speed, lstEnemyData[i].targetDistance);
				}


				switch (lstEnemyData[i].state)
				{
					case EnemyState.Spawn:
						lstEnemyData[i].state = EnemyState.Idle;
						break;

					case EnemyState.Idle:

						EnemyIdle(i);

						break;

					case EnemyState.Normal:

						EnemyNormal(i);

						break;

					case EnemyState.Active:

						EnemyActiv(i);

						break;

					case EnemyState.Death:

						if (!lstEnemyData[i].isWaiting && !lstEnemyData[i].isDeath)
						{
							lstEnemyData[i].totalWaitTime = 0;
							lstEnemyData[i].targetWaitTime = 2000;
							lstEnemyData[i].isWaiting = true;

						}
						else
						{
							EnemyWaiting(i);
						}



						break;
				}


				Canvas.SetLeft(lstEnemyData[i].imgEnemy, lstEnemyData[i].position.X);
				Canvas.SetTop(lstEnemyData[i].imgEnemy, lstEnemyData[i].position.Y);
			}

		}

		public static void EnemyBehaviorValuesSetup(int index)
		{

			bool radDir;
			int num;

			SystemOperator.randomNum = new Random(SystemOperator.timeSeed++);

			num = SystemOperator.randomNum.Next(1, 3);
			radDir = (num == 1) ? false : true;

			SystemOperator.randomNum = new Random(SystemOperator.timeSeed++);
			lstEnemyData[index].direction = radDir;
			lstEnemyData[index].acceleration = 1;
			lstEnemyData[index].targetDistance = new Vector(SystemOperator.randomNum.Next(32, 96), 0);


			if (lstEnemyData[index].state == EnemyState.Active)
			{
				SystemOperator.randomNum = new Random(SystemOperator.timeSeed++);
				lstEnemyData[index].acceleration = 2;
				lstEnemyData[index].targetDistance = new Vector(SystemOperator.randomNum.Next(280, 320), 0);
			}

			lstEnemyData[index].valueSetComp = true;

		}

		public static void EnemyIdle(int index)
		{

			if (lstEnemyData[index].isWaiting)
			{
				EnemyWaiting(index);
			}
			else
			{

				if (SystemOperator.moveCommonAmountX != 0 || SystemOperator.moveCommonAmountY != 0)
				{

					if (!EnemyTriggerCollisionCheck(index))
					{

						lstEnemyData[index].state = EnemyState.Normal;
					}
					else
					{
						lstEnemyData[index].state = EnemyState.Active;
					}

				}

			}

		}

		public static void EnemyNormal(int index)
		{

			EnemyHorizontalMove(index, lstEnemyData[index].direction, lstEnemyData[index].position,
								lstEnemyData[index].size, lstEnemyData[index].speed, true);


			lstEnemyData[index].state = EnemyState.Idle;

		}

		public static void EnemyActiv(int index)
		{

			EnemyHorizontalMove(index, lstEnemyData[index].direction, lstEnemyData[index].position,
								lstEnemyData[index].size, lstEnemyData[index].speed * lstEnemyData[index].acceleration, false);

			lstEnemyData[index].state = EnemyState.Idle;
		}


		private static void EnemyHorizontalMove(int index, bool direction, Vector pos, Vector size,
			int speed, bool wait)
		{

			if (!lstEnemyData[index].valueSetComp)
			{
				EnemyBehaviorValuesSetup(index);
			}

			if (lstEnemyData[index].totalDistance.X < lstEnemyData[index].targetDistance.X)
			{

				if (!direction)
				{
					if (!BlockCheck.BlockCheckLeft(pos.X, pos.Y, (int)size.Y, speed) &&
						pos.X > 0)
					{
						if (pos.X - SystemOperator.PixelPerSecond(speed) > 0)
						{
							pos.X -= SystemOperator.PixelPerSecond(speed);
						}
						else
						{
							lstEnemyData[index].targetDistance = new Vector(0, lstEnemyData[index].targetDistance.Y);
							lstEnemyData[index].direction = true;
						}

					}

				}
				else
				{

					if (!BlockCheck.BlockCheckRight(pos.X, pos.Y, (int)size.X, (int)size.Y, speed) &&
						pos.X + size.X < 1024 - lstEnemyData[index].size.X)
					{

						if (pos.X + SystemOperator.PixelPerSecond(speed) < 992)
						{
							pos.X += SystemOperator.PixelPerSecond(speed);
						}
						else
						{
							lstEnemyData[index].targetDistance = new Vector(0, lstEnemyData[index].targetDistance.Y);
							lstEnemyData[index].direction = false;
						}

					}

				}

				lstEnemyData[index].position.X = pos.X;
				lstEnemyData[index].totalDistance.X += SystemOperator.PixelPerSecond(speed);
			}
			else
			{
				if (wait)
				{
					lstEnemyData[index].isWaiting = true;
					lstEnemyData[index].totalWaitTime = 0;
					lstEnemyData[index].targetWaitTime = 2000;

				}

				lstEnemyData[index].targetDistance = new Vector(0, 0);
				lstEnemyData[index].totalDistance = new Vector(0, 0);
				lstEnemyData[index].valueSetComp = false;
			}

		}

		private static void EnemyWaiting(int index)
		{
			if (lstEnemyData[index].totalWaitTime < lstEnemyData[index].targetWaitTime)
			{
				lstEnemyData[index].totalWaitTime += MainWindow.elapsedTime;
			}
			else
			{
				lstEnemyData[index].isWaiting = false;
				lstEnemyData[index].perceive = false;

				if (lstEnemyData[index].state == EnemyState.Death)
				{
					lstEnemyData[index].isDeath = true;
				}
			}
		}

		public void EnemyJumping(int index, bool direction, Vector pos, int width, int jumpPower, Vector target)
		{

			if (!BlockCheck.BlockCheckTop(pos.X, pos.Y, width, jumpPower))
			{
				if (pos.Y - SystemOperator.PixelPerSecond(jumpPower) > 0)
				{
					lstEnemyData[index].jumpCount++;

					lstEnemyData[index].isJumping = true;
				}
			}


			if (lstEnemyData[index].isJumping)
			{
				if (lstEnemyData[index].jumpTotalLength < lstEnemyData[index].jumpMaxHeight)
				{
					lstEnemyData[index].position.Y -= SystemOperator.PixelPerSecond(jumpPower);
					lstEnemyData[index].jumpTotalLength += SystemOperator.PixelPerSecond(jumpPower);
				}
				else
				{
					lstEnemyData[index].isJumping = false;
					lstEnemyData[index].jumpTotalLength = 0;
				}
			}

		}

		private static void EnemyFalling(int index, bool direction, Vector pos, Vector size, int speed, Vector target)
		{

			if (!lstEnemyData[index].isLadder)
			{
				if ((!BlockCheck.BlockCheckBottom(pos.X, pos.Y, (int)size.X, (int)size.Y, lstEnemyData[index].weight)))
				{

					lstEnemyData[index].position.Y += SystemOperator.PixelPerSecond(lstEnemyData[index].weight);

					if (!lstEnemyData[index].isFalling)
					{
						lstEnemyData[index].fallingStartPoint = pos.Y;
					}

					lstEnemyData[index].isFalling = true;

				}
				else
				{
					if (lstEnemyData[index].isFalling)
					{
						int block = 0;

						block = (int)(pos.Y - lstEnemyData[index].fallingStartPoint) / 32;

						if (block > lstEnemyData[index].fallingEndure)
						{
							Sound.SoundEffectSelector(SeName.Shock);
							Sound.SoundEffectPlayer(SeName.Shock);

							lstEnemyData[index].life -= 1;

						}
					}

					lstEnemyData[index].isFalling = false;
					lstEnemyData[index].jumpCount = 0;
				}

			}

		}

		public static void EnemyInvincibleTimeCount(int index)
		{
			if (lstEnemyData[index].totalInvincibleTime < 1000)
			{
				lstEnemyData[index].totalInvincibleTime += MainWindow.elapsedTime;
				lstEnemyData[index].imgEnemy.Opacity = 0.5;
			}
			else
			{
				lstEnemyData[index].imgEnemy.Opacity = 1;
				lstEnemyData[index].isDamage = false;
			}
		}

		public static bool EnemyTriggerCollisionCheck(int index)
		{

			Vector p1 = new Vector(PlayerStatus.playerPos.X, PlayerStatus.playerPos.Y);
			Vector size1 = new Vector(PlayerStatus.playerSize.X, PlayerStatus.playerSize.Y);

			Vector p2 = lstEnemyData[index].triggerAreaPos;

			Vector size2 = new Vector(lstEnemyData[index].triggerAreaOffset.X + lstEnemyData[index].triggerAreaSize.X * 32, lstEnemyData[index].triggerAreaSize.Y * 32);

			if (!lstEnemyData[index].direction)
			{
				if (CollisionCheck.Collision(p1, p2, size1, size2))
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
				size2.X += lstEnemyData[index].size.X;

				if (CollisionCheck.Collision(p1, p2, size1, size2))
				{
					return true;
				}
				else
				{
					return false;
				}
			}


		}
	}


	public class SpawnEnemy
	{

		public static List<EnemyData> lstEnemyData = new List<EnemyData>();


		public static int DatabaseEnemyNameSearch(int target)
		{
			int index;

			for (int i = 0; i < StageData.lstDbEnemy.Count; i++)
			{
				if (lstEnemyData[target].name == StageData.lstDbEnemy[i].name)
				{
					return index = i;
				}
			}

			return index = -1;
		}

		public static void SpawnSelect(Canvas canvas, string name)
		{

			int seed = Environment.TickCount;

			Random rnd = new Random(seed++);


			rnd = new Random(seed++);

			int ranDir = rnd.Next(0, 2);
			int ranPosX;

			Console.WriteLine(ranDir);
			rnd = new Random(seed++);

			if (ranDir == 1)
			{
				ranPosX = rnd.Next(200, 400);
			}
			else
			{
				ranPosX = rnd.Next(600, 800);
			}

			Vector pos = new Vector(ranPosX, 671);

			GenerateEnemy(canvas, 1, pos);


		}

		//Generaters
		public static void GenerateEnemy(Canvas canvas, int enemyid, Vector setpos)
		{

			lstEnemyData.Add(new EnemyData
			{
				name = StageData.lstDbEnemy[enemyid].name,
				imgEnemy = new Image
				{
					Source = ImageData.ImageSourceSelector(CategoryName.Enemy, StageData.lstDbEnemy[enemyid].spriteIdleL),
					Width = StageData.lstDbEnemy[enemyid].size.X,
					Height = StageData.lstDbEnemy[enemyid].size.Y,
				},
				spriteIdleL = StageData.lstDbEnemy[enemyid].spriteIdleL,
				spriteIdleR = StageData.lstDbEnemy[enemyid].spriteIdleR,
				spriteSpawnL = StageData.lstDbEnemy[enemyid].spriteSpawnL,
				spriteSpawnR = StageData.lstDbEnemy[enemyid].spriteSpawnR,
				spriteMoveL = StageData.lstDbEnemy[enemyid].spriteMoveL,
				spriteMoveR = StageData.lstDbEnemy[enemyid].spriteMoveR,
				spriteAttackL = StageData.lstDbEnemy[enemyid].spriteAttackL,
				spriteAttackR = StageData.lstDbEnemy[enemyid].spriteAttackR,
				spriteDamageL = StageData.lstDbEnemy[enemyid].spriteDamageL,
				spriteDamageR = StageData.lstDbEnemy[enemyid].spriteDamageR,
				spriteDeathL = StageData.lstDbEnemy[enemyid].spriteDeathL,
				spriteDeathR = StageData.lstDbEnemy[enemyid].spriteDeathR,

				position = setpos,
				size = StageData.lstDbEnemy[enemyid].size,


				life = StageData.lstDbEnemy[enemyid].life,
				ofepower = StageData.lstDbEnemy[enemyid].offense,
				speed = StageData.lstDbEnemy[enemyid].speed,
				weight = StageData.lstDbEnemy[enemyid].weight,
				jumpMaxHeight = StageData.lstDbEnemy[enemyid].jumpMaxHeight,

				triggerAreaOffset = StageData.lstDbEnemy[enemyid].triggerAreaOffset,
				triggerAreaPos = StageData.lstDbEnemy[enemyid].triggerAreaPos,
				triggerAreaSize = StageData.lstDbEnemy[enemyid].triggerAreaSize,

				floating = StageData.lstDbEnemy[enemyid].floating,
				slipThrogh = StageData.lstDbEnemy[enemyid].slipThrough,
				useLadder = StageData.lstDbEnemy[enemyid].useLadder,
				dropItem = StageData.lstDbEnemy[enemyid].dropItem,
				dropItemId = StageData.lstDbEnemy[enemyid].dropItemID
			});

			int index = lstEnemyData.Count - 1;

			canvas.Children.Add(lstEnemyData[index].imgEnemy);

			Canvas.SetLeft(lstEnemyData[index].imgEnemy, lstEnemyData[index].position.X);
			Canvas.SetTop(lstEnemyData[index].imgEnemy, lstEnemyData[index].position.Y);
			Canvas.SetZIndex(SpawnEnemy.lstEnemyData[index].imgEnemy, ImageZindex.enemy);
		}

		public static void RemoveEnemy(Canvas canvas)
		{
			for (int i = 0; i < lstEnemyData.Count; i++)
			{
				if (lstEnemyData[i].isDeath)
				{
					if (i == 0)
					{
						GameTransition.growthEnemy = true;

					}

					canvas.Children.Remove(lstEnemyData[i].imgEnemy);
					lstEnemyData.RemoveAt(i);
					StageManager.numKillEnemy++;
					Console.WriteLine(StageManager.numKillEnemy);
				}

			}

		}

		public static void EnemyTriggerAreaSetting(int index)
		{

			if (!lstEnemyData[index].direction)
			{
				lstEnemyData[index].triggerAreaPos.X =
					lstEnemyData[index].position.X - (lstEnemyData[index].triggerAreaSize.X * 32 + lstEnemyData[index].triggerAreaOffset.X);

			}
			else
			{

				lstEnemyData[index].triggerAreaPos.X = lstEnemyData[index].position.X + lstEnemyData[index].size.X;

			}

			lstEnemyData[index].triggerAreaPos.Y = lstEnemyData[index].position.Y;

			CollisionCheck.ColliderCheckMaskSetting(lstEnemyData[0].triggerAreaPos.X, lstEnemyData[0].triggerAreaPos.Y,
				lstEnemyData[index].triggerAreaOffset.X + lstEnemyData[index].triggerAreaSize.X * 32,
				lstEnemyData[index].triggerAreaOffset.Y + lstEnemyData[index].triggerAreaSize.Y * 32);
		}

		public static void EnemyDeathItemDrop(Canvas canvas, Vector position)
		{
			
		}

		public static void ReSpawnEnemy(Canvas canvas)
		{
			if (StageManager.respawnEnemy && lstEnemyData.Count == 0)
			{
				SystemOperator.randomNum = new Random(SystemOperator.timeSeed++);

				Vector setpos = new Vector(SystemOperator.randomNum.Next(96, 672), SystemOperator.randomNum.Next(448, 704));
				GenerateEnemy(canvas, 1, setpos);

				setpos = new Vector(SystemOperator.randomNum.Next(96, 672), SystemOperator.randomNum.Next(448, 704));
				GenerateEnemy(canvas, 1, setpos);

			}
		}
	}
}
