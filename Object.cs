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


namespace Zitulmyth
{

	public enum ObjectAttribute
	{
		DisplayOnly = 0,
		Physicality = 1,
		EmptyCollider = 2,
		Ladder = 3,
		Platform = 4,
		Goalgate = 5,
	}

	public class ObjectData
	{
		public string objName;
		public ObjectAttribute objectAttribute;
		public string spriteNameA;
		public string spriteNameB;
		public Image imgObject;
		public Vector position;
		public Vector size;
		public int zindex;

		public bool destructable;
		public int durability;
		public bool damaging;
		public int damageValue;

		public bool toggleSwitch;
		public int targetSwitchTime;
		public int totalSwitchTime;
		public int targetExpirationTime;
		public int totalExpirationTime;

		public bool operable;
		public bool automove;
		public Vector totalDistance;
		public Vector targetDistance;

		public int keyFlame;
		public int totalAnimTime;
		public TargetType targetType;
		public int targetId;
		public bool triggerAction;
		public bool triggerType;
		public int talkID;

		/*
		
		public int autoMoveRangeX { get; set; }
		public int autoMoveRangeY { get; set; }
		public int autoMoveSpeed { get; set; }
		public int influenceSpeed{ get; set; }
		public int influenceJump { get; set; }
		public int influenceFall { get; set; }
		 * 
		 */

	}

	public class ObjectChecker
	{
		public static List<ObjectData> lstObject = new List<ObjectData>();
		public static int activeObject; //store index
		public static int oldActiveObject;
		public static bool isTrigger;
		public static int touchLadderCount;
		public static int onPlatCount;

		public static Vector triggerTargetPosition;

		public static bool obstacleUp;
		public static bool obstacleDown;
		public static bool obstacleLeft;
		public static bool obstacleRight;

		public static int DatabaseObjectNameSearch(int target)
		{
			int index;

			for (int i = 0; i < StageData.lstDbObject.Count; i++)
			{
				if (lstObject[target].objName == StageData.lstDbObject[i].name)
				{
					return index = i;
				}
			}

			return index = -1;
		}

		public static void PlayerOverlappedWithObject()
		{

			for (int i = 0; i < lstObject.Count; i++)
			{

				Vector p1 = PlayerStatus.playerPos;
				Vector size1 = PlayerStatus.playerSize;

				Vector p2 = lstObject[i].position;
				Vector size2 = lstObject[i].size;

				if (CollisionCheck.Collision(p1, p2, size1, size2))
				{
					if (lstObject[i].objectAttribute == ObjectAttribute.Ladder)
					{
						PlayerStatus.isLadder = true;
					}
					else
					{
						PlayerStatus.isLadder = false;
					}
				}

			}
		}

		public static void CollisionPtoActionCollider()
		{

			touchLadderCount = 0;
			onPlatCount = 0;

			for (int i = 0; i < lstObject.Count; i++)
			{

				Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
				Vector size1 = new Vector(PlayerStatus.playerSize.X, PlayerStatus.playerSize.Y);

				Vector p2 = new Vector(Canvas.GetLeft(lstObject[i].imgObject), Canvas.GetTop(lstObject[i].imgObject));
				Vector size2 = new Vector(lstObject[i].size.X, lstObject[i].size.Y);

				switch (lstObject[i].objectAttribute)
				{
					case ObjectAttribute.EmptyCollider:

						if (!isTrigger)
						{

							if (CollisionCheck.Collision(p1, p2, size1, size2))
							{

								if (lstObject[i].triggerAction)
								{
									GetTriggerTargetPosition(i);

									activeObject = i;

									isTrigger = true;

									Canvas.SetLeft(ImageData.imgPopCanTalk, triggerTargetPosition.X - 16);
									Canvas.SetTop(ImageData.imgPopCanTalk, triggerTargetPosition.Y - 32);
									ImageData.imgPopCanTalk.Visibility = Visibility.Visible;
								}
							}
							else
							{
								ImageData.imgPopCanTalk.Visibility = Visibility.Hidden;
							}
						}
						else
						{
							if (!TriggerExitCheck())
							{
								oldActiveObject = 0;
								isTrigger = false;
							}
						}


						break;

					case ObjectAttribute.Physicality:

						Vector move1 = new Vector(0,0);
						Vector move2 = new Vector(0, 0);

						if (!PlayerStatus.playerDirection)
						{
							move1.X = -PlayerStatus.moveSpeed;
						}
						else
						{
							move1.X = PlayerStatus.moveSpeed;
						}

						if (PlayerStatus.jumping)
						{
							move1.Y = -PlayerStatus.jumpPower;
						}
						else
						{
							move1.Y = PlayerStatus.weight;
						}

						if (CollisionCheck.CollisionWithObstacle(p1, p2, size1, size2, move1, move2)){
							if (p1.X + size1.X < p2.X)
							{
								obstacleRight = true;
							}

							if(p1.X > p2.X + size2.X)
							{
								obstacleLeft = true;
							}

							if(p1.Y + size1.Y < p2.Y)
							{
								obstacleDown = true;
							}

							if (p1.Y > p2.Y + size2.Y)
							{
								obstacleUp = true;
							}
						}
						else
						{
							obstacleUp = false;
							obstacleDown = false;
							obstacleLeft = false;
							obstacleRight = false;
						}
						
						break;

					case ObjectAttribute.Ladder:

						if (CollisionCheck.TouchTheLadder(p1, p2, size1, size2))
						{
							PlayerStatus.jumpCount = 0;
							PlayerStatus.fallingStartPoint = PlayerStatus.playerPos.Y;
							touchLadderCount++;
						}

						break;

					case ObjectAttribute.Platform:

						if (CollisionCheck.OnThePlatform(p1, p2, size1, size2))
						{
							PlayerStatus.jumpCount = 0;
							onPlatCount++;
							
						}

						break;
					case ObjectAttribute.Goalgate:

						if (CollisionCheck.TouchGoal(p1, p2, size1, size2))
						{
							if(lstObject[i].toggleSwitch)
								StageManager.clearFlag = true;
						}

						break;
				}

				if(touchLadderCount == 0)
				{
					PlayerStatus.isLadder = false;
				}
				else
				{
					PlayerStatus.isLadder = true;
				}

				if(onPlatCount == 0)
				{
					PlayerStatus.isPlat = false;
				}
				else
				{
					PlayerStatus.isPlat = true;
				}
			}


		}

		private static bool TriggerExitCheck()
		{
			Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
			Vector size1 = new Vector(PlayerStatus.playerSize.X, PlayerStatus.playerSize.Y);

			Vector p2 = new Vector(Canvas.GetLeft(lstObject[activeObject].imgObject),
									Canvas.GetTop(lstObject[activeObject].imgObject));
			Vector size2 = new Vector(lstObject[activeObject].size.X, lstObject[activeObject].size.Y);

			if (CollisionCheck.Collision(p1, p2, size1, size2))
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		public static void GetTriggerTargetPosition(int index)
		{

			if (lstObject[index].targetType == TargetType.Player)
			{
				Vector playerPos = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
				triggerTargetPosition = playerPos;
			}
			else
			{
				triggerTargetPosition = lstObject[lstObject[index].targetId].position;
			}


		}


	}

	public class ObjectBehavior
	{

		public static void OnTriggerReactEvent()
		{
			if (!TalkCommander.isTalk)
			{
				TalkCommander.TalkDataInit();
				TalkCommander.isTalk = true;
				KeyController.keyControlLocking = true;
			}

		}

		public static void OnTriggerTouchEvent()
		{

			if (!TalkCommander.isTalk && !ObjectChecker.lstObject[ObjectChecker.activeObject].triggerType)
			{
				if (ObjectChecker.activeObject != ObjectChecker.oldActiveObject)
				{
					TalkCommander.TalkDataInit();
					TalkCommander.isTalk = true;
					KeyController.keyControlLocking = true;
				}
			}
		}

	}
}
