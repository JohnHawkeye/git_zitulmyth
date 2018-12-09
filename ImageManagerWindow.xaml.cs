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

	public class ImagePattern
	{
		public string fileName;
		public string patternName;
		public List<Int32Rect> cropRange;
	}

	public class ImageCategory
	{
		public string name { get; set; }
		public List<ImageCategory> pattern { get; set; }
	}


	public partial class ImageManagerWindow : Window
	{

		private List<ImageCategory> lstImageCollection = new List<ImageCategory>();

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

			lstPatternPlayer.Add(new ImagePattern { patternName="Idle" });
			lstPatternPlayer.Add(new ImagePattern { patternName = "Idle" });
			lstPatternPlayer.Add(new ImagePattern { patternName = "Idle" });
			lstPatternPlayer.Add(new ImagePattern { patternName = "Idle" });

			TreeViewItem tviPlyer = new TreeViewItem();

			tviPlyer.Header = "Player";
			tviPlyer.IsExpanded = true;

			ImagePatternAdding(lstPatternPlayer,tviPlyer);

			TreeViewItem tviEnemy = new TreeViewItem();

			tviEnemy.Header = "Enemy";
			tviEnemy.IsExpanded = true;

			TreeViewItem tviObject = new TreeViewItem();

			tviObject.Header = "Object";
			tviObject.IsExpanded = true;

			TreeViewItem tviNpc = new TreeViewItem();

			tviNpc.Header = "Npc";
			tviNpc.IsExpanded = true;

			TreeViewItem tviItem = new TreeViewItem();

			tviItem.Header = "Item";
			tviItem.IsExpanded = true;

			TreeViewItem tviBlock = new TreeViewItem();

			tviBlock.Header = "Block";
			tviBlock.IsExpanded = true;

			TreeViewItem tviScenery = new TreeViewItem();

			tviScenery.Header = "Scenery";
			tviScenery.IsExpanded = true;

			TreeViewItem tviSystem = new TreeViewItem();

			tviSystem.Header = "System";
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

				treeViewItem.Items.Add(child);
			}
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
			TreeViewItem item = trvCategory.SelectedItem as TreeViewItem; 

			tbkCategory.Text = item.Header.ToString();
		}
	}
}
