using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Zitulmyth
{
	public class MaterialFileName
	{
		public string folderName { get; set; }
		public string fileName { get; set; }

		public override string ToString() => $"{folderName}:{fileName}";

		public override bool Equals(object obj)
		{
			var p = obj as MaterialFileName;
			if (p == null)
				return false;
			return (this.folderName == p.folderName && this.fileName == p.fileName);
		}

		public override int GetHashCode() => folderName.GetHashCode() ^ fileName.GetHashCode();

	}

	public class ListBoxFileName
	{
		public int id { get; set; }
		public string fileName { get;  set; }
	}

    public partial class MaterialBrowser : Window
    {

		private List<string> lstFolder { get; set; }
		private List<ListBoxFileName> lstFileName = new List<ListBoxFileName>();

		private List<MaterialFileName> materialFileName = new List<MaterialFileName>();

		public const string configFileName = "Assets/materialbrowser.config";

		//
		private DirectoryInfo MaterialDirectoryCheck(string name)
		{
			lstFolder.Add(name);

			if (Directory.Exists("Assets/" + name))
			{
				return null;
			}

			return Directory.CreateDirectory("Assets/" + name);
		}

		private void MaterialFileNameListUpdate()
		{
			lstFileName.Clear();

			for(int i= 0; i < materialFileName.Count; i++)
			{
				if(lstFolder[lsbFolder.SelectedIndex] == materialFileName[i].folderName)
				{
					lstFileName.Add(new ListBoxFileName {
						id = i, fileName = materialFileName[i].fileName });
				}
			}

			lsbFile.ItemsSource = lstFileName;
			lsbFile.DisplayMemberPath = "fileName";
			
			lsbFile.Items.Refresh();
		}

		private void MaterialFileNameConfigWriting()
		{
			
			//write
			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(List<MaterialFileName>));

			StreamWriter sw = new StreamWriter(
				configFileName, false, new UTF8Encoding(false));

			serializer.Serialize(sw, materialFileName);
			sw.Close();

		}

		private void MaterialFileNameConfigReading()
		{

			System.Xml.Serialization.XmlSerializer serializer =
				new System.Xml.Serialization.XmlSerializer(typeof(List<MaterialFileName>));

			StreamReader sr = new StreamReader(
				configFileName, new UTF8Encoding(false));

			materialFileName =
				(List<MaterialFileName>)serializer.Deserialize(sr);
			sr.Close();
		}

		//
		public MaterialBrowser()
        {
            InitializeComponent();
        }

		

		private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.materialBrowser.Close();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//DirectoryCheck
			lstFolder = new List<string>();

			MaterialDirectoryCheck("player");
			MaterialDirectoryCheck("enemy");
			MaterialDirectoryCheck("object");
			MaterialDirectoryCheck("npc");
			MaterialDirectoryCheck("item");
			MaterialDirectoryCheck("block");
			MaterialDirectoryCheck("scenery");
			MaterialDirectoryCheck("system");

			MaterialDirectoryCheck("music");
			MaterialDirectoryCheck("sound");

			
			lsbFolder.ItemsSource = lstFolder;
			lsbFolder.SelectedIndex = 0;

			if (!File.Exists(configFileName))
			{
				MaterialFileNameConfigWriting();
			}


			MaterialFileNameConfigReading();

			MaterialFileNameListUpdate();

			
		}

		private void btnImport_Click(object sender, RoutedEventArgs e)
		{

			if (lsbFolder.SelectedIndex >= 0)
			{
				try
				{
					OpenFileDialog fileDialog = new OpenFileDialog();

					fileDialog.InitialDirectory = @"C:";
					fileDialog.Title = "ファイルを選択してください";
					fileDialog.Filter = "イメージファイル (*.jpg, *.gif, *.png)|*.jpg;*.gif;*.png";
					fileDialog.Multiselect = true;

					DialogResult dialogResult = fileDialog.ShowDialog();
					if (dialogResult == System.Windows.Forms.DialogResult.Cancel)
					{
						return;
					}

					string[] fileName = fileDialog.FileNames;
					string[] safeFileName = fileDialog.SafeFileNames;

					for (int i = 0; i < fileName.Length; i++)
					{
						File.Copy(fileName[i], "Assets/"+ lstFolder[lsbFolder.SelectedIndex] + "/" + safeFileName[i], true);
			
						materialFileName.Add(new MaterialFileName {folderName= lstFolder[lsbFolder.SelectedIndex],fileName= safeFileName[i] });
					}

					materialFileName =  materialFileName.Distinct().ToList();

					
					MaterialFileNameConfigWriting();
					MaterialFileNameListUpdate();

				}
				catch (Exception ex)
				{
					System.Windows.MessageBox.Show(ex.Message);
				}
			}
			
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if(lsbFile.SelectedIndex >= 0)
			{
				string path = "Assets/" + lstFolder[lsbFolder.SelectedIndex] + "/" +
											lstFileName[lsbFile.SelectedIndex].fileName;

				if (File.Exists(path))
				{
					MessageBoxResult result = System.Windows.MessageBox.Show(
						"素材 " + lstFileName[lsbFile.SelectedIndex].fileName + 
						" を削除しようとしています。\n本当に削除しますか？", "素材の削除",
						MessageBoxButton.YesNo, MessageBoxImage.Warning);

					if (result == MessageBoxResult.Yes)
					{
						File.Delete(path);

						materialFileName.RemoveAt(lstFileName[lsbFile.SelectedIndex].id);
						MaterialFileNameConfigWriting();
						MaterialFileNameListUpdate();

					}
				}
			}
		}

		private void lsbFolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			MaterialFileNameListUpdate();
		}

		private void btnSelect_Click(object sender, RoutedEventArgs e)
		{
			if(lsbFile.SelectedIndex >= 0)
			{
				MainWindow.imageManager.tbkFileName.Text = lstFolder[lsbFolder.SelectedIndex].ToString() +"/"+ lstFileName[lsbFile.SelectedIndex].fileName.ToString();
				MainWindow.imageManager.ChildSelector()[MainWindow.imageManager.patternListIndex].fileName = MainWindow.imageManager.tbkFileName.Text;

				MainWindow.imageManager.ChildSelector()[MainWindow.imageManager.patternListIndex].cropRange.Clear();
				MainWindow.imageManager.PatternListLoading(MainWindow.imageManager.ChildSelector(), MainWindow.imageManager.parentName);
				
				this.Close();
			}
			
		}
	}
}
