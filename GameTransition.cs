using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
		EditMode,

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
		public static bool growthEnemy;

		public static bool endSplashLogo = false;
		private static int splashLogoPhase = 0;
		private static int splashWaitTotal = 0;

		public static bool duringTransition = false;
		public static int eventCount = 0;

		public static bool eventStart = false;
		public static Timer eventTimer;
		public static bool eventTimerStart=false;
		public static bool eventWaiting = false;
		
		public static bool eventBalloonIsOpen = false;

		public static Image eventTargetImage = new Image();
		public static bool charaRenderStart = false;
		public static int charaRenderIndex = 0;
		public static double renderRateTotal = 0;

		public static bool screenFadeStart = false;
		public static int screenFadeIndex = 0;
		public static double screenFadeTotal = 0;

		public static Vector eventCharaMoveDis = new Vector(0,0);
		public static int eventCharaMoveSpd;
		public static int charaMoveIndex = 0;
		public static bool charaMoveStart;

		public static bool stageTestPlay = false;

		public static void GameTransitionController(Canvas canvas, Canvas caLife,Canvas caMana)
		{
			switch (gameTransition)
			{

				case GameTransitionType.Title:

					if (!endSplashLogo)
					{
						splashLogoPhase = 4;
						switch (splashLogoPhase)
						{
							case 0:

								if (splashWaitTotal < 1000)
								{
									splashWaitTotal += MainWindow.elapsedTime;
								}
								else
								{
									splashLogoPhase++;
									splashWaitTotal = 0;
								}
								break;

							case 1:
								if (screenFadeTotal < 1)
								{
									double temp = (double)MainWindow.elapsedTime / 2000;
									screenFadeTotal += Math.Round(temp, 2);

									ImageData.imgTitle[0].Opacity = screenFadeTotal;

								}
								else
								{
									ImageData.imgTitle[0].Opacity = 1;
									splashLogoPhase++;
								}
								break;

							case 2:

								if (splashWaitTotal < 2000)
								{
									splashWaitTotal += MainWindow.elapsedTime;
								}
								else
								{
									splashLogoPhase++;
									splashWaitTotal = 0;
								}
								break;

							case 3:
								if (screenFadeTotal > 0)
								{
									double temp = (double)MainWindow.elapsedTime / 2000;
									screenFadeTotal -= Math.Round(temp, 2);

									ImageData.imgTitle[0].Opacity = screenFadeTotal;

								}
								else
								{
									ImageData.imgTitle[0].Opacity = 0;
									splashLogoPhase++;
									
								}
								break;
							case 4:
								if (splashWaitTotal < 1000)
								{
									splashWaitTotal += MainWindow.elapsedTime;

								}
								else
								{
									endSplashLogo = true;
									screenFadeTotal = 0;

									Sound.SoundBgmSelector(BgmName.Opening);
									Sound.bgm.Play();

									MainWindow.TitleOpen(canvas);
									
								}
								break;
						}

					}
					else
					{

						MainWindow.countTime += MainWindow.elapsedTime;

						if (MainWindow.countTime >= 1000)
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
						}

					}
									   					
					break;

				case GameTransitionType.StageInit:

					StageInit.InitBlockData();
					StageDataSetting.SetData();

					StageInit.StageBlockSet(canvas);
					StageManager.StageObjectsSetting(canvas);

					MainWindow.lblMode.Content = "ゲームモード：ステージ開始";
					gameTransition = GameTransitionType.StageStart;

					break;

				case GameTransitionType.StageStart:

					if (!eventStart)
					{
						duringTransition = true;

						StageEvent.InitEvent();
						eventCount = 0;
						eventStart = true;
					}

					break;

				case GameTransitionType.StageDuring:

					if (StageManager.StageClearCheck())
					{
						gameTransition = GameTransitionType.StageEnd;
						MainWindow.lblMode.Content = "ゲームモード：ステージ終了";
					}
					else
					{
						if (growthEnemy)
						{	
							SpawnEnemy.SpawnSelect(canvas, EnemyName.Zigitu01);
							growthEnemy = false;
						}
					}

					break;

				case GameTransitionType.StageEnd:

					if (!eventStart)
					{
						duringTransition = true;
						eventStart = true;
					}

					break;

				case GameTransitionType.StageNext:

					if (!stageTestPlay)
					{

						StageManager.stageNum++;
						if (StageManager.stageNum >= StageOrder.lstStageOrder.Count)
						{
							MainWindow.timerFrameUpdate.Stop();

							MessageBox.Show("すべてのステージが終わりました。ゲームを終了します。", "ゲームの終了", MessageBoxButton.OK, MessageBoxImage.Information);

							Application.Current.Shutdown();
						}

						StageInit.StageBlockRemove(canvas);
						StageInit.StageObjectsRemove(canvas);
						StageInit.StageEnemyRemove(canvas);
						growthEnemy = false;
						StageInit.StageItemRemove(canvas);

						StageManager.lstClearCondition.Clear();
						StageManager.clearFlag = false;

						MainWindow.lblMode.Content = "ゲームモード：ステージ準備";
						gameTransition = GameTransitionType.StageInit;
					}
					else
					{
						MainWindow.timerFrameUpdate.Stop();
						gameTransition = GameTransitionType.EditMode;
						stageTestPlay = false;
						StageManager.clearFlag = false;

						MainWindow.stageEditor.StageLoad();

						MainWindow.stageEditor.StageEditorDataSetting();
						StageEditorOperator.EditorPlayerPaletteSetting();

						MainWindow.stageEditor.tbcEditSelect.IsEnabled = true;
						MainWindow.stageEditor.btnStageNumDecrease.IsEnabled = true;
						MainWindow.stageEditor.btnStageNumIncrease.IsEnabled = true;
						MainWindow.stageEditor.btnStageOrder.IsEnabled = true;
						MainWindow.stageEditor.btnStageTestPlay.IsEnabled = true;
						MainWindow.stageEditor.btnEventWindowOpen.IsEnabled = true;
					}

					break;

				case GameTransitionType.EditMode:

					break;
			}
			
		}

		public static void EventController(Canvas canvas)
		{

			if(eventCount != StageEvent.listEvent.Count)
			{
				if (!eventWaiting && !screenFadeStart && !charaMoveStart)
				{
					switch (StageEvent.listEvent[eventCount].eventType)
					{
						case EventCommandEnum.Wait:
							eventWaiting = true;
							EventWaiting(StageEvent.listEvent[eventCount].eventValue);
							Console.WriteLine("wait");
							break;

						case EventCommandEnum.Balloon:

							Vector blpos = StageEvent.listEvent[eventCount].balloonPos;
							string blstring = StageEvent.listEvent[eventCount].balloonMsg;

							if(StageEvent.listEvent[eventCount].targetImgType == TargetType.Player)
							{
								eventTargetImage = ImageData.imgPlayer;
							}

							if (StageEvent.listEvent[eventCount].targetImgType == TargetType.Object)
							{
								eventTargetImage = SelectObjectImage();
							}

							
							BalloonMessage.OpenBalloon(eventCount, canvas, blpos, eventTargetImage, blstring,false);

							
							eventBalloonIsOpen = true;

							break;

						case EventCommandEnum.UiVisibility:

							if (!StageEvent.listEvent[eventCount].uiVisible)
							{
								MainWindow.stpPlayerStatus.Visibility = Visibility.Hidden;
							}
							else
							{
								MainWindow.stpPlayerStatus.Visibility = Visibility.Visible;
							}
							break;

						case EventCommandEnum.KeyLock:
							KeyController.keyControlLocking = StageEvent.listEvent[eventCount].flagKeyLock;
							Console.WriteLine("keylock");
							break;

						case EventCommandEnum.Move:



							if(StageEvent.listEvent[eventCount].targetImgType == TargetType.Object)
							{
								eventTargetImage = SelectObjectImage();
							}

							charaMoveIndex = eventCount;
							charaMoveStart = true;

							break;

						case EventCommandEnum.BgmPlay:

							Sound.SoundBgmSelector(StageEvent.listEvent[eventCount].bgmName);
							Sound.bgm.Play();

							Console.WriteLine("bgm");
							break;

						case EventCommandEnum.SePlay:

							Sound.SoundEffectSelector(StageEvent.listEvent[eventCount].seName);

							Sound.SoundEffectPlayer(StageEvent.listEvent[eventCount].seName);

							Console.WriteLine("se");
							break;

						case EventCommandEnum.CharaFadeIn:

							ImageData.imgPlayer.Opacity = 0;
							ImageData.imgPlayer.Width = 32;
							ImageData.imgPlayer.Height = 64;

							renderRateTotal = 0;

							charaRenderIndex = eventCount;
							charaRenderStart = true;

							break;

						case EventCommandEnum.CharaImageChange:

							switch (StageEvent.listEvent[eventCount].targetImgType)
							{
								case TargetType.Player:
									ImageData.imgPlayer.Source = 
										ImageData.ImageSourceSelector(StageEvent.listEvent[eventCount].categoryName,
																		StageEvent.listEvent[eventCount].patternName);
									break;
								case TargetType.Enemy:

									//
									SpawnEnemy.lstEnemyData[0].imgEnemy.Source =
										ImageData.ImageSourceSelector(StageEvent.listEvent[eventCount].categoryName,
																		StageEvent.listEvent[eventCount].patternName);
									break;
								case TargetType.Object:

									SelectObjectImage().Source =

										ImageData.ImageSourceSelector(StageEvent.listEvent[eventCount].categoryName,
																		StageEvent.listEvent[eventCount].patternName);

									break;
								case TargetType.Item:
									break;
							}
							
							break;

						case EventCommandEnum.ScreenFadeIn:

							

							switch (StageEvent.listEvent[eventCount].color)
							{
								case ColorEnum.White:
									MainWindow.canScreenFade.Background = new SolidColorBrush(Colors.White);
									break;
								case ColorEnum.Black:
									MainWindow.canScreenFade.Background = new SolidColorBrush(Colors.Black);
									break;

							}
							
							MainWindow.canScreenFade.Opacity = 0;

							MainWindow.canScreenFade.Visibility = Visibility.Visible;

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

							SpawnEnemy.GenerateEnemy(canvas,StageEvent.listEvent[eventCount].setPosition);

							break;

						case EventCommandEnum.EventEnd:

							duringTransition = false;
							eventStart = false;

							if(gameTransition == GameTransitionType.StageStart)
							{
								if (!StageEvent.listEvent[eventCount].eventOnly)
								{
									MainWindow.lblMode.Content = "ゲームモード：ステージプレイ";
									gameTransition = GameTransitionType.StageDuring;
								}
								else
								{
									MainWindow.lblMode.Content = "ゲームモード：ステージ初期化";
									gameTransition = GameTransitionType.StageNext;
								}
								
							}
							else if(gameTransition == GameTransitionType.StageEnd)
							{
								MainWindow.lblMode.Content = "ゲームモード：ステージ初期化";
								gameTransition = GameTransitionType.StageNext;
							}

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

		public static void ChangeImage()
		{

		}

		public static void CharaRender()
		{
			if (renderRateTotal < 1)
			{
				double temp = (double)MainWindow.elapsedTime/ StageEvent.listEvent[charaRenderIndex].eventValue;
				renderRateTotal += Math.Round(temp,2);

				ImageData.imgPlayer.Opacity = renderRateTotal;

			}
			else
			{
				ImageData.imgPlayer.Opacity = 1;
				charaRenderStart = false;
			}
		}

		public static void ScreenFade(Canvas canvas)
		{

			if (StageEvent.listEvent[screenFadeIndex].fadeType)
			{
				if (screenFadeTotal < 1)
				{
					double temp = (double)MainWindow.elapsedTime / StageEvent.listEvent[screenFadeIndex].eventValue;
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
					double temp = (double)MainWindow.elapsedTime / StageEvent.listEvent[screenFadeIndex].eventValue;
					screenFadeTotal -= Math.Round(temp, 2);

					MainWindow.canScreenFade.Opacity = screenFadeTotal;

				}
				else
				{
					MainWindow.canScreenFade.Opacity = 0;

					MainWindow.canScreenFade.Visibility = Visibility.Hidden;

					screenFadeStart = false;
				}
			}

		}

		public static void CharaMove()
		{
			if(StageEvent.listEvent[charaMoveIndex].moveTotal.X < StageEvent.listEvent[charaMoveIndex].moveDistance.X)
			{

				double x = Canvas.GetLeft(eventTargetImage);
				double dis = 0;
				double ac = StageEvent.listEvent[charaMoveIndex].moveSpeed;

				dis = (double)PlayerStatus.moveSpeed * MainWindow.elapsedTime * 0.01 * ac;

				if (!StageEvent.listEvent[charaMoveIndex].direction)
				{
					Canvas.SetLeft(eventTargetImage, x - dis);
				}
				else
				{
					Canvas.SetLeft(eventTargetImage, x + dis);
				}

				StageEvent.listEvent[charaMoveIndex].moveTotal.X += dis;

			}
			else
			{
				charaMoveStart = false;
			}

		}

		public static Image SelectObjectImage()
		{
			Image temp = new Image();

			switch (StageEvent.listEvent[eventCount].eventType)
			{
				case EventCommandEnum.CharaImageChange:

					temp = ObjectChecker.lstObject[StageEvent.listEvent[eventCount].targetId].imgObject;

					break;

				case EventCommandEnum.Move:

					temp = ObjectChecker.lstObject[StageEvent.listEvent[eventCount].targetId].imgObject;

					break;

				case EventCommandEnum.Balloon:

					temp = ObjectChecker.lstObject[StageEvent.listEvent[eventCount].targetId].imgObject;

					break;

			}

			return temp;

		}


	}


}
