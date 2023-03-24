using CSCore.SoundIn;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;

namespace xna
{
    internal class AudioInput
    {
        
        public static WaveInEvent waveIn = null;
        private static int SamplesRecorded = 0;

        public static float MicMinPeak = 0.054f;
        public static float MicPeakInterval = 0.001f;
        public static float LastPeak = 0;

        public static void OnDataAvailable(object sender, WaveInEventArgs args)
        {
            float max = 0;
            // interpret as 16 bit audio
            for (int index = 0; index < args.BytesRecorded; index += 2)
            {
                short sample = (short)((args.Buffer[index + 1] << 8) |
                                        args.Buffer[index + 0]);
                // to floating point
                var sample32 = sample / 32768f;
                // absolute value 
                if (sample32 < 0) sample32 = -sample32;
                // is this the max value?
                if (sample32 > max) max = sample32;
            }
            SamplesRecorded++;
            if (SamplesRecorded >= 100)
            {
                RestartRecording();
            }
            LastPeak = max;

            if (SettingsMenu.SettingSwap)
            {
                if (Game1.Ghost != null)
                {
                    Game1.Ghost.IsTalking = max > MicMinPeak;
                }
            }else{
                if (Game1.Cat != null)
                {
                    Game1.Cat.IsTalking = max > MicMinPeak;
                }
            }
        }

        private static void RestartRecording()
        {
            SamplesRecorded = 0;
            waveIn.StopRecording();
            waveIn.Dispose();
            waveIn = null;
            StartRecording();
        }
        public static void StartRecording()
        {
            waveIn = new WaveInEvent();
            waveIn.DataAvailable += OnDataAvailable;

            try
            {
                waveIn.StartRecording();
            }
            catch (Exception)
            {

                //throw;
            }
        }
    }
}
