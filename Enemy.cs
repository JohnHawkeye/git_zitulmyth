using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zitulmyth.Checking;
using Zitulmyth.Data;
using System.Windows;
using System.Windows.Controls;

namespace Zitulmyth
{
	public enum EnemyState
	{
		Spawn,
		Idle,
		Active,
		Damage,
		Death,
	}

	public class EnemyBehavior
	{
		private int enemyJumpCount = 0;
		private int enemyJumpPower = 8;
		private bool enemyJumping = false;
		private int jumpTotalLength = 0;
		private int jumpMaxHeight = 64;


		public static void EnemyAction()
		{

			for(int i = 0; i < SpawnEnemy.lstEnemyData.Count; i++)
			{
				switch (SpawnEnemy.lstEnemyData[i].state)
				{
					case EnemyState.Spawn:
						SpawnEnemy.lstEnemyData[i].state = EnemyState.Idle;
						break;

					case EnemyState.Idle:

						break;
					case EnemyState.Active:
						break;
					case EnemyState.Damage:
						break;
					case EnemyState.Death:
						break;
				}


				Canvas.SetLeft(SpawnEnemy.lstEnemyData[i].imgEnemy, SpawnEnemy.lstEnemyData[i].position.X);
				Canvas.SetTop(SpawnEnemy.lstEnemyData[i].imgEnemy, SpawnEnemy.lstEnemyData[i].position.Y);
			}

		}

		private void EnemyHorizontalMove(int index, bool direction,Vector pos,int blockW,int speed,Vector target)
		{
			if (target.X < SpawnEnemy.lstEnemyData[index].totalDistance.X)
			{

				if (!direction)
				{
					if (!BlockCheck.BlockCheckLeft(pos.X, pos.Y, speed))
					{
						if (pos.X - speed > 0)
						{
							pos.X -= speed;
						}
						else
						{
							SpawnEnemy.lstEnemyData[index].targetDistance = new Vector(0, SpawnEnemy.lstEnemyData[index].targetDistance.Y);
							SpawnEnemy.lstEnemyData[index].direction = true;
						}

						SpawnEnemy.lstEnemyData[index].direction = false;
					}

				}
				else
				{

					if (!BlockCheck.BlockCheckRight(pos.X + blockW * 32, pos.Y, speed))
					{

						if (pos.X + speed < 992)
						{
							pos.X += speed;
						}
						else
						{
							SpawnEnemy.lstEnemyData[index].targetDistance = new Vector(0, SpawnEnemy.lstEnemyData[index].targetDistance.Y);
							SpawnEnemy.lstEnemyData[index].direction = false;
						}

						SpawnEnemy.lstEnemyData[index].direction = true;

					}

				}

				SpawnEnemy.lstEnemyData[index].position.X = pos.X;
				SpawnEnemy.lstEnemyData[index].totalDistance.X += pos.X;
			}
			else
			{
				SpawnEnemy.lstEnemyData[index].targetDistance = new Vector(0, SpawnEnemy.lstEnemyData[index].targetDistance.Y);
			}

		}

		public void EnemyJumping(int index, bool direction, Vector pos, int blockW, int speed, Vector target)
		{

			if (!BlockCheck.BlockCheckTop(pos.X, pos.Y, SpawnEnemy.lstEnemyData[index].jumpPower))
			{
				if (pos.Y - SpawnEnemy.lstEnemyData[index].jumpPower > 0)
				{
					SpawnEnemy.lstEnemyData[index].jumpCount++;

					SpawnEnemy.lstEnemyData[index].isJumping = true;
				}
			}


			if (SpawnEnemy.lstEnemyData[index].isJumping)
			{
				if (SpawnEnemy.lstEnemyData[index].jumpTotalLength < SpawnEnemy.lstEnemyData[index].jumpMaxHeight)
				{
					pos.Y -= SpawnEnemy.lstEnemyData[index].jumpPower;

					SpawnEnemy.lstEnemyData[index].jumpTotalLength += SpawnEnemy.lstEnemyData[index].jumpPower;
				}
				else
				{
					SpawnEnemy.lstEnemyData[index].isJumping = false;
					SpawnEnemy.lstEnemyData[index].jumpTotalLength = 0;
				}
			}

		}

		private void EnemyFalling(int index, bool direction, Vector pos, int blockW,int blockH, int speed, Vector target)
		{

			if (!SpawnEnemy.lstEnemyData[index].isLadder && !BlockCheck.BlockCheckTopLadder(pos.X, pos.Y, SpawnEnemy.lstEnemyData[index].weight))
			{
				if ((!BlockCheck.BlockCheckBottom(pos.X, pos.Y, SpawnEnemy.lstEnemyData[index].weight,blockH)) &&
					!BlockCheck.BlockCheckOnPlat(pos.X, pos.Y, SpawnEnemy.lstEnemyData[index].weight, blockH))
				{
					pos.Y += SpawnEnemy.lstEnemyData[index].weight;
					
					if (!PlayerStatus.fallingStart)
					{
						PlayerStatus.fallingStartPoint = pos.Y;
					}
					PlayerStatus.fallingStart = true;

				}
				else
				{
					if (PlayerStatus.fallingStart)
					{
						int block = 0;

						block = (int)(pos.Y - PlayerStatus.fallingStartPoint) / 32;

						if (block > PlayerStatus.fallingEndure)
						{
							Sound.seDamage.Play();
						}
					}

					PlayerStatus.fallingStart = false;
					PlayerStatus.isGround = true;
					PlayerStatus.jumpCount = 0;
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
				case EnemyName.Zigytu01:

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

			lstEnemyData.Add(SetEnemyData(EnemyName.Zigytu01,setpos,false));

			int index = lstEnemyData.Count - 1;

			canvas.Children.Add(lstEnemyData[index].imgEnemy);

			Canvas.SetLeft(lstEnemyData[index].imgEnemy, lstEnemyData[index].position.X);
			Canvas.SetTop(lstEnemyData[index].imgEnemy, lstEnemyData[index].position.Y);
		}

		public static EnemyData SetEnemyData(EnemyName name, Vector setpos,bool dir)
		{
			var enemy = new EnemyData();
			
			switch (name)
			{
				case EnemyName.Zigytu01:

					enemy = new EnemyData
					{
						name = EnemyName.Zigytu01,
						speed = 4, life = 2, ofepower = 1, defpower = 0, weight = 6, direction = dir,
						state = EnemyState.Spawn,
						pixSize = new Vector(32, 64), position = setpos, triggerArea = new Vector(32, 64), widthblock = 1,heightblock = 2,
						imgEnemy = new Image(){ Source = ImageData.cbEnemy[1],Width = 32,Height = 64},
						deathEvent = EnemyDeathEvent.Pop,
					};

					break;

				case EnemyName.Boar:

					enemy = new EnemyData
					{
						name = EnemyName.Zigytu01,
						speed = 4, life = 2, ofepower = 1, defpower = 0, weight = 6,direction = dir,
						state = EnemyState.	Spawn,
						pixSize = new Vector(32, 64), position = setpos, triggerArea = new Vector(32, 64), widthblock = 1,heightblock = 2,
						imgEnemy = new Image() { Source = (!dir)?ImageData.cbBoar[1]:ImageData.cbBoar[0], Width = 64, Height = 32},
						deathEvent = EnemyDeathEvent.Pop,
					};

					break;
			}
			
			return enemy;
		}
	}
}
