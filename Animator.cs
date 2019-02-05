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
	
	public class Animator
	{
		private static int keyFlameTime = 800;

		public static void AnimationEnemy()
		{
			for (int i = 0; i < SpawnEnemy.lstEnemyData.Count; i++)
			{
				int keyFlameNum = 0;
				List<CroppedBitmap> animationCells = new List<CroppedBitmap>();

				

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
							animationCells = AnimationSourceSelector(CategoryName.Enemy, SpawnEnemy.lstEnemyData[i].spriteDeathL);
							SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.Count);
							cbAnimEnemyCell = animationCells[SpawnEnemy.lstEnemyData[i].keyFlame];
							keyFlameNum = animationCells.Count;
						}
						else
						{
							animationCells = AnimationSourceSelector(CategoryName.Enemy, SpawnEnemy.lstEnemyData[i].spriteDeathR);
							SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.Count);
							cbAnimEnemyCell = animationCells[SpawnEnemy.lstEnemyData[i].keyFlame];
							keyFlameNum = animationCells.Count;
						}
					}
					else
					{
						if (!SpawnEnemy.lstEnemyData[i].direction)
						{
							animationCells = AnimationSourceSelector(CategoryName.Enemy, SpawnEnemy.lstEnemyData[i].spriteIdleL);
							SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.Count);
							cbAnimEnemyCell = animationCells[SpawnEnemy.lstEnemyData[i].keyFlame];
							keyFlameNum = animationCells.Count;
						}
						else
						{
							animationCells = AnimationSourceSelector(CategoryName.Enemy, SpawnEnemy.lstEnemyData[i].spriteIdleR);
							SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.Count);
							cbAnimEnemyCell = animationCells[SpawnEnemy.lstEnemyData[i].keyFlame];
							keyFlameNum = animationCells.Count;
						}


						if (SpawnEnemy.lstEnemyData[i].isDamage)
						{
							if (!SpawnEnemy.lstEnemyData[i].direction)
							{
								animationCells = AnimationSourceSelector(CategoryName.Enemy, SpawnEnemy.lstEnemyData[i].spriteDamageL);
								SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.Count);
								cbAnimEnemyCell = animationCells[SpawnEnemy.lstEnemyData[i].keyFlame];
								keyFlameNum = animationCells.Count;
							}
							else
							{
								animationCells = AnimationSourceSelector(CategoryName.Enemy, SpawnEnemy.lstEnemyData[i].spriteDamageR);
								SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.Count);
								cbAnimEnemyCell = animationCells[SpawnEnemy.lstEnemyData[i].keyFlame];
								keyFlameNum = animationCells.Count;
							}

						}
						else
						{
							if (SpawnEnemy.lstEnemyData[i].perceive)
							{
								if (!SpawnEnemy.lstEnemyData[i].direction)
								{
									animationCells = AnimationSourceSelector(CategoryName.Enemy, SpawnEnemy.lstEnemyData[i].spriteIdleL);
									SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.Count);
									cbAnimEnemyCell = animationCells[SpawnEnemy.lstEnemyData[i].keyFlame];
									keyFlameNum = animationCells.Count;
								}
								else
								{
									animationCells = AnimationSourceSelector(CategoryName.Enemy, SpawnEnemy.lstEnemyData[i].spriteIdleR);
									SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.Count);
									cbAnimEnemyCell = animationCells[SpawnEnemy.lstEnemyData[i].keyFlame];
									keyFlameNum = animationCells.Count;
								}


							}

							if (SpawnEnemy.lstEnemyData[i].state == EnemyState.Active)
							{
								if (!SpawnEnemy.lstEnemyData[i].direction)
								{
									animationCells = AnimationSourceSelector(CategoryName.Enemy, SpawnEnemy.lstEnemyData[i].spriteAttackL);
									SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.Count);
									cbAnimEnemyCell = animationCells[SpawnEnemy.lstEnemyData[i].keyFlame];
									keyFlameNum = animationCells.Count;
								}
								else
								{
									animationCells = AnimationSourceSelector(CategoryName.Enemy, SpawnEnemy.lstEnemyData[i].spriteAttackR);
									SpawnEnemy.lstEnemyData[i].keyFlame = CellNumCheck(SpawnEnemy.lstEnemyData[i].keyFlame, animationCells.Count);
									cbAnimEnemyCell = animationCells[SpawnEnemy.lstEnemyData[i].keyFlame];
									keyFlameNum = animationCells.Count;
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

				animationCells = AnimationSourceSelector(CategoryName.Item, Item.lstItemData[i].sprite);

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

				if (!ObjectChecker.lstObject[i].toggleSwitch)
				{
					animationCells = AnimationSourceSelector(CategoryName.Object, ObjectChecker.lstObject[i].spriteNameA);
				}
				else
				{
					animationCells = AnimationSourceSelector(CategoryName.Object, ObjectChecker.lstObject[i].spriteNameB);
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
