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

	public enum ObjectName
	{
		Npc_Opsa,
		Npc_Yeeda,
		Obj_Chair,
		Obj_Table,
		Obj_Huton,
		Obj_CampFire,
	}

	public class ObjectData
	{
		public ObjectName objName;
		public Image imgObject;
		public CroppedBitmap cbSource;
		public Vector position;
		public int width;
		public int height;
		public int zindex;
		public int expirationTime;
		public int totalTime;
	}

	 public class Object
	{
		public static List<ObjectData> lstObject = new List<ObjectData>();



	}

	public class ObjectBehavior
	{

	}
}
