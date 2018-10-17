using System;
using System.IO;
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

		public static MediaElement bgm = new MediaElement();
		public static MediaElement seChannelA = new MediaElement();
		public static MediaElement seChannelB = new MediaElement();

		

		public static int sePlayTime = 0;
		public static bool seStop;

		public static void SoundEffectLoad(Canvas canvas)
		{

			string dirpath = Directory.GetCurrentDirectory();

			canvas.Children.Add(seChannelA);
			seChannelA.Source = new Uri(dirpath + "\\itemget.wav", UriKind.Absolute);
			seChannelA.LoadedBehavior = MediaState.Manual;

			canvas.Children.Add(seChannelB);
			seChannelB.Source = new Uri(dirpath + "\\shock.wav", UriKind.Absolute);
			seChannelB.LoadedBehavior = MediaState.Manual;

			canvas.Children.Add(bgm);
			bgm.Source = new Uri(dirpath + "\\opening.mp3", UriKind.Absolute);
			bgm.LoadedBehavior = MediaState.Manual;
			bgm.Play();
		}


	}
}
