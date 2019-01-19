using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Zitulmyth.Data;

namespace Zitulmyth
{
	public class DatabasePlayer
	{
		public string spriteIdleL;
		public string spriteIdleR;
		public string spriteMoveL;
		public string spriteMoveR;
		public string spriteAttackL;
		public string spriteAttackR;
		public string spriteJumpL;
		public string spriteJumpR;
		public string spriteDamageL;
		public string spriteDamageR;
		public string spriteLadder;
		public string spriteSquatL;
		public string spriteSquatR;
		public string spriteDeathL;
		public string spriteDeathR;
	}

	public class DatabaseBlock
	{
		public string name { get; set; }
		public string sprite { get; set; }
		public bool passage { get; set; }
	}

	public class DatabaseEnemy
	{
		public string name { get; set; }
		public string sprite { get; set; }
	}

	public class DatabaseObject
	{
		public string name { get; set; }
		public string spriteA { get; set; }
		public string spriteB { get; set; }
		public Vector size { get; set; }
		public ObjectAttribute attribute { get; set; }
		public bool destructable { get; set; }
		public int durability { get; set; }
		public bool switchingTimer { get; set; }
		public int targetTime { get; set; }
		public bool damaging { get; set; }
		public int damagevalue { get; set; }
		public bool underwater { get; set; }
		public bool slide { get; set; }
		public bool operable { get; set; }
		public bool automove { get; set; }
		public int autoMoveRangeX { get; set; }
		public int autoMoveRangeY { get; set; }
		public int autoMoveSpeed { get; set; }
		public bool triggerAction { get; set; }
		public bool triggerType { get; set; }
		public int influenceSpeed{ get; set; }
		public int influenceJump { get; set; }
		public int influenceFall { get; set; }
	}

	public class DatabaseItem
	{
		public string name { get; set; }
		public string sprite { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public partial class DatabaseWindow : Window
	{

		public DatabasePlayer lstViewDbPlayer = new DatabasePlayer();

		public List<DatabaseBlock> lstViewDbBlock = new List<DatabaseBlock>();
		public List<DatabaseEnemy> lstViewDbEnemy = new List<DatabaseEnemy>();
		public List<DatabaseObject> lstViewDbObject = new List<DatabaseObject>();
		public List<DatabaseItem> lstViewDbItem = new List<DatabaseItem>();

		public List<string> lstNameBlock = new List<string>();
		public List<string> lstNameEnemy = new List<string>();
		public List<string> lstNameObject = new List<string>();
		public List<string> lstNameItem = new List<string>();

		const string dbFileNamePlayer = "Assets/json/data/data_player.json";
		const string dbFileNameBlock = "Assets/json/data/data_block.json";
		const string dbFileNameEnemy = "Assets/json/data/data_enemy.json";
		const string dbFileNameObject = "Assets/json/data/data_object.json";
		const string dbFileNameItem = "Assets/json/data/data_item.json";

		public static MaxListChangerDialog maxListChangerDialog;
		public static int maxListNum;

		public static SpriteViewerDialog spriteViewerDialog;
		public CategoryName spriteCategory;
		public int selectedIndex = 0;

		public Image ctlRefSpritePlayer;
		public bool choiceSpriteObject;


		public DatabaseWindow()
		{
			InitializeComponent();
		}

		private void btnUpdate_Click(object sender, RoutedEventArgs e)
		{

			if (tabDatabase.SelectedIndex >= 0)
			{
				btnUpdate.Content = "書き込み中";
				btnUpdate.IsEnabled = false;

				DataContractJsonSerializer json;
				FileStream fs;

				switch (tabDatabase.SelectedIndex)
				{
					case 0:

						lstViewDbPlayer.spriteIdleL = imgPlayerIdleL.Tag.ToString();
						lstViewDbPlayer.spriteIdleR = imgPlayerIdleR.Tag.ToString();
						lstViewDbPlayer.spriteMoveL = imgPlayerMoveL.Tag.ToString();
						lstViewDbPlayer.spriteMoveR = imgPlayerMoveR.Tag.ToString();
						lstViewDbPlayer.spriteAttackL = imgPlayerAttackL.Tag.ToString();
						lstViewDbPlayer.spriteAttackR = imgPlayerAttackR.Tag.ToString();
						lstViewDbPlayer.spriteJumpL = imgPlayerJumpL.Tag.ToString();
						lstViewDbPlayer.spriteJumpR = imgPlayerJumpR.Tag.ToString();
						lstViewDbPlayer.spriteDamageL = imgPlayerDamageL.Tag.ToString();
						lstViewDbPlayer.spriteDamageR = imgPlayerDamageR.Tag.ToString();
						lstViewDbPlayer.spriteLadder = imgPlayerLadder.Tag.ToString();
						lstViewDbPlayer.spriteSquatL = imgPlayerSquatL.Tag.ToString();
						lstViewDbPlayer.spriteSquatR = imgPlayerSquatR.Tag.ToString();
						lstViewDbPlayer.spriteDeathL = imgPlayerDeathL.Tag.ToString();
						lstViewDbPlayer.spriteDeathR = imgPlayerDeathR.Tag.ToString();

						json = new DataContractJsonSerializer(typeof(DatabasePlayer));
						fs = new FileStream(dbFileNamePlayer, FileMode.Create);

						try
						{ json.WriteObject(fs, lstViewDbPlayer); }
						finally
						{ fs.Close(); }

						break;

					case 1:

						json = new DataContractJsonSerializer(typeof(List<DatabaseBlock>));
						fs = new FileStream(dbFileNameBlock, FileMode.Create);

						try
						{ json.WriteObject(fs, lstViewDbBlock); }
						finally
						{ fs.Close(); }

						break;

					case 2:
						json = new DataContractJsonSerializer(typeof(List<DatabaseEnemy>));
						fs = new FileStream(dbFileNameEnemy, FileMode.Create);

						try
						{ json.WriteObject(fs, lstViewDbEnemy); }
						finally
						{ fs.Close(); }
						break;

					case 3:
						json = new DataContractJsonSerializer(typeof(List<DatabaseObject>));
						fs = new FileStream(dbFileNameObject, FileMode.Create);

						try
						{ json.WriteObject(fs, lstViewDbObject); }
						finally
						{ fs.Close(); }
						break;

					case 4:
						json = new DataContractJsonSerializer(typeof(List<DatabaseItem>));
						fs = new FileStream(dbFileNameItem, FileMode.Create);

						try
						{ json.WriteObject(fs, lstViewDbItem); }
						finally
						{ fs.Close(); }
						break;

				}

				btnUpdate.Content = "適用";
				btnUpdate.IsEnabled = true;
			}


		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			DatabaseReading();

			PlayerGraphicsUpdate();
			ListBoxBlockUpdate();
			ListBoxObjectUpdate();
		}

		private void DatabaseReading()
		{

			//player
			if (File.Exists(dbFileNamePlayer))
			{
				DataContractJsonSerializer json_p = new DataContractJsonSerializer(typeof(DatabasePlayer));
				FileStream fs_p = new FileStream(dbFileNamePlayer, FileMode.Open);

				try { lstViewDbPlayer = (DatabasePlayer)json_p.ReadObject(fs_p); }
				finally { fs_p.Close(); }
			}
			else
			{
				MessageBox.Show("data_player.json ファイルがありません", "ファイルチェック", MessageBoxButton.OK, MessageBoxImage.Information);
			}


			//block
			if (File.Exists(dbFileNameBlock))
			{
				DataContractJsonSerializer json_b = new DataContractJsonSerializer(typeof(List<DatabaseBlock>));
				FileStream fs_b = new FileStream(dbFileNameBlock, FileMode.Open);

				try { lstViewDbBlock = (List<DatabaseBlock>)json_b.ReadObject(fs_b); }
				finally { fs_b.Close(); }
			}
			else
			{
				MessageBox.Show("data_block.json ファイルがありません", "ファイルチェック", MessageBoxButton.OK, MessageBoxImage.Information);
			}


			//enemy
			if (File.Exists(dbFileNameEnemy))
			{
				DataContractJsonSerializer json_e = new DataContractJsonSerializer(typeof(List<DatabaseEnemy>));
				FileStream fs_e = new FileStream(dbFileNameEnemy, FileMode.Open);

				try { lstViewDbEnemy = (List<DatabaseEnemy>)json_e.ReadObject(fs_e); }
				finally { fs_e.Close(); }

			}
			else
			{
				MessageBox.Show("data_enemy.json ファイルがありません", "ファイルチェック", MessageBoxButton.OK, MessageBoxImage.Information);
			}


			//object
			if (File.Exists(dbFileNameObject))
			{
				DataContractJsonSerializer json_o = new DataContractJsonSerializer(typeof(List<DatabaseObject>));
				FileStream fs_o = new FileStream(dbFileNameObject, FileMode.Open);

				try { lstViewDbObject = (List<DatabaseObject>)json_o.ReadObject(fs_o); }
				finally { fs_o.Close(); }

			}
			else
			{
				MessageBox.Show("data_object.json ファイルがありません", "ファイルチェック", MessageBoxButton.OK, MessageBoxImage.Information);
			}

			//item
			if (File.Exists(dbFileNameItem))
			{
				DataContractJsonSerializer json_i = new DataContractJsonSerializer(typeof(List<DatabaseItem>));
				FileStream fs_i = new FileStream(dbFileNameItem, FileMode.Open);

				try { lstViewDbItem = (List<DatabaseItem>)json_i.ReadObject(fs_i); }
				finally { fs_i.Close(); }

			}
			else
			{
				MessageBox.Show("data_item.json ファイルがありません", "ファイルチェック", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		public void PlayerGraphicsUpdate()
		{

			imgPlayerIdleL.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteIdleL);
			imgPlayerIdleR.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteIdleR);
			imgPlayerMoveL.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteMoveL);
			imgPlayerMoveR.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteMoveR);
			imgPlayerAttackL.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteAttackL);
			imgPlayerAttackR.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteAttackR);
			imgPlayerJumpL.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteJumpL);
			imgPlayerJumpR.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteJumpR);
			imgPlayerDamageL.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteDamageL);
			imgPlayerDamageR.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteDamageR);
			imgPlayerLadder.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteLadder);
			imgPlayerSquatL.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteSquatL);
			imgPlayerSquatR.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteSquatR);
			imgPlayerDeathL.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteDeathL);
			imgPlayerDeathR.Source = ImageData.ImageSourceSelector(CategoryName.Player, lstViewDbPlayer.spriteDeathR);

		}

		public void ListBoxBlockUpdate()
		{
			lstNameBlock.Clear();

			for (int i = 0; i < lstViewDbBlock.Count; i++)
			{
				lstNameBlock.Add(i.ToString() + ": " + lstViewDbBlock[i].name);
			}

			lsbBlock.ItemsSource = lstNameBlock;
			lsbBlock.Items.Refresh();
			lsbBlock.SelectedIndex = 0;
		}

		public void ListBoxObjectUpdate()
		{
			lstNameObject.Clear();

			for (int i = 0; i < lstViewDbObject.Count; i++)
			{
				lstNameObject.Add(i.ToString() + ": " + lstViewDbObject[i].name);
			}

			lsbObject.ItemsSource = lstNameObject;
			lsbObject.Items.Refresh();
			lsbObject.SelectedIndex = 0;
		}

		private void imgPlayerIdleL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerIdleR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerMoveL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerMoveR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerAttackL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerAttackR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerJumpL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerJumpR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerDamageL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerDamageR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerLadder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerSquatL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerSquatR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerDeathL_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		private void imgPlayerDeathR_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			imgPlayerClick((Image)sender);
		}

		/// <summary>
		/// 
		/// </summary>
		/// 
		private void btnBlockMaxList_Click(object sender, RoutedEventArgs e)
		{
			maxListChangerDialog = new MaxListChangerDialog();


			maxListChangerDialog.oldnum = lstViewDbBlock.Count;
			maxListChangerDialog.txbNumber.Text = lstViewDbBlock.Count.ToString();
			maxListChangerDialog.ShowDialog();
		}

		private void lsbBlock_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (lsbBlock.SelectedIndex >= 0)
			{
				int index = lsbBlock.SelectedIndex;

				txbBlockName.Text = lstViewDbBlock[index].name;
				imgBlockCB.Source = ImageData.ImageSourceSelector(CategoryName.Block, lstViewDbBlock[index].sprite);
				ckbPassage.IsChecked = lstViewDbBlock[index].passage;
			}
		}

		private void ckbPassage_Click(object sender, RoutedEventArgs e)
		{
			if (lsbBlock.SelectedIndex >= 0)
			{
				int index = lsbBlock.SelectedIndex;

				lstViewDbBlock[index].passage = (bool)ckbPassage.IsChecked;

			}
		}

		private void txbBlockName_KeyDown(object sender, KeyEventArgs e)
		{
			if (lsbBlock.SelectedIndex >= 0)
			{
				if (e.Key == Key.Return)
				{
					int index = lsbBlock.SelectedIndex;

					lstViewDbBlock[index].name = txbBlockName.Text;

					ListBoxBlockUpdate();
					lsbBlock.SelectedIndex = index;
				}

			}
		}

		private void imgBlockCB_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			selectedIndex = lsbBlock.SelectedIndex;
			spriteViewerDialog = new SpriteViewerDialog();
			spriteCategory = CategoryName.Block;
			spriteViewerDialog.ShowDialog();

		}

		private void Window_Closed(object sender, EventArgs e)
		{
			StageDataSetting.DataBaseReading(); //main table reload
		}

		private void imgPlayerClick(Image img)
		{
			ctlRefSpritePlayer = img;

			spriteViewerDialog = new SpriteViewerDialog();
			spriteCategory = CategoryName.Player;
			spriteViewerDialog.ShowDialog();
		}

		private void lsbObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				txbObjectName.Text = lstViewDbObject[index].name;
				imgObjectA.Source = ImageData.ImageSourceSelector(CategoryName.Object, lstViewDbObject[index].spriteA);
				imgObjectB.Source = ImageData.ImageSourceSelector(CategoryName.Object, lstViewDbObject[index].spriteB);
				switch (lstViewDbObject[index].attribute)
				{
					case ObjectAttribute.DisplayOnly:
						rdbObjDisplay.IsChecked = true;
						break;

					case ObjectAttribute.Physicality:
						rdbObjPhysical.IsChecked = true;
						break;

					case ObjectAttribute.EmptyCollider:
						rdbObjCollider.IsChecked = true;
						break;

					case ObjectAttribute.Ladder:
						rdbObjLadder.IsChecked = true;
						break;

					case ObjectAttribute.Platform:
						rdbObjPlat.IsChecked = true;
						break;

					case ObjectAttribute.Goalgate:
						rdbObjGoal.IsChecked = true;
						break;
				}
				ckbObjDestructable.IsChecked = lstViewDbObject[index].destructable;
				txbObjDurability.Text = lstViewDbObject[index].durability.ToString();
				ckbObjTimer.IsChecked = lstViewDbObject[index].switchingTimer;
				txbTime.Text = lstViewDbObject[index].targetTime.ToString();
				ckbObjDamage.IsChecked = lstViewDbObject[index].damaging;
				txbObjDamage.Text = lstViewDbObject[index].damagevalue.ToString();
				ckbObjWater.IsChecked = lstViewDbObject[index].underwater;
				ckbObjSlide.IsChecked = lstViewDbObject[index].slide;
				ckbObjOperable.IsChecked = lstViewDbObject[index].operable;
				ckbObjAutoMove.IsChecked = lstViewDbObject[index].automove;
				txbObjAutoX.Text = lstViewDbObject[index].autoMoveRangeX.ToString();
				txbObjAutoY.Text = lstViewDbObject[index].autoMoveRangeY.ToString();
				txbObjAutoSpeed.Text = lstViewDbObject[index].autoMoveSpeed.ToString();
				ckbObjTriggerAction.IsChecked = lstViewDbObject[index].triggerAction;
				ckbObjTriggerType.IsChecked = lstViewDbObject[index].triggerType;
				txbObjInfluenceSpeed.Text = lstViewDbObject[index].influenceSpeed.ToString();
				txbObjInfluenceJump.Text = lstViewDbObject[index].influenceJump.ToString();
				txbObjInfluenceFall.Text = lstViewDbObject[index].influenceFall.ToString();

			}
		}

		private void btnObjectMaxList_Click(object sender, RoutedEventArgs e)
		{
			maxListChangerDialog = new MaxListChangerDialog();


			maxListChangerDialog.oldnum = lstViewDbObject.Count;
			maxListChangerDialog.txbNumber.Text = lstViewDbObject.Count.ToString();
			maxListChangerDialog.ShowDialog();
		}

		private void txbObjectName_KeyDown(object sender, KeyEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				if (e.Key == Key.Return)
				{
					int index = lsbObject.SelectedIndex;

					lstViewDbObject[index].name = txbObjectName.Text;

					ListBoxObjectUpdate();
					lsbObject.SelectedIndex = index;
				}

			}
		}

		private void imgObjectA_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			choiceSpriteObject = false;
			selectedIndex = lsbObject.SelectedIndex;
			spriteViewerDialog = new SpriteViewerDialog();
			spriteCategory = CategoryName.Object;
			spriteViewerDialog.ShowDialog();
		}

		private void imgObjectB_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			choiceSpriteObject = true;
			selectedIndex = lsbObject.SelectedIndex;
			spriteViewerDialog = new SpriteViewerDialog();
			spriteCategory = CategoryName.Object;
			spriteViewerDialog.ShowDialog();
		}

		private void txbObjDurability_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (SystemOperator.IsNumeric(txbObjDurability.Text))
				{
					if (lsbObject.SelectedIndex >= 0)
					{

						int index = lsbObject.SelectedIndex;

						lstViewDbObject[index].durability = int.Parse(txbObjDurability.Text);

					}
				}
				else
				{
					MessageBox.Show("数値を入力してください。");

				}


			}

		}

		private void txbTime_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (SystemOperator.IsNumeric(txbTime.Text))
				{
					if (lsbObject.SelectedIndex >= 0)
					{

						int index = lsbObject.SelectedIndex;

						lstViewDbObject[index].targetTime = int.Parse(txbTime.Text);

					}
				}
				else
				{
					MessageBox.Show("数値を入力してください。");

				}
			}
		}

		private void txbObjDamage_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (SystemOperator.IsNumeric(txbObjDamage.Text))
				{
					if (lsbObject.SelectedIndex >= 0)
					{

						int index = lsbObject.SelectedIndex;

						lstViewDbObject[index].damagevalue = int.Parse(txbObjDamage.Text);

					}
				}
				else
				{
					MessageBox.Show("数値を入力してください。");

				}
			}
		}

		private void txbObjAutoX_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (SystemOperator.IsNumeric(txbObjAutoX.Text))
				{
					if (lsbObject.SelectedIndex >= 0)
					{

						int index = lsbObject.SelectedIndex;

						lstViewDbObject[index].autoMoveRangeX = int.Parse(txbObjAutoX.Text);

					}
				}
				else
				{
					MessageBox.Show("数値を入力してください。");

				}
			}
		}

		private void txbObjAutoY_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (SystemOperator.IsNumeric(txbObjAutoY.Text))
				{
					if (lsbObject.SelectedIndex >= 0)
					{

						int index = lsbObject.SelectedIndex;

						lstViewDbObject[index].autoMoveRangeY = int.Parse(txbObjAutoY.Text);

					}
				}
				else
				{
					MessageBox.Show("数値を入力してください。");

				}
			}
		}

		private void txbObjAutoSpeed_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (SystemOperator.IsNumeric(txbObjAutoSpeed.Text))
				{
					if (lsbObject.SelectedIndex >= 0)
					{

						int index = lsbObject.SelectedIndex;

						lstViewDbObject[index].autoMoveSpeed = int.Parse(txbObjAutoSpeed.Text);

					}
				}
				else
				{
					MessageBox.Show("数値を入力してください。");

				}
			}
		}

		private void txbObjInfluenceSpeed_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (SystemOperator.IsNumeric(txbObjInfluenceSpeed.Text))
				{
					if (lsbObject.SelectedIndex >= 0)
					{

						int index = lsbObject.SelectedIndex;

						lstViewDbObject[index].influenceSpeed = int.Parse(txbObjInfluenceSpeed.Text);

					}
				}
				else
				{
					MessageBox.Show("数値を入力してください。");

				}
			}
		}

		private void txbObjInfluenceJump_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (SystemOperator.IsNumeric(txbObjInfluenceJump.Text))
				{
					if (lsbObject.SelectedIndex >= 0)
					{

						int index = lsbObject.SelectedIndex;

						lstViewDbObject[index].influenceJump = int.Parse(txbObjInfluenceJump.Text);

					}
				}
				else
				{
					MessageBox.Show("数値を入力してください。");

				}
			}
		}

		private void txbObjInfluenceFall_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				if (SystemOperator.IsNumeric(txbObjInfluenceFall.Text))
				{
					if (lsbObject.SelectedIndex >= 0)
					{

						int index = lsbObject.SelectedIndex;

						lstViewDbObject[index].influenceFall = int.Parse(txbObjInfluenceFall.Text);


					}
				}
				else
				{
					MessageBox.Show("数値を入力してください。");

				}
			}

		}

		private void rdbObjDisplay_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				if ((bool)rdbObjDisplay.IsChecked)
				{
					lstViewDbObject[index].attribute = ObjectAttribute.DisplayOnly;
				}

			}
		}

		private void rdbObjPhysical_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				if ((bool)rdbObjPhysical.IsChecked)
				{
					lstViewDbObject[index].attribute = ObjectAttribute.Physicality;
				}

			}
		}

		private void rdbObjCollider_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				if ((bool)rdbObjCollider.IsChecked)
				{
					lstViewDbObject[index].attribute = ObjectAttribute.EmptyCollider;
				}

			}
		}

		private void rdbObjLadder_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				if ((bool)rdbObjLadder.IsChecked)
				{
					lstViewDbObject[index].attribute = ObjectAttribute.Ladder;
				}

			}
		}

		private void rdbObjPlat_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				if ((bool)rdbObjPlat.IsChecked)
				{
					lstViewDbObject[index].attribute = ObjectAttribute.Platform;
				}

			}
		}

		private void rdbObjGoal_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				if ((bool)rdbObjGoal.IsChecked)
				{
					lstViewDbObject[index].attribute = ObjectAttribute.Goalgate;
				}

			}
		}

		private void ckbObjDestructable_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				lstViewDbObject[index].destructable = (bool)ckbObjDestructable.IsChecked;
				
			}
		}

		private void ckbObjTimer_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				lstViewDbObject[index].switchingTimer = (bool)ckbObjTimer.IsChecked;

			}
		}

		private void ckbObjDamage_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				lstViewDbObject[index].damaging = (bool)ckbObjDamage.IsChecked;

			}
		}

		private void ckbObjWater_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				lstViewDbObject[index].underwater = (bool)ckbObjWater.IsChecked;

			}
		}

		private void ckbObjSlide_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				lstViewDbObject[index].slide = (bool)ckbObjSlide.IsChecked;

			}
		}

		private void ckbObjOperable_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				lstViewDbObject[index].operable = (bool)ckbObjOperable.IsChecked;

			}
		}

		private void ckbObjAutoMove_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				lstViewDbObject[index].automove = (bool)ckbObjAutoMove.IsChecked;

			}
		}

		private void ckbObjTriggerAction_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				lstViewDbObject[index].triggerAction = (bool)ckbObjTriggerAction.IsChecked;

			}
		}

		private void ckbObjTriggerType_Click(object sender, RoutedEventArgs e)
		{
			if (lsbObject.SelectedIndex >= 0)
			{
				int index = lsbObject.SelectedIndex;

				lstViewDbObject[index].triggerType = (bool)ckbObjTriggerType.IsChecked;

			}
		}
	}
}
