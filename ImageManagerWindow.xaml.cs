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
		public int index;
		public string fileName;
		public string patternName;
		public List<Int32Rect> cropRange = new List<Int32Rect>();
	}
	
	public partial class ImageManagerWindow : Window
	{
		private string parentName;
		private string childName;

		public List<Image> previewImage = new List<Image>();

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

			TreeViewItem tviPlyer = new TreeViewItem();
			tviPlyer.Header = CategoryName.Player.ToString();
			tviPlyer.IsExpanded = true;

			ImagePatternAdding(lstPatternPlayer,tviPlyer);

			TreeViewItem tviEnemy = new TreeViewItem();
			tviEnemy.Header = CategoryName.Enemy.ToString();
			tviEnemy.IsExpanded = true;

			TreeViewItem tviObject = new TreeViewItem();
			tviObject.Header = CategoryName.Object.ToString();
			tviObject.IsExpanded = true;

			TreeViewItem tviNpc = new TreeViewItem();
			tviNpc.Header = CategoryName.Npc.ToString();
			tviNpc.IsExpanded = true;

			TreeViewItem tviItem = new TreeViewItem();
			tviItem.Header = CategoryName.Item.ToString();
			tviItem.IsExpanded = true;

			TreeViewItem tviBlock = new TreeViewItem();
			tviBlock.Header = CategoryName.Block.ToString();
			tviBlock.IsExpanded = true;

			TreeViewItem tviScenery = new TreeViewItem();
			tviScenery.Header = CategoryName.Scenery.ToString();
			tviScenery.IsExpanded = true;

			TreeViewItem tviSystem = new TreeViewItem();
			tviSystem.Header = CategoryName.System.ToString();
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

		private void ImagePatternAdding (List<ImagePattern> imagePattern,TreeViewItem treeViewItem)
		{
			for(int i = 0; i < imagePattern.Count; i++)
			{
				
				TreeViewItem child = new TreeViewItem();
				child.Header = imagePattern[i].patternName;
				imagePattern[i].index = i;
				treeViewItem.Items.Add(child);

			}
		}

		private void PatternViewUpdate()
		{
			CategoryName cn = (CategoryName)Enum.Parse(typeof(CategoryName), parentName,false);

			switch (cn)
			{
				case CategoryName.Player:
					PatternListLoading(lstPatternPlayer, "player");
					break;

				case CategoryName.Enemy:
					PatternListLoading(lstPatternPlayer, "enemy");
					break;

				case CategoryName.Object:
					PatternListLoading(lstPatternPlayer, "object");
					break;

				case CategoryName.Npc:
					PatternListLoading(lstPatternPlayer, "npc");
					break;

				case CategoryName.Item:
					PatternListLoading(lstPatternPlayer, "item");
					break;

				case CategoryName.Block:
					PatternListLoading(lstPatternPlayer, "block");
					break;

				case CategoryName.Scenery:
					PatternListLoading(lstPatternPlayer, "scenery");
					break;

				case CategoryName.System:
					PatternListLoading(lstPatternPlayer, "system");
					break;

				default:
					PatternListLoading(lstPatternPlayer, "player");
					break;
			}

			tbkCategory.Text = parentName;

		}

		private void PatternListLoading(List<ImagePattern> imgptn,string folderName)
		{

			int index = 0;

			for(int i = 0; i < imgptn.Count; i++)
			{
				if(childName == imgptn[i].patternName)
				{
					index = i;
					break;
				}
			}

			txbPattern.Text = imgptn[index].patternName;
			tbkFileName.Text = imgptn[index].fileName;

			int marginTop = 10, marginLeft = 10;
			int colCount = 0, rowCount = 0;
			int imageSize = 64;

			BitmapImage previewBitmap = new BitmapImage();
			CroppedBitmap previewCropped = new CroppedBitmap();
			previewBitmap = new BitmapImage(
				new Uri("Assets/"+ folderName +"/"+ imgptn[index].fileName, UriKind.Relative));

			for (int i = 0; i < previewImage.Count; i++) 
			{
				cvsImagePreview.Children.Remove(previewImage[i]);
			}

			previewImage.Clear();
			for(int i = 0; i < imgptn[index].cropRange.Count; i++)
			{
				previewCropped = new CroppedBitmap(previewBitmap, imgptn[index].cropRange[i]);
				int w = imgptn[index].cropRange[i].Width;
				int h = imgptn[index].cropRange[i].Height;
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

		public ImageManagerWindow()
		{
			InitializeComponent();

			ImageCategoryAdding();
	
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
			
		}

		private void PreviewImageClickOpenDialog(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("おされたっぽい");
		}
	}
}
