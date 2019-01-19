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
using Zitulmyth.Enums;
using static Zitulmyth.StageEditorWindow;

namespace Zitulmyth
{

	public enum SystemTargetName
	{
		Player,
		Enemy,
		Item,
		Ui,
	}


	public class SystemOperator
	{

		public static int timeSeed = Environment.TickCount;
		public static Random randomNum;

		public static double moveCommonAmountX;
		public static double moveCommonAmountY;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>

		public static double BlockPerSecond()
		{
			double temp;
			temp = Math.Round(32 * (double)MainWindow.elapsedTime / 500, 2, MidpointRounding.AwayFromZero);

			return temp;

		}
		

		public static void BoundObject(ref Vector pos ,bool charaDir,ref Vector total, Vector target , ref Vector bps,
										ref double coefficient, ref bool boundDir,int weight,int speed,int jumppower,
										Vector size,ref bool isKnockBack)
		{

			double addY = 0;
			double radian = 0;

			

			bps.X = target.X / 32 * BlockPerSecond() * 2;
			bps.Y = target.Y * bps.X / target.X;

			addY = coefficient * weight / (target.X / 2)*0.102;

			radian = Math.Atan(bps.Y - addY / bps.X) * Math.PI / 180;

			if (!boundDir)
			{

				if (total.Y < target.Y)
				{

					if (!charaDir)
					{
						if (!BlockCheck.BlockCheckLeft(pos.X - bps.X, pos.Y,(int)size.Y, speed) &&
							pos.X - bps.X > 0)
						{
							pos.X -= bps.X;
						}

					}
					else
					{
						if (!BlockCheck.BlockCheckRight(pos.X + bps.X, pos.Y, (int)size.X, (int)size.Y, speed) &&
							pos.X + bps.X < 1024 - (int)size.X)
						{
							pos.X += bps.X;
						}
					}

					if (!BlockCheck.BlockCheckTop(pos.X, pos.Y - bps.Y, (int)size.X,jumppower) &&
						pos.Y - bps.Y > 0)
					{
						pos.Y -= bps.Y;
					}



					total.X += Math.Sqrt(Math.Pow(bps.X, 2));
					total.Y += Math.Sqrt(Math.Pow(bps.Y, 2));

					bps.X += bps.X;
					bps.Y += bps.Y;

					coefficient++;

				}
				else
				{
					boundDir = true;
					total.Y = target.Y;
				}
			}
			else
			{

				if (total.Y > 0)
				{

					if (!charaDir)
					{
						if (!BlockCheck.BlockCheckLeft(pos.X - bps.X, pos.Y, (int)size.Y, speed) &&
							pos.X - bps.X > 0)
						{
							pos.X -= bps.X;
						}

					}
					else
					{
						if (!BlockCheck.BlockCheckRight(pos.X + bps.X, pos.Y, (int)size.X, (int)size.Y ,speed) &&
							pos.X + bps.X < 1024 - (int)size.X)
						{
							pos.X += bps.X;
						}
					}

					if (!BlockCheck.BlockCheckBottom(pos.X, pos.Y + bps.Y , (int)size.X, (int)size.Y,weight) &&
						pos.Y + bps.Y < 768 - (int)size.Y)
					{
						pos.Y += bps.Y;
					}

					total.X += Math.Sqrt(Math.Pow(bps.X, 2));
					total.Y -= Math.Sqrt(Math.Pow(bps.Y, 2));

					bps.X += bps.X;
					bps.Y += bps.Y;

					coefficient--;

				}
				else
				{
					boundDir = false;
					isKnockBack = false;
				}
			}
		}

		public static bool FaceEachOther(double posX ,double attackerX)
		{
			return (attackerX < posX) ? true : false;
		}

		public static Vector FromCodeToBlocks(Point point)
		{
			Vector blockPos = new Vector();

			blockPos.X = (int)Math.Ceiling(point.X / 32);
			blockPos.Y = (int)Math.Ceiling(point.Y / 32);

			if (blockPos.X == 0) blockPos.X = 1;
			if (blockPos.Y == 0) blockPos.Y = 1;
			if (blockPos.X >= 32) blockPos.X = 32;
			if (blockPos.Y >= 24) blockPos.Y = 24;

			return blockPos ;
		}
		
		public static void EditorObjectDataListConverter(int index ,bool target)
		{	//target => false:removeAt  true:lastadd
			StageEditorOperator.lstObjectDataConvert.Clear();

			for(int i=0;i < stageEditorData.objectName.Length; i++)
			{
				StageEditorOperator.lstObjectDataConvert.Add(new EditorObjectDataListConvert {
					objectName = stageEditorData.objectName[i],
					objectPosition = stageEditorData.objectPosition[i],
					objectSize = stageEditorData.objectSize[i],
					objectZindex = stageEditorData.objectZindex[i],
					objectToggleSwitch = stageEditorData.objectToggleSwitch[i],
					objectTargetType = stageEditorData.objectTargetType[i],
					objectTargetId = stageEditorData.objectTalkID[i],
					objectTalkID = stageEditorData.objectTalkID[i],
				});
			}

			if (!target)
			{
				StageEditorOperator.lstObjectDataConvert.RemoveAt(index);
			}
			else
			{
				StageEditorOperator.lstObjectDataConvert.Add(new EditorObjectDataListConvert { });
			}
		
			int arynum = StageEditorOperator.lstObjectDataConvert.Count;

			stageEditorData.objectName = new string[arynum];
			stageEditorData.objectPosition = new Vector[arynum];
			stageEditorData.objectSize = new Vector[arynum];
			stageEditorData.objectZindex = new int[arynum];
			stageEditorData.objectToggleSwitch = new bool[arynum];
			stageEditorData.objectTargetType = new TargetType[arynum];
			stageEditorData.objectTargetId = new int[arynum];
			stageEditorData.objectTalkID = new int[arynum];

			for (int i = 0; i < stageEditorData.objectName.Length; i++)
			{
				stageEditorData.objectName[i] = StageEditorOperator.lstObjectDataConvert[i].objectName;
				stageEditorData.objectPosition[i] = StageEditorOperator.lstObjectDataConvert[i].objectPosition;
				stageEditorData.objectSize[i] = StageEditorOperator.lstObjectDataConvert[i].objectSize;
				stageEditorData.objectZindex[i] = StageEditorOperator.lstObjectDataConvert[i].objectZindex;
				stageEditorData.objectToggleSwitch[i] = StageEditorOperator.lstObjectDataConvert[i].objectToggleSwitch;
				stageEditorData.objectTargetType[i] = StageEditorOperator.lstObjectDataConvert[i].objectTargetType;
				stageEditorData.objectTargetId[i] = StageEditorOperator.lstObjectDataConvert[i].objectTargetId;
				stageEditorData.objectTalkID[i] = StageEditorOperator.lstObjectDataConvert[i].objectTalkID;
			}

		}

		public static void EditorEnemyDataListConverter(int index, bool target)
		{   //target => false:removeAt  true:lastadd
			StageEditorOperator.lstEnemyDataConvert.Clear();

			for (int i = 0; i < stageEditorData.enemyName.Length; i++)
			{
				StageEditorOperator.lstEnemyDataConvert.Add(new EditorEnemyDataListConvert
				{
					enemyName = stageEditorData.enemyName[i],
					enemyPosition = stageEditorData.enemyPosition[i],
					enemyDirection = stageEditorData.enemyDirection[i],

				});
			}

			if (!target)
			{
				StageEditorOperator.lstEnemyDataConvert.RemoveAt(index);
			}
			else
			{
				StageEditorOperator.lstEnemyDataConvert.Add(new EditorEnemyDataListConvert { });
			}

			int arynum = StageEditorOperator.lstEnemyDataConvert.Count;

			stageEditorData.enemyName = new EnemyName[arynum];
			stageEditorData.enemyPosition = new Vector[arynum];
			stageEditorData.enemyDirection = new bool[arynum];

			for (int i = 0; i < stageEditorData.enemyName.Length; i++)
			{
				stageEditorData.enemyName[i] = StageEditorOperator.lstEnemyDataConvert[i].enemyName;
				stageEditorData.enemyPosition[i] = StageEditorOperator.lstEnemyDataConvert[i].enemyPosition;
				stageEditorData.enemyDirection[i] = StageEditorOperator.lstEnemyDataConvert[i].enemyDirection;
			}

		}


		public static void EditorItemDataListConverter(int index, bool target)
		{   //target => false:removeAt  true:lastadd
			StageEditorOperator.lstItemDataConvert.Clear();

			for (int i = 0; i < stageEditorData.itemName.Length; i++)
			{
				StageEditorOperator.lstItemDataConvert.Add(new EditorItemDataListConvert
				{
					itemName = stageEditorData.itemName[i],
					itemPosition = stageEditorData.itemPosition[i],

				});
			}

			if (!target)
			{
				StageEditorOperator.lstItemDataConvert.RemoveAt(index);
			}
			else
			{
				StageEditorOperator.lstItemDataConvert.Add(new EditorItemDataListConvert { });
			}

			int arynum = StageEditorOperator.lstItemDataConvert.Count;

			stageEditorData.itemName = new ItemName[arynum];
			stageEditorData.itemPosition = new Vector[arynum];

			for (int i = 0; i < stageEditorData.itemName.Length; i++)
			{
				stageEditorData.itemName[i] = StageEditorOperator.lstItemDataConvert[i].itemName;
				stageEditorData.itemPosition[i] = StageEditorOperator.lstItemDataConvert[i].itemPosition;
			}

		}

		public static bool IsNumeric(string stTarget)
		{
			double dNullable;

			return double.TryParse(
				stTarget,
				System.Globalization.NumberStyles.Any,
				null,
				out dNullable
			);
		}
	}
}
