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
	public class EnemyBehavior
	{
		private int enemyJumpCount = 0;
		private int enemyJumpPower = 8;
		private bool enemyJumping = false;
		private int jumpTotalLength = 0;
		private int jumpMaxHeight = 64;

		private void MoveEnemy()
		{
			double posX = Canvas.GetLeft(EnemyData.lstSpawnEnemy[0].imgEnemy);
			double posY = Canvas.GetTop(EnemyData.lstSpawnEnemy[0].imgEnemy);

			if (enemyJumpCount == 0)
			{
				if (BlockCheck.BlockCheckTop(posX, posY, enemyJumpPower))
				{
					if (posY - enemyJumpPower > 0)
					{
						enemyJumpCount++;

						enemyJumping = true;

					}
				}
			}

			if (enemyJumping)
			{
				if (jumpTotalLength < jumpMaxHeight)
				{
					posY -= enemyJumpPower;
					jumpTotalLength += enemyJumpPower;
				}
				else
				{
					enemyJumping = false;
					jumpTotalLength = 0;
				}
			}

			Canvas.SetLeft(EnemyData.lstSpawnEnemy[0].imgEnemy, posX);
			Canvas.SetTop(EnemyData.lstSpawnEnemy[0].imgEnemy, posY);
		}

		private void FallingEnemy()
		{

			double posX = Canvas.GetLeft(EnemyData.lstSpawnEnemy[0].imgEnemy);
			double posY = Canvas.GetTop(EnemyData.lstSpawnEnemy[0].imgEnemy);


			if (BlockCheck.BlockCheckBottom(posX, posY, PlayerStatus.weight))
			{
				if (posY + 32 < 23 * 32)
				{
					posY += PlayerStatus.weight;
				}

			}

			Canvas.SetTop(EnemyData.lstSpawnEnemy[0].imgEnemy, posY);
		}

	}


	public class SpawnEnemy
	{
		
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
			var _imgEnemy = new Image()
			{
				Source = ImageData.cbEnemy[1],
				Width = 32,
				Height = 64,
			};

			var enemy = new SpawnEnemyList
			{
				enemyName = EnemyName.Zigytu01,
				enemyHp = 1,
				enemySize = new Vector(32, 64),
				enemyStartPos = setpos,
				enemyOfePower = 0,
				enemyDefPower = 0,
				enemySpeed = 0,
				enemyWeight = 1,
				imgEnemy = _imgEnemy,
				deathEffect = EnemyDeathEffect.Pop,
			};

			EnemyData.lstSpawnEnemy.Add(enemy);

			int index = EnemyData.lstSpawnEnemy.Count - 1;

			canvas.Children.Add(EnemyData.lstSpawnEnemy[index].imgEnemy);

			Canvas.SetLeft(EnemyData.lstSpawnEnemy[index].imgEnemy, EnemyData.lstSpawnEnemy[index].enemyStartPos.X);
			Canvas.SetTop(EnemyData.lstSpawnEnemy[index].imgEnemy, EnemyData.lstSpawnEnemy[index].enemyStartPos.Y);
		}
	}
}
