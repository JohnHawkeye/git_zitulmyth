using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Media;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Zitulmyth.Enums;
using Zitulmyth.Data;

namespace Zitulmyth
{
	[DataContract]
	public class EventDataProperty
	{
		public int ID { get; set; }

		[DataMember]
		public EventCommandEnum eventType { get; set; }
		[DataMember]
		public int eventValue { get; set; }
		[DataMember]
		public BgmName bgmName { get; set; }
		[DataMember]
		public SeName seName { get; set; }
		[DataMember]
		public bool flagKeyLock { get; set; }
		[DataMember]
		public Vector balloonPos { get; set; }
		[DataMember]
		public string balloonMsg { get; set; }
		[DataMember]
		public bool balloonEnterClose { get; set; }
		[DataMember]
		public Vector setPosition { get; set; }
		[DataMember]
		public TargetImageType targetImgType { get; set; }
		[DataMember]
		public ObjectName objectName { get; set; }
		[DataMember]
		public TargetCBSource targetCBSource { get; set; }
		[DataMember]
		public int imgIndex { get; set; }
		[DataMember]
		public Vector moveDistance { get; set; }
		[DataMember]
		public Vector moveTotal { get; set; }
		[DataMember]
		public double moveSpeed { get; set; }
		[DataMember]
		public bool direction { get; set; }
		[DataMember]
		public bool fadeType { get; set; }
		[DataMember]
		public bool uiVisible { get; set; }
		[DataMember]
		public ColorEnum color { get; set; }
		[DataMember]
		public bool eventOnly { get; set; }
	}

	public partial class EventEditorWindow : Window
	{

		private ObservableCollection<EventDataProperty> obe = new ObservableCollection<EventDataProperty>();
		private List<EventDataProperty> propertyEventData = new List<EventDataProperty>();
		
		public EventEditorWindow()
		{
			InitializeComponent();

			
		}

		private void EventEditor_Closed(object sender, EventArgs e)
		{

		}

		private void dgEventData_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			propertyEventData.Clear();
			dgEventData.ItemsSource = "";

			StageEvent.InitEvent();

			foreach(var i in StageEvent.listEvent)
			{
				propertyEventData.Add(new EventDataProperty {

					eventType = i.eventType ,
					eventValue =i.eventValue,
					bgmName = i.bgmName,
					seName = i.seName,
					flagKeyLock = i.flagKeyLock,
					balloonPos = i.balloonPos,
					balloonMsg = i.balloonMsg,
					balloonEnterClose = i.balloonEnterClose,
					setPosition =i.setPosition,
					targetImgType = i.targetImgType,
					objectName = i.objectName,
					targetCBSource = i.targetCBSource,
					imgIndex = i.imgIndex,
					moveDistance = i.moveDistance,
					moveTotal = i.moveTotal,
					moveSpeed = i.moveSpeed,
					direction = i.direction,
					fadeType = i.fadeType,
					uiVisible = i.uiVisible,
					color = i.color,
					eventOnly = i.eventOnly,
					
				});

			}

			
			dgEventData.ItemsSource = propertyEventData;

		}

		private void btnJsonWrite_Click(object sender, RoutedEventArgs e)
		{
			DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<EventDataProperty>));
		
			FileStream fs = new FileStream("json/event/event" + StageManager.stageNum.ToString() + ".json",FileMode.Create);
			try
			{
				json.WriteObject(fs, propertyEventData);
			}
			finally
			{
				fs.Close();
			}
			
			//Console.WriteLine(json);
		}

		private void btnJsonRead_Click(object sender, RoutedEventArgs e)
		{
			DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<EventDataProperty>));

			FileStream fs = new FileStream("json/event/event" + StageManager.stageNum.ToString() + ".json", FileMode.Open);

			try
			{
				propertyEventData.Clear();
				propertyEventData = (List<EventDataProperty>)json.ReadObject(fs);
				dgEventData.ItemsSource = propertyEventData;
			}
			finally
			{
				fs.Close();
			}


		}
	}
}
