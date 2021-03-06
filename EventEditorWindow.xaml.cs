﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
using System.Text.RegularExpressions;
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
		public TargetType targetImgType { get; set; }
		[DataMember]
		public int targetId { get; set; }
		[DataMember]
		public CategoryName categoryName { get; set; }
		[DataMember]
		public string patternName { get; set; }
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
		private string[] strEventCommand;
		private string[] strCategoryName;
		private string[] strTargetImage;
		private List<string> lstPatternName = new List<string>();
		private int loadingCount = 0;
		
		private TextBox txbOptValue = new TextBox();
		private TextBox txbOptBalloonX = new TextBox();
		private TextBox txbOptBalloonY = new TextBox();
		private TextBox txbOptMessage = new TextBox();
		private TextBox txbOptStartX = new TextBox();
		private TextBox txbOptStartY = new TextBox();
		private TextBox txbOptDistanceX = new TextBox();
		private TextBox txbOptDistanceY = new TextBox();
		private TextBox txbOptSpeed = new TextBox();
		private TextBox txbOptTargetId = new TextBox();

		private CheckBox ckbOptKeyLock = new CheckBox();
		private CheckBox ckbOptEnterClose = new CheckBox();
		private CheckBox ckbOptDirection = new CheckBox();
		private CheckBox ckbOptFadeType = new CheckBox();
		private CheckBox ckbOptUiVisibility = new CheckBox();
		private CheckBox ckbOptEventOnly = new CheckBox();

		private Label ctlDataIndex = new Label();
		private ComboBox ctlEventType = new ComboBox();
		private Button ctlUpdate = new Button();

		private ComboBox cmbOptBgmName = new ComboBox();
		private ComboBox cmbOptSeName = new ComboBox();
		private ComboBox cmbOptTargetImage = new ComboBox();
		private ComboBox cmbOptColor = new ComboBox();
		private ComboBox cmbOptCategoryName = new ComboBox();
		private ComboBox cmbOptPatternName = new ComboBox();

		public EventEditorWindow()
		{
			InitializeComponent();
		}

		private void btnJsonWrite_Click(object sender, RoutedEventArgs e)
		{

			MessageBoxResult result =
				MessageBox.Show("イベントデータを上書きしますか？","データの上書き",
				MessageBoxButton.YesNo, MessageBoxImage.Warning);

			if(result == MessageBoxResult.Yes)
			{
				DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<EventDataProperty>));

				FileStream fs = new FileStream("Assets/json/event/" + StageOrder.lstStageOrder[StageManager.stageNum].eventFileName, FileMode.Create);
				try
				{
					json.WriteObject(fs, propertyEventData);
				}
				finally
				{
					fs.Close();
				}
			}

		}
		
		private void EventEditor_Loaded(object sender, RoutedEventArgs e)
		{

			strEventCommand = Enum.GetNames(typeof(EventCommandEnum));
			strCategoryName = Enum.GetNames(typeof(CategoryName));
			strTargetImage = Enum.GetNames(typeof(TargetType));
			
			ctlEventType = cmbEventType;
			ctlDataIndex = lblDataIndex;
			ctlUpdate = btnUpdate;

			OptionControlSetting();
			
			string fileName = "Assets/json/event/" + StageOrder.lstStageOrder[StageManager.stageNum].eventFileName;

			if (File.Exists(fileName))
			{

				DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(List<EventDataProperty>));

				FileStream fs = new FileStream("Assets/json/event/" + StageOrder.lstStageOrder[StageManager.stageNum].eventFileName, FileMode.Open);

				try
				{
					propertyEventData.Clear();
					propertyEventData = (List<EventDataProperty>)json.ReadObject(fs);

					for (int i = 0; i < propertyEventData.Count; i++)
					{
						propertyEventData[i].ID = i;
					}

					dgEventData.ItemsSource = propertyEventData;
				}
				finally
				{
					fs.Close();
				}

			}
			else
			{

				MessageBox.Show("イベントファイルが読み込めません。","読み込みエラー", MessageBoxButton.OK, MessageBoxImage.Information);

				this.Close();

			}



		}
		
		private void dgEventData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			int row = dgEventData.Items.IndexOf(dgEventData.SelectedItem);

			if (row >= 0)
			{
				cmbEventType.ItemsSource = strEventCommand;
				cmbEventType.SelectedItem = propertyEventData[row].eventType.ToString();

				lblDataIndex.Content = row.ToString() + ":";

				OptionControlDataSet(row);
			}

			Console.WriteLine(row);
		}

		private void OptionControlDataSet(int row)
		{
			switch (cmbEventType.SelectedItem)
			{

				case "Wait":

					txbOptValue.Text = propertyEventData[row].eventValue.ToString();
					break;

				case "Balloon":

					txbOptBalloonX.Text = propertyEventData[row].balloonPos.X.ToString();
					txbOptBalloonY.Text = propertyEventData[row].balloonPos.Y.ToString();
					txbOptMessage.Text = propertyEventData[row].balloonMsg;
					ckbOptEnterClose.IsChecked = propertyEventData[row].balloonEnterClose;
					cmbOptTargetImage.SelectedItem = propertyEventData[row].targetImgType.ToString();
					txbOptTargetId.Text = propertyEventData[row].targetId.ToString();
					break;

				case "BgmPlay":
					cmbOptBgmName.SelectedItem = propertyEventData[row].bgmName.ToString();
					break;

				case "SePlay":
					cmbOptSeName.SelectedItem = propertyEventData[row].seName.ToString();
					break;

				case "Move":
					cmbOptTargetImage.SelectedItem = propertyEventData[row].targetImgType.ToString();
					txbOptTargetId.Text = propertyEventData[row].targetId.ToString();
					txbOptDistanceX.Text = propertyEventData[row].moveDistance.X.ToString();
					txbOptDistanceY.Text = propertyEventData[row].moveDistance.Y.ToString();
					txbOptSpeed.Text = propertyEventData[row].moveSpeed.ToString();
					ckbOptDirection.IsChecked = propertyEventData[row].direction;

					break;

				case "KeyLock":
					ckbOptKeyLock.IsChecked = propertyEventData[row].flagKeyLock;
					break;

				case "UiVisibility":
					ckbOptUiVisibility.IsChecked = propertyEventData[row].uiVisible;
					break;

				case "CharaFadeIn":
					txbOptValue.Text = propertyEventData[row].eventValue.ToString();
					cmbOptTargetImage.SelectedItem = propertyEventData[row].targetImgType.ToString();
					break;

				case "CharaImageChange":
					cmbOptTargetImage.SelectedItem = propertyEventData[row].targetImgType.ToString();
					txbOptTargetId.Text = propertyEventData[row].targetId.ToString();

					cmbOptCategoryName.SelectedItem = propertyEventData[row].categoryName.ToString();
					if(propertyEventData[row].patternName == null)
					{
						cmbOptPatternName.SelectedIndex = 0;
					}
					else
					{
						cmbOptPatternName.SelectedItem = propertyEventData[row].patternName.ToString();
					}
					

					break;

				case "ScreenFadeIn":
					txbOptValue.Text = propertyEventData[row].eventValue.ToString();
					ckbOptFadeType.IsChecked = propertyEventData[row].fadeType;
					cmbOptColor.SelectedItem = propertyEventData[row].color.ToString();
					break;

				case "ScreenFadeOut":
					txbOptValue.Text = propertyEventData[row].eventValue.ToString();
					ckbOptFadeType.IsChecked = propertyEventData[row].fadeType;
					cmbOptColor.SelectedItem = propertyEventData[row].color.ToString();
					break;

				case "GenerateEnemy":
					txbOptStartX.Text = propertyEventData[row].setPosition.X.ToString();
					txbOptStartY.Text = propertyEventData[row].setPosition.Y.ToString();
					break;

				case "EventEnd":
					ckbOptEventOnly.IsChecked = propertyEventData[row].eventOnly;
					break;

			}
		}

		private void cmbEventType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			grdOption.Children.Clear();

			grdOption.Children.Add(ctlEventType);
			grdOption.Children.Add(ctlDataIndex);
			grdOption.Children.Add(ctlUpdate);

			switch (cmbEventType.SelectedItem)
			{

				case "Wait":

					grdOption.Children.Add(txbOptValue);
					txbOptValue.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(txbOptValue, 0);

					//margin + 20left

					break;
				case "Balloon":

					//56+20
					grdOption.Children.Add(txbOptBalloonX);
					txbOptBalloonX.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(txbOptBalloonX, 0);

					grdOption.Children.Add(txbOptBalloonY);
					txbOptBalloonY.Margin = new Thickness(260, 6, 0, 0);
					Grid.SetRow(txbOptBalloonY, 0);

					grdOption.Children.Add(txbOptMessage);
					txbOptMessage.Margin = new Thickness(10, 6, 0, 0);
					Grid.SetRow(txbOptMessage, 1);

					grdOption.Children.Add(ckbOptEnterClose);
					ckbOptEnterClose.Margin = new Thickness(10, 6, 0, 0);
					Grid.SetRow(ckbOptEnterClose, 2);

					grdOption.Children.Add(cmbOptTargetImage);
					cmbOptTargetImage.Margin = new Thickness(10+20+112, 6, 0, 0);
					Grid.SetRow(cmbOptTargetImage, 2);

					grdOption.Children.Add(txbOptTargetId);
					txbOptTargetId.Margin = new Thickness(10+40+112+128, 6, 0, 0);
					Grid.SetRow(txbOptTargetId, 2);

					break;

				case "BgmPlay":
					grdOption.Children.Add(cmbOptBgmName);
					cmbOptBgmName.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(cmbOptBgmName, 0);

					break;

				case "SePlay":
					grdOption.Children.Add(cmbOptSeName);
					cmbOptSeName.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(cmbOptSeName, 0);
					break;

				case "Move":
					grdOption.Children.Add(cmbOptTargetImage);
					cmbOptTargetImage.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(cmbOptTargetImage, 0);

					grdOption.Children.Add(txbOptTargetId);
					txbOptTargetId.Margin = new Thickness(184+128+20, 6, 0, 0);
					Grid.SetRow(txbOptTargetId, 0);

					grdOption.Children.Add(txbOptDistanceX);
					txbOptDistanceX.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(txbOptDistanceX, 1);

					grdOption.Children.Add(txbOptDistanceY);
					txbOptDistanceY.Margin = new Thickness(184+56+20, 6, 0, 0);
					Grid.SetRow(txbOptDistanceY, 1);

					grdOption.Children.Add(txbOptSpeed);
					txbOptSpeed.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(txbOptSpeed, 2);

					grdOption.Children.Add(ckbOptDirection);
					ckbOptDirection.Margin = new Thickness(184+56+20, 6, 0, 0);
					Grid.SetRow(ckbOptDirection, 2);
					break;

				case "KeyLock":
					grdOption.Children.Add(ckbOptKeyLock);
					ckbOptKeyLock.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(ckbOptKeyLock, 0);

					break;

				case "UiVisibility":
					grdOption.Children.Add(ckbOptUiVisibility);
					ckbOptUiVisibility.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(ckbOptUiVisibility, 0);
					break;

				case "CharaFadeIn":
					grdOption.Children.Add(ckbOptFadeType);
					ckbOptFadeType.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(ckbOptFadeType, 0);
					break;

				case "CharaImageChange":
					grdOption.Children.Add(cmbOptTargetImage);
					cmbOptTargetImage.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(cmbOptTargetImage, 0);

					grdOption.Children.Add(txbOptTargetId);
					txbOptTargetId.Margin = new Thickness(184+128+20, 6, 0, 0);
					Grid.SetRow(txbOptTargetId, 0);

					grdOption.Children.Add(cmbOptCategoryName);
					cmbOptCategoryName.Margin = new Thickness(184, 6, 0, 0);
					cmbOptCategoryName.SelectionChanged += new SelectionChangedEventHandler(CategoryChosedPatternReading);
					Grid.SetRow(cmbOptCategoryName, 1);

					grdOption.Children.Add(cmbOptPatternName);
					cmbOptPatternName.Margin = new Thickness(184 + 20 + 128, 6, 0, 0);
					Grid.SetRow(cmbOptPatternName, 1);
					break;

				case "ScreenFadeIn":
					grdOption.Children.Add(txbOptValue);
					txbOptValue.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(txbOptValue, 0);

					grdOption.Children.Add(ckbOptFadeType);
					ckbOptFadeType.Margin = new Thickness(184+56+20, 6, 0, 0);
					Grid.SetRow(ckbOptFadeType, 0);

					grdOption.Children.Add(cmbOptColor);
					cmbOptColor.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(cmbOptColor, 1);
					break;

				case "ScreenFadeOut":
					grdOption.Children.Add(txbOptValue);
					txbOptValue.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(txbOptValue, 0);

					grdOption.Children.Add(ckbOptFadeType);
					ckbOptFadeType.Margin = new Thickness(184 + 56 + 20, 6, 0, 0);
					Grid.SetRow(ckbOptFadeType, 0);

					grdOption.Children.Add(cmbOptColor);
					cmbOptColor.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(cmbOptColor, 1);
					break;

				case "GenerateEnemy":
					grdOption.Children.Add(txbOptStartX);
					txbOptStartX.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(txbOptStartX, 0);

					grdOption.Children.Add(txbOptStartY);
					txbOptStartY.Margin = new Thickness(184+56+20, 6, 0, 0);
					Grid.SetRow(txbOptStartY, 0);
					break;

				case "EventEnd":
					grdOption.Children.Add(ckbOptEventOnly);
					ckbOptEventOnly.Margin = new Thickness(184, 6, 0, 0);
					Grid.SetRow(ckbOptEventOnly, 0);
					break;

			}

			btnUpdate.IsEnabled = true;
			btnInsert.IsEnabled = true;
			btnDelete.IsEnabled = true;
			btnAdd.IsEnabled = true;
		}

		public void OptionControlSetting()
		{

			//txt
			txbOptValue = new TextBox {

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Right,
				Text = "1000",
				Width = 56, Height = 26,
				FontSize = 16,
				ToolTip = "値"
		
			};
			
			txbOptBalloonX = new TextBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Right,
				Text = "1000",
				Width = 56,
				Height = 26,
				FontSize = 16,
				ToolTip = "ふきだしの位置X"
			};

			txbOptBalloonY = new TextBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Right,
				Text = "1000",
				Width = 56,
				Height = 26,
				FontSize = 16,
				ToolTip = "ふきだしの位置Y"
			};

			txbOptMessage = new TextBox
			{

				HorizontalAlignment = HorizontalAlignment.Center,
				VerticalAlignment = VerticalAlignment.Top,
				AcceptsReturn = true,
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
				Text = "",
				Width = 512,
				Height = 26,
				FontSize = 16,
				ToolTip = "メッセージ"
			};

			txbOptStartX = new TextBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Right,
				Text = "1000",
				Width = 56,
				Height = 26,
				FontSize = 16,
				ToolTip = "StartPositionX"
			};

			txbOptStartY = new TextBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Right,
				Text = "1000",
				Width = 56,
				Height = 26,
				FontSize = 16,
				ToolTip = "StartPositionY"
			};

			txbOptDistanceX = new TextBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Right,
				Text = "1000",
				Width = 56,
				Height = 26,
				FontSize = 16,
				ToolTip = "DistanceX"
			};

			txbOptDistanceY = new TextBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Right,
				Text = "1000",
				Width = 56,
				Height = 26,
				FontSize = 16,
				ToolTip = "DistanceY"
			};

			txbOptSpeed = new TextBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Right,
				Text = "1000",
				Width = 56,
				Height = 26,
				FontSize = 16,
				ToolTip = "Speed"
			};

			txbOptTargetId = new TextBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Right,
				Text = "0",
				Width = 56,
				Height = 26,
				FontSize = 16,
				ToolTip = "TargetId"
			};

			//checkbox

			ckbOptKeyLock = new CheckBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Center,

				Width = 112,
				Height = 26,
				FontSize = 16,
				Content = "：KeyLock",
				Foreground = new SolidColorBrush(Colors.White)
			};

			ckbOptEnterClose = new CheckBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Center,

				Width = 112,
				Height = 26,
				FontSize = 16,
				Content = "：EnterClose",
				IsChecked = true,
				Foreground = new SolidColorBrush(Colors.White)
			};

			ckbOptDirection = new CheckBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Center,

				Width = 112,
				Height = 26,
				FontSize = 16,
				Content = "：Direction",
				ToolTip = "False:Left, True:Right",
				Foreground = new SolidColorBrush(Colors.White)
			};

			ckbOptFadeType = new CheckBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Center,

				Width = 112,
				Height = 26,
				FontSize = 16,
				Content = "：FadeType",
				ToolTip = "False:FadeOut,True:FadeIn",
				Foreground = new SolidColorBrush(Colors.White)
			};

			ckbOptUiVisibility = new CheckBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Center,

				Width = 112,
				Height = 26,
				FontSize = 16,
				Content = "：UIVisible",
				Foreground = new SolidColorBrush(Colors.White)
			};

			ckbOptEventOnly = new CheckBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Center,

				Width = 112,
				Height = 26,
				FontSize = 16,
				Content = "：EventOnly",
				Foreground = new SolidColorBrush(Colors.White)
			};

		//combobox

			cmbOptBgmName = new ComboBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Top,

				Width = 128,
				Height = 26,
				FontSize = 16,
				ToolTip = "BgmList",
				ItemsSource = new string[] { "None", "Opening", "Darkness" },

			};

			cmbOptSeName = new ComboBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Top,

				Width = 128,
				Height = 26,
				FontSize = 16,
				ToolTip = "SeList",
				ItemsSource = new string[] { "None", "Player_Damage", "Fog", "Item_Get", "Shock" },

			};

			cmbOptTargetImage = new ComboBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Top,

				Width = 128,
				Height = 26,
				FontSize = 16,
				ToolTip = "TargetImage",
				ItemsSource = strTargetImage,

			};

			cmbOptColor = new ComboBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Top,

				Width = 128,
				Height = 26,
				FontSize = 16,
				ToolTip = "Color",
				ItemsSource = new string[]{ "White", "Black" },
			};

			cmbOptCategoryName = new ComboBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Top,

				Width = 128,
				Height = 26,
				FontSize = 16,
				ToolTip = "カテゴリを選んでください",
				ItemsSource = strCategoryName,

			};

			cmbOptPatternName = new ComboBox
			{

				HorizontalAlignment = HorizontalAlignment.Left,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalContentAlignment = HorizontalAlignment.Left,
				VerticalContentAlignment = VerticalAlignment.Top,

				Width = 128,
				Height = 26,
				FontSize = 16,
				ToolTip = "変更したい画像のパターンを選択してください。",
				ItemsSource = { },

			};

		}

		private void CategoryChosedPatternReading(object sender, SelectionChangedEventArgs e)
		{

			CategoryName cn = (CategoryName)Enum.Parse(typeof(CategoryName), ((ComboBox)sender).SelectedItem.ToString());
			lstPatternName.Clear();
			lstPatternName = ImageData.PatternNameListCreating(cn);

			cmbOptPatternName.ItemsSource = lstPatternName;

		}

		private void btnUpdate_Click(object sender, RoutedEventArgs e)
		{

			int row = dgEventData.Items.IndexOf(dgEventData.SelectedItem);

			if (row >= 0)
			{
				propertyEventData[row].eventType =
					(EventCommandEnum)Enum.Parse(typeof(EventCommandEnum), cmbEventType.SelectedItem.ToString());

				OptionDataUpdate(row);

				dgEventData.ItemsSource = propertyEventData;
				dgEventData.Items.Refresh();

				stpOption.IsEnabled = false;
				
				loadingCount = 1;
				Console.WriteLine("PushButton");
			}

		}

		private void OptionDataUpdate(int row)
		{
			switch (cmbEventType.SelectedItem)
			{

				case "Wait":

					if (SystemOperator.IsNumeric(txbOptValue.Text))
					{
						propertyEventData[row].eventValue = int.Parse(txbOptValue.Text);
					}
					else
					{
						MessageBox.Show("値が正しくありません。半角数字を入力してください。", "入力", MessageBoxButton.OK, MessageBoxImage.Information);
					}

					break;

				case "Balloon":

					if (SystemOperator.IsNumeric(txbOptBalloonX.Text)&&
						SystemOperator.IsNumeric(txbOptBalloonY.Text)&&
						SystemOperator.IsNumeric(txbOptTargetId.Text))
					{
						propertyEventData[row].balloonPos =
						new Vector(int.Parse(txbOptBalloonX.Text), int.Parse(txbOptBalloonY.Text));

						propertyEventData[row].balloonMsg = txbOptMessage.Text;
						propertyEventData[row].balloonEnterClose = (bool)ckbOptEnterClose.IsChecked;

						propertyEventData[row].targetImgType =
							(TargetType)Enum.Parse(typeof(TargetType), cmbOptTargetImage.SelectedItem.ToString());

						propertyEventData[row].targetId = int.Parse(txbOptTargetId.Text);
					}
					else
					{
						MessageBox.Show("値が正しくありません。半角数字を入力してください。", "入力", MessageBoxButton.OK, MessageBoxImage.Information);
					}



					break;

				case "BgmPlay":

					propertyEventData[row].bgmName =
						(BgmName)Enum.Parse(typeof(BgmName), cmbOptBgmName.SelectedItem.ToString());

					break;

				case "SePlay":

					propertyEventData[row].seName =
						(SeName)Enum.Parse(typeof(SeName), cmbOptSeName.SelectedItem.ToString());

					break;

				case "Move":

					if(SystemOperator.IsNumeric(txbOptTargetId.Text)&&
					   SystemOperator.IsNumeric(txbOptSpeed.Text))
					{
						propertyEventData[row].targetImgType =
						(TargetType)Enum.Parse(typeof(TargetType), cmbOptTargetImage.SelectedItem.ToString());

						propertyEventData[row].targetId = int.Parse(txbOptTargetId.Text);

						propertyEventData[row].moveDistance =
							new Vector(int.Parse(txbOptDistanceX.Text), int.Parse(txbOptDistanceY.Text));

						propertyEventData[row].moveSpeed = int.Parse(txbOptSpeed.Text);
						propertyEventData[row].direction = (bool)ckbOptDirection.IsChecked;
					}
					else
					{
						MessageBox.Show("値が正しくありません。半角数字を入力してください。", "入力", MessageBoxButton.OK, MessageBoxImage.Information);
					}

					break;

				case "KeyLock":

					propertyEventData[row].flagKeyLock = (bool)ckbOptKeyLock.IsChecked;

					break;

				case "UiVisibility":

					propertyEventData[row].uiVisible = (bool)ckbOptUiVisibility.IsChecked;

					break;

				case "CharaFadeIn":

					if (SystemOperator.IsNumeric(txbOptValue.Text))
					{
						propertyEventData[row].eventValue = int.Parse(txbOptValue.Text);

						propertyEventData[row].targetImgType =
							(TargetType)Enum.Parse(typeof(TargetType), cmbOptTargetImage.SelectedItem.ToString());

					}
					else
					{
						MessageBox.Show("値が正しくありません。半角数字を入力してください。", "入力", MessageBoxButton.OK, MessageBoxImage.Information);
					}

					break;

				case "CharaImageChange":

					if (SystemOperator.IsNumeric(txbOptTargetId.Text))
					{
						propertyEventData[row].targetImgType =
						(TargetType)Enum.Parse(typeof(TargetType), cmbOptTargetImage.SelectedItem.ToString());

						propertyEventData[row].targetId = int.Parse(txbOptTargetId.Text);

						propertyEventData[row].categoryName =
							(CategoryName)Enum.Parse(typeof(CategoryName), cmbOptCategoryName.SelectedItem.ToString());

						propertyEventData[row].patternName = cmbOptPatternName.SelectedItem.ToString();
					}
					else
					{
						MessageBox.Show("値が正しくありません。半角数字を入力してください。", "入力", MessageBoxButton.OK, MessageBoxImage.Information);
					}

					break;

				case "ScreenFadeIn":

					if (SystemOperator.IsNumeric(txbOptValue.Text))
					{
						propertyEventData[row].eventValue = int.Parse(txbOptValue.Text);

						propertyEventData[row].fadeType = (bool)ckbOptFadeType.IsChecked;

						propertyEventData[row].color =
							(ColorEnum)Enum.Parse(typeof(ColorEnum), cmbOptColor.SelectedItem.ToString());
					}
					else
					{
						MessageBox.Show("値が正しくありません。半角数字を入力してください。", "入力", MessageBoxButton.OK, MessageBoxImage.Information);
					}

					break;

				case "ScreenFadeOut":

					if (SystemOperator.IsNumeric(txbOptValue.Text))
					{
						propertyEventData[row].eventValue = int.Parse(txbOptValue.Text);

						propertyEventData[row].fadeType = (bool)ckbOptFadeType.IsChecked;

						propertyEventData[row].color =
							(ColorEnum)Enum.Parse(typeof(ColorEnum), cmbOptColor.SelectedItem.ToString());
					}
					else
					{
						MessageBox.Show("値が正しくありません。半角数字を入力してください。", "入力", MessageBoxButton.OK, MessageBoxImage.Information);
					}

					break;

				case "GenerateEnemy":

					if (SystemOperator.IsNumeric(txbOptStartX.Text)&&
						SystemOperator.IsNumeric(txbOptStartY.Text))
					{
						propertyEventData[row].setPosition =
							new Vector(int.Parse(txbOptStartX.Text), int.Parse(txbOptStartY.Text));

					}
					else
					{
						MessageBox.Show("値が正しくありません。半角数字を入力してください。", "入力", MessageBoxButton.OK, MessageBoxImage.Information);
					}

					break;

				case "EventEnd":

					propertyEventData[row].eventOnly = (bool)ckbOptEventOnly.IsChecked;
					
					break;

			}
		}


		private void dgEventData_LoadingRow(object sender, DataGridRowEventArgs e)
		{

			if (loadingCount >= propertyEventData.Count)
			{
				stpOption.IsEnabled = true;
			}
			else
			{
				loadingCount++;
			}
		
			
		}

		private void btnUpdate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void btnInsert_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void btnAdd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void btnDelete_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void btnInsert_Click(object sender, RoutedEventArgs e)
		{
			int row = dgEventData.Items.IndexOf(dgEventData.SelectedItem);

			if (row >= 0)
			{
				propertyEventData.Insert(row,new EventDataProperty());

				for (int i = 0; i < propertyEventData.Count; i++)
				{
					propertyEventData[i].ID = i;
				}

				dgEventData.ItemsSource = propertyEventData;
				dgEventData.Items.Refresh();

				stpOption.IsEnabled = false;

				loadingCount = 1;

			}
		}

		private void btnAdd_Click(object sender, RoutedEventArgs e)
		{
			int row = dgEventData.Items.IndexOf(dgEventData.SelectedItem);

			if (row >= 0)
			{
				propertyEventData.Add(new EventDataProperty());

				for (int i = 0; i < propertyEventData.Count; i++)
				{
					propertyEventData[i].ID = i;
				}

				dgEventData.ItemsSource = propertyEventData;
				dgEventData.Items.Refresh();

				stpOption.IsEnabled = false;

				loadingCount = 1;

			}
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			int row = dgEventData.Items.IndexOf(dgEventData.SelectedItem);

			if (row >= 0)
			{

				MessageBoxResult result =
					MessageBox.Show(row.ToString() + " 番のイベントを削除しますか？", "イベントの削除",
					MessageBoxButton.YesNo, MessageBoxImage.Warning);

				if (result == MessageBoxResult.Yes)
				{

					propertyEventData.RemoveAt(row);

					for (int i = 0; i < propertyEventData.Count; i++)
					{
						propertyEventData[i].ID = i;
					}

					dgEventData.ItemsSource = propertyEventData;
					dgEventData.Items.Refresh();

					btnUpdate.IsEnabled = false;
					btnAdd.IsEnabled = false;
					btnDelete.IsEnabled = false;
					btnInsert.IsEnabled = false;

					loadingCount = 1;
				}


			}
		}

		private void EventEditor_Closing(object sender, CancelEventArgs e)
		{
			MainWindow.isOpenEventEditorWindow = false;
			MainWindow.stageEditor.IsEnabled = true;
		}

		private void btnCloseWindow_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.isOpenEventEditorWindow = false;
			MainWindow.stageEditor.IsEnabled = true;

			MainWindow.eventEditor.Close();
		}
	}
	
}
