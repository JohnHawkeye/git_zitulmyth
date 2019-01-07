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
									dbb.Add(new DatabaseBlock { name="", sprite="", passage=false});
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
									dbe.Add(new DatabaseEnemy { name = "", sprite = "" });
								}

							}

							MainWindow.databaseWindow.lstViewDbEnemy.Clear();
							MainWindow.databaseWindow.lstViewDbEnemy = dbe;

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
									dbo.Add(new DatabaseObject { name = "", sprite = "" });
								}

							}

							MainWindow.databaseWindow.lstViewDbObject.Clear();
							MainWindow.databaseWindow.lstViewDbObject = dbo;
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
									dbi.Add(new DatabaseItem { name = "", sprite = "" });
								}

							}

							MainWindow.databaseWindow.lstViewDbItem.Clear();
							MainWindow.databaseWindow.lstViewDbItem = dbi;
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
