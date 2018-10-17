﻿using System;
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

	public partial class MainWindow : Window
	{

		//timer
		private Timer timerFrameUpdate;
		public static int countTime = 0;
		private int lastTime;
		private int nowTime;
		public static int elapsedTime;

		//Controls
		public static Canvas canScreenFade = new Canvas();
		public static StackPanel stpPlayerStatus;

		public static bool titleStrSwitch = true;

		//mainwindow
		public MainWindow()
		{
			this.InitializeComponent();

			this.timerFrameUpdate = new Timer(15);
			this.timerFrameUpdate.Elapsed += FrameUpdateTimer_Update;
			this.timerFrameUpdate.Start();

		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			this.InitGame();
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			KeyController.InputKeyDown(sender, e);
		}

		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			KeyController.InputKeyUp(sender, e);
		}

//inits
		private void InitGame()
		{

			this.GetNowTime();
			lastTime = nowTime;

			StageDataSetting.SetData();
			ImageData.ImageLoadFirst();
			Sound.SoundEffectLoad(Canvas);

			this.TitleOpen();

			//objects maked

			MainWeapon.InitMainWeapon(Canvas);

			BalloonMessage.GenerateBalloon(Canvas);
			stpPlayerStatus = Canvas.FindName("spPlayerStatus")as StackPanel;
			stpPlayerStatus.Visibility = Visibility.Hidden;
			Canvas.SetZIndex(spPlayerStatus, 2);

			var _popcantalk = new Image
			{
				Source = ImageData.cbSystem,
				Width = 64,Height=32,
			};

			ImageData.imgPopCanTalk = _popcantalk;
			ImageData.imgPopCanTalk.Visibility = Visibility.Hidden;
			Canvas.Children.Add(ImageData.imgPopCanTalk);
			Canvas.SetZIndex(ImageData.imgPopCanTalk, 15);


			StageInit.InitPlayer(Canvas);
			StageInit.InitPlayerStatus(CaLife, CaMana);
		}

		private void TitleOpen()
		{
			ImageData.imgTitle[0] = new Image
			{
				Source = ImageData.cbTitle[0],
				Width = 1024,Height = 768,
			};

			this.Canvas.Children.Add(ImageData.imgTitle[0]);
			Canvas.SetLeft(ImageData.imgTitle[0], 0);
			Canvas.SetTop(ImageData.imgTitle[0], 0);


			ImageData.imgTitle[1] = new Image
			{
				Source = ImageData.cbTitle[1],Width = 448,Height = 32,
			};

			this.Canvas.Children.Add(ImageData.imgTitle[1]);
			Canvas.SetLeft(ImageData.imgTitle[1],288);
			Canvas.SetTop(ImageData.imgTitle[1], 608);
		}

//frameupdate
		protected void FrameUpdateTimer_Update(object sender, ElapsedEventArgs e)
		{
			try
			{
				this.Dispatcher.Invoke(() =>
				{
					//TimeManagement
					this.GetNowTime();
					elapsedTime = nowTime - lastTime;


					NowTimeLabel.Content = elapsedTime.ToString();

					if (elapsedTime < 0)
					{
						elapsedTime += 59999;
					}


					KeyController.KeyInterval();

					//GameTransition
					if (!GameTransition.duringTransition)
					{
						GameTransition.GameTransitionController(Canvas, CaLife, CaMana);
					}

					//EventAction
					if (GameTransition.eventStart)
					{
						if (GameTransition.charaRenderStart)
						{
							GameTransition.CharaRender();
						}

						if (GameTransition.screenFadeStart)
						{
							GameTransition.ScreenFade(Canvas);
						}

						if (GameTransition.charaMoveStart)
						{
							GameTransition.CharaMove();
						}

						if (!GameTransition.eventBalloonIsOpen)
						{
							if (GameTransition.eventCount < StageEvent.listEvent.Count)
							{
								GameTransition.EventController(Canvas);
							}
						}
						else
						{
							if (KeyController.keyReturn)
							{
								EnterKeyAction(Canvas);
							}
						}
					}

					if (KeyController.keyReturn)
					{
						EnterKeyAction(Canvas);
					}

					if (GameTransition.gameTransition == GameTransitionType.StageDuring)
					{
						Animator.AnimationObject();

						if (TalkCommander.isTalk && !TalkCommander.isTalkOpenBalloon)
						{
							TalkCommander.TalkWithNpc(Canvas);
						}

						//first action,last Processing including deletion of list

						PlayerBehavior.MovePlayer(Canvas);
						PlayerBehavior.FallingPlayer();
						Item.FallingItems();
						EnemyBehavior.EnemyAction();

						SubWeapon.SubWeaponPosUpdate(Canvas);

						if (PlayerStatus.isMainAttack)
						{
							MainWeapon.MainWeaponAttack(Canvas);
							MainWeapon.MainWeaponCollision(Canvas);
						}
						
						ObjectChecker.CollisionPtoActionCollider();
						PlayerBehavior.CollisionPtoE();
						SubWeapon.CollisionSubWeapon(Canvas);

						PlayerStatus.PlayerStatusUpdate();
						PlayerBehavior.DamageInvinsibleTimer();
					}

					if (Sound.seStop)
					{
						if (Sound.sePlayTime < 60)
						{
							Sound.sePlayTime++;
						}
						else
						{
							Sound.sePlayTime = 0;
							Sound.seStop = false;
						}
					}

					lastTime = nowTime;

				});
			}
			catch (TaskCanceledException)
			{

			}
		}

		public static void EnterKeyAction(Canvas canvas)
		{
			switch (GameTransition.gameTransition)
			{
				case GameTransitionType.Title:

					canvas.Children.Remove(ImageData.imgTitle[0]); canvas.Children.Remove(ImageData.imgTitle[1]);
					countTime = 0;
					GameTransition.gameTransition = GameTransitionType.StageInit;

//debug stagechange
					StageManager.stageNum = 2;
					GameTransition.eventNum = 4;
					break;

				case GameTransitionType.StageStart:

					if (!KeyController.keyReturnInterval)
					{
						BalloonMessage.spnBalloon.Visibility = Visibility.Hidden;
						BalloonMessage.txtBalloon.Text = "";
						GameTransition.eventBalloonIsOpen = false;

						KeyController.keyReturnInterval = true;
					}

					break;

				case GameTransitionType.StageDuring:

					if (!KeyController.keyReturnInterval)
					{
						BalloonMessage.spnBalloon.Visibility = Visibility.Hidden;
						BalloonMessage.txtBalloon.Text = "";
						TalkCommander.isTalkOpenBalloon = false;

						KeyController.keyReturnInterval = true;
					}


					break;

				case GameTransitionType.StageEnd:

					if (!KeyController.keyReturnInterval)
					{
						BalloonMessage.spnBalloon.Visibility = Visibility.Hidden;
						BalloonMessage.txtBalloon.Text = "";
						GameTransition.eventBalloonIsOpen = false;

						KeyController.keyReturnInterval = true;
					}

					break;
			}
		}
	

//TimeManager

		private void GetNowTime()
		{
			DateTime dateTime = DateTime.Now;
			int seconds, millisec;

			seconds = dateTime.Second;
			millisec = dateTime.Millisecond;

			nowTime = millisec + (seconds * 1000);
		}
	}
}
