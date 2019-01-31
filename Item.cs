﻿using System;
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
		BoarMeat,
	}

	public enum ItemAttribute
	{
		Consumable = 0,
		Equipment = 1,
		Tool = 2,
		Treasure = 3,
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

		public int keyFlame;
		public int totalAnimTime;

	}

	public class Item
	{

		public static List<ItemData> lstItemData = new List<ItemData>();

		public static  ItemData SetItemData(ItemName name,Vector targetpos)
		{
			var item = new ItemData();

			switch (name)
			{
				case ItemName.Coin:

					item = new ItemData
						{
							itemName = ItemName.Coin,
							imgItem = new Image { Source = ImageData.ImageSourceSelector(CategoryName.Item,"Coin"), Width = 32, Height = 32 },
							position = targetpos,
							weight = 160,	width = 1,height = 1,expirationTime = 1000,totalTime = 0,
						};

					break;

				case ItemName.Apple:

					item = new ItemData
						{
							itemName = ItemName.Apple,
							imgItem = new Image { Source = ImageData.ImageSourceSelector(CategoryName.Item, "Apple"), Width = 32, Height = 32 },
							position = targetpos,
							weight = 160,width = 1,height = 1,expirationTime = 1000,totalTime = 0
						};

					break;

				case ItemName.StarFruit:
					item = new ItemData
						{
							itemName = ItemName.StarFruit,
							imgItem = new Image { Source = ImageData.ImageSourceSelector(CategoryName.Item, "StarFruit"), Width = 32, Height = 32 },
							position = targetpos,
							weight = 160,width= 1,height=1,	expirationTime = 1000,	totalTime = 0
						};
					break;

				case ItemName.TreeBranch:
					item = new ItemData
						{
							itemName = ItemName.TreeBranch,
							imgItem = new Image { Source = ImageData.ImageSourceSelector(CategoryName.Item, "TreeBranch"), Width = 64, Height = 32 },
							position = targetpos,
							weight = 160,width= 2,height=1,expirationTime = 1000,totalTime = 0
						};
					break;

				case ItemName.BoarMeat:
					item = new ItemData
						{
							itemName = ItemName.BoarMeat,
							imgItem = new Image { Source = ImageData.ImageSourceSelector(CategoryName.Item, "BoarMeat"), Width = 32, Height = 32 },
							position = targetpos,
							weight = 160,width= 2,height=1,expirationTime = 1000,totalTime = 0
						};
					break;

			}
			return item;
		}

		public static void ItemGenerate(Canvas canvas,ItemName name,Vector targetpos)
		{
			lstItemData.Add(SetItemData(name, targetpos));
			
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

				if (!BlockCheck.BlockCheckBottom(posX, posY,lstItemData[i].width*32 ,lstItemData[i].height * 32, lstItemData[i].weight))
				{
					
					posY += SystemOperator.PixelPerSecond(lstItemData[i].weight);
				
				}

				Canvas.SetTop(lstItemData[i].imgItem, posY);
			}


		}
	}

}
