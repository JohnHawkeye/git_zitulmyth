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
using System.Text.RegularExpressions;

namespace Zitulmyth
{
    /// <summary>
    /// MaxListChangerDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class MaxListChangerDialog : Window
    {

		public int oldnum;

        public MaxListChangerDialog()
        {
            InitializeComponent();
        }

		private void txbNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !new Regex("[0-9]").IsMatch(e.Text);
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			int newnum = int.Parse(txbNumber.Text);

			if(newnum >=1 && newnum <= 255)
			{
				if(oldnum != newnum)
				{
				
					switch (MainWindow.databaseWindow.tabDatabase.SelectedIndex)
					{

						case 1://block

							List<DatabaseBlock> dbb = new List<DatabaseBlock>();

							for (int i = 0; i < newnum; i++)
							{
								if(i < MainWindow.databaseWindow.lstViewDbBlock.Count)
								{
									dbb.Add(MainWindow.databaseWindow.lstViewDbBlock[i]);
								}
								else
								{
									dbb.Add(new DatabaseBlock { name="ブロック " + i, sprite="", passage=false});
								}
								
							}

							MainWindow.databaseWindow.lstViewDbBlock.Clear();
							MainWindow.databaseWindow.lstViewDbBlock = dbb;

							MainWindow.databaseWindow.ListBoxBlockUpdate();

							break;

						case 2://enemy

							List<DatabaseEnemy> dbe = new List<DatabaseEnemy>();

							for (int i = 0; i < newnum; i++)
							{
								if (i < MainWindow.databaseWindow.lstViewDbEnemy.Count)
								{
									dbe.Add(MainWindow.databaseWindow.lstViewDbEnemy[i]);
								}
								else
								{
									dbe.Add(new DatabaseEnemy {
										name = "エネミー " + i,
										life = 0, offense = 0,
										speed = 0, weight = 0, jumpMaxHeight = 0,
										triggerAreaOffset = new Vector(0,0),
										triggerAreaPos = new Vector(0,0),
										triggerAreaSize = new Vector(0,0),
										dropItemID = 0});
								}

							}

							MainWindow.databaseWindow.lstViewDbEnemy.Clear();
							MainWindow.databaseWindow.lstViewDbEnemy = dbe;

							MainWindow.databaseWindow.ListBoxEnemyUpdate();
							break;

						case 3://object

							List<DatabaseObject> dbo = new List<DatabaseObject>();

							for (int i = 0; i < newnum; i++)
							{
								if (i < MainWindow.databaseWindow.lstViewDbObject.Count)
								{
									dbo.Add(MainWindow.databaseWindow.lstViewDbObject[i]);
								}
								else
								{
									dbo.Add(new DatabaseObject {
										name = "オブジェクト " + i,
										spriteA = "", spriteB = "",
										size = new Vector(32,32),
										attribute = ObjectAttribute.DisplayOnly,
										destructable = false, durability = 0,
										switchingTimer = false, targetTime = 0,
										damaging = false, damagevalue = 0,
										underwater = false, slide = false, operable = false,
										automove = false, autoMoveRangeX = 0, autoMoveRangeY = 0, autoMoveSpeed = 0,
										triggerAction = false, triggerType = false,
										influenceSpeed = 0, influenceJump = 0, influenceFall = 0,
										dropItemID = 0});
								}

							}

							MainWindow.databaseWindow.lstViewDbObject.Clear();
							MainWindow.databaseWindow.lstViewDbObject = dbo;

							MainWindow.databaseWindow.ListBoxObjectUpdate();

							break;

						case 4://item

							List<DatabaseItem> dbi = new List<DatabaseItem>();

							for (int i = 0; i < newnum; i++)
							{
								if (i < MainWindow.databaseWindow.lstViewDbItem.Count)
								{
									dbi.Add(MainWindow.databaseWindow.lstViewDbItem[i]);
								}
								else
								{
									dbi.Add(new DatabaseItem { name = "アイテム " + i,
										attribute = ItemAttribute.Consumable,
										weight = 0,
										maxLife = 0, nowLife = 0, maxMana = 0, nowMana = 0,
										offense = 0, meleeSpeed = 0, defense = 0,
										damageInterbal = 0,
										speed = 0, jumpMaxHeight = 0, jumpCount = 0, addingWeight = 0,
										score = 0, money = 0, timeLimit = 0, switchId = 0

									});
								}

							}

							MainWindow.databaseWindow.lstViewDbItem.Clear();
							MainWindow.databaseWindow.lstViewDbItem = dbi;

							MainWindow.databaseWindow.ListBoxItemUpdate();
							break;

					}


				}

				this.Close();

			}
			else
			{
				MessageBox.Show("数値が正しくありません。1-255の範囲で入力してください。", "最大数範囲", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

	}
}
