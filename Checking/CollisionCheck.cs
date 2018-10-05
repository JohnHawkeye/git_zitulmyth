using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Zitulmyth.Checking
{

	public class CollisionCheck
	{
		public static bool Collision(Vector p1,Vector p2,Vector size1,Vector size2)
		{
			if (p1.Y < p2.Y + size2.Y && p1.Y + size1.Y > p2.Y &&
				p1.X+size1.X > p2.X && p1.X < p2.X + size2.X)
			{
				return true;
			}
			else
			{
				return false;
			}
			
		}

		public static bool CollisionBottom(Vector p1, Vector p2, Vector size1, Vector size2)
		{
			if (p1.Y+size1.Y > p2.Y && (p1.X + size1.X > p2.X && p1.X < p2.X + size2.X))
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		public static bool CollisionLeft(Vector p1, Vector p2, Vector size1, Vector size2)
		{
			if (p1.X<p2.X+size2.X && (p1.Y+size1.Y>p2.Y && p1.Y< p2.Y+size2.Y))
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		public static bool CollisionRight(Vector p1, Vector p2, Vector size1, Vector size2)
		{
			if (p1.X + size1.X > p2.X && (p1.Y + size1.Y > p2.Y && p1.Y < p2.Y + size2.Y))
			{
				return true;
			}
			else
			{
				return false;
			}

		}

	}
}
