using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object
{
    public class MusicObject
    {
        private string _currentSong = "";
        private AudioStreamPlayer _streamPlayer;
        private Dictionary<string, float> _lastPlayedPosition;
        private string _overworldMusic;

        public string OverworldTheme
        {
            get => _overworldMusic;
            set => _overworldMusic = value;
        }

        public MusicObject()
        {
            if(string.IsNullOrEmpty(_overworldMusic))
                _overworldMusic = MusicAssets.OVERWORLD_1;

            _streamPlayer = new AudioStreamPlayer();
            _lastPlayedPosition = new Dictionary<string, float>();
        }

        /// <summary>
        /// Change the current stream player with a new one.
        /// This will stop the current music playing so we can host new music from
        /// the new Stream Player. This will usually be needed when scene hopping.
        /// </summary>
        /// <param name="streamPlayer"></param>
        public void SetStreamPlayer(AudioStreamPlayer streamPlayer)
        {
            // stop the ongoing stream player if it's playing
            try
            {
                if (_streamPlayer.Playing)
                    SavePlaybackPosition();
            }
            catch (System.ObjectDisposedException) 
            { 
                // This bug is possible when loading a new game after quitting from inside the menu.
                // This catch will stop that from happening.
            }
            finally
            {
                _streamPlayer = streamPlayer;
            }
        }

        public void PlayMusic(string music, bool isBoss = false)
        {
            if (music != _currentSong)
            {
                if (_streamPlayer.Playing)
                    SavePlaybackPosition();

                _currentSong = music;
                _streamPlayer.Stream = ResourceLoader.Load<AudioStream>(music);
                
                float seekToPosition = 0;

                // if this song we're about to play is already saved, then resume playing where it left off
                if (_lastPlayedPosition.ContainsKey(_currentSong) && !isBoss)
                    seekToPosition = _lastPlayedPosition[_currentSong];

                _streamPlayer.Play(seekToPosition);
            }
        }

        private void SavePlaybackPosition()
        {
            float playbackPosition = _streamPlayer.GetPlaybackPosition();
            _streamPlayer.Stop();

            // save our playback position to resume later if need be
            if (_lastPlayedPosition.ContainsKey(_currentSong))
                _lastPlayedPosition[_currentSong] = playbackPosition;
            else
                _lastPlayedPosition.Add(_currentSong, playbackPosition);

        }
    }
}
