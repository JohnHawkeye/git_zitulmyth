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
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		//player parameters
		public static int playerMaxHp = 3;
		public static int playerNowHp;
		public static int playerMaxMana = 5;
		public static int playerNowMana;
		private int damageInvincibleTotal = 0;
		private int damageInvincible = 60;
		private bool flagDamaged = false;

		private Vector playerSize =  new Vector(32, 64);
		public static int playerImageZindex = 5;
		private bool direction = true;	//f:left t:right
		private int weight = 2;
		public static int speed = 2;
		private int jumpPower = 8;
		private int jumpTimes = 1;
		public static int jumpCount = 0;
		public  int jumpMaxHeight = 64;
		public int jumpTotalLength = 0;
		public static bool jumping = false;

		//weapon parameters
		private int subWeaponSpeed = 8;
		private int subWeaponRange = 320;
		private int subWeaponTotalDistance = 0;
		private bool subWeaponDirection;

		//timer
		private Timer timerFrameUpdate;
		public static int countTime = 0;
		private int sePlayTime = 0;
		private bool seStop;
		private int lastTime;
		private int nowTime;
		public static int elapsedTime;

		//Controls
		public static Canvas canScreenFade = new Canvas();
		public static StackPanel stpPlayerStatus;

		//Sound
		System.Media.SoundPlayer seDamage = new System.Media.SoundPlayer("player_damage.wav");
		public static System.Media.SoundPlayer seFog = new System.Media.SoundPlayer("fog.wav");
		public static System.Media.SoundPlayer bgmDarkness = new System.Media.SoundPlayer("darkness.wav");

		public static MediaPlayer mp = new MediaPlayer();

		//input
		private Key? keyDownCursor = null;
		private bool keyDownSpace = false;
		private bool keyDownD = false;
		public static bool keyControlLocking = true;
		private bool keyDownReturn = false;

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

//inits
		private void InitGame()
		{

			this.GetNowTime();
			lastTime = nowTime;

			StageDataSetting.SetData();

			ImageData.ImageLoadFirst();

			this.TitleOpen();

			BalloonMessage.GenerateBalloon(Canvas);
			stpPlayerStatus = Canvas.FindName("PlayerStatus")as StackPanel;
			stpPlayerStatus.Visibility = Visibility.Hidden;

			StageData.imgScenery = new Image { Source = ImageData.cbEmpty,Width=1024,Height=768, };
			this.Canvas.Children.Add(StageData.imgScenery);
			Canvas.SetLeft(StageData.imgScenery, 0);
			Canvas.SetTop(StageData.imgScenery, 0);
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

					NowTimeLabel.Content = nowTime.ToString();

					if (elapsedTime < 0)
					{
						elapsedTime += 59999;
					}

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
							if (GameTransition.eventCount < EventData.listEvent.Count)
							{
								GameTransition.EventController(Canvas);
							}
						}
						else
						{
							if (this.keyDownReturn)
							{
								EnterKeyAction(Canvas);
							}
						}
					}

					if (this.keyDownReturn)
					{
						EnterKeyAction(Canvas);
					}

					if (GameTransition.gameTransition == GameTransitionType.StageDuring)
					{
						this.MovePlayer();
						this.FallingPlayer();
						//this.MoveEnemy();
						//this.FallingEnemy();
						this.SubWeaponPosUpdate();

						this.CollisionPtoE();
						this.CollisionSubWeapon();

						this.PlayerStatusUpdate();
						this.DamageInvinsibleTimer();
					}

					if (seStop)
					{
						if (sePlayTime < 60)
						{
							sePlayTime++;
						}
						else
						{
							sePlayTime = 0;
							seStop = false;
						}
					}

					lastTime = nowTime;

				});
			}
			catch (TaskCanceledException)
			{

			}
		}

//input key
		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			if (!keyControlLocking)
			{
				if (e.Key == Key.Left || e.Key == Key.Right)
				{
					this.keyDownCursor = e.Key;
				}

				if (e.Key == Key.Space)
				{
					this.keyDownSpace = true;

				}

				if (e.Key == Key.D)
				{
					this.keyDownD = true;
				}

			}

			if (e.Key == Key.Return)
			{
				this.keyDownReturn = true;
			}

		}

		private void Window_KeyUp(object sender, KeyEventArgs e)
		{
			if(e.Key ==Key.Left || e.Key == Key.Right)
			{
				this.keyDownCursor = null;
			}

			if(e.Key == Key.Space)
			{
				this.keyDownSpace = false;
			}

			if(e.Key == Key.D)
			{
				this.keyDownD = false;
			}

			if(e.Key == Key.Return)
			{
				this.keyDownReturn = false;
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
					break;

				case GameTransitionType.StageStart:

					BalloonMessage.spnBalloon.Visibility = Visibility.Hidden;
					BalloonMessage.txtBalloon.Text = "";
					GameTransition.eventBalloonIsOpen = false;

					break;

				case GameTransitionType.StageEnd:

					BalloonMessage.spnBalloon.Visibility = Visibility.Hidden;
					BalloonMessage.txtBalloon.Text = "";
					GameTransition.eventBalloonIsOpen = false;

					break;
			}
		}

//player Behavior
		private void MovePlayer()
		{
			double posX = Canvas.GetLeft(ImageData.imgPlayer);
			double posY = Canvas.GetTop(ImageData.imgPlayer);

			if(this.keyDownCursor == Key.Left)
			{
				if (BlockCheck.BlockCheckLeft(posX, posY, speed))
				{
					if (posX - speed > 0)
					{
						posX -= speed;
					}
					this.direction = false;
				}

			}else

			if(this.keyDownCursor == Key.Right)
			{
				if (BlockCheck.BlockCheckRight(posX, posY, speed))
				{

					if(posX+speed < 1024)
					{
						posX += speed;
					}
					this.direction = true;
					
				}
				
			}

			//jump

			if (this.keyDownSpace && jumpCount==0)
			{
				if (BlockCheck.BlockCheckTop(posX, posY, this.jumpPower))
				{
					if (posY - jumpPower > 0)
					{
						jumpCount++;
						
						jumping = true;

					}
				}
			}

			if (jumping)
			{
				if (jumpTotalLength < jumpMaxHeight)
				{
					posY -= jumpPower;
					jumpTotalLength += jumpPower;
				}
				else
				{
					jumping = false;
					jumpTotalLength = 0;
				}
			}

			if(GameTransition.gameTransition == GameTransitionType.StageDuring)
			{
				if (!this.direction)
				{
					ImageData.imgPlayer.Source = ImageData.cbPlayer[1];
				}
				else
				{
					ImageData.imgPlayer.Source = ImageData.cbPlayer[0];
				}
			}


			Canvas.SetLeft(ImageData.imgPlayer, posX);
			Canvas.SetTop(ImageData.imgPlayer, posY);

			//Attack
			if (this.keyDownD)
			{

				if (ImageData.imgSubWeapon.Count == 0)
				{
					var _imgSubWeapon = new Image()
					{
						Source = ImageData.cbSubWeapon,						
						Width = 32,
						Height = 32,
						Name = "sw0",
					};

					ImageData.imgSubWeapon.Add(_imgSubWeapon);
					Canvas.Children.Add(ImageData.imgSubWeapon[0]);

					if (!direction)
					{
						RotateTransform rt = new RotateTransform(180);
						rt.CenterX = 16;
						rt.CenterY = 16;
						ImageData.imgSubWeapon[0].RenderTransform = rt;

						subWeaponDirection = false;
					}
					else
					{
						subWeaponDirection = true;
					}
					
					Canvas.SetLeft(ImageData.imgSubWeapon[0], posX);
					Canvas.SetTop(ImageData.imgSubWeapon[0], posY);
				}

			}
		}

		private void FallingPlayer()
		{
			
			double posX = Canvas.GetLeft(ImageData.imgPlayer);
			double posY = Canvas.GetTop(ImageData.imgPlayer);


			if (BlockCheck.BlockCheckBottom(posX, posY, this.weight))
			{
				if(posY+32 < 23 * 32)
				{
					posY += weight;
				}
					
			}

			Canvas.SetTop(ImageData.imgPlayer, posY);
		}

		//enemy Behavior
		private void MoveEnemy()
		{
			double posX = Canvas.GetLeft(EnemyData.lstSpawnEnemy[0].imgEnemy);
			double posY = Canvas.GetTop(EnemyData.lstSpawnEnemy[0].imgEnemy);

			//move x

			//move jump

			if (this.keyDownSpace && jumpCount == 0)
			{
				if (BlockCheck.BlockCheckTop(posX, posY, this.jumpPower))
				{
					if (posY - jumpPower > 0)
					{
						jumpCount++;

						jumping = true;

					}
				}
			}

			if (jumping)
			{
				if (jumpTotalLength < jumpMaxHeight)
				{
					posY -= jumpPower;
					jumpTotalLength += jumpPower;
				}
				else
				{
					jumping = false;
					jumpTotalLength = 0;
				}
			}


			Canvas.SetLeft(EnemyData.lstSpawnEnemy[0].imgEnemy, posX);
			Canvas.SetTop(EnemyData.lstSpawnEnemy[0].imgEnemy, posY);
		}

		private void FallingEnemy()
		{

			double posX = Canvas.GetLeft(EnemyData.lstSpawnEnemy[0].imgEnemy);
			double posY = Canvas.GetTop(EnemyData.lstSpawnEnemy[0].imgEnemy);


			if (BlockCheck.BlockCheckBottom(posX, posY, this.weight))
			{
				if (posY + 32 < 23 * 32)
				{
					posY += weight;
				}

			}

			Canvas.SetTop(EnemyData.lstSpawnEnemy[0].imgEnemy, posY);
		}

		private void CollisionPtoE()
		{

			if (!flagDamaged)
			{
				for (int i = 0; i < EnemyData.lstSpawnEnemy.Count; i++)
				{

					Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgPlayer), Canvas.GetTop(ImageData.imgPlayer));
					Vector size1 = new Vector(playerSize.X, playerSize.Y);

					Vector p2 = new Vector(Canvas.GetLeft(EnemyData.lstSpawnEnemy[i].imgEnemy),
											Canvas.GetTop(EnemyData.lstSpawnEnemy[i].imgEnemy));
					Vector size2 = new Vector(EnemyData.lstSpawnEnemy[i].enemySize.X, EnemyData.lstSpawnEnemy[i].enemySize.Y);

					if (CollisionCheck.Collision(p1, p2, size1, size2))
					{

						if (!seStop)
						{
							seDamage.Stop();
							seDamage.Play();
							seStop = true;
						}

						if (playerNowHp > 0)
						{
							playerNowHp -= EnemyData.lstSpawnEnemy[i].enemyOfePower;
						}

						flagDamaged = true;
						damageInvincibleTotal = 0;
						Console.WriteLine("Break");
						break;

					}

				}
			}
		}

		private void CollisionSubWeapon()
		{
			for(int i = 0; i < EnemyData.lstSpawnEnemy.Count; i++)
			{
				if (ImageData.imgSubWeapon.Count >= 1)
				{
					Vector p1 = new Vector(Canvas.GetLeft(ImageData.imgSubWeapon[0]), Canvas.GetTop(ImageData.imgSubWeapon[0]));
					Vector size1 = new Vector(32, 32);

					Vector p2 = new Vector(Canvas.GetLeft(EnemyData.lstSpawnEnemy[i].imgEnemy), Canvas.GetTop(EnemyData.lstSpawnEnemy[i].imgEnemy));
					Vector size2 = new Vector(EnemyData.lstSpawnEnemy[i].enemySize.X, EnemyData.lstSpawnEnemy[i].enemySize.Y);

					if (CollisionCheck.Collision(p1, p2, size1, size2))
					{

						Canvas.Children.Remove(ImageData.imgSubWeapon[0]);
						ImageData.imgSubWeapon.Remove(ImageData.imgSubWeapon[0]);

						if (!seStop)
						{
							seFog.Stop();
							seFog.Play();
							seStop = true;
						}

						EnemyData.lstSpawnEnemy[i].enemyHp -= 1;

						if(EnemyData.lstSpawnEnemy[i].enemyHp <= 0)
						{
							bool popOn = false;
							EnemyName name = EnemyName.Zigytu01;

							if(EnemyData.lstSpawnEnemy[i].deathEffect == EnemyDeathEffect.Pop)
							{
								if(GameTransition.gameTransition == GameTransitionType.StageDuring)
								{
									popOn = true;
								}
								
							}

							Canvas.Children.Remove(EnemyData.lstSpawnEnemy[i].imgEnemy);
							EnemyData.lstSpawnEnemy.RemoveAt(i);
							GameTransition.numKillEnemy++;
							Console.WriteLine(GameTransition.numKillEnemy);

							if (popOn)
							{
								if(GameTransition.numKillEnemy < 10)
								{
									SpawnEnemy.SpawnSelect(Canvas, name);
								}
									
							}
						}

						break;
					}
				}
			}
			
		}

		private void PlayerStatusUpdate()
		{
			for(int i = playerMaxHp-1; i >=0; i--)
			{
				if (i > playerNowHp-1)
				{
					ImageData.imgLife[i].Source = ImageData.cbLife[1];
				}
				else
				{
					ImageData.imgLife[i].Source = ImageData.cbLife[0];
				}
				
			}
		}

		private void SubWeaponPosUpdate()
		{
			if (ImageData.imgSubWeapon.Count == 1)
			{
				double posX = Canvas.GetLeft(ImageData.imgSubWeapon[0]);
				double posY = Canvas.GetTop(ImageData.imgSubWeapon[0]);

				if (subWeaponTotalDistance < subWeaponRange)
				{
					if (!subWeaponDirection)
					{
						posX -= subWeaponSpeed;
					}
					else
					{
						posX += subWeaponSpeed;
					}
					
					subWeaponTotalDistance += subWeaponSpeed;
					Canvas.SetLeft(ImageData.imgSubWeapon[0],posX);
				}
				else
				{
					subWeaponTotalDistance = 0;
					ImageData.imgSubWeapon[0].Name = "";
					Canvas.Children.Remove(ImageData.imgSubWeapon[0]);
					ImageData.imgSubWeapon.Remove(ImageData.imgSubWeapon[0]);
					
				}
			}
		}

//TimeManager
		private void DamageInvinsibleTimer()
		{
			if (damageInvincibleTotal < damageInvincible)
			{
				damageInvincibleTotal++;
			}
			else
			{
				damageInvincibleTotal = 0;
				flagDamaged = false;
			}
		}

		private void GetNowTime()
		{
			DateTime dateTime = DateTime.Now;
			int seconds, millisec;

			seconds = dateTime.Second;
			millisec = dateTime.Millisecond;

			nowTime = millisec + (seconds * 1000);
		}

//Generaters
		public static void GenerateEnemy(Canvas canvas ,Vector setpos)
		{
			var _imgEnemy = new Image()
			{
				Source = ImageData.cbEnemy[1],
				Width = 32,
				Height = 64,
			};

			var enemy = new SpawnEnemyList
			{
				enemyName = EnemyName.Zigytu01,
				enemyHp = 1,
				enemySize = new Vector(32, 64),
				enemyStartPos = setpos,
				enemyOfePower = 0,
				enemyDefPower = 0,
				enemySpeed = 0,
				enemyWeight = 1,
				imgEnemy = _imgEnemy,
				deathEffect = EnemyDeathEffect.Pop,
			};

			EnemyData.lstSpawnEnemy.Add(enemy);

			int index = EnemyData.lstSpawnEnemy.Count-1;

			canvas.Children.Add(EnemyData.lstSpawnEnemy[index].imgEnemy);

			Canvas.SetLeft(EnemyData.lstSpawnEnemy[index].imgEnemy, EnemyData.lstSpawnEnemy[index].enemyStartPos.X);
			Canvas.SetTop(EnemyData.lstSpawnEnemy[index].imgEnemy, EnemyData.lstSpawnEnemy[index].enemyStartPos.Y);
		}

	}
}
