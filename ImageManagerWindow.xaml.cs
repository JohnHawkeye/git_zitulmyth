using System;
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
using System.IO;
using System.Runtime.Serialization.Json;
using Zitulmyth.Data;

namespace Zitulmyth
{
	public enum CategoryName
	{
		Player,
		Enemy,
		Object,
		Npc,
		Item,
		Block,
		Scenery,
		System
	}

	public class ImagePattern
	{
		public string fileName;
		public string patternName;
		public List<Int32Rect> cropRange = new List<Int32Rect>();
	}

	public partial class ImageManagerWindow : Window
	{
		public CroppingImageDialog croppingDialog;
		public Int32Rect croppingRange;

		public string parentName;
		private string childName;
		public int patternListIndex = 0;
		public int croppingIndex = 0;
		public bool isNewPattern = false;
		private int namingNum = 0;
		private string namingStr = "新規パターン(0)";

		public List<Image> previewImage = new List<Image>();
		public BitmapImage bmiPreviewCloseButton;
		public List<Image> previewCloseButton = new List<Image>();

		private TreeViewItem tviPlyer = new TreeViewItem();
		private TreeViewItem tviEnemy = new TreeViewItem();
		private TreeViewItem tviObject = new TreeViewItem();
		private TreeViewItem tviNpc = new TreeViewItem();
		private TreeViewItem tviItem = new TreeViewItem();
		private TreeViewItem tviBlock = new TreeViewItem();
		private TreeViewItem tviScenery = new TreeViewItem();
		private TreeViewItem tviSystem = new TreeViewItem();

		private List<TreeViewItem> tviChildPlayer = new List<TreeViewItem>();
		private List<TreeViewItem> tviChildEnemy = new List<TreeViewItem>();
		private List<TreeViewItem> tviChildObject = new List<TreeViewItem>();
		private List<TreeViewItem> tviChildNpc = new List<TreeViewItem>();
		private List<TreeViewItem> tviChildItem = new List<TreeViewItem>();
		private List<TreeViewItem> tviChildBlock = new List<TreeViewItem>();
		private List<TreeViewItem> tviChildScenery = new List<TreeViewItem>();
		private List<TreeViewItem> tviChildSystem = new List<TreeViewItem>();

		//data
		private List<ImagePattern> lstPatternPlayer = new List<ImagePattern>();
		private List<ImagePattern> lstPatternEnemy = new List<ImagePattern>();
		private List<ImagePattern> lstPatternObject = new List<ImagePattern>();
		private List<ImagePattern> lstPatternNpc = new List<ImagePattern>();
		private List<ImagePattern> lstPatternItem = new List<ImagePattern>();
		private List<ImagePattern> lstPatternBlock = new List<ImagePattern>();
		private List<ImagePattern> lstPatternScenery = new List<ImagePattern>();
		private List<ImagePattern> lstPatternSystem = new List<ImagePattern>();

		private DirectoryInfo CropDirectoryCheck()
		{

			if (Directory.Exists("Assets/json/crop" ))
			{
				return null;
			}

			return Directory.CreateDirectory("Assets/json/crop");
		}

		private void ImageCategoryAdding()
		{

			PatternDataRead(CategoryName.Player.ToString(), lstPatternPlayer);
			PatternDataRead(CategoryName.Enemy.ToString(), lstPatternEnemy);
			PatternDataRead(CategoryName.Object.ToString(), lstPatternObject);
			PatternDataRead(CategoryName.Npc.ToString(), lstPatternNpc);
			PatternDataRead(CategoryName.Item.ToString(), lstPatternItem);
			PatternDataRead(CategoryName.Block.ToString(), lstPatternBlock);
			PatternDataRead(CategoryName.Scenery.ToString(), lstPatternScenery);
			PatternDataRead(CategoryName.System.ToString(), lstPatternSystem);

			tviPlyer.Header = CategoryName.Player.ToString();
			ImagePatternAdding(lstPatternPlayer, tviChildPlayer, tviPlyer);
			tviPlyer.IsExpanded = true;


			tviEnemy.Header = CategoryName.Enemy.ToString();
			ImagePatternAdding(lstPatternEnemy, tviChildEnemy, tviEnemy);
			tviEnemy.IsExpanded = true;


			tviObject.Header = CategoryName.Object.ToString();
			ImagePatternAdding(lstPatternObject, tviChildObject, tviObject);
			tviObject.IsExpanded = true;

			tviNpc.Header = CategoryName.Npc.ToString();
			ImagePatternAdding(lstPatternNpc, tviChildNpc, tviNpc);
			tviNpc.IsExpanded = true;

			tviItem.Header = CategoryName.Item.ToString();
			ImagePatternAdding(lstPatternItem, tviChildItem, tviItem);
			tviItem.IsExpanded = true;

			tviBlock.Header = CategoryName.Block.ToString();
			ImagePatternAdding(lstPatternBlock, tviChildBlock, tviBlock);
			tviBlock.IsExpanded = true;

			tviScenery.Header = CategoryName.Scenery.ToString();
			ImagePatternAdding(lstPatternScenery, tviChildScenery, tviScenery);
			tviScenery.IsExpanded = true;

			tviSystem.Header = CategoryName.System.ToString();
			ImagePatternAdding(lstPatternSystem, tviChildSystem, tviSystem);
			tviSystem.IsExpanded = true;

			trvCategory.Items.Add(tviPlyer);
			trvCategory.Items.Add(tviEnemy);
			trvCategory.Items.Add(tviObject);
			trvCategory.Items.Add(tviNpc);
			trvCategory.Items.Add(tviItem);
			trvCategory.Items.Add(tviBlock);
			trvCategory.Items.Add(tviScenery);
			trvCategory.Items.Add(tviSystem);

		}

		private void ImagePatternAdding(List<ImagePattern> imagePattern, List<TreeViewItem> tvichild, TreeViewItem treeViewItem)
		{
			tvichild.Clear();

			for (int i = 0; i < imagePattern.Count; i++)
			{

				tvichild.Add(new TreeViewItem { Header = imagePattern[i].patternName });
				treeViewItem.Items.Add(tvichild[i]);

			}
		}

		private TreeViewItem ParentSelector()
		{
			CategoryName cn = (CategoryName)Enum.Parse(typeof(CategoryName), parentName, false);

			switch (cn)
			{
				case CategoryName.Player:
					return tviPlyer;

				case CategoryName.Enemy:
					return tviEnemy;

				case CategoryName.Object:
					return tviObject;

				case CategoryName.Npc:
					return tviNpc;

				case CategoryName.Item:
					return tviItem;

				case CategoryName.Block:
					return tviBlock;

				case CategoryName.Scenery:
					return tviScenery;

				case CategoryName.System:
					return tviSystem;

				default:
					return tviPlyer;
			}
		}

		public List<ImagePattern> ChildSelector()
		{
			CategoryName cn = (CategoryName)Enum.Parse(typeof(CategoryName), parentName, false);

			switch (cn)
			{
				case CategoryName.Player:
					return lstPatternPlayer;

				case CategoryName.Enemy:
					return lstPatternEnemy;

				case CategoryName.Object:
					return lstPatternObject;

				case CategoryName.Npc:
					return lstPatternNpc;

				case CategoryName.Item:
					return lstPatternItem;

				case CategoryName.Block:
					return lstPatternBlock;

				case CategoryName.Scenery:
					return lstPatternScenery;

				case CategoryName.System:
					return lstPatternSystem;

				default:
					return lstPatternPlayer;
			}
		}

		private void PatternViewUpdate()
		{

			PatternListLoading(ChildSelector(), parentName);

			tbkCategory.Text = parentName;

			txbPattern.IsEnabled = true;
			btnPatternNameUpdate.IsEnabled = true;
			btnImageRefer.IsEnabled = true;
		}

		public void PatternListLoading(List<ImagePattern> imgptn, string folderName)
		{
			for (int i = 0; i < imgptn.Count; i++)
			{
				if (childName == imgptn[i].patternName)
				{
					patternListIndex = i;
					break;
				}
			}

			txbPattern.Text = imgptn[patternListIndex].patternName;
			tbkFileName.Text = imgptn[patternListIndex].fileName;

			int marginTop = 10, marginLeft = 10;
			int colCount = 0, rowCount = 0;
			int imageSize = 64;

			BitmapImage previewBitmap = new BitmapImage();
			CroppedBitmap previewCropped = new CroppedBitmap();

			for (int i = 0; i < previewImage.Count; i++)
			{
				cvsImagePreview.Children.Remove(previewImage[i]);
			}

			for (int i = 0; i < previewCloseButton.Count; i++)
			{
				cvsImagePreview.Children.Remove(previewCloseButton[i]);
			}

			if (File.Exists("Assets/" + imgptn[patternListIndex].fileName))
			{				
				previewBitmap = new BitmapImage(
					new Uri("Assets/" + imgptn[patternListIndex].fileName, UriKind.Relative));

				previewImage.Clear();
				previewCloseButton.Clear();
				for (int i = 0; i < imgptn[patternListIndex].cropRange.Count; i++)
				{
					previewCropped = new CroppedBitmap(previewBitmap, imgptn[patternListIndex].cropRange[i]);
					int w = imgptn[patternListIndex].cropRange[i].Width;
					int h = imgptn[patternListIndex].cropRange[i].Height;
					previewImage.Add(new Image { Tag = i, Source = previewCropped, Width = w, Height = h });
					cvsImagePreview.Children.Add(previewImage[i]);
					previewImage[i].MouseLeftButtonDown += new MouseButtonEventHandler(PreviewImageClickOpenDialog);
					Canvas.SetLeft(previewImage[i], marginLeft + i * imageSize);
					Canvas.SetTop(previewImage[i], marginTop + rowCount * imageSize);

					previewCloseButton.Add(new Image { Source = bmiPreviewCloseButton, Width = 16, Height = 16 });
					cvsImagePreview.Children.Add(previewCloseButton[i]);
					previewCloseButton[i].MouseLeftButtonDown += new MouseButtonEventHandler(PreviewCloseButtonClick);
					Canvas.SetLeft(previewCloseButton[i], marginLeft + i * imageSize + 48);
					Canvas.SetTop(previewCloseButton[i], marginTop + rowCount * imageSize);

					colCount++;
					if (colCount % 4 == 0)
						rowCount++;
				}
			}

			previewBitmap = new BitmapImage(new Uri("Assets/icon/icon_newpattern.png", UriKind.Relative));
			previewImage.Add(new Image {Tag = "new", Source = previewBitmap, Width = imageSize, Height = imageSize });
			cvsImagePreview.Children.Add(previewImage[previewImage.Count - 1]);
			previewImage[previewImage.Count - 1].MouseLeftButtonDown += new MouseButtonEventHandler(PreviewImageClickOpenDialog);
			Canvas.SetLeft(previewImage[previewImage.Count - 1], marginLeft + colCount * imageSize);
			Canvas.SetTop(previewImage[previewImage.Count - 1], marginTop + rowCount * imageSize);


		}

		private void PatternNonSelected()
		{
			TreeViewItem selected = (TreeViewItem)trvCategory.SelectedItem;

			parentName = selected.Header.ToString();
			tbkCategory.Text = parentName;
			txbPattern.Text = "-";
			txbPattern.IsEnabled = false;
			btnPatternNameUpdate.IsEnabled = false;
			tbkFileName.Text = "-";
			btnImageRefer.IsEnabled = false;

			for (int i = 0; i < previewImage.Count; i++)
			{
				cvsImagePreview.Children.Remove(previewImage[i]);
			}
			previewImage.Clear();


			for (int i = 0; i < previewCloseButton.Count; i++)
			{
				cvsImagePreview.Children.Remove(previewCloseButton[i]);
			}
			previewCloseButton.Clear();
		}

		public void PatternDataWrite()
		{

			DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<ImagePattern>));

			FileStream fs = new FileStream("Assets/json/crop/" + parentName.ToLowerInvariant() + ".json", FileMode.Create);

			try
			{
				this.IsEnabled = false;
				json.WriteObject(fs, ChildSelector());
			}
			finally
			{
				fs.Close();
				this.IsEnabled = true;
			}
		}


		public void PatternDataRead(string parentname,List<ImagePattern> imagePattern)
		{

			string fileName = "Assets/json/crop/" + parentname.ToLowerInvariant() + ".json";

			if (File.Exists(fileName))
			{

				DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<ImagePattern>));

				FileStream fs = new FileStream(fileName, FileMode.Open);

				try
				{
					List<ImagePattern> lstNewPattern = new List<ImagePattern>();

					lstNewPattern = (List<ImagePattern>)json.ReadObject(fs);

					for(int i = 0; i < lstNewPattern.Count; i++)
					{
						imagePattern.Add(new ImagePattern { fileName = lstNewPattern[i].fileName, patternName = lstNewPattern[i].patternName });

						for(int j = 0; j < lstNewPattern[i].cropRange.Count; j++)
						{
							imagePattern[i].cropRange.Add(lstNewPattern[i].cropRange[j]);
						}
					}
					
				}

				finally
				{
					fs.Close();
				}

			}

		}



		private bool PatternNameCheck(List<ImagePattern> imgptn)
		{
			bool flag = false;

			for (int i = 0; i < imgptn.Count; i++)
			{
				if (patternListIndex != i)
				{
					if (txbPattern.Text == imgptn[i].patternName)
					{
						MessageBox.Show("同じパターン名があります。\n違う名前に設定してください。", "パターン名", MessageBoxButton.OK, MessageBoxImage.Information);
						flag = true;
						break;
					}
				}

			}

			return flag;
		}

/// <summary>
/// 
/// </summary>
/// 
		public ImageManagerWindow()
		{
			InitializeComponent();

			CropDirectoryCheck();
			bmiPreviewCloseButton = new BitmapImage(new Uri("Assets/icon/closebtn.png", UriKind.Relative));


			ImageCategoryAdding();

			tviPlyer.IsSelected = true;
			PatternNonSelected();

		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{

		}

		private void btnImageRefer_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.materialBrowser = new MaterialBrowser();
			MainWindow.materialBrowser.btnSelect.Visibility = Visibility.Visible;
			MainWindow.materialBrowser.ShowDialog();
		}

		private void trvCategory_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{

			TreeViewItem item = (TreeViewItem)e.NewValue;
			tbkCategory.Text = item.Header.ToString();

			if (item.Parent.GetType().Equals(typeof(TreeViewItem)))
			{

				TreeViewItem parentitem = (TreeViewItem)item.Parent;
				Console.WriteLine(parentitem.Header.ToString() + " , " + item.Header.ToString());

				parentName = parentitem.Header.ToString();
				childName = item.Header.ToString();

				tbkCategory.Text = parentName;
				txbPattern.Text = childName;

				PatternViewUpdate();
			}
			else
			{
				PatternNonSelected();
			}

		}

		private void PreviewImageClickOpenDialog(object sender, RoutedEventArgs e)
		{

			if (File.Exists("Assets/" + tbkFileName.Text))
			{

				if (((Image)sender).Tag.GetType() == typeof(int))
				{
					croppingRange = ChildSelector()[patternListIndex].cropRange[(int)((Image)sender).Tag];
					croppingIndex = (int)((Image)sender).Tag;
					isNewPattern = false;
				}
				else
				{
					croppingRange = new Int32Rect(0, 0, 1, 1);
					isNewPattern = true;
				}

				croppingDialog = new CroppingImageDialog();
				croppingDialog.ShowDialog();

			}
			else
			{
				MessageBox.Show("素材となる画像ファイルを参照してください。", "画像ファイル参照未設定", MessageBoxButton.OK, MessageBoxImage.Information);
			}

		}

		private void PreviewCloseButtonClick(object sender,RoutedEventArgs e)
		{
			
			int index = previewCloseButton.IndexOf(sender as Image);

			cvsImagePreview.Children.Remove(previewCloseButton[index]);
			previewCloseButton.RemoveAt(index);

			cvsImagePreview.Children.Remove(previewImage[index]);
			previewImage.RemoveAt(index);

			ChildSelector()[patternListIndex].cropRange.RemoveAt(index);

			PatternViewUpdate();

			PatternDataWrite();

		}

		private void btnPatternNameUpdate_Click(object sender, RoutedEventArgs e)
		{
			if (!PatternNameCheck(ChildSelector()))
			{
				ChildSelector()[patternListIndex].patternName = txbPattern.Text;

				TreeViewItem selected = (TreeViewItem)trvCategory.SelectedItem;
				selected.Header = txbPattern.Text;

				PatternDataWrite();
			}
		}

		private void btnAddPattern_Click(object sender, RoutedEventArgs e)
		{

			if (trvCategory.Items.IndexOf((TreeViewItem)trvCategory.SelectedItem) > -1)
			{
				TreeViewItem item = (TreeViewItem)trvCategory.SelectedItem;
				parentName = item.Header.ToString();


				for (int i = 0; i < ChildSelector().Count; i++)
				{
					if ("新規パターン(" + namingNum + ")" == ChildSelector()[i].patternName)
					{
						namingNum++;
						namingStr = "新規パターン(" + namingNum + ")";
						
						break;
					}

				}

				ChildSelector().Add(new ImagePattern { patternName = namingStr, fileName = "" });

				TreeViewItem newItem = new TreeViewItem();
				newItem.Header = ChildSelector()[ChildSelector().Count - 1].patternName;
				ParentSelector().Items.Add(newItem);

				PatternDataWrite();

			}
		}

		private void btnDeletePattern_Click(object sender, RoutedEventArgs e)
		{
			TreeViewItem selected = (TreeViewItem)trvCategory.SelectedItem;

			if (selected != null)
			{
				if (trvCategory.Items.IndexOf(selected) <= -1)
				{

					MessageBoxResult result =
						MessageBox.Show("パターン " + selected.Header.ToString() + " を削除しますか？",
							"パターンの削除", MessageBoxButton.YesNo, MessageBoxImage.Warning);

					if (result == MessageBoxResult.Yes)
					{
						TreeViewItem parent = (TreeViewItem)selected.Parent;
						parent.Items.Remove(selected);

						for (int i = 0; i < ChildSelector().Count; i++)
						{

							if (ChildSelector()[i].patternName == selected.Header.ToString())
							{
								ChildSelector().RemoveAt(i);
								break;
							}
						}

						PatternDataWrite();
					}
				}
			}
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			ImageData.SpriteReading();
		}

		private void btnDuplicate_Click(object sender, RoutedEventArgs e)
		{

			if (trvCategory.Items.IndexOf((TreeViewItem)trvCategory.SelectedItem) > -1)
			{
				TreeViewItem item = (TreeViewItem)trvCategory.SelectedItem;
				parentName = item.Header.ToString();


				for (int i = 0; i < ChildSelector().Count; i++)
				{
					if (namingStr == ChildSelector()[i].patternName)
					{
						namingNum++;
						namingStr = "複製パターン(" + namingNum + ")";
						
						break;
					}

				}

				int lstnum = ChildSelector().Count;
				ImagePattern tempPattern = new ImagePattern();

				if(lstnum >= 1)
				{
					tempPattern = ChildSelector()[lstnum-1];
				}

				ChildSelector().Add(new ImagePattern { patternName = namingStr, fileName = tempPattern.fileName,cropRange= new List<Int32Rect>( tempPattern.cropRange) });

				TreeViewItem newItem = new TreeViewItem();
				newItem.Header = ChildSelector()[ChildSelector().Count - 1].patternName;
				ParentSelector().Items.Add(newItem);

				PatternDataWrite();

			}
		}
	}
}
