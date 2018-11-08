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

namespace Zitulmyth
{

	public enum TalkType
	{
		Normal,
		Selection,
		SetFlag,
	}

	public class TalkData
	{
		public int branchID;
		public TalkType type;
		public string message;
		public bool speaker; //false:player,true:other
		public int[] destination;
		public bool branchEnd;
		public int talkFlagID;
	}

	public class TalkCommander
	{
		public static List<TalkData> lstTalkMessage = new List<TalkData>();
		public static bool isTalk;
		public static bool isTalkOpenBalloon;
		public static bool isTalkSelecting;
		private static int selectCount;
		public static int talkNumCount;
		private static int[] memoryDestinatiion;
		private static int selectBranch;
		public static int memoryTalkFlagID;

		public static void TalkDataInit()
		{
			switch (ObjectChecker.lstObject[ObjectChecker.activeObject].talkID)
			{
				case 0:

					lstTalkMessage.Add(new TalkData { branchID = 0, type = TalkType.Normal,speaker = true,
						message = "よく来たな、ヴトルプ。\n今日も遊びに来たのか？"});
					lstTalkMessage.Add(new TalkData { branchID = 0, type = TalkType.Normal, speaker = false,
						message = "うん。\nちょっと気晴らしにね。" });
					lstTalkMessage.Add(new TalkData { branchID = 0, type = TalkType.Normal, speaker = true,
						message = "そうか。\n元気な事はいいが、\n高い所は気を付けるのだ。" });
					lstTalkMessage.Add(new TalkData { branchID = 0, type = TalkType.Normal, speaker = true,
						message = "いくら身軽なお前とて、\n怪我をするやもしれん。" });
					lstTalkMessage.Add(new TalkData { branchID = 0, type = TalkType.Selection, speaker = false,destination = new int[] { 5, 6 },
						message = "・大丈夫だよ。\n・気をつけるよ。" });
					lstTalkMessage.Add(new TalkData { branchID = 5, type = TalkType.Normal, speaker = true,branchEnd = true,
						message = "まあ、そうだな。\nもう心配する程でもないか。" });
					lstTalkMessage.Add(new TalkData { branchID = 6, type = TalkType.Normal, speaker = true,branchEnd = true,
						message = "うむ。\n常に状況をよく見て\n行動するのだ。" });

					break;

				case 1:

					lstTalkMessage.Add(new TalkData { branchID = 0, type = TalkType.Normal, speaker = false,
						message = "どうしようかな" });
					lstTalkMessage.Add(new TalkData { branchID = 0, type = TalkType.Selection, speaker = false,destination = new int[] {1,2},
						message = "・家に帰る\n・まだ遊ぶ" });
					lstTalkMessage.Add(new TalkData { branchID = 1, type = TalkType.SetFlag, speaker = false, talkFlagID = 2311 });
					lstTalkMessage.Add(new TalkData { branchID = 2, type = TalkType.SetFlag, speaker = false, talkFlagID = 0});

					break;

			}

			

			talkNumCount = 0;
		}

		public static void TalkWithNpc(Canvas canvas)
		{
			if(talkNumCount < lstTalkMessage.Count)
			{

				Vector pos = new Vector(0, 0);
				Image target = new Image();

				if(lstTalkMessage[talkNumCount].branchID == selectBranch)
				{

					if (!lstTalkMessage[talkNumCount].speaker)
					{
						pos = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
					}
					else
					{

						ObjectChecker.GetTriggerTargetPosition(ObjectChecker.activeObject);
						pos = ObjectChecker.triggerTargetPosition;

					}


					switch (lstTalkMessage[talkNumCount].type)
					{
						case TalkType.Normal:

							BalloonMessage.OpenBalloon(0, canvas, pos, target, lstTalkMessage[talkNumCount].message, true);
							isTalkSelecting = false;
							ImageData.imgHandCursor.Visibility = Visibility.Hidden;
							isTalkOpenBalloon = true;
							break;

						case TalkType.Selection:

							BalloonMessage.OpenBalloon(0, canvas, pos, target, lstTalkMessage[talkNumCount].message, true);
							isTalkSelecting = true;
							ImageData.imgHandCursor.Visibility = Visibility.Visible;
							Canvas.SetLeft(ImageData.imgHandCursor, Canvas.GetLeft(BalloonMessage.spnBalloon) - 20);
							Canvas.SetTop(ImageData.imgHandCursor, Canvas.GetTop(BalloonMessage.spnBalloon));
							isTalkOpenBalloon = true;
							break;

						case TalkType.SetFlag:
							
							ImageData.imgHandCursor.Visibility = Visibility.Hidden;
							memoryTalkFlagID = lstTalkMessage[talkNumCount].talkFlagID;

							talkNumCount++;

							break;
					}

					
				}
				else
				{
					talkNumCount++;
				}

				

			}
			else
			{
				talkNumCount = 0;
				selectBranch = 0;
				lstTalkMessage.Clear();

				ObjectChecker.oldActiveObject = ObjectChecker.activeObject;
				isTalk = false;
				KeyController.keyControlLocking = false;

			}


		}

		public static void TalkSelecting(Canvas canvas)
		{
			if (!KeyController.keyReturnInterval) {

				if (KeyController.keyUp)
				{
					if (selectCount == 0)
					{
						selectCount = lstTalkMessage[talkNumCount].destination.Length - 1;
					}
					else
					{
						selectCount--;
					}

					KeyController.keyReturnInterval = true;

				}

				if (KeyController.keyDown)
				{
					if (selectCount == lstTalkMessage[talkNumCount].destination.Length - 1)
					{
						selectCount = 0;
					}
					else
					{
						selectCount++;
					}

					KeyController.keyReturnInterval = true;
				}

			}

			Canvas.SetTop(ImageData.imgHandCursor,Canvas.GetTop(BalloonMessage.spnBalloon) + selectCount * 32);
		}

		public static void TalkEnterTheNext()
		{
			if(lstTalkMessage[talkNumCount].type == TalkType.Selection)
			{

				talkNumCount = selectBranch = lstTalkMessage[talkNumCount].destination[selectCount];
				memoryDestinatiion = lstTalkMessage[talkNumCount].destination;

				selectCount = 0;
				isTalkSelecting = false;

			}
			else
			{
				if (lstTalkMessage[talkNumCount].branchEnd)
				{
					selectBranch = 0;
				}

				talkNumCount++;
			}

			BalloonMessage.spnBalloon.Visibility = Visibility.Hidden;
			BalloonMessage.txtBalloon.Text = "";
			TalkCommander.isTalkOpenBalloon = false;

			KeyController.keyReturnInterval = true;

		}

		public static bool BranchCheck()
		{

			if(lstTalkMessage[talkNumCount].branchID == selectBranch)
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
