using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Media;

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

					break;
			}
		}

		private static void Event001()
		{
			listEvent.Add(new EventList { eventType = EventCommandEnum.KeyLock, flagKeyLock = true });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 2000 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaFadeIn, eventValue = 3000, imgTarget = ImageData.imgPlayer });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 3000 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });

			listEvent.Add(new EventList	{ eventType = EventCommandEnum.CharaImageChange,
				imgTarget = ImageData.imgPlayer,
				imgSource = ImageData.cbPlayer[1]
			});

			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.CharaImageChange,
				imgTarget = ImageData.imgPlayer,
				imgSource = ImageData.cbPlayer[0]
			});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1400 });

			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.Balloon,
				balloonEnterClose = true,
				balloonPos = new Vector(300, 671),
				balloonMsg = "ここはどこだ・・・？"
			});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.Balloon,
				balloonEnterClose = true,
				balloonPos = new Vector(300, 671),
				balloonMsg = "暗くて何も見えない・・・"
			});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });

			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.Move,
				moveDistance = new Vector(64, 0),
				moveTotal = new Vector(0, 0),
				moveSpeed = 2,
				imgTarget = ImageData.imgPlayer
			});

			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 2000 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.SePlay, bgm = MainWindow.seFog });
			listEvent.Add(new EventList { eventType = EventCommandEnum.GenerateEnemy, setPosition = new Vector(500, 671) });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 600 });

			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.CharaImageChange,
				targetType = 1,
				imgSource = ImageData.cbEnemy[0]
			});
			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.Balloon,
				balloonEnterClose = true,
				balloonPos = new Vector(364, 671),
				balloonMsg = "！！"
			});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });

			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.Balloon,
				balloonEnterClose = true,
				balloonPos = new Vector(364, 671),
				balloonMsg = "いったいなんだこれは・・・？\nとても不吉だ・・・。"
			});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList
			{
				eventType = EventCommandEnum.Balloon,
				balloonEnterClose = true,
				balloonPos = new Vector(364, 671),
				balloonMsg = "だが・・・、放っては\nいけない気もする・・・。"
			});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),balloonMsg= "攻撃・・・してみるか・・・？"});

			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 600 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.BgmPlay, bgm = MainWindow.bgmDarkness });

			listEvent.Add(new EventList { eventType = EventCommandEnum.KeyLock, flagKeyLock = false });
			listEvent.Add(new EventList { eventType = EventCommandEnum.UiVisibility, uiVisible = true });

			listEvent.Add(new EventList { eventType = EventCommandEnum.EventEnd });

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
				balloonPos =new Vector(364, 671),balloonMsg= "もう・・・\n出ないのか・・・？"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 800 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.Balloon, balloonEnterClose = true,
				balloonPos =new Vector(364, 671),balloonMsg= "うっ！\nまぶしい！！"});
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 400 });

			listEvent.Add(new EventList { eventType = EventCommandEnum.ScreenFadeIn, eventValue = 1000,fadeType = true });
			listEvent.Add(new EventList { eventType = EventCommandEnum.Wait, eventValue = 1000 });
			listEvent.Add(new EventList { eventType = EventCommandEnum.CharaImageChange,
				imgTarget = ImageData.imgPlayer,imgSource= ImageData.cbEmpty
			});
			listEvent.Add(new EventList { eventType = EventCommandEnum.ScreenFadeOut, eventValue = 1000, fadeType = false });

			listEvent.Add(new EventList { eventType = EventCommandEnum.UiVisibility, uiVisible = false });

			listEvent.Add(new EventList { eventType = EventCommandEnum.EventEnd });
		}
	}


}
