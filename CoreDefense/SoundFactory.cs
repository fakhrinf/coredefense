using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace CoreDefense
{
    public class SoundFactory
    {
        SoundEffect btnHover, btnClick;
        SoundEffect powerUp, explode;        

        SoundPlayer mainBGMSP, playBGMSP;
        
        public bool isMainBGMSPplay = false;
        public bool isPlayBGMSPplay = false;
        
        private static SoundFactory Instance;
        public static SoundFactory Init
        {
            get
            {
                if (Instance == null)
                    Instance = new SoundFactory();
                return Instance;
            }
        }

        public bool isPlay = false;
        public bool isAlreadyPlay = false;

        public bool SoundFXOn = true;
        public bool BGMOn = true;

        public int count = 0;
        string soundDirectory;

        public string SoundDir(string fileName) 
        {
            soundDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Content\\Sound\\" + fileName;

            return soundDirectory;
        }

        //code untuk ngecilin volume .wav
        //tapi malah jadi kecil semuanya        
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        uint currVol;

        public void LoadContent(ContentManager content) 
        {
            btnClick = content.Load<SoundEffect>("Sound\\button-48");
            btnHover = content.Load<SoundEffect>("Sound\\button-50");

            powerUp = content.Load<SoundEffect>("Sound\\NFF-event");
            explode = content.Load<SoundEffect>("Sound\\NFF-fireball-02");
            
            waveOutGetVolume(IntPtr.Zero, out currVol);
            waveOutSetVolume(IntPtr.Zero, ushort.MaxValue / 2); 
        }

        public void btnHoverPlay() 
        {
            if (SoundFXOn)
                btnHover.Play();                        
        }

        public void btnHoverStop() 
        {
            isPlay = false;
            isAlreadyPlay = false;
        }

        public void btnClickPlay() 
        {
            if (SoundFXOn)
                btnClick.Play();
        }

        public void powerUpPlay() 
        {
            if (SoundFXOn)
                powerUp.Play();
        }

        public void explodePlay()
        {
            if (SoundFXOn)
                explode.Play();
        }

        public void playBGM(int collection)
        {
            switch (collection)
            {
                case 0:
                    mainBGMSP = new SoundPlayer(SoundDir("POL-hot-pursuit-short.wav"));
                    if (BGMOn)
                    {
                        if (!isMainBGMSPplay)
                            mainBGMSP.PlayLooping();
                        isMainBGMSPplay = true;
                    }
                    else
                        stopBGM();
                    break;
                case 1:
                    mainBGMSP = new SoundPlayer(SoundDir("POL-rocket-station-short.wav"));
                    if (BGMOn)
                    {
                        if (!isMainBGMSPplay)
                            mainBGMSP.PlayLooping();
                        isMainBGMSPplay = true;
                    }
                    else
                        stopBGM();
                    break;
                default:
                    break;
            }                                    
        }

        public void stopBGM()
        {            
            mainBGMSP.Stop();
            isMainBGMSPplay = false;                
        }
    }
}
