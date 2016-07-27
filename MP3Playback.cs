using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio;
using NAudio.Wave;


namespace lcmp
{
    class MP3Playback
    {
        public IWavePlayer waveOutDevice;
        public AudioFileReader audioFileReader;
        public MP3Playback()
        {
        }

        public void Load(string location)
        {
            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader(location);
            waveOutDevice.Init(audioFileReader);
        }

        public void Play()
        {
            try
            {
                waveOutDevice.Play();
            }
            catch
            {

            }
        }
        public void Stop()
        {
            try
            {
                waveOutDevice.Stop();
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            try
            {
                waveOutDevice.Dispose();
                audioFileReader.Dispose();
            }
            catch
            {

            }
        }
    }
}
