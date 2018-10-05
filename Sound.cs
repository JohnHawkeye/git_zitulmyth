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
	public class Sound
	{

		public static System.Media.SoundPlayer seDamage = new System.Media.SoundPlayer("player_damage.wav");
		public static System.Media.SoundPlayer seFog = new System.Media.SoundPlayer("fog.wav");
		public static System.Media.SoundPlayer bgmDarkness = new System.Media.SoundPlayer("darkness.wav");

		public static MediaPlayer mp = new MediaPlayer();

		public static int sePlayTime = 0;
		public static bool seStop;


	}
}
