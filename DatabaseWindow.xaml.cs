using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		public string sprite { get; set; }
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

		public List<DatabaseBlock> lstViewDbBlock = new  List<DatabaseBlock>();
		public List<DatabaseEnemy> lstViewDbEnemy = new List<DatabaseEnemy> ();
		public List<DatabaseObject> lstViewDbObject = new List<DatabaseObject>();
		public List<DatabaseItem> lstViewDbItem = new List<DatabaseItem>();

		public List<string> lstNameBlock = new List<string>();
		public List<string> lstNameEnemy = new List<string>();
		public List<string> lstNameObject = new List<string>();
		public List<string> lstNameItem = new List<string>();

		const string dbFileNamePlayer = "Assets/json/data/data_player.json";
		const string dbFileNameBlock =	"Assets/json/data/data_block.json";
		const string dbFileNameEnemy =	"Assets/json/data/data_enemy.json";
		const string dbFileNameObject = "Assets/json/data/data_object.json";
		const string dbFileNameItem=	"Assets/json/data/data_item.json";

		public static MaxListChangerDialog maxListChangerDialog;
		public static int maxListNum;

		public static SpriteViewerDialog spriteViewerDialog;
		public CategoryName spriteCategory;
		public int selectedIndex = 0;
		

		public DatabaseWindow()
		{
			InitializeComponent();
		}

		private void btnUpdate_Click(object sender, RoutedEventArgs e)
		{

			if(tabDatabase.SelectedIndex >= 0)
			{
				btnUpdate.Content = "書き込み中";
				btnUpdate.IsEnabled = false;

				DataContractJsonSerializer json;
				FileStream fs;

				switch (tabDatabase.SelectedIndex)
				{
					case 0:

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

			ListBoxBlockUpdate();
		}

		private void DatabaseReading()
		{

			//player
			if (File.Exists(dbFileNamePlayer))
			{
				DataContractJsonSerializer json_p = new DataContractJsonSerializer(typeof(DatabasePlayer));
				FileStream fs_p = new FileStream(dbFileNamePlayer, FileMode.Open);

				try		{ lstViewDbPlayer = (DatabasePlayer)json_p.ReadObject(fs_p); }
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

				try		{ lstViewDbBlock = (List<DatabaseBlock>)json_b.ReadObject(fs_b); }
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

				try		{ lstViewDbEnemy = (List<DatabaseEnemy>)json_e.ReadObject(fs_e); }
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

				try		{ lstViewDbObject = (List<DatabaseObject>)json_o.ReadObject(fs_o); }
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

				try		{ lstViewDbItem = (List<DatabaseItem>)json_i.ReadObject(fs_i); }
				finally { fs_i.Close(); }

			}
			else
			{
				MessageBox.Show("data_item.json ファイルがありません", "ファイルチェック", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		public void ListBoxBlockUpdate()
		{
			lstNameBlock.Clear();

			for(int i = 0; i < lstViewDbBlock.Count; i++)
			{
				lstNameBlock.Add( i.ToString() + ": "+ lstViewDbBlock[i].name);
			}

			lsbBlock.ItemsSource = lstNameBlock;
			lsbBlock.Items.Refresh();
			lsbBlock.SelectedIndex = 0;
		}

		private void btnBlockMaxList_Click(object sender, RoutedEventArgs e)
		{
			maxListChangerDialog = new MaxListChangerDialog();


			maxListChangerDialog.oldnum = lstViewDbBlock.Count;
			maxListChangerDialog.txbNumber.Text = lstViewDbBlock.Count.ToString();
			maxListChangerDialog.ShowDialog();
		}

		private void lsbBlock_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(lsbBlock.SelectedIndex >= 0)
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

		private void txbBlockName_TextChanged(object sender, TextChangedEventArgs e)
		{
			
		}

		private void txbBlockName_KeyDown(object sender, KeyEventArgs e)
		{
			if (lsbBlock.SelectedIndex >= 0)
			{
				if(e.Key == Key.Return)
				{
					int index = lsbBlock.SelectedIndex;

					lstViewDbBlock[index].name = txbBlockName.Text;

					ListBoxBlockUpdate();
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
	}
}
