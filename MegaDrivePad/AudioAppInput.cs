using CSCore.CoreAudioAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xna
{
    internal class AudioAppInput
    {
        private static AudioSessionManager2 GetDefaultAudioSessionManager2(DataFlow dataFlow)
        {
            using (var enumerator = new MMDeviceEnumerator())
            {
                using (var device = enumerator.GetDefaultAudioEndpoint(dataFlow, Role.Console))
                {
                    return AudioSessionManager2.FromMMDevice(device);
                }
            }
        }

        public static AudioSessionControl DiscordSession = null;
        public static int CurrentPID = 0;
        public static bool Talking = false;

        public static void CheckAudioLevels()
        {
            Talking = false;

            if (DiscordSession == null)
            {
                using (AudioSessionManager2 sessionManager = GetDefaultAudioSessionManager2(DataFlow.Render))
                {
                    using (var sessionEnumerator = sessionManager.GetSessionEnumerator())
                    {
                        foreach (AudioSessionControl session in sessionEnumerator)
                        {
                            using (AudioSessionControl2 audioSessionControl2 = session.QueryInterface<AudioSessionControl2>())
                            {
                                Process process = audioSessionControl2.Process;
                                string name = audioSessionControl2.DisplayName;
                                int PID = 0;
                                if (process != null)
                                {
                                    if (string.IsNullOrEmpty(name))
                                    {
                                        if (!string.IsNullOrEmpty(process.MainWindowTitle))
                                        {
                                            name = process.MainWindowTitle;
                                        }
                                        else if (!string.IsNullOrEmpty(process.ProcessName))
                                        {
                                            name = process.ProcessName;
                                        }
                                        else
                                        {
                                            name = "--unnamed--";
                                        }
                                    }
                                    PID = process.Id;
                                }

                                if (name == "Discord")
                                {
                                    if (process.Id != SettingsMenu.IgnoreDiscordPid)
                                    {
                                        DiscordSession = session;
                                        CurrentPID = PID;
                                    }
                                }
                            }
                        }
                    }
                }
            }else{
                using (AudioMeterInformation audioMeterInformation = DiscordSession.QueryInterface<AudioMeterInformation>())
                {
                    var value = audioMeterInformation.GetPeakValue();
                    if (value != 0)
                    {
                        Talking = true;
                    }
                }
            }

            if (SettingsMenu.SettingSwap)
            {
                if (Game1.Cat != null)
                {
                    Game1.Cat.IsTalking = Talking;
                }
            }else{
                if (Game1.Ghost != null)
                {
                    Game1.Ghost.IsTalking = Talking;
                }
            }
        }
    }
}
