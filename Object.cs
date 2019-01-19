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
		public Image imgObject;
		public Vector position;
		public Vector size;
		public int zindex;
		public bool toggleSwitch;
		public int totalSwitchTime;
		public int totalExpirationTime;
		public int nowDurability;
		public Vector totalDistance;
		public int keyFlame;
		public int totalAnimTime;
		public TargetType targetType;
		public int targetId;
		public int talkID;
		
	}

	public class ObjectChecker
	{
		public static List<ObjectData> lstObject = new List<ObjectData>();
		public static int activeObject; //store index
		public static int oldActiveObject;
		public static bool isTrigger;

		public static Vector triggerTargetPosition;

		public static int DatabaseObjectNameSearch(int target)
		{
			int index;

			for(int i = 0; i < StageData.lstDbObject.Count; i++)
			{
				if(lstObject[target].objName == StageData.lstDbObject[i].name)
				{
					return index = i;
				}
			}

			return index = -1;
		}

		public static void CollisionPtoActionCollider()
		{
			if (!isTrigger)
			{
				for (int i = 0; i < lstObject.Count; i++)
				{

					Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
					Vector size1 = new Vector(PlayerStatus.playerSize.X, PlayerStatus.playerSize.Y);

					Vector p2 = new Vector(Canvas.GetLeft(lstObject[i].imgObject), Canvas.GetTop(lstObject[i].imgObject));
					Vector size2 = new Vector(lstObject[i].size.X, lstObject[i].size.Y);

					bool triggeraction = false;

					if (DatabaseObjectNameSearch(i) >= 0)
					{
						triggeraction = StageData.lstDbObject[DatabaseObjectNameSearch(i)].triggerAction;
					}

					if (CollisionCheck.Collision(p1, p2, size1, size2) && triggeraction)
					{

						GetTriggerTargetPosition(i);

						activeObject = i;

						isTrigger = true;

						Canvas.SetLeft(ImageData.imgPopCanTalk, triggerTargetPosition.X - 16);
						Canvas.SetTop(ImageData.imgPopCanTalk, triggerTargetPosition.Y - 32);
						ImageData.imgPopCanTalk.Visibility = Visibility.Visible;

						break;
					}
					else
					{
						ImageData.imgPopCanTalk.Visibility = Visibility.Hidden;
					}

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

			bool triggertype = false;

			if (ObjectChecker.DatabaseObjectNameSearch(ObjectChecker.activeObject) >= 0)
			{
				triggertype = StageData.lstDbObject[ObjectChecker.DatabaseObjectNameSearch(ObjectChecker.activeObject)].triggerType;
			}

			if (!TalkCommander.isTalk && !triggertype)
			{
				if(ObjectChecker.activeObject != ObjectChecker.oldActiveObject)
				{
					TalkCommander.TalkDataInit();
					TalkCommander.isTalk = true;
					KeyController.keyControlLocking = true;
				}
			}
		}

	}
}
