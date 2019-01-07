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
	//View only . Do not refer to it.
	public class EditorListViewStageOrder
	{
		public int id { get; set; }
		public string name { get; set; }
		public string stageFileName { get; set; }
		public string eventFileName { get; set; }
	}

	public partial class StageOrderWindow : Window
	{

		public List<EditorListViewStageOrder> lstListViewStageOrder = new List<EditorListViewStageOrder>();

		public StageOrderWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{

			ListViewStageOrderUpdate();
			
		}

		private void ListViewStageOrderUpdate()
		{
			lstListViewStageOrder.Clear();

			for (int i = 0; i < StageOrder.lstStageOrder.Count; i++)
			{
				lstListViewStageOrder.Add(new EditorListViewStageOrder
				{
					id = i,
					name = StageOrder.lstStageOrder[i].name,
					stageFileName = StageOrder.lstStageOrder[i].stageFileName,
					eventFileName = StageOrder.lstStageOrder[i].eventFileName,

				});
			}

			lsvStageOrder.ItemsSource = lstListViewStageOrder;
			lsvStageOrder.Items.Refresh();
			lsvStageOrder.DataContext = lstListViewStageOrder;
		}

		private void btnNewStage_Click(object sender, RoutedEventArgs e)
		{

			bool overlapCheck = false;
			char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();

			System.Text.RegularExpressions.Regex r =
				new System.Text.RegularExpressions.Regex(
					"[\\x00-\\x1f<>:\"/\\\\|?*]" +
					"|^(CON|PRN|AUX|NUL|COM[0-9]|LPT[0-9]|CLOCK\\$)(\\.|$)" +
					"|[\\. ]$",
				System.Text.RegularExpressions.RegexOptions.IgnoreCase);


			for (int i = 0; i< StageOrder.lstStageOrder.Count; i++)
			{
				if(StageOrder.lstStageOrder[i].name == txbStageName.Text)
				{
					overlapCheck = true;
					break;
				}
			}

			if(txbStageName.Text != null && txbStageName.Text !="")
			{
				if (txbStageName.Text.IndexOfAny(invalidChars) < 0 && !r.IsMatch(txbStageName.Text))
				{
					if (!overlapCheck)
					{

						StageNewFile();
						EventNewFile();

						StageOrder.lstStageOrder.Add(new StageOrderList
						{
							id = StageOrder.lstStageOrder.Count-1,
							name = txbStageName.Text,
							stageFileName = txbStageName.Text + ".json",
							eventFileName = "event_" + txbStageName.Text + ".json",

						});

						StageOrder.OrderListWriting(StageOrder.lstStageOrder);

						ListViewStageOrderUpdate();
					}
					else
					{
						MessageBox.Show("同じステージ名はつけられません。", "ステージ名", MessageBoxButton.OK, MessageBoxImage.Information);
					}
				}
				else
				{
					MessageBox.Show("ステージ名に付けることのできない文字があります。", "ステージ名", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				
			}
			else
			{
				MessageBox.Show("ステージ名を入力してください。", "ステージ名", MessageBoxButton.OK, MessageBoxImage.Information);
			}

		}

		private void StageNewFile()
		{

			StageEditorData dataNewStage = new StageEditorData();

			MainWindow.stageEditor.CreateNewStageFile(dataNewStage);

			DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(StageEditorData));

			FileStream fs = new FileStream("Assets/json/stage/" + txbStageName.Text + ".json", FileMode.Create);

			try
			{
				json.WriteObject(fs, dataNewStage);
			}
			finally
			{
				fs.Close();
			}
		}

		private void EventNewFile()
		{
			List<EventData> lstNewEventData = new List<EventData>();
			lstNewEventData.Add(new EventData { });

			DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<EventData>));

			FileStream fs = new FileStream("Assets/json/event/" + "event_" + txbStageName.Text + ".json", FileMode.Create);
			try
			{
				json.WriteObject(fs, lstNewEventData);

			}
			finally
			{
				fs.Close();
			}
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			if (lsvStageOrder.SelectedIndex >= 0 && StageOrder.lstStageOrder.Count >=2)
			{
				MessageBoxResult result =
				MessageBox.Show("ステージ [ " + StageOrder.lstStageOrder[lsvStageOrder.SelectedIndex].name + " ]を削除します。\nよろしいですか？", "ステージの削除",
				MessageBoxButton.YesNo, MessageBoxImage.Warning);

				btnDelete.IsEnabled = false;
				

				if (result == MessageBoxResult.Yes)
				{
					if (File.Exists("Assets/json/stage/" + StageOrder.lstStageOrder[lsvStageOrder.SelectedIndex].stageFileName))
					{
						File.Delete("Assets/json/stage/" + StageOrder.lstStageOrder[lsvStageOrder.SelectedIndex].stageFileName);
					}

					if (File.Exists("Assets/json/event/" + StageOrder.lstStageOrder[lsvStageOrder.SelectedIndex].eventFileName))
					{
						File.Delete("Assets/json/event/" + StageOrder.lstStageOrder[lsvStageOrder.SelectedIndex].eventFileName);
					}

					StageOrder.lstStageOrder.RemoveAt(lsvStageOrder.SelectedIndex);
					StageOrder.OrderListWriting(StageOrder.lstStageOrder);

					ListViewStageOrderUpdate();

					//StageEditorDataReset
					StageManager.stageNum = 0;
					MainWindow.stageEditor.StageLoad();
					MainWindow.stageEditor.StageEditorDataSetting();
					StageEditorOperator.EditorPlayerPaletteSetting();
				}

				btnDelete.IsEnabled = true;
			}
		}

		private void btnItemDown_Click(object sender, RoutedEventArgs e)
		{
			if(lsvStageOrder.SelectedIndex >= 0)
			{
				if(lsvStageOrder.SelectedIndex < StageOrder.lstStageOrder.Count-1)
				{
					StageOrderList solMemory = new StageOrderList();

					solMemory = StageOrder.lstStageOrder[lsvStageOrder.SelectedIndex];
					StageOrder.lstStageOrder.RemoveAt(lsvStageOrder.SelectedIndex);
					StageOrder.lstStageOrder.Insert(lsvStageOrder.SelectedIndex + 1, solMemory);

					StageOrder.OrderListWriting(StageOrder.lstStageOrder);
					ListViewStageOrderUpdate();
					ResetStageNum(solMemory.name);
				}
			}
		}

		private void btnItemUp_Click(object sender, RoutedEventArgs e)
		{
			if (lsvStageOrder.SelectedIndex >= 0)
			{
				if (lsvStageOrder.SelectedIndex >= 1)
				{
					StageOrderList solMemory = new StageOrderList();

					solMemory = StageOrder.lstStageOrder[lsvStageOrder.SelectedIndex];
					StageOrder.lstStageOrder.RemoveAt(lsvStageOrder.SelectedIndex);
					StageOrder.lstStageOrder.Insert(lsvStageOrder.SelectedIndex - 1, solMemory);

					StageOrder.OrderListWriting(StageOrder.lstStageOrder);
					ListViewStageOrderUpdate();
					ResetStageNum(solMemory.name);
				}
			}
		}

		private void ResetStageNum(string name)
		{
			int resetNum = 0;

			for(int i = 0; i < StageOrder.lstStageOrder.Count; i++)
			{
				if(name == StageOrder.lstStageOrder[i].name)
				{
					resetNum = i;
					break;
				}
				
			}

			StageManager.stageNum = resetNum;
		}

		private void btnRefer_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
