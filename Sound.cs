using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Media;

namespace EnglishSquares
{
	/// <summary>
	/// Summary description for Sound.
	/// </summary>
	public class SoundFX
	{
		//[DllImport("winmm.dll")]
		//private static extern bool sndPlaySound(string strSound, string hFile, UInt32 fdwSound);

		[DllImport("winmm.dll")]
		protected static extern int mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback );

		[DllImport("kernel32.dll", SetLastError=true, CharSet=CharSet.Auto)]
		public static extern int GetShortPathName(
			[MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath,
			[MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszShortPath,
			[MarshalAs(UnmanagedType.U4)] int cchBuffer);

		private const Int32 SND_SYNC = 0;
		private const Int32 SND_ASYNC = 1;
		private const Int32 SND_FILENAME = 131072;

		public SoundFX()
		{
		}

		private static void playMP3(string strSound)
		{
			// get short path to avoid bug on mcisendstring api
			string longpath = AppDomain.CurrentDomain.BaseDirectory;
			StringBuilder sb = new StringBuilder(300);
			int ok = GetShortPathName(longpath, sb, sb.Capacity);
			string shortpath = sb.ToString();

			strSound = shortpath + "sounds\\" + strSound;
			mciSendString("Open " + strSound, null, 0, IntPtr.Zero);
			mciSendString("Play " + strSound, null, 0, IntPtr.Zero);
		}

		private static void stopMP3(string strSound)
		{
			mciSendString("Stop all", null, 0, IntPtr.Zero);
			mciSendString("Close all", null, 0, IntPtr.Zero);
		}

		private static void playSound(string strSound)
		{
			strSound = AppDomain.CurrentDomain.BaseDirectory + "sounds\\" + strSound;

            // updated to not use API
            using (SoundPlayer player = new SoundPlayer(strSound))
            {
                player.Play();
            }

            //sndPlaySound(strSound, null, SND_ASYNC | SND_FILENAME);			
		}

		public static void playArtillery()
		{
			playSound("artillery.wav");
		}

		public static void playMuskets()
		{
			playSound("muskets.wav");
		}

		public static void playCavalry()
		{
			playSound("cavalry.wav");
		}

		public static void playCavalryMarch()
		{
			playSound("HornMarch.wav");
		}

		public static void playInfantryMarch()
		{
			playSound("DrumMarch.wav");
		}

		public static void playPleaseRegister()
		{
			playSound("cavalry.wav");
		}

		public static void playMusic()
		{
			playMP3("title.mp3");
		}

		public static void stopMusic()
		{
			stopMP3("title.mp3");
		}

		public static void playBackgroundMusic()
		{
			playMP3("background.mp3");
		}

		public static void playDefeatMusic()
		{
			playMP3("title.mp3");
		}

		public static void playWinMusic()
		{
			playMP3("title.mp3");
		}

		public static void playDing()
		{
			playSound("muskets.wav");
		}
	}
}



