﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zitulmyth.Checking;
using Zitulmyth.Data;
using System.Windows;
using System.Windows.Controls;
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
		public static int timeSeed = Environment.TickCount;
		public static Random randomNum;


		public static void EnemyAction()
		{

			for (int i = 0; i < lstEnemyData.Count; i++)
			{

				if (lstEnemyData[i].isDamage)
				{
					EnemyInvincibleTimeCount(i);
				}
				
				EnemyFalling(i,lstEnemyData[i].direction, lstEnemyData[i].position,
					lstEnemyData[i].widthblock, lstEnemyData[i].heightblock,lstEnemyData[i].speed, lstEnemyData[i].targetDistance);

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
						break;

					case EnemyState.Death:
						break;
				}


				Canvas.SetLeft(lstEnemyData[i].imgEnemy, lstEnemyData[i].position.X);
				Canvas.SetTop(lstEnemyData[i].imgEnemy, lstEnemyData[i].position.Y);
			}

		}

		private static void EnemyHorizontalMove(int index, bool direction,Vector pos,int blockW,int blockH,int speed,Vector target)
		{

			if (lstEnemyData[index].totalDistance.X < target.X )
			{

				if (!direction)
				{
					if (!BlockCheck.BlockCheckLeft(pos.X, pos.Y + blockH * 32, speed))
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

					if (!BlockCheck.BlockCheckRight(pos.X + blockW * 32, pos.Y + blockH * 32, speed))
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
				lstEnemyData[index].totalDistance.X += SystemOperator.BlockPerSecond() * speed; ;
			}
			else
			{
				lstEnemyData[index].targetDistance = new Vector(0, 0);
				lstEnemyData[index].totalDistance = new Vector(0, 0);
				lstEnemyData[index].isWaiting = true;
				lstEnemyData[index].totalWaitTime = 0;
				lstEnemyData[index].targetWaitTime = 2000;
				lstEnemyData[index].isMovingH = false;
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
							Sound.seChannelB.Play();
							lstEnemyData[index].life -= 1;

						}
					}

					lstEnemyData[index].isFalling = false;
					lstEnemyData[index].jumpCount = 0;
				}

			}

		}

		public static void EnemyIdle(int index)
		{
			bool radDir;
			int num;


			if (!lstEnemyData[index].isMovingH)
			{

				randomNum = new Random(timeSeed++);

				num = randomNum.Next(1, 3);
				radDir = (num == 1) ? false : true;

				randomNum = new Random(timeSeed++);
				lstEnemyData[index].direction = radDir;
				lstEnemyData[index].targetDistance = new Vector(randomNum.Next(32, 96), 0);
							

				lstEnemyData[index].isMovingH = true;
			}
			
			switch(lstEnemyData[index].name)
			{
				case EnemyName.Boar:
					EnemyHorizontalMove(index, lstEnemyData[index].direction, lstEnemyData[index].position, 
						lstEnemyData[index].widthblock, lstEnemyData[index].heightblock,
						lstEnemyData[index].speed, lstEnemyData[index].targetDistance);

					break;

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
						name = EnemyName.Boar,
						speed = 2, life = 4, ofepower = 1, defpower = 0, weight = 6,direction = dir,
						state = EnemyState.Spawn,
						pixSize = new Vector(32, 64), position = setpos, triggerArea = new Vector(32, 64), widthblock = 2,heightblock = 1,
						imgEnemy = new Image() { Source = (!dir)?ImageData.cbBoar[1]:ImageData.cbBoar[0], Width = 64, Height = 32},
						deathEvent = EnemyDeathEvent.Pop,
					};

					break;
			}
			
			return enemy;
		}
	}
}
