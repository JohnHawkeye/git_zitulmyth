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
	public enum AnimationType
	{
		Idle,
		Spawn,
		Move,
		Attack,
		Damage,
		Death,
	}

	public class CBEnemyData
	{
		public EnemyName name;

		public List<CroppedBitmap> idleL = new List<CroppedBitmap>();
		public List<CroppedBitmap> spawnL = new List<CroppedBitmap>();
		public List<CroppedBitmap> moveL = new List<CroppedBitmap>();
		public List<CroppedBitmap> attackL = new List<CroppedBitmap>();
		public List<CroppedBitmap> damageL = new List<CroppedBitmap>();
		public List<CroppedBitmap> deathL = new List<CroppedBitmap>();

		public List<CroppedBitmap> idleR = new List<CroppedBitmap>();
		public List<CroppedBitmap> spawnR = new List<CroppedBitmap>();
		public List<CroppedBitmap> moveR = new List<CroppedBitmap>();
		public List<CroppedBitmap> attackR = new List<CroppedBitmap>();
		public List<CroppedBitmap> damageR = new List<CroppedBitmap>();
		public List<CroppedBitmap> deathR = new List<CroppedBitmap>();

	}

	public class Animator
	{
		private static int keyFlameTime = 800;

		private static CBEnemyData cbDataZigitu = new CBEnemyData();
		private static CBEnemyData cbDataBoar = new CBEnemyData();

		private static List<CroppedBitmap> cbDataCoin = new List<CroppedBitmap>();
		private static List<CroppedBitmap> cbDataBoarMeat = new List<CroppedBitmap>();

		private static List<CroppedBitmap> cbDataFireCamp = new List<CroppedBitmap>();

		public static void CBDataSettings()
		{

			cbDataZigitu.idleL = AnimationSourceSelector(CategoryName.Enemy, "ZigituIdle");
			cbDataZigitu.spawnL = AnimationSourceSelector(CategoryName.Enemy, "ZigituSpawn");
			cbDataZigitu.moveL = AnimationSourceSelector(CategoryName.Enemy, "ZigituIdle");
			cbDataZigitu.attackL = AnimationSourceSelector(CategoryName.Enemy, "ZigituIdle");
			cbDataZigitu.damageL = AnimationSourceSelector(CategoryName.Enemy, "ZigituSpawn");
			cbDataZigitu.deathL = AnimationSourceSelector(CategoryName.Enemy, "ZigituSpawn");
			cbDataZigitu.idleR = AnimationSourceSelector(CategoryName.Enemy, "ZigituIdle");
			cbDataZigitu.spawnR = AnimationSourceSelector(CategoryName.Enemy, "ZigituSpawn");
			cbDataZigitu.moveR = AnimationSourceSelector(CategoryName.Enemy, "ZigituIdle");
			cbDataZigitu.attackR = AnimationSourceSelector(CategoryName.Enemy, "ZigituIdle");
			cbDataZigitu.damageR = AnimationSourceSelector(CategoryName.Enemy, "ZigituSpawn");
			cbDataZigitu.deathR = AnimationSourceSelector(CategoryName.Enemy, "ZigituSpawn");

			cbDataBoar.idleL = AnimationSourceSelector(CategoryName.Enemy, "BoarIdleL");
			cbDataBoar.spawnL = AnimationSourceSelector(CategoryName.Enemy, "BoarIdleL");
			cbDataBoar.moveL = AnimationSourceSelector(CategoryName.Enemy, "BoarIdleL");
			cbDataBoar.attackL = AnimationSourceSelector(CategoryName.Enemy, "BoarDashL");
			cbDataBoar.damageL = AnimationSourceSelector(CategoryName.Enemy, "BoarDamageL");
			cbDataBoar.deathL = AnimationSourceSelector(CategoryName.Enemy, "BoarDeathL");
			cbDataBoar.idleR = AnimationSourceSelector(CategoryName.Enemy, "BoarIdleR");
			cbDataBoar.spawnR = AnimationSourceSelector(CategoryName.Enemy, "BoarIdleR");
			cbDataBoar.moveR = AnimationSourceSelector(CategoryName.Enemy, "BoarIdleR");
			cbDataBoar.attackR = AnimationSourceSelector(CategoryName.Enemy, "BoarDashR");
			cbDataBoar.damageR = AnimationSourceSelector(CategoryName.Enemy, "BoarDamageR");
			cbDataBoar.deathR = AnimationSourceSelector(CategoryName.Enemy, "BoarDeathR");

			cbDataCoin = AnimationSourceSelector(CategoryName.Item, "Coin");
			cbDataBoarMeat = AnimationSourceSelector(CategoryName.Item, "BoarMeat");

			cbDataFireCamp = AnimationSourceSelector(CategoryName.Object, "FireCamp");
		}

		public static void AnimationEnemy()
		{
			for (int i = 0; i < SpawnEnemy.lstEnemyData.Count; i++)
			{
				int keyFlameNum = 0;
				CBEnemyData animationCells = new CBEnemyData();

				switch (SpawnEnemy.lstEnemyData[i].name)
				{
					case EnemyName.Zigitu01:
						animationCells = cbDataZigitu;
						break;

					case EnemyName.Boar:
						animationCells = cbDataBoar;
						break;
				}

				if (SpawnEnemy.lstEnemyData[i].totalAnimTime < keyFlameTime)
				{

					SpawnEnemy.lstEnemyData[i].totalAnimTime += MainWindow.elapsedTime;

				}
				else
				{

					CroppedBitmap cbAnimEnemyCell = new CroppedBitmap();

					if (SpawnEnemy.lstEnemyData[i].state == EnemyState.Death)
					{
						if (!SpawnEnemy.lstEnemyData[i].direction)
						{
							SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.deathL.Count);
							cbAnimEnemyCell = animationCells.deathL[SpawnEnemy.lstEnemyData[i].keyFlame];
							keyFlameNum = animationCells.deathL.Count;
						}
						else
						{
							SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.deathR.Count);
							cbAnimEnemyCell = animationCells.deathR[SpawnEnemy.lstEnemyData[i].keyFlame];
							keyFlameNum = animationCells.deathR.Count;
						}
					}
					else
					{
						if (!SpawnEnemy.lstEnemyData[i].direction)
						{
							SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.idleL.Count);
							cbAnimEnemyCell = animationCells.idleL[SpawnEnemy.lstEnemyData[i].keyFlame];
							keyFlameNum = animationCells.idleL.Count;
						}
						else
						{
							SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.idleR.Count);
							cbAnimEnemyCell = animationCells.idleR[SpawnEnemy.lstEnemyData[i].keyFlame];
							keyFlameNum = animationCells.idleR.Count;
						}


						if (SpawnEnemy.lstEnemyData[i].isDamage)
						{
							if (!SpawnEnemy.lstEnemyData[i].direction)
							{
								SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.damageL.Count);
								cbAnimEnemyCell = animationCells.damageL[SpawnEnemy.lstEnemyData[i].keyFlame];
								keyFlameNum = animationCells.damageL.Count;
							}
							else
							{
								SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.damageR.Count);
								cbAnimEnemyCell = animationCells.damageR[SpawnEnemy.lstEnemyData[i].keyFlame];
								keyFlameNum = animationCells.damageR.Count;
							}

						}
						else
						{
							if (SpawnEnemy.lstEnemyData[i].perceive)
							{
								if (!SpawnEnemy.lstEnemyData[i].direction)
								{
									SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.damageL.Count);
									cbAnimEnemyCell = animationCells.damageL[SpawnEnemy.lstEnemyData[i].keyFlame];
									keyFlameNum = animationCells.damageL.Count;
								}
								else
								{
									SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.damageR.Count);
									cbAnimEnemyCell = animationCells.damageR[SpawnEnemy.lstEnemyData[i].keyFlame];
									keyFlameNum = animationCells.damageR.Count;
								}


							}

							if (SpawnEnemy.lstEnemyData[i].state == EnemyState.Active)
							{
								if (!SpawnEnemy.lstEnemyData[i].direction)
								{
									SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.attackL.Count);
									cbAnimEnemyCell = animationCells.attackL[SpawnEnemy.lstEnemyData[i].keyFlame];
									keyFlameNum = animationCells.attackL.Count;
								}
								else
								{
									SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.attackR.Count);
									cbAnimEnemyCell = animationCells.attackR[SpawnEnemy.lstEnemyData[i].keyFlame];
									keyFlameNum = animationCells.attackR.Count;
								}


							}
						}

					}

					SpawnEnemy.lstEnemyData[i].imgEnemy.Source = cbAnimEnemyCell;

					if (SpawnEnemy.lstEnemyData[i].keyFlame < keyFlameNum - 1)
					{
						SpawnEnemy.lstEnemyData[i].keyFlame++;
					}
					else
					{
						SpawnEnemy.lstEnemyData[i].keyFlame = 0;
					}

					SpawnEnemy.lstEnemyData[i].totalAnimTime = 0;

				}


			}
		}

		public static void AnimationItem()
		{
			for (int i = 0; i < Item.lstItemData.Count; i++)
			{
				int keyFlameNum = 0;
				List<CroppedBitmap> animationCells = new List<CroppedBitmap>();
				bool noAnim = false;

				switch (Item.lstItemData[i].itemName)
				{
					case ItemName.Coin:
						animationCells = cbDataCoin;
						break;

					case ItemName.BoarMeat:
						animationCells = cbDataBoarMeat;
						break;

					default:
						noAnim = true;
						break;
				}

				if (!noAnim)
				{

					if (Item.lstItemData[i].totalAnimTime < keyFlameTime)
					{

						Item.lstItemData[i].totalAnimTime += MainWindow.elapsedTime;

					}
					else
					{

						CroppedBitmap cbAnimCell = new CroppedBitmap();

						Item.lstItemData[i].keyFlame = CellNumCheck(Item.lstItemData[i].keyFlame, animationCells.Count);
						cbAnimCell = animationCells[Item.lstItemData[i].keyFlame];
						keyFlameNum = animationCells.Count;

						Item.lstItemData[i].imgItem.Source = cbAnimCell;

						if (Item.lstItemData[i].keyFlame < keyFlameNum - 1)
						{
							Item.lstItemData[i].keyFlame++;
						}
						else
						{
							Item.lstItemData[i].keyFlame = 0;
						}

						Item.lstItemData[i].totalAnimTime = 0;

					}
				}

			}


		}


		public static void AnimationObject()
		{
			for (int i = 0; i < ObjectChecker.lstObject.Count; i++)
			{
				int keyFlameNum = 0;
				List<CroppedBitmap> animationCells = new List<CroppedBitmap>();
				bool noAnim = false;

				switch (ObjectChecker.lstObject[i].objName)
				{
					case ObjectName.Obj_CampFire:
						animationCells = cbDataFireCamp;
						break;

					default:
						noAnim = true;
						break;
				}

				if (!noAnim)
				{

					if (ObjectChecker.lstObject[i].totalAnimTime < keyFlameTime)
					{

						ObjectChecker.lstObject[i].totalAnimTime += MainWindow.elapsedTime;

					}
					else
					{

						CroppedBitmap cbAnimCell = new CroppedBitmap();

						ObjectChecker.lstObject[i].keyFlame = CellNumCheck(ObjectChecker.lstObject[i].keyFlame, animationCells.Count);
						cbAnimCell = animationCells[ObjectChecker.lstObject[i].keyFlame];
						keyFlameNum = animationCells.Count;

						ObjectChecker.lstObject[i].imgObject.Source = cbAnimCell;

						if (ObjectChecker.lstObject[i].keyFlame < keyFlameNum - 1)
						{
							ObjectChecker.lstObject[i].keyFlame++;
						}
						else
						{
							ObjectChecker.lstObject[i].keyFlame = 0;
						}

						ObjectChecker.lstObject[i].totalAnimTime = 0;

					}
				}



			}
		}

		private static int CellNumCheck(int keyflame, int cellnum)
		{
			if (keyflame > cellnum - 1)
			{
				return 0;
			}
			else
			{
				return keyflame;
			}

		}

		public static List<CroppedBitmap> AnimationSourceSelector(CategoryName category, string target)
		{
			switch (category)
			{
				case CategoryName.Player:
					return AnimatiionPatternSelector(ImageData.spritePlayer, target);

				case CategoryName.Enemy:
					return AnimatiionPatternSelector(ImageData.spriteEnemy, target);

				case CategoryName.Object:
					return AnimatiionPatternSelector(ImageData.spriteObject, target);

				case CategoryName.Npc:
					return AnimatiionPatternSelector(ImageData.spriteNpc, target);

				case CategoryName.Item:
					return AnimatiionPatternSelector(ImageData.spriteItem, target);

				case CategoryName.Block:
					return AnimatiionPatternSelector(ImageData.spriteBlock, target);

				case CategoryName.Scenery:
					return AnimatiionPatternSelector(ImageData.spriteScenery, target);

				case CategoryName.System:
					return AnimatiionPatternSelector(ImageData.spriteSystem, target);

				default:
					return AnimatiionPatternSelector(ImageData.spritePlayer, target);
			}
		}
		public static List<CroppedBitmap> AnimatiionPatternSelector(List<SpriteSource> sprite, string target)
		{

			for (int i = 0; i < sprite.Count; i++)
			{
				if (sprite[i].patternSource.patternName == target)
				{
					return sprite[i].patternSource.croppedBitmap;
				}
			}

			List<CroppedBitmap> lstNoImages = new List<CroppedBitmap>();
			lstNoImages.Add(new CroppedBitmap(ImageData.bmiNoImage, new Int32Rect(0, 0, 32, 32)));

			return lstNoImages;
		}
	}

}
