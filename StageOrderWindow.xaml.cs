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

			for (int i = 0; i< StageOrder.lstStageOrder.Count; i++)
			{
				if(StageOrder.lstStageOrder[i].name == txbStageName.Text)
				{
					overlapCheck = true;
					break;
				}
			}

			if(txbStageName != null)
			{
				if (txbStageName.Text.IndexOfAny(invalidChars) < 0)
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
							eventFileName = txbStageName.Text + ".json",
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

			List<StageEditorData> lstNewStage = new List<StageEditorData>();
			lstNewStage.Add(new StageEditorData { });

			DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<StageEditorData>));

			FileStream fs = new FileStream("Assets/json/stage/" + txbStageName.Text + ".json", FileMode.Create);

			try
			{
				json.WriteObject(fs, lstNewStage);
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

			FileStream fs = new FileStream("Assets/json/event/event_" + txbStageName.Text + ".json", FileMode.Create);
			try
			{
				json.WriteObject(fs, lstNewEventData);

			}
			finally
			{
				fs.Close();
			}
		}
	}
}
