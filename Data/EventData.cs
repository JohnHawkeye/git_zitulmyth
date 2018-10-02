using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Media;
using Zitulmyth.Enums;

namespace Zitulmyth.Data
{
	public struct EventList
	{
		public EventCommandEnum eventType;
		public int eventValue;
		public SoundPlayer bgm;
		public bool flagKeyLock;
		public Vector balloonPos;
		public String balloonMsg;
		public bool balloonEnterClose;
		public CroppedBitmap imgSource;
		public int targetType;
		public Vector setPosition;
		public Image imgTarget;
		public Vector moveDistance;
		public Vector moveTotal;
		public double moveSpeed;
		public bool fadeType;
		public bool uiVisible;
		public ColorEnum color;
		public bool eventOnly;
	}

	public class EventData
	{
		public static List<EventList> listEvent = new List<EventList>();

		public static void InitEvent()
		{
			listEvent.Clear();

			switch (GameTransition.eventNum)
			{
				case 1:
					Event001();
					break;
				case 2:
					Event002();
					break;
				case 3:
					Event003();
					break;
				case 4:
					Event004();
					break;
			}
		}

		private static void Event001()
		{
			listEvent.Add(new EventList { eventType = EventCommandEnum.KeyLock, flagKeyLock = true });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 2000 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaFadeIn, eventValue = 3000,
				imgTarget = ImageData.imgPlayer });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 3000 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange, imgTarget = ImageData.imgPlayer,
				imgSource = ImageData.cbPlayer[1] });

			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange, imgTarget = ImageData.imgPlayer,
				imgSource = ImageData.cbPlayer[0] });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1400 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos = new Vector(300, 671),imgTarget = ImageData.imgPlayer, balloonMsg = "ここはどこだ・・・？"	});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList	{ eventType = EventCommandEnum.Balloon,	balloonEnterClose = true,
				balloonPos = new Vector(300, 671),imgTarget = ImageData.imgPlayer, balloonMsg = "暗くて何も見えない・・・" });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });

			listEvent.Add(new EventList {eventType = EventCommandEnum.Move, moveDistance = new Vector(64, 0),
				moveTotal = new Vector(0, 0), moveSpeed = 2, imgTarget = ImageData.imgPlayer });

			listEvent.Add(new EventList { eventType = EventCommandEnum.SePlay, bgm = MainWindow.seFog });
			listEvent.Add(new EventList { eventType = EventCommandEnum.GenerateEnemy, setPosition = new Vector(500, 671) });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 600 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange, targetType = 1,
				imgSource = ImageData.cbEnemy[0] });
			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.Balloon,
				balloonEnterClose = true,
				balloonPos = new Vector(364, 671),
				imgTarget = ImageData.imgPlayer,
				balloonMsg = "！！"
			});

			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });

			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.Balloon,
				balloonEnterClose = true,
				balloonPos = new Vector(364, 671),
				imgTarget = ImageData.imgPlayer,
				balloonMsg = "いったいなんだこれは・・・？\nとても不吉だ・・・。"
			});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.Balloon,
				balloonEnterClose = true,
				balloonPos = new Vector(364, 671),
				imgTarget = ImageData.imgPlayer,
				balloonMsg = "だが・・・、放っては\nいけない気もする・・・。"
			});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.Balloon,
				balloonEnterClose = true,
				balloonPos = new Vector(364, 671),
				imgTarget = ImageData.imgPlayer,
				balloonMsg = "攻撃・・・してみるか・・・？"
			});

			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 600 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.BgmPlay, bgm = MainWindow.bgmDarkness });

			listEvent.Add(new EventList { eventType = EventCommandEnum.KeyLock, flagKeyLock = false });
			listEvent.Add(new EventList { eventType = EventCommandEnum.UiVisibility, uiVisible = true });

			listEvent.Add(new EventList { eventType = EventCommandEnum.EventEnd, eventOnly = false });

		}

		private static void Event002()
		{
			listEvent.Add(new EventList { eventType = EventCommandEnum.KeyLock, flagKeyLock = true });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 2000 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange,
				imgTarget = ImageData.imgPlayer,imgSource= ImageData.cbPlayer[1]});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange,
				imgTarget = ImageData.imgPlayer,imgSource = ImageData.cbPlayer[0]});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 800 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),imgTarget = ImageData.imgPlayer, balloonMsg= "もう・・・\n出ないのか・・・？"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 800 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),imgTarget = ImageData.imgPlayer,balloonMsg= "うっ！\nまぶしい！！"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 400 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.ScreenFadeIn, eventValue = 1000,fadeType = true,color=ColorEnum.White });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange,
				imgTarget = ImageData.imgPlayer,imgSource= ImageData.cbEmpty });
			listEvent.Add(new EventList { eventType = EventCommandEnum.UiVisibility, uiVisible = false });
			listEvent.Add(new EventList { eventType = EventCommandEnum.ScreenFadeOut, eventValue = 1000, fadeType = false, color = ColorEnum.White });

			listEvent.Add(new EventList { eventType = EventCommandEnum.ScreenFadeIn, eventValue = 1000, fadeType = true, color = ColorEnum.Black });
			listEvent.Add(new EventList { eventType = EventCommandEnum.EventEnd, eventOnly = false });
		}

		private static void Event003()
		{
			listEvent.Add(new EventList { eventType = EventCommandEnum.KeyLock, flagKeyLock = true });
			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange, imgTarget = StageData.imgNpc[0], imgSource = ImageData.cbNpc[1] });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.ScreenFadeOut, eventValue = 1000, fadeType = false, color = ColorEnum.Black });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 2000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange, imgTarget = ImageData.imgPlayer, imgSource = ImageData.cbPlayer[2] });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),imgTarget = ImageData.imgPlayer,balloonMsg= "はっ・・・！\n夢か・・・"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 800 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),imgTarget = ImageData.imgPlayer,balloonMsg= "最近、よく見るんだよな・・・"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange, imgTarget = StageData.imgNpc[0], imgSource = ImageData.cbNpc[0] });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 600 });
			listEvent.Add(new EventList	{ eventType = EventCommandEnum.Move, moveDistance = new Vector(192, 0),
				moveTotal = new Vector(0, 0), moveSpeed = 4, imgTarget = StageData.imgNpc[0]});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 600 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange, imgTarget = ImageData.imgPlayer, imgSource = ImageData.cbPlayer[3] });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),imgTarget = StageData.imgNpc[0],balloonMsg= "おう、ヴトルプ、起きたか"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 600 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),imgTarget = StageData.imgNpc[0],balloonMsg= "なんか、うなされていたが、\n大丈夫か・・・？"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),imgTarget = ImageData.imgPlayer,balloonMsg= "うん、平気。\nでも、最近多いんだ。"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),imgTarget = StageData.imgNpc[0],balloonMsg= "ふむ、そうか・・・。"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 600 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),imgTarget = StageData.imgNpc[0],balloonMsg= "もし、気になるなら、\nオババ様に聞くといいさ。"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 600 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),imgTarget = ImageData.imgPlayer,balloonMsg= "うん、そうしてみる。"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.ScreenFadeIn, eventValue = 1000, fadeType = true, color = ColorEnum.Black });
			listEvent.Add(new EventList { eventType = EventCommandEnum.EventEnd, eventOnly= true });
		}

		private static void Event004()
		{
			listEvent.Add(new EventList { eventType = EventCommandEnum.KeyLock, flagKeyLock = true });
			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange, imgTarget = ImageData.imgPlayer, imgSource = ImageData.cbEmpty });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.ScreenFadeOut, eventValue = 1000, fadeType = false, color = ColorEnum.Black });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange, imgTarget = ImageData.imgPlayer, imgSource = ImageData.cbPlayer[0] });
			listEvent.Add(new EventList	{ eventType = EventCommandEnum.Move, moveDistance = new Vector(96, 0),
				moveTotal = new Vector(0, 0), moveSpeed = 4, imgTarget = ImageData.imgPlayer});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 800 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos = new Vector(0, 0),imgTarget = ImageData.imgPlayer,balloonMsg= "オババ様に話すべきかも\nしれないけれど・・・"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos = new Vector(0, 0),imgTarget = ImageData.imgPlayer,balloonMsg= "ちょっと苦手なんだよな・・・"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos = new Vector(0, 0),imgTarget = ImageData.imgPlayer,balloonMsg= "あまり考えたくないや"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos = new Vector(0, 0),imgTarget = ImageData.imgPlayer,balloonMsg= "ちょっと遊んでいようっと"});


			listEvent.Add(new EventList { eventType = EventCommandEnum.KeyLock, flagKeyLock = false });
			listEvent.Add(new EventList { eventType = EventCommandEnum.UiVisibility, uiVisible = true });

			listEvent.Add(new EventList { eventType = EventCommandEnum.EventEnd, eventOnly = false });
		}

	}
}
