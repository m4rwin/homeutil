using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;

namespace MUtility
{
    public class Sound
    {
        public static void MakeSound(UnmanagedMemoryStream path)
        {
            try
            {
                SoundPlayer player = new SoundPlayer();
                player.Stream = path;
                player.Play();
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}
