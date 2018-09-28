using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Zitulmyth.Data
{

	public enum EnemyDeathEffect
	{
		None,
		Pop,
	}

	public enum EnemyName
	{
		Zigytu01,
	}

	public class EnemyData
	{
		public static List<SpawnEnemyList> lstSpawnEnemy = new List<SpawnEnemyList>();
	}

	public class SpawnEnemyList
	{
		public EnemyName enemyName;
		public int enemySpeed;
		public int enemyHp;
		public int enemyOfePower;
		public int enemyDefPower;
		public int enemyWeight;
		public Vector enemySize;
		public Vector enemyStartPos;
		public Image imgEnemy;
		public EnemyDeathEffect deathEffect;
	}
}
