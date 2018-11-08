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
		Active,
		Death,
	}

	public class EnemyBehavior
	{
		public static void EnemyAction()
		{

			for (int i = 0; i < lstEnemyData.Count; i++)
			{

				EnemyTriggerAreaSetting(i);

				if(lstEnemyData[i].state != EnemyState.Death)
				{
					if (EnemyTriggerCollisionCheck(i))
					{

						if (lstEnemyData[i].state != EnemyState.Active)
						{
							lstEnemyData[i].isValueSetted = false;
						}

						lstEnemyData[i].state = EnemyState.Active;
						
					}
					else
					{
						lstEnemyData[i].state = EnemyState.Idle;
					}

					EnemyBehaviorValuesSetup(i);
				}

				if (lstEnemyData[i].isDamage)
				{
					EnemyInvincibleTimeCount(i);

					if (lstEnemyData[i].isKnockBack)
					{

						SystemOperator.BoundObject(ref lstEnemyData[i].position,SystemOperator.FaceEachOther(lstEnemyData[i].position.X,PlayerStatus.playerPos.X),
							ref lstEnemyData[i].totalDistance,lstEnemyData[i].targetDistance,ref lstEnemyData[i].bps,
							ref lstEnemyData[i].coefficient,ref lstEnemyData[i].boundDirection,
							lstEnemyData[i].weight, lstEnemyData[i].speed, lstEnemyData[i].jumpPower,
							lstEnemyData[i].widthblock, lstEnemyData[i].heightblock, ref lstEnemyData[i].isKnockBack);
					}
				}

				if (!lstEnemyData[i].isKnockBack) {
					EnemyFalling(i, lstEnemyData[i].direction, lstEnemyData[i].position,
									lstEnemyData[i].widthblock, lstEnemyData[i].heightblock,
									lstEnemyData[i].speed, lstEnemyData[i].targetDistance);
				}
				

				switch (lstEnemyData[i].state)
				{
					case EnemyState.Spawn:
						lstEnemyData[i].state = EnemyState.Idle;
						break;

					case EnemyState.Idle:

						if (!lstEnemyData[i].isWaiting)
						{
							EnemyIdle(i);
						}
						else { EnemyWaiting(i); }
						

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


			if (!lstEnemyData[index].isValueSetted)
			{

				SystemOperator.randomNum = new Random(SystemOperator.timeSeed++);

				num = SystemOperator.randomNum.Next(1, 3);
				radDir = (num == 1) ? false : true;

				SystemOperator.randomNum = new Random(SystemOperator.timeSeed++);
				lstEnemyData[index].direction = radDir;
				lstEnemyData[index].acceleration = 1;
				lstEnemyData[index].targetDistance = new Vector(SystemOperator.randomNum.Next(32, 96), 0);


				if(lstEnemyData[index].state == EnemyState.Active)
				{
					SystemOperator.randomNum = new Random(SystemOperator.timeSeed++);
					lstEnemyData[index].acceleration = 2;
					lstEnemyData[index].targetDistance = new Vector(SystemOperator.randomNum.Next(280, 320), 0);
				}

				lstEnemyData[index].isValueSetted = true;
			}

		}

		public static void EnemyIdle(int index)
		{
	

			switch (lstEnemyData[index].name)
			{
				case EnemyName.Boar:
					EnemyHorizontalMove(index, lstEnemyData[index].direction, lstEnemyData[index].position,
										lstEnemyData[index].widthblock, lstEnemyData[index].heightblock,
										lstEnemyData[index].speed, lstEnemyData[index].targetDistance,true);

					break;

			}
		}

		public static void EnemyActiv(int index)
		{
		

			switch (lstEnemyData[index].name)
			{
				case EnemyName.Boar:
					EnemyHorizontalMove(index, lstEnemyData[index].direction, lstEnemyData[index].position,
										lstEnemyData[index].widthblock, lstEnemyData[index].heightblock,
										lstEnemyData[index].speed * lstEnemyData[index].acceleration,
										lstEnemyData[index].targetDistance,false);

					break;

			}
		}


		private static void EnemyHorizontalMove(int index, bool direction,Vector pos,int blockW,int blockH,
			int speed,Vector target,bool wait)
		{

			if (lstEnemyData[index].totalDistance.X < target.X )
			{

				if (!direction)
				{
					if (!BlockCheck.BlockCheckLeft(pos.X, pos.Y + blockH * 32, speed)&&
						pos.X>0)
					{
						if (pos.X - SystemOperator.BlockPerSecond() * speed > 0)
						{
							pos.X -= SystemOperator.BlockPerSecond() * speed;
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

					if (!BlockCheck.BlockCheckRight(pos.X + blockW * 32, pos.Y + blockH * 32, speed)&&
						pos.X + blockW * 32 < 1024 - lstEnemyData[index].widthblock * 32)
					{

						if (pos.X + SystemOperator.BlockPerSecond() * speed < 992)
						{
							pos.X += SystemOperator.BlockPerSecond() * speed;
						}
						else
						{
							lstEnemyData[index].targetDistance = new Vector(0, lstEnemyData[index].targetDistance.Y);
							lstEnemyData[index].direction = false;
						}

					}

				}

				lstEnemyData[index].position.X = pos.X;
				lstEnemyData[index].totalDistance.X += SystemOperator.BlockPerSecond() * speed;
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

				lstEnemyData[index].isValueSetted = false;
			}

		}

		private static void EnemyWaiting(int index)
		{
			if(lstEnemyData[index].totalWaitTime< lstEnemyData[index].targetWaitTime)
			{
				lstEnemyData[index].totalWaitTime += MainWindow.elapsedTime;
			}
			else
			{
				lstEnemyData[index].isWaiting = false;
				lstEnemyData[index].perceive = false;

				if(lstEnemyData[index].state == EnemyState.Death)
				{
					lstEnemyData[index].isDeath = true;
				}
			}
		}

		public void EnemyJumping(int index, bool direction, Vector pos, int blockW, int jumpPower, Vector target)
		{

			if (!BlockCheck.BlockCheckTop(pos.X, pos.Y,jumpPower))
			{
				if (pos.Y - SystemOperator.BlockPerSecond() * jumpPower > 0)
				{
					lstEnemyData[index].jumpCount++;

					lstEnemyData[index].isJumping = true;
				}
			}


			if (lstEnemyData[index].isJumping)
			{
				if (lstEnemyData[index].jumpTotalLength < lstEnemyData[index].jumpMaxHeight)
				{
					lstEnemyData[index].position.Y -= SystemOperator.BlockPerSecond() * jumpPower;
					lstEnemyData[index].jumpTotalLength += SystemOperator.BlockPerSecond() * jumpPower;
				}
				else
				{
					lstEnemyData[index].isJumping = false;
					lstEnemyData[index].jumpTotalLength = 0;
				}
			}

		}

		private static void EnemyFalling(int index, bool direction, Vector pos, int blockW,int blockH, int speed, Vector target)
		{

			if (!lstEnemyData[index].isLadder && !BlockCheck.BlockCheckTopLadder(pos.X, pos.Y+blockH * 32, lstEnemyData[index].weight))
			{
				if ((!BlockCheck.BlockCheckBottom(pos.X, pos.Y + blockH *32, lstEnemyData[index].weight)) &&
					!BlockCheck.BlockCheckOnPlat(pos.X, pos.Y + blockH * 32, lstEnemyData[index].weight))
				{
					
					lstEnemyData[index].position.Y += SystemOperator.BlockPerSecond()*lstEnemyData[index].weight;
					
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
			if(lstEnemyData[index].totalInvincibleTime < 1000)
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

			Vector size2 = new Vector(lstEnemyData[index].triggerAreaOffset.X + lstEnemyData[index].triggerAreaSize.X * 32, lstEnemyData[index].triggerAreaSize.Y* 32);

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
				size2.X += lstEnemyData[index].pixSize.X;

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


		public static void SpawnSelect(Canvas canvas,EnemyName name){

			int seed = Environment.TickCount;

			Random rnd = new Random(seed++);

			switch (name)
			{
				case EnemyName.Zigitu01:

					rnd = new Random(seed++);

					int ranDir = rnd.Next(0,2);
					int ranPosX;

					Console.WriteLine(ranDir);
					rnd = new Random(seed++);

					if (ranDir == 1)
					{
						ranPosX = rnd.Next(200,400);
					}
					else
					{
						ranPosX = rnd.Next(600, 800);
					}

					Vector pos = new Vector(ranPosX, 671);

					GenerateEnemy(canvas,pos);

					break;
			}
		}

		//Generaters
		public static void GenerateEnemy(Canvas canvas, Vector setpos)
		{

			lstEnemyData.Add(SetEnemyData(EnemyName.Zigitu01, setpos,false));

			int index = lstEnemyData.Count - 1;

			canvas.Children.Add(lstEnemyData[index].imgEnemy);

			Canvas.SetLeft(lstEnemyData[index].imgEnemy, lstEnemyData[index].position.X);
			Canvas.SetTop(lstEnemyData[index].imgEnemy, lstEnemyData[index].position.Y);
		}

		public static void RemoveEnemy(Canvas canvas)
		{
			for(int i = 0; i < lstEnemyData.Count; i++)
			{
				if(lstEnemyData[i].isDeath)
				{
					canvas.Children.Remove(lstEnemyData[i].imgEnemy);
					lstEnemyData.RemoveAt(i);
					StageManager.numKillEnemy++;
					Console.WriteLine(StageManager.numKillEnemy);
				}

			}

		}

		public static EnemyData SetEnemyData(EnemyName name, Vector setpos,bool dir)
		{
			var enemy = new EnemyData();
			
			switch (name)
			{
				case EnemyName.Zigitu01:

					enemy = new EnemyData
					{
						name = EnemyName.Zigitu01,
						speed = 4, life = 2, ofepower = 1, defpower = 0, weight = 6, direction = dir,
						state = EnemyState.Spawn,
						pixSize = new Vector(32, 64), position = setpos,
						triggerAreaPos = new Vector(32, 64),triggerAreaSize = new Vector(0,0),triggerAreaOffset = new Vector(0, 0),
						widthblock = 1,heightblock = 2,
						imgEnemy = new Image(){ Source = ImageData.lstCBEnemy[0].lstCBSpawn[0],Width = 32,Height = 64},
						deathEvent = EnemyDeathEvent.Pop,
					};

					break;

				case EnemyName.Boar:

					enemy = new EnemyData
					{
						name = EnemyName.Boar,
						speed = 2, life = 5, ofepower = 1, defpower = 0, weight = 6, direction = dir,
						state = EnemyState.Spawn,
						pixSize = new Vector(64, 32), position = setpos,
						triggerAreaPos = new Vector(64, 32), triggerAreaSize = new Vector(5, 1), triggerAreaOffset = new Vector(0, 0),
						widthblock = 2, heightblock = 1,
						imgEnemy = new Image() { Source = (!dir) ? ImageData.lstCBEnemy[0].lstCBIdle[1] : ImageData.lstCBEnemy[0].lstCBIdle[0], Width = 64, Height = 32 },
						deathEvent = EnemyDeathEvent.Pop,

					};

					break;
			}
			
			return enemy;
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

				lstEnemyData[index].triggerAreaPos.X = lstEnemyData[index].position.X + lstEnemyData[index].pixSize.X;

					//(lstEnemyData[index].position.X + lstEnemyData[index].pixSize.X + lstEnemyData[index].triggerAreaOffset.X) + lstEnemyData[index].triggerAreaSize.X * 32;
			}

			lstEnemyData[index].triggerAreaPos.Y = lstEnemyData[index].position.Y;

			CollisionCheck.ColliderCheckMaskSetting(lstEnemyData[0].triggerAreaPos.X, lstEnemyData[0].triggerAreaPos.Y,
				lstEnemyData[index].triggerAreaOffset.X + lstEnemyData[index].triggerAreaSize.X * 32,
				lstEnemyData[index].triggerAreaOffset.Y + lstEnemyData[index].triggerAreaSize.Y * 32);
		}

		public static void EnemyDeathItemDrop(Canvas canvas,EnemyName name,Vector position)
		{


			switch (name)
			{
				case EnemyName.Boar:

					Item.ItemGenerate(canvas, ItemName.BoarMeat, position);
					break;
				
			}
		}

		public static void ReSpawnEnemy(Canvas canvas)
		{
			if(StageManager.respawnEnemy && lstEnemyData.Count == 0)
			{
				SystemOperator.randomNum = new Random(SystemOperator.timeSeed++);

				Vector setpos = new Vector(SystemOperator.randomNum.Next(96, 672), SystemOperator.randomNum.Next(448, 704));
				lstEnemyData.Add(SetEnemyData(EnemyName.Boar, setpos, true));

				setpos = new Vector(SystemOperator.randomNum.Next(96, 672), SystemOperator.randomNum.Next(448, 704));
				lstEnemyData.Add(SetEnemyData(EnemyName.Boar, setpos, true));

				for (int i = 0; i < SpawnEnemy.lstEnemyData.Count; i++)
				{

					canvas.Children.Add(SpawnEnemy.lstEnemyData[i].imgEnemy);
					Canvas.SetLeft(SpawnEnemy.lstEnemyData[i].imgEnemy, SpawnEnemy.lstEnemyData[i].position.X);
					Canvas.SetTop(SpawnEnemy.lstEnemyData[i].imgEnemy, SpawnEnemy.lstEnemyData[i].position.Y);
					Canvas.SetZIndex(SpawnEnemy.lstEnemyData[i].imgEnemy, 6);

				}
			}
		}
	}
}
