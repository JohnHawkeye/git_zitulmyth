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

		private string parentName;
		private string childName;
		private int patternListIndex = 0;
		private int namingNum = 0;
		private string namingStr = "新規パターン";

		public List<Image> previewImage = new List<Image>();

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


		private void ImageCategoryAdding()
		{

			

			lstPatternPlayer.Add(new ImagePattern { patternName="Idle",fileName = "vtulp01.png",
				cropRange = new List<Int32Rect> {new Int32Rect(0,0,32,64), new Int32Rect(32, 0, 32, 64) }});
			lstPatternPlayer.Add(new ImagePattern { patternName = "A",fileName = "vtulp01.png",
				cropRange = new List<Int32Rect> { new Int32Rect(0, 0, 32, 64) } });
			lstPatternPlayer.Add(new ImagePattern { patternName = "C" });
			lstPatternPlayer.Add(new ImagePattern { patternName = "D" });

			
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

		private void ImagePatternAdding (List<ImagePattern> imagePattern,List<TreeViewItem> tvichild,TreeViewItem treeViewItem)
		{
			tvichild.Clear();

			for(int i = 0; i < imagePattern.Count; i++)
			{

				tvichild.Add(new TreeViewItem {Header= imagePattern[i].patternName });
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

		private List<ImagePattern> ChildSelector()
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

		private void PatternListLoading(List<ImagePattern> imgptn,string folderName)
		{
			for(int i = 0; i < imgptn.Count; i++)
			{
				if(childName == imgptn[i].patternName)
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
			previewBitmap = new BitmapImage(
				new Uri("Assets/"+ folderName +"/"+ imgptn[patternListIndex].fileName, UriKind.Relative));

			for (int i = 0; i < previewImage.Count; i++) 
			{
				cvsImagePreview.Children.Remove(previewImage[i]);
			}

			previewImage.Clear();
			for(int i = 0; i < imgptn[patternListIndex].cropRange.Count; i++)
			{
				previewCropped = new CroppedBitmap(previewBitmap, imgptn[patternListIndex].cropRange[i]);
				int w = imgptn[patternListIndex].cropRange[i].Width;
				int h = imgptn[patternListIndex].cropRange[i].Height;
				previewImage.Add(new Image { Source = previewCropped, Width = w, Height = h });
				cvsImagePreview.Children.Add(previewImage[i]);
				previewImage[i].MouseLeftButtonDown += new MouseButtonEventHandler(PreviewImageClickOpenDialog);
				Canvas.SetLeft(previewImage[i], marginLeft + i* imageSize);
				Canvas.SetTop(previewImage[i], marginTop + rowCount* imageSize);
				colCount++;
				if (colCount % 4 == 0)
					rowCount++;
			}

			previewBitmap = new BitmapImage(new Uri("Assets/icon/icon_newpattern.png", UriKind.Relative));
			previewImage.Add(new Image { Source = previewBitmap, Width = imageSize, Height = imageSize });
			cvsImagePreview.Children.Add(previewImage[previewImage.Count-1]);
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

			for(int i = 0; i < previewImage.Count; i++)
			{
				cvsImagePreview.Children.Remove(previewImage[i]);
			}

			previewImage.Clear();
			
		}

		private void PatternDataWrite(List<ImagePattern> imgptn)
		{

			DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<ImagePattern>));

			FileStream fs = new FileStream("Assets/json/crop/"+ parentName +".json", FileMode.Create);

			try
			{
				btnPatternNameUpdate.IsEnabled = false;
				json.WriteObject(fs, imgptn);
			}
			finally
			{
				fs.Close();
				btnPatternNameUpdate.IsEnabled = true;
			}
		}

		private bool PatternNameCheck(List<ImagePattern> imgptn)
		{
			bool flag = false;

			for(int i =0;i < imgptn.Count; i++)
			{
				if(patternListIndex != i)
				{
					if (txbPattern.Text == imgptn[i].patternName)
					{
						MessageBox.Show("同じパターン名があります。\n違う名前に設定してください。","パターン名",MessageBoxButton.OK,MessageBoxImage.Information);
						flag = true;
						break;
					}
				}
				
			}

			return flag;
		}

		public ImageManagerWindow()
		{
			InitializeComponent();


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

			if (item.Parent.GetType().Equals(typeof(TreeViewItem))){

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
			croppingRange = new Int32Rect(0,0,0,0);
			croppingDialog = new CroppingImageDialog();
			croppingDialog.ShowDialog();
		}

		private void btnPatternNameUpdate_Click(object sender, RoutedEventArgs e)
		{
			if (!PatternNameCheck(ChildSelector()))
			{
				ChildSelector()[patternListIndex].patternName = txbPattern.Text;

				TreeViewItem selected = (TreeViewItem)trvCategory.SelectedItem;
				selected.Header = txbPattern.Text;
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
					if (namingStr == ChildSelector()[i].patternName)
					{
						namingStr = "新規パターン(" + namingNum + ")";
						namingNum++;
						break;
					}
					
				}

				ChildSelector().Add(new ImagePattern {patternName = namingStr,fileName="" });

				TreeViewItem newItem = new TreeViewItem();
				newItem.Header = ChildSelector()[ChildSelector().Count-1].patternName;
				ParentSelector().Items.Add(newItem);
				
			}
		}

		private void btnDeletePattern_Click(object sender, RoutedEventArgs e)
		{
			TreeViewItem selected = (TreeViewItem)trvCategory.SelectedItem;

			if(selected != null)
			{
				if (trvCategory.Items.IndexOf(selected) <= -1)
				{

					MessageBoxResult result =
						MessageBox.Show("パターン "+ selected.Header.ToString() +" を削除しますか？",
							"パターンの削除",MessageBoxButton.YesNo, MessageBoxImage.Warning);

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
					}	
				}
			}
		}
	}
}
