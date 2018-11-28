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

	public enum BgmName
	{
		None = 0,
		Opening = 1,
		Darkness = 2,
	}

	public enum SeName
	{
		None = 0,
		Player_Damage = 1,
		Fog = 2,
		Item_Get = 3,
		Shock = 4,

	}

	public class Sound
	{

		public static MediaElement bgm = new MediaElement();
		public static MediaElement seListenerPlayer = new MediaElement();
		public static MediaElement seListenerEnemy = new MediaElement();
		public static MediaElement seListenerObject = new MediaElement();
		
		public static string dirpath = Directory.GetCurrentDirectory();

		public static int sePlayTime = 0;
		public static bool seStop;

		public static void SoundEffectLoad(Canvas canvas)
		{

			canvas.Children.Add(bgm);
			bgm.LoadedBehavior = MediaState.Manual;

			SoundBgmSelector(BgmName.Opening);



			canvas.Children.Add(seListenerPlayer);
			seListenerPlayer.LoadedBehavior = MediaState.Manual;

			canvas.Children.Add(seListenerEnemy);
			seListenerEnemy.LoadedBehavior = MediaState.Manual;

			canvas.Children.Add(seListenerObject);
			seListenerObject.LoadedBehavior = MediaState.Manual;

		}

		public static void SoundBgmSelector(BgmName name)
		{
			switch (name)
			{
				case BgmName.None:
					bgm.Source = new Uri(dirpath + "", UriKind.Absolute);
					break;

				case BgmName.Opening:
					bgm.Source = new Uri(dirpath + "\\Assets\\opening.mp3", UriKind.Absolute);
					break;

				case BgmName.Darkness:
					bgm.Source = new Uri(dirpath + "\\Assets\\darkness.wav", UriKind.Absolute);
					break;
			}
		}

		public static void SoundEffectSelector(SeName name)
		{

			switch (name)
			{
				case SeName.None:
					seListenerPlayer.Source = new Uri(dirpath + "", UriKind.Absolute);
					break;

				case SeName.Player_Damage:
					seListenerPlayer.Source = new Uri(dirpath + "\\Assets\\player_damage.wav", UriKind.Absolute);
					break;

				case SeName.Fog:
					seListenerEnemy.Source = new Uri(dirpath + "\\Assets\\fog.wav", UriKind.Absolute);
					break;

				case SeName.Item_Get:
					seListenerObject.Source = new Uri(dirpath + "\\Assets\\itemget.wav", UriKind.Absolute);
					break;

				case SeName.Shock:
					seListenerEnemy.Source = new Uri(dirpath + "\\Assets\\shock.wav", UriKind.Absolute);
					break;
			}

		}

		public static void SoundEffectPlayer(SeName name)
		{
			switch (name)
			{
				case SeName.Player_Damage:
					seListenerPlayer.Play();
					break;

				case SeName.Fog:
					seListenerEnemy.Play();
					break;

				case SeName.Item_Get:
					seListenerObject.Play();
					break;

				case SeName.Shock:
					seListenerEnemy.Play();
					break;
			}
		}
	}
}
