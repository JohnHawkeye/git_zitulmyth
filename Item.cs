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
	public enum ItemName
	{
		Coin,
		Apple,
		StarFruit,
		TreeBranch,
	}

	public class Item
	{

		public static List<ItemData> lstItemData = new List<ItemData>();

		public static void ItemGenerate(Canvas canvas)
		{
			if(StageManager.stageNum == 2)
			{
				lstItemData.Add(new ItemData
				{
					itemName = ItemName.Coin,
					imgItem = new Image { Source = ImageData.cbItem[0], Width = 32, Height = 32 },
					position = new Vector(128, 384),
					weight = 6,width= 1,height=1,
					expirationTime = 1000,
					totalTime = 0,
				});


				lstItemData.Add(new ItemData
				{
					itemName = ItemName.Coin,
					imgItem = new Image { Source = ImageData.cbItem[0], Width = 32, Height = 32 },
					position = new Vector(192, 384),
					weight = 6,	width= 1,height=1,expirationTime = 1000,	totalTime = 0
				});

				lstItemData.Add(new ItemData
				{
					itemName = ItemName.StarFruit,
					imgItem = new Image { Source = ImageData.cbItem[3], Width = 32, Height = 32 },
					position = new Vector(288, 384),
					weight = 4,width= 1,height=1,	expirationTime = 1000,	totalTime = 0
				});

				lstItemData.Add(new ItemData
				{
					itemName = ItemName.TreeBranch,
					imgItem = new Image { Source = ImageData.cbItem[4], Width = 64, Height = 32 },
					position = new Vector(320, 544),
					weight = 4,width= 2,height=1,expirationTime = 1000,totalTime = 0
				});

				lstItemData.Add(new ItemData
				{
					itemName = ItemName.Apple,
					imgItem = new Image { Source = ImageData.cbItem[2], Width = 32, Height = 32 },
					position = new Vector(480, 384),
					weight = 4,width= 1,height=1,expirationTime = 1000,totalTime = 0
				});

				lstItemData.Add(new ItemData
				{
					itemName = ItemName.Apple,
					imgItem = new Image { Source = ImageData.cbItem[2], Width = 32, Height = 32 },
					position = new Vector(768, 384),
					weight = 4,width= 1,height=1,expirationTime = 1000,totalTime = 0
				});

				for(int i = 0; i < lstItemData.Count; i++)
				{
					canvas.Children.Add(lstItemData[i].imgItem);
					Canvas.SetLeft(lstItemData[i].imgItem, lstItemData[i].position.X);
					Canvas.SetTop(lstItemData[i].imgItem, lstItemData[i].position.Y);
					Canvas.SetZIndex(lstItemData[i].imgItem, 6);
				}
			}
		}

		public static void FallingItems()
		{
			for(int i = 0; i < lstItemData.Count; i++)
			{

				double posX = Canvas.GetLeft(lstItemData[i].imgItem);
				double posY = Canvas.GetTop(lstItemData[i].imgItem);

				if (!BlockCheck.BlockCheckBottom(posX, posY, lstItemData[i].weight,lstItemData[i].height)&&
					!BlockCheck.BlockCheckOnPlat(posX,posY, lstItemData[i].weight,lstItemData[i].height))
				{
					
					posY += lstItemData[i].weight;
				
				}

				Canvas.SetTop(lstItemData[i].imgItem, posY);
			}


		}
	}

	public class ItemData
	{

		public ItemName itemName;
		public Image imgItem;
		public Vector position;
		public int width;
		public int height;
		public int weight;
		public int expirationTime;
		public int totalTime;

	}
}
