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
	}

	public class TalkData
	{
		public int branchiID;
		public TalkType type;
		public String message;
		public bool speaker; //false:player,true:other
	}

	public class TalkCommander
	{
		public static List<TalkData> lstTalkMessage = new List<TalkData>();
		public static bool isTalk;
		public static bool isTalkOpenBalloon;
		public static int talkNumCount;

		public static void TalkDataInit()
		{

			lstTalkMessage.Add(new TalkData	{ branchiID = 0, type = TalkType.Normal,speaker = true,
				message = "よく来たな、ヴトルプ。\n今日も遊びに来たのか？"});
			lstTalkMessage.Add(new TalkData { branchiID = 0, type = TalkType.Normal, speaker = false,
				message = "うん。\nちょっと気晴らしにね。" });
			lstTalkMessage.Add(new TalkData { branchiID = 0, type = TalkType.Normal, speaker = true,
				message = "そうか。\n元気な事はいいが、\n高い所は気を付けるのだ。" });
			lstTalkMessage.Add(new TalkData { branchiID = 0, type = TalkType.Normal, speaker = true,
				message = "いくら身軽なお前とて、\n怪我をするやもしれん。" });
			lstTalkMessage.Add(new TalkData { branchiID = 0, type = TalkType.Selection, speaker = false,
				message = "<b1>・大丈夫だよ。<b2>・気をつけるよ。" });
			lstTalkMessage.Add(new TalkData { branchiID = 1, type = TalkType.Normal, speaker = true,
				message = "まあ、そうだな。\nもう心配する程でもないか。" });
			lstTalkMessage.Add(new TalkData { branchiID = 2, type = TalkType.Normal, speaker = true,
				message = "うむ。\n常に状況をよく見て\n行動するのだ。" });

			talkNumCount = 0;
		}

		public static void TalkWithNpc(Canvas canvas)
		{
			if(talkNumCount < lstTalkMessage.Count)
			{
				Vector pos = new Vector(0, 0);
				Image target = new Image();

				if (!lstTalkMessage[talkNumCount].speaker)
				{
					pos = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
				}
				else
				{

					ObjectChecker.GetTriggerTargetPosition(ObjectChecker.activeObject);


					pos = ObjectChecker.triggerTargetPosition;

				}

				//pos.Y -= 32;

				BalloonMessage.OpenBalloon(0, canvas, pos, target, lstTalkMessage[talkNumCount].message,true);

				isTalkOpenBalloon = true;
				talkNumCount++;
			}
			else
			{
				isTalk = false;
				KeyController.keyControlLocking = false;
				lstTalkMessage.Clear();
			}

		}

	}

	
}
