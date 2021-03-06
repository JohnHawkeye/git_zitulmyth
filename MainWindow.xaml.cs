﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using Zitulmyth.Checking;
using Zitulmyth.Data;
using Zitulmyth.Enums;

namespace Zitulmyth
{

	public partial class MainWindow : Window
	{

		public bool isDeactivated;

		//timer
		public static Timer timerFrameUpdate;
		public static int countTime = 0;
		private int lastTime;
		private int nowTime;
		public static int elapsedTime;

		//Controls

		public static bool closeMainWindow = false;
		public static Button ctlImageButton;
		public static Button ctlMaterialButton;
		public static Button ctlDatabaseButton;

		public static Canvas mainCanvas;
		public static Label lblMode;
		public static Canvas canScreenFade = new Canvas();
		public static StackPanel stpPlayerStatus;

		public static bool titleStrSwitch = true;

		//window settings
		public static bool isOpenStageEditorWindow = false;
		public static bool isOpenEventEditorWindow = false;
		
		public static StageEditorWindow stageEditor;
		public static EventEditorWindow eventEditor;
		public static MaterialBrowser materialBrowser;
		public static ImageManagerWindow imageManager;
		public static DatabaseWindow databaseWindow;

		public static int gameWindowWidth = 1024;
		public static int gameWindowHeight = 768;

		//mouse
		public static Vector mouseMainCanvasPosition;
		public static bool isMouseLeftClicked;
		public static bool isMouseRightClicked;
		public static bool isMouseDragged;

		//mainwindow
		public MainWindow()
		{
			this.InitializeComponent();

			timerFrameUpdate = new Timer(40);
			timerFrameUpdate.Elapsed += FrameUpdateTimer_Update;
			timerFrameUpdate.Start();

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

			ImageData.SpriteReading();				//cropped image data

			StageManager.stageNum = 0;
			StageOrder.OrderListInit();				//stage order
			StageDataSetting.DataBaseReading();		//database
			

			ImageData.SystemImagesReading();
			Sound.SoundEffectLoad(Canvas);
			
			SplashLogoOpen();


			//controlls maked
			mainCanvas = this.FindName("Canvas") as Canvas;
			lblMode = FindName("lblGameMode") as Label;

			ctlDatabaseButton = btnViewDatabaseWindow;
			ctlMaterialButton = btnViewMaterialBrowser;
			ctlImageButton = btnViewImageManager;

			CollisionCheck.ColliderCheckMaskGenerater(Canvas);
			MainWeapon.InitMainWeapon(Canvas);

			BalloonMessage.GenerateBalloon(Canvas);
			stpPlayerStatus = Canvas.FindName("spPlayerStatus")as StackPanel;
			stpPlayerStatus.Visibility = Visibility.Hidden;
			Canvas.SetZIndex(spPlayerStatus, ImageZindex.status);

			var _popcantalk = new Image
			{
				Source = ImageData.cbSystem,
				Width = 64,Height=32,
				Visibility = Visibility.Hidden,
			};

			ImageData.imgPopCanTalk = _popcantalk;
			Canvas.Children.Add(ImageData.imgPopCanTalk);
			Canvas.SetZIndex(ImageData.imgPopCanTalk, ImageZindex.status);


			ImageData.imgHandCursor = new Image
			{
				Source = ImageData.cbHandCursor,
				Width = 32,	Height = 32,
				Visibility = Visibility.Hidden,
			};

			Canvas.Children.Add(ImageData.imgHandCursor);
			Canvas.SetZIndex(ImageData.imgHandCursor, ImageZindex.handCursor);

			canScreenFade.Width = 1024;
			canScreenFade.Height = 768;
			canScreenFade.Visibility = Visibility.Hidden;
			Canvas.Children.Add(canScreenFade);
			Canvas.SetLeft(canScreenFade, 0);
			Canvas.SetTop(canScreenFade, 0);
			Canvas.SetZIndex(canScreenFade, ImageZindex.fade);

			StageInit.InitPlayer(Canvas);
			StageInit.InitPlayerStatus(CaLife, CaMana);
		}

		private void SplashLogoOpen()
		{
			ImageData.imgTitle[0] = new Image
			{
				Source = ImageData.cbSplash,
				Width = 1024,
				Height = 768,
			};

			ImageData.imgTitle[0].Opacity = 0;
			this.Canvas.Children.Add(ImageData.imgTitle[0]);
			Canvas.SetLeft(ImageData.imgTitle[0], 0);
			Canvas.SetTop(ImageData.imgTitle[0], 0);

		}

		public static void TitleOpen(Canvas canvas)
		{

			canvas.Children.Remove(ImageData.imgTitle[0]);

			ImageData.imgTitle[0] = new Image
			{
				Source = ImageData.cbTitle[0],
				Width = 1024,Height = 768,
			};

			canvas.Children.Add(ImageData.imgTitle[0]);

			ImageData.imgTitle[1] = new Image
			{
				Source = ImageData.cbTitle[1],Width = 448,Height = 32,
			};

			ImageData.imgTitle[0].Opacity = 1;
			canvas.Children.Add(ImageData.imgTitle[1]);
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

					//debug
					lblDebugA.Content = PlayerStatus.playerPos.X +"," + PlayerStatus.playerPos.Y;
					lblDebugB.Content = SystemOperator.PixelPerSecond(PlayerStatus.weight);

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
						if (GameTransition.lstEventTask.Count > 0)
						{
							GameTransition.EventTaskCommander();
						}


						if (GameTransition.charaRenderStart)
						{
							GameTransition.CharaRender();
						}

						if (GameTransition.screenFadeStart)
						{
							GameTransition.ScreenFade(Canvas);
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
					
	//got to Edit Mode
					if(GameTransition.gameTransition == GameTransitionType.Title)
					{
						if (KeyController.keyE)
						{
							mainCanvas.Children.Remove(ImageData.imgTitle[0]); mainCanvas.Children.Remove(ImageData.imgTitle[1]);
							countTime = 0;
							GameTransition.gameTransition = GameTransitionType.EditMode;
							timerFrameUpdate.Stop();
							btnViewStageEditorWindow.IsEnabled = true;
							btnViewMaterialBrowser.IsEnabled = true;
							btnViewImageManager.IsEnabled = true;
							btnViewDatabaseWindow.IsEnabled = true;

							lblMode.Content = "ゲームモード：エディット";
						}
					}
					
	//StageDuring game play
					if (GameTransition.gameTransition == GameTransitionType.StageDuring &&
						!isDeactivated)
					{
						Animator.AnimationItem();
						Animator.AnimationObject();

						if (ObjectChecker.isTrigger && !TalkCommander.isTalk)
						{
							ObjectBehavior.OnTriggerTouchEvent();
						}

						if (TalkCommander.isTalk && !TalkCommander.isTalkOpenBalloon)
						{
							TalkCommander.TalkWithNpc(Canvas);
						}

						if (TalkCommander.isTalkSelecting)
						{
							TalkCommander.TalkSelecting(Canvas);
						}

						//first action,last Processing including deletion of list
						SystemOperator.moveCommonAmountX = 0;
						SystemOperator.moveCommonAmountY = 0;

						ObjectChecker.CollisionPtoActionCollider();
						PlayerBehavior.CollisionPtoE();

						PlayerBehavior.MovePlayer(Canvas);
						PlayerBehavior.FallingPlayer();

						Item.FallingItems();

						EnemyBehavior.EnemyAction();
						Animator.AnimationEnemy();

						SubWeapon.SubWeaponPosUpdate(Canvas);

						if (PlayerStatus.isMainAttack)
						{
							MainWeapon.MainWeaponAttack(Canvas);
							MainWeapon.MainWeaponCollision(Canvas);
						}
						
						
						SubWeapon.CollisionSubWeapon(Canvas);

						PlayerStatus.PlayerStatusUpdate();
						PlayerBehavior.DamageInvinsibleTimer();

						SpawnEnemy.RemoveEnemy(Canvas);
						SpawnEnemy.ReSpawnEnemy(Canvas);
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

					if (GameTransition.endSplashLogo)
					{
						Sound.bgm.Stop();
						canvas.Children.Remove(ImageData.imgTitle[0]); canvas.Children.Remove(ImageData.imgTitle[1]);
						countTime = 0;
						lblMode.Content = "ゲームモード：ステージ準備";
						GameTransition.gameTransition = GameTransitionType.StageInit;

					}
					
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
						if (TalkCommander.isTalkOpenBalloon)
						{
							TalkCommander.TalkEnterTheNext();
						}
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

//control event
		private void GameWindow_Closed(object sender, EventArgs e)
		{
			if (isOpenStageEditorWindow)
			{
				stageEditor.Close();
			}
			
		}

		private void GameWindow_Activated(object sender, EventArgs e)
		{
			isDeactivated = false;
		}

		private void GameWindow_Deactivated(object sender, EventArgs e)
		{
			isDeactivated = true;
		}

		private void Canvas_MouseMove(object sender, MouseEventArgs e)
		{

			Vector blockPos;

			switch (StageEditorOperator.paletteMode)
			{
				case PaletteMode.Player:
					Point point = e.GetPosition(mainCanvas);

					if (point.X < 1024 - 32)
					{
						Canvas.SetLeft(StageEditorOperator.imgEditorPlayer, point.X);
						mouseMainCanvasPosition.X = point.X;
					}
					else
					{
						Canvas.SetLeft(StageEditorOperator.imgEditorPlayer, 1024 - 32);
						mouseMainCanvasPosition.X = 1024 - 32;
					}

					if (point.Y < 768 - 64)
					{
						Canvas.SetTop(StageEditorOperator.imgEditorPlayer, point.Y);
						mouseMainCanvasPosition.Y = point.Y;
					}
					else
					{
						Canvas.SetTop(StageEditorOperator.imgEditorPlayer, 768 - 64);
						mouseMainCanvasPosition.Y = 768 - 64;
					}
					break;

				case PaletteMode.Block:
					blockPos = SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
					Canvas.SetLeft(StageEditorOperator.imgEditorPointerCursor, (blockPos.X - 1) * 32);
					Canvas.SetTop(StageEditorOperator.imgEditorPointerCursor, (blockPos.Y - 1) * 32);
					break;

				case PaletteMode.Object:
					blockPos = SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
					Canvas.SetLeft(StageEditorOperator.imgEditorPointerCursor, (blockPos.X - 1) * 32);
					Canvas.SetTop(StageEditorOperator.imgEditorPointerCursor, (blockPos.Y - 1) * 32);
					break;

				case PaletteMode.Enemy:
					blockPos = SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
					Canvas.SetLeft(StageEditorOperator.imgEditorPointerCursor, (blockPos.X - 1) * 32);
					Canvas.SetTop(StageEditorOperator.imgEditorPointerCursor, (blockPos.Y - 1) * 32);
					break;

				case PaletteMode.Item:
					blockPos = SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
					Canvas.SetLeft(StageEditorOperator.imgEditorPointerCursor, (blockPos.X - 1) * 32);
					Canvas.SetTop(StageEditorOperator.imgEditorPointerCursor, (blockPos.Y - 1) * 32);
					break;
			}

		}

		private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{

			if (!isMouseLeftClicked)
			{
				Vector blockPos;

				switch (StageEditorOperator.paletteMode)
				{
					case PaletteMode.Player:

						StageEditorOperator.EditorPlayerStartPosDecision();
						break;

					case PaletteMode.Block:

						blockPos =  SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
						StageEditorOperator.EditSetupBlockOnMainCanvas(blockPos);

						break;

					case PaletteMode.Object:

						blockPos = SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
						StageEditorOperator.EditSetupObjectOnMainCanvas(blockPos);

						break;

					case PaletteMode.Enemy:
						blockPos = SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
						StageEditorOperator.EditSetupEnemyOnMainCanvas(blockPos);
						break;

					case PaletteMode.Item:
						blockPos = SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
						StageEditorOperator.EditSetupItemOnMainCanvas(blockPos);
						break;
				}

				
				isMouseLeftClicked = true;
			}
			
		}

		private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			isMouseLeftClicked = false;
		}

		private void Canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!isMouseRightClicked)
			{
				Vector blockPos;
				switch (StageEditorOperator.paletteMode)
				{
					case PaletteMode.Player:

						StageEditorOperator.imgEditorPlayer.Opacity = 1;
					
						Canvas.SetLeft(StageEditorOperator.imgEditorPlayer, StageEditorOperator.memoryPlayerStartPos.X);
						Canvas.SetTop(StageEditorOperator.imgEditorPlayer, StageEditorOperator.memoryPlayerStartPos.Y);

						StageEditorOperator.paletteMode = PaletteMode.None;
						StageEditorWindow.ctlGridMain.IsEnabled = true;
						stageEditor.Focus();
						break;

					case PaletteMode.Block:

						blockPos = SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
						StageEditorOperator.EditRemoveBlockOnMainCanvas(blockPos);

						break;

					case PaletteMode.Object:
						blockPos = SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
						StageEditorOperator.EditRemoveObjectOnMainCanvas(blockPos);
						break;

					case PaletteMode.Enemy:
						blockPos = SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
						StageEditorOperator.EditRemoveEnemyOnMainCanvas(blockPos);
						break;

					case PaletteMode.Item:
						blockPos = SystemOperator.FromCodeToBlocks(e.GetPosition(mainCanvas));
						StageEditorOperator.EditRemoveItemOnMainCanvas(blockPos);
						break;
				}

				isMouseRightClicked = true;
			}
		
		}

		private void Canvas_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			isMouseRightClicked = false;
		}

		private void btnViewStageEditorWindow_Click(object sender, RoutedEventArgs e)
		{
			if (!isOpenStageEditorWindow)
			{
				stageEditor = new StageEditorWindow();
				stageEditor.Show();
				stageEditor.Focus();
				isOpenStageEditorWindow = true;

				btnViewDatabaseWindow.IsEnabled = false;
				btnViewMaterialBrowser.IsEnabled = false;
				btnViewImageManager.IsEnabled = false;
				
			}
			
		}

		private void btnViewMaterialBrowser_Click(object sender, RoutedEventArgs e)
		{
			materialBrowser = new MaterialBrowser();
			materialBrowser.ShowDialog();
			materialBrowser.Focus();
		}

		private void btnViewImageManager_Click(object sender, RoutedEventArgs e)
		{
			imageManager = new ImageManagerWindow();
			imageManager.ShowDialog();
			imageManager.Focus();
		}

		private void btnViewDatabaseWindow_Click(object sender, RoutedEventArgs e)
		{
			databaseWindow = new DatabaseWindow();
			databaseWindow.ShowDialog();
			
		}
	}
}
