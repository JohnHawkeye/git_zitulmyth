using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Zitulmyth.Data;

namespace Zitulmyth.Checking
{
	public class BlockCheck
	{

		private static int offSetXa = 0;
		private static int offSetXb = 0;
		private static int offSetY = 0;

		public static void CharacterPositonOffset(double posX, double posY,int valueX,int valueY)
		{
			double x = posX + 16;
			double y = posY;

			x += valueX;
			y += valueY;

			offSetXa = (int)Math.Floor(x / 32);
			offSetXb = (int)Math.Ceiling(x / 32);
			offSetY = (int)(y / 32);

			if (offSetXa < 0) { offSetXa = 0; }
			if (offSetXb < 0) { offSetXb = 0; }
			if (offSetXb > 31) { offSetXb = 31; }
			if (offSetXb > 31) { offSetXb = 31; }
			if (offSetY < 0) { offSetY = 0; }
			if (offSetY > 23) { offSetY = 23; }
		}

		public static bool BlockCheckLeft(double posx, double posy, int speed)
		{

			CharacterPositonOffset(posx, posy, -speed,0);

			if (StageData.indicateStage[offSetY + 2, offSetXa] == BlockType.None ||
				StageData.indicateStage[offSetY + 2, offSetXa] == BlockType.InvisiblePlat ||
				StageData.indicateStage[offSetY + 2, offSetXa] == BlockType.WoodPlatform ||
				StageData.indicateStage[offSetY + 2, offSetXa] == BlockType.LadderMid ||
				StageData.indicateStage[offSetY + 2, offSetXa] == BlockType.LadderBottom)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		public static bool BlockCheckRight(double posx, double posy, int speed)
		{

			CharacterPositonOffset(posx, posy, speed, 0);

			
			if (StageData.indicateStage[offSetY + 2, offSetXb] == BlockType.None ||
				StageData.indicateStage[offSetY + 2, offSetXb] == BlockType.InvisiblePlat||
				StageData.indicateStage[offSetY + 2, offSetXb] == BlockType.WoodPlatform||
				StageData.indicateStage[offSetY + 2, offSetXb] == BlockType.LadderMid||
				StageData.indicateStage[offSetY + 2, offSetXb] == BlockType.LadderBottom)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		public static bool BlockCheckTop(double posx, double posy, int jumppower)
		{

			CharacterPositonOffset(posx, posy, 0, -jumppower);

			if (StageData.indicateStage[offSetY,offSetXa] == BlockType.None ||
				StageData.indicateStage[offSetY, offSetXb] == BlockType.None)
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}

		public static bool BlockCheckBottom(double posx, double posy, int weight)
		{

			CharacterPositonOffset(posx, posy, 0, weight);
			offSetY++;

		
			if (StageData.indicateStage[offSetY + 1, offSetXa] == BlockType.None||
				StageData.indicateStage[offSetY + 1, offSetXb] == BlockType.None)
			{
				return true;
			}
			else
			{
				PlayerStatus.jumpCount = 0;
				return false;
			}

		}

		public static bool BlockCheckLadder(double posx, double posy, int speed)
		{

			CharacterPositonOffset(posx, posy, -speed, 0);

			if (StageData.indicateStage[offSetY + 1, offSetXa] == BlockType.LadderTop ||
				StageData.indicateStage[offSetY + 1, offSetXa] == BlockType.LadderMid ||
				StageData.indicateStage[offSetY + 1, offSetXa] == BlockType.LadderBottom)
			{
				return true;
			}
			else
			{
				return false;
			}
		
		}

		public static bool BlockCheckTopLadder(double posx, double posy, int weight)
		{

			CharacterPositonOffset(posx, posy, 0, weight);
			offSetY++;

			if (StageData.indicateStage[offSetY + 1, offSetXa] == BlockType.LadderTop)
			{
				return true;
			}
			else
			{
				return false;
			}
			
		}

		public static bool BlockCheckGround(double posx, double posy, int weight)
		{
		
			CharacterPositonOffset(posx, posy, 0, weight);
			offSetY++;

			if (StageData.indicateStage[offSetY + 1, offSetXa] == BlockType.GreenGround)
			{
				return true;
			}
			else
			{
				return false;
			}

		}


		public static bool BlockCheckOnPlat(double posx, double posy, int weight)
		{

			CharacterPositonOffset(posx, posy, 0, weight);
			offSetY++;

			if (StageData.indicateStage[offSetY + 1, offSetXa] == BlockType.WoodPlatform ||
				StageData.indicateStage[offSetY + 1, offSetXa] == BlockType.InvisiblePlat)
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
