using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Timers;
using Zitulmyth.Data;
using Zitulmyth.Enums;

namespace Zitulmyth
{
	public enum GameTransitionType
	{
		Title,
		StageInit,
		StageStart,
		StageDuring,
		StageEnd,
		StageNext,

	}

	public enum EventCommandEnum
	{
		Wait,
		Balloon,
		BgmPlay,
		SePlay,
		Move,
		KeyLock,
		UiVisibility,
		CharaFadeIn,
		CharaImageChange,
		ScreenFadeIn,
		ScreenFadeOut,
		GenerateEnemy,

		EventEnd,
		
	}

	public class GameTransition
	{
		public static GameTransitionType gameTransition = GameTransitionType.Title;

		public static bool duringTransition = false;
		public static int eventNum = 1;
		public static int eventCount = 0;

		public static bool eventStart = false;
		public static Timer eventTimer;
		public static bool eventTimerStart=false;
		public static bool eventWaiting = false;
		
		public static bool eventBalloonIsOpen = false;

		public static bool charaRenderStart = false;
		public static int charaRenderIndex = 0;
		public static double renderRateTotal = 0;

		public static bool screenFadeStart = false;
		public static int screenFadeIndex = 0;
		public static double screenFadeTotal = 0;

		public static Vector eventCharaMoveDis;
		public static int eventCharaMoveSpd;
		public static int charaMoveIndex = 0;
		public static bool charaMoveStart;

		public static int numKillEnemy = 0;

		public static void GameTransitionController(Canvas canvas, Canvas caLife,Canvas caMana)
		{
			switch (gameTransition)
			{

				case GameTransitionType.Title:

					MainWindow.countTime += MainWindow.elapsedTime;

					if(MainWindow.countTime >= 1000)
					{
						MainWindow.titleStrSwitch = !MainWindow.titleStrSwitch;
						MainWindow.countTime = 0;

						if (!MainWindow.titleStrSwitch)
						{
							ImageData.imgTitle[1].Visibility = Visibility.Hidden;
						}
						else
						{
							ImageData.imgTitle[1].Visibility = Visibility.Visible;
						}
						Console.WriteLine("Title");
					}
					
					break;

				case GameTransitionType.StageInit:

					if (StageManager.stageNum >= 1)
					{
						ImageData.ImageLoadAfterSecond();
					}

					StageInit.InitBlockData();
					StageDataSetting.SetData();

					StageInit.StageBlockSet(canvas);
					StageManager.StageObjectsSetting(canvas);

					GameTransition.gameTransition = GameTransitionType.StageStart;
					Console.WriteLine("StageInit");

					break;

				case GameTransitionType.StageStart:

					if (!eventStart)
					{
						duringTransition = true;

						EventData.InitEvent();
						eventCount = 0;
						eventStart = true;
					}
					Console.WriteLine("StageStart");

					break;

				case GameTransitionType.StageDuring:

					if (numKillEnemy >= 10)
					{
						gameTransition = GameTransitionType.StageEnd;
						numKillEnemy = 0;
					}

					break;

				case GameTransitionType.StageEnd:

					if (!eventStart)
					{
						duringTransition = true;

						EventData.InitEvent();
						eventCount = 0;
						eventStart = true;
					}

					break;

				case GameTransitionType.StageNext:

					StageManager.stageNum++;

					StageInit.StageBlockRemove(canvas);
					StageInit.StageObjectsRemove(canvas);

					gameTransition = GameTransitionType.StageInit;

					break;
			}
			
		}

		public static void EventController(Canvas canvas)
		{

			if(eventCount != EventData.listEvent.Count)
			{
				if (!eventWaiting && !screenFadeStart && !charaMoveStart)
				{
					switch (EventData.listEvent[eventCount].eventType)
					{
						case EventCommandEnum.Wait:
							eventWaiting = true;
							EventWaiting(EventData.listEvent[eventCount].eventValue);
							Console.WriteLine("wait");
							break;

						case EventCommandEnum.Balloon:

							Vector blpos = EventData.listEvent[eventCount].balloonPos;
							String blstring = EventData.listEvent[eventCount].balloonMsg;

							if (EventData.listEvent[eventCount].targetType == TargetType.Object)
							{
								SelectObjectImage();
							}

							Image blTarget = EventData.listEvent[eventCount].imgTarget;
							BalloonMessage.OpenBalloon(eventCount, canvas, blpos, blTarget, blstring);

							
							eventBalloonIsOpen = true;

							break;

						case EventCommandEnum.UiVisibility:

							if (!EventData.listEvent[eventCount].uiVisible)
							{
								MainWindow.stpPlayerStatus.Visibility = Visibility.Hidden;
							}
							else
							{
								MainWindow.stpPlayerStatus.Visibility = Visibility.Visible;
							}
							break;

						case EventCommandEnum.KeyLock:
							KeyController.keyControlLocking = EventData.listEvent[eventCount].flagKeyLock;
							Console.WriteLine("keylock");
							break;

						case EventCommandEnum.Move:

							if(EventData.listEvent[eventCount].targetType == TargetType.Object)
							{
								SelectObjectImage();
							}

							charaMoveIndex = eventCount;
							charaMoveStart = true;

							break;

						case EventCommandEnum.BgmPlay:
							if(EventData.listEvent[eventCount].bgm != null)
								EventData.listEvent[eventCount].bgm.PlayLooping();
							Console.WriteLine("bgm");
							break;

						case EventCommandEnum.SePlay:
							if (EventData.listEvent[eventCount].bgm != null)
								EventData.listEvent[eventCount].bgm.Play();
							Console.WriteLine("se");
							break;

						case EventCommandEnum.CharaFadeIn:

							EventData.listEvent[eventCount].imgTarget.Opacity = 0;
							EventData.listEvent[eventCount].imgTarget.Source = ImageData.cbPlayer[0];
							EventData.listEvent[eventCount].imgTarget.Width = 32;
							EventData.listEvent[eventCount].imgTarget.Height = 64;

							renderRateTotal = 0;

							charaRenderIndex = eventCount;
							charaRenderStart = true;

							break;

						case EventCommandEnum.CharaImageChange:

							switch (EventData.listEvent[eventCount].targetType)
							{
								case TargetType.Player:
									EventData.listEvent[eventCount].imgTarget.Source = EventData.listEvent[eventCount].imgSource;
									break;
								case TargetType.Enemy:
									EnemyData.lstSpawnEnemy[0].imgEnemy.Source = EventData.listEvent[eventCount].imgSource;
									break;
								case TargetType.Object:

									SelectObjectImage();

									break;
								case TargetType.Item:
									break;
							}
							
							break;

						case EventCommandEnum.ScreenFadeIn:

							MainWindow.canScreenFade.Width = 1024;
							MainWindow.canScreenFade.Height = 768;

							switch (EventData.listEvent[eventCount].color)
							{
								case ColorEnum.White:
									MainWindow.canScreenFade.Background = new SolidColorBrush(Colors.White);
									break;
								case ColorEnum.Black:
									MainWindow.canScreenFade.Background = new SolidColorBrush(Colors.Black);
									break;

							}
							
							MainWindow.canScreenFade.Opacity = 0;

							canvas.Children.Add(MainWindow.canScreenFade);
							Canvas.SetLeft(MainWindow.canScreenFade, 0);
							Canvas.SetTop(MainWindow.canScreenFade, 0);
							Canvas.SetZIndex(MainWindow.canScreenFade,10);

							screenFadeTotal = 0;
							screenFadeIndex = eventCount;
							screenFadeStart = true;

							break;

						case EventCommandEnum.ScreenFadeOut:

							screenFadeTotal = 1;
							screenFadeIndex = eventCount;
							screenFadeStart = true;
							break;

						case EventCommandEnum.GenerateEnemy:

							SpawnEnemy.GenerateEnemy(canvas,EventData.listEvent[eventCount].setPosition);

							break;

						case EventCommandEnum.EventEnd:

							duringTransition = false;
							eventStart = false;

							if(gameTransition == GameTransitionType.StageStart)
							{
								if (!EventData.listEvent[eventCount].eventOnly)
								{
									gameTransition = GameTransitionType.StageDuring;
								}
								else
								{
									gameTransition = GameTransitionType.StageNext;
								}
								
							}
							else if(gameTransition == GameTransitionType.StageEnd)
							{
								gameTransition = GameTransitionType.StageNext;
							}

							eventNum++;

							Console.WriteLine("end");
							
							break;
					}

					eventCount++;
				}

			}
			

		}

		private static void EventWaiting(int time)
		{
			if (!eventTimerStart)
			{
				eventTimer = new Timer(time);
				eventTimer.Elapsed += (sender, e) =>
				{
					try
					{
						eventTimer.Stop();
						eventTimerStart = false;
						eventWaiting = false;
					}
					finally
					{

					}
				};

				eventTimer.Start();

			}
		}

		public static void CharaRender()
		{
			if (renderRateTotal < 1)
			{
				double temp = (double)MainWindow.elapsedTime/ EventData.listEvent[charaRenderIndex].eventValue;
				renderRateTotal += Math.Round(temp,2);

				EventData.listEvent[charaRenderIndex].imgTarget.Opacity = renderRateTotal;

			}
			else
			{
				EventData.listEvent[charaRenderIndex].imgTarget.Opacity = 1;
				charaRenderStart = false;
			}
		}

		public static void ScreenFade(Canvas canvas)
		{

			if (EventData.listEvent[screenFadeIndex].fadeType)
			{
				if (screenFadeTotal < 1)
				{
					double temp = (double)MainWindow.elapsedTime / EventData.listEvent[screenFadeIndex].eventValue;
					screenFadeTotal += Math.Round(temp, 2);

					MainWindow.canScreenFade.Opacity = screenFadeTotal;

				}
				else
				{
					MainWindow.canScreenFade.Opacity = 1;

					screenFadeStart = false;
				}
			}
			else
			{
				if (screenFadeTotal > 0)
				{
					double temp = (double)MainWindow.elapsedTime / EventData.listEvent[screenFadeIndex].eventValue;
					screenFadeTotal -= Math.Round(temp, 2);

					MainWindow.canScreenFade.Opacity = screenFadeTotal;

				}
				else
				{
					MainWindow.canScreenFade.Opacity = 0;

					canvas.Children.Remove(MainWindow.canScreenFade);

					screenFadeStart = false;
				}
			}

		}

		public static void CharaMove()
		{
			if(EventData.listEvent[charaMoveIndex].moveTotal.X < EventData.listEvent[charaMoveIndex].moveDistance.X)
			{

				double x = Canvas.GetLeft(EventData.listEvent[charaMoveIndex].imgTarget);
				double dis = 0;
				double ac = EventData.listEvent[charaMoveIndex].moveSpeed;

				dis = (double)PlayerStatus.moveSpeed * MainWindow.elapsedTime * 0.01 * ac;

				if (EventData.listEvent[charaMoveIndex].moveDistance.X >= 0)
				{
					Canvas.SetLeft(EventData.listEvent[charaMoveIndex].imgTarget, x + dis);
				}
				else
				{
					Canvas.SetLeft(EventData.listEvent[charaMoveIndex].imgTarget, x - dis);
				}

				EventList el = EventData.listEvent[charaMoveIndex];
				el.moveTotal.X += dis;

				EventData.listEvent[charaMoveIndex] = el;

			}
			else
			{
				charaMoveStart = false;
			}

		}

		public static void SelectObjectImage()
		{
			switch (EventData.listEvent[eventCount].eventType)
			{
				case EventCommandEnum.CharaImageChange:

					for (int i = 0; i < Object.lstObject.Count; i++)
					{
						if (Object.lstObject[i].objName == EventData.listEvent[eventCount].objectName)
						{
							Object.lstObject[i].imgObject.Source = EventData.listEvent[eventCount].imgSource;
							break;
						}
					}

					break;

				case EventCommandEnum.Move:

					for (int i = 0; i < Object.lstObject.Count; i++)
					{
						if (Object.lstObject[i].objName == EventData.listEvent[eventCount].objectName)
						{
							EventData.listEvent[eventCount].imgTarget = Object.lstObject[i].imgObject;
							
							break;
						}
					}

					break;

				case EventCommandEnum.Balloon:

					for (int i = 0; i < Object.lstObject.Count; i++)
					{
						if (Object.lstObject[i].objName == EventData.listEvent[eventCount].objectName)
						{
							EventData.listEvent[eventCount].imgTarget = Object.lstObject[i].imgObject;

							break;
						}
					}

					break;
			}



		}

	}


}
