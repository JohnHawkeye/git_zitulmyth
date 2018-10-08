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
	public class KeyController
	{
		//input
		public static bool keyLeft = false;
		public static bool keyRight = false;
		public static bool keyUp = false;
		public static bool keyDown = false;
		public static bool keySpace = false;
		public static bool keyA = false;
		public static bool keyS = false;
		public static bool keyD = false;
		public static bool keyControlLocking = true;
		public static bool keyReturn = false;

		//input key
		public static void InputKeyDown(object sender, KeyEventArgs e)
		{
			if (!keyControlLocking)
			{
				if (e.Key == Key.Left)
				{
					keyLeft = true;
				}

				if (e.Key == Key.Right)
				{
					keyRight = true;
				}

				if (e.Key == Key.Up)
				{
					keyUp = true;
				}

				if (e.Key == Key.Down)
				{
					keyDown = true;
				}

				if (e.Key == Key.Space)
				{
					keySpace = true;
				}

				if (e.Key == Key.A)
				{
					keyA = true;
				}

				if (e.Key == Key.S)
				{
					keyS = true;
				}

				if (e.Key == Key.D)
				{
					keyD = true;
				}

			}

			if (e.Key == Key.Return)
			{
				keyReturn = true;
			}

		}

		public static void InputKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Left)
			{
				keyLeft = false;
			}
			if (e.Key == Key.Right)
			{
				keyRight = false;
			}
			if (e.Key == Key.Up)
			{
				keyUp = false;
			}
			if (e.Key == Key.Down)
			{
				keyDown = false;
			}

			if (e.Key == Key.Space)
			{
				keySpace = false;
			}

			if (e.Key == Key.A)
			{
				keyA = false;
			}

			if (e.Key == Key.S)
			{
				keyS = false;
			}

			if (e.Key == Key.D)
			{
				keyD = false;
			}

			if (e.Key == Key.Return)
			{
				keyReturn = false;
			}
		}
	}
}
