using PianoTrainingMidi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PianoTrainingMidi.Models
{
    public class Game : BaseObject
    {
        #region property ChordToPlay
        private Chord _chordToPlay;
        public Chord ChordToPlay
        {
            get { return _chordToPlay; }
            set { SetProperty(ref _chordToPlay, value); }
        }
        #endregion

        public bool _chordToPlayRescheduled = false;

        #region property NumberPlayed
        private int _numberPlayed;
        public int NumberPlayed
        {
            get { return _numberPlayed; }
            set { SetProperty(ref _numberPlayed, value); }
        }
        #endregion

        #region property NumberCorrect
        private int _numberCorrect;
        public int NumberCorrect
        {
            get { return _numberCorrect; }
            set { SetProperty(ref _numberCorrect, value); }
        }
        #endregion

        #region property NumberWrong
        private int _numberWrong;
        public int NumberWrong
        {
            get { return _numberWrong; }
            set { SetProperty(ref _numberWrong, value); }
        }
        #endregion

        #region property Playlist
        private List<Chord> _playlist;
        public List<Chord> Playlist
        {
            get { return _playlist; }
            set { SetProperty(ref _playlist, value); }
        }
        #endregion

        private List<Chord> _allChords;
        private Random _random = new Random();

        public Game(List<Chord> allChords, List<Chord> playlist)
        {
            _allChords = allChords;
            Playlist = new List<Chord>(playlist);
        }

        public bool SelectNextChordToPlay()
        {
            _chordToPlayRescheduled = false;
            if (Playlist == null || !Playlist.Any())
            {
                ChordToPlay = null;
                return false;
            }

            int index = _random.Next(0, Playlist.Count);
            ChordToPlay = Playlist[index].GetCopy();
            Playlist.RemoveAt(index);
            return true;
        }

        public (bool Correct, Chord Played) CheckChordPlayed(List<Note> notes)
        {
            if (notes == null || !notes.Any())
            {
                return (false, null);
            }

            string playedString = string.Join("-", notes.OrderBy(x => x.MidiNote).Select(x => x.NoteLetter));
            Chord played = _allChords.FirstOrDefault(x => x.ChordString == playedString);
            bool result = played != null && played.ChordString == ChordToPlay.ChordString;

            if (played != null)
            {
                if (result)
                {
                    NumberCorrect++;
                }
                else
                {
                    NumberWrong++;
                    if (!_chordToPlayRescheduled)
                    {
                        // played it wrong. Try again in playlist ... but only add once.
                        Playlist.Add(ChordToPlay);
                        _chordToPlayRescheduled = true;
                    }
                }
            }

            return (result, played);
        }
    }
}
