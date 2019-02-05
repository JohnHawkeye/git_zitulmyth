using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zitulmyth.Checking;
using Zitulmyth.Data;

namespace Zitulmyth
{
	public enum ItemAttribute
	{
		Consumable = 0,
		Equipment = 1,
		Tool = 2,
		Treasure = 3,
	}

	public class ItemData
	{

		public string itemName;
		public string sprite;
		public ItemAttribute attribute;
		public Image imgItem;

		public Vector position;
		public Vector size;

		public int weight;

		public int maxLife;
		public int nowLife;
		public int maxMana;
		public int nowMana;

		public int offense;
		public int meleespeed;
		public int defense;

		public int damageInterval;
		public bool invinsible;
		public bool knockback;

		public int speed;
		public int jumpMaxHeight;
		public int jumpCount;
		public int addingWeight;

		public int score;
		public int money;
		public int timeLimit;
		public int switchId;
		public bool timeStop;
		public bool bomb;

		public int expirationTime;
		public int totalTime;

		public int keyFlame;
		public int totalAnimTime;

	}

	public class Item
	{

		public static List<ItemData> lstItemData = new List<ItemData>();

		public static int DatabaseItemNameSearch(int target)
		{
			int index;

			for (int i = 0; i < StageData.lstDbItem.Count; i++)
			{
				if (lstItemData[target].itemName == StageData.lstDbItem[i].name)
				{
					return index = i;
				}
			}

			return index = -1;
		}

		public static void ItemGenerate(Canvas canvas,int itemid,Vector targetpos)
		{
			lstItemData.Add(new ItemData {
				itemName = StageData.lstDbItem[itemid].name,
				sprite = StageData.lstDbItem[itemid].sprite,
				attribute = StageData.lstDbItem[itemid].attribute,
				imgItem = new Image {
					Source = ImageData.ImageSourceSelector(CategoryName.Item, StageData.lstDbItem[itemid].sprite),
					Width = StageData.lstDbItem[itemid].size.X,
					Height = StageData.lstDbItem[itemid].size.Y,
				},
				position = targetpos,
				size = StageData.lstDbItem[itemid].size,
				weight = StageData.lstDbItem[itemid].weight,
				maxLife = StageData.lstDbItem[itemid].maxLife,
				nowLife = StageData.lstDbItem[itemid].nowLife,
				maxMana = StageData.lstDbItem[itemid].maxMana,
				nowMana = StageData.lstDbItem[itemid].nowMana,
				offense = StageData.lstDbItem[itemid].offense,
				meleespeed = StageData.lstDbItem[itemid].meleeSpeed,
				defense = StageData.lstDbItem[itemid].defense,
				damageInterval = StageData.lstDbItem[itemid].damageInterval,
				invinsible = StageData.lstDbItem[itemid].invincible,
				knockback = StageData.lstDbItem[itemid].knockback,
				speed = StageData.lstDbItem[itemid].speed,
				jumpMaxHeight = StageData.lstDbItem[itemid].jumpMaxHeight,
				jumpCount = StageData.lstDbItem[itemid].jumpCount,
				addingWeight = StageData.lstDbItem[itemid].addingWeight,
				score =  StageData.lstDbItem[itemid].score,
				money = StageData.lstDbItem[itemid].money,
				timeLimit = StageData.lstDbItem[itemid].timeLimit,
				switchId = StageData.lstDbItem[itemid].switchId,
				timeStop = StageData.lstDbItem[itemid].timeStop,
				bomb = StageData.lstDbItem[itemid].bomb
			});
			
			int index = lstItemData.Count - 1;

			canvas.Children.Add(lstItemData[index].imgItem);
			Canvas.SetLeft(lstItemData[index].imgItem, lstItemData[index].position.X);
			Canvas.SetTop(lstItemData[index].imgItem, lstItemData[index].position.Y);
			Canvas.SetZIndex(lstItemData[index].imgItem, ImageZindex.item);
		}

		public static void FallingItems()
		{
			for(int i = 0; i < lstItemData.Count; i++)
			{

				double posX = Canvas.GetLeft(lstItemData[i].imgItem);
				double posY = Canvas.GetTop(lstItemData[i].imgItem);

				if (!BlockCheck.BlockCheckBottom(posX, posY,(int)lstItemData[i].size.X ,(int)lstItemData[i].size.Y, lstItemData[i].weight))
				{
					
					posY += SystemOperator.PixelPerSecond(lstItemData[i].weight);
				
				}

				Canvas.SetTop(lstItemData[i].imgItem, posY);
			}


		}
	}

}
