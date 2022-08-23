using PianoTrainingMidi.Helpers;
using PianoTrainingMidi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace PianoTrainingMidi.ViewModels
{
    public class MainViewModel : BaseObject
    {
        #region property Chords
        private Chords _chords = new Chords();
        public Chords Chords
        {
            get { return _chords; }
            set { SetProperty(ref _chords, value); }
        }
        #endregion

        #region property ChordsSelected
        private List<Chord> _chordsSelected;
        public List<Chord> ChordsSelected
        {
            get { return _chordsSelected; }
            set { SetProperty(ref _chordsSelected, value); }
        }
        #endregion

        #region property NotesPlayed
        private ObservableCollection<Note> _notesPlayed = new ObservableCollection<Note>();
        public ObservableCollection<Note> NotesPlayed
        {
            get { return _notesPlayed; }
            set { SetProperty(ref _notesPlayed, value); }
        }
        #endregion

        #region property ChordToPlayString
        private string _chordToPlayString = "";
        public string ChordToPlayString
        {
            get { return _chordToPlayString; }
            set { SetProperty(ref _chordToPlayString, value); }
        }
        #endregion

        #region property ChordPlayed
        private Chord _chordPlayed;
        public Chord ChordPlayed
        {
            get { return _chordPlayed; }
            set { SetProperty(ref _chordPlayed, value); OnPropertyChanged(nameof(ChordPlayedString)); }
        }
        #endregion

        #region property ChordPlayedString
        public string ChordPlayedString
        {
            get 
            { 
                if (!IsGameActive)
                {
                    return "";
                }
                return ChordPlayed == null ? "" : ChordPlayed.Name; }
        }
        #endregion

        #region property ChordPlayedBackground
        private SolidColorBrush _chordPlayedBackground = new SolidColorBrush(Colors.Transparent);
        public SolidColorBrush ChordPlayedBackground
        {
            get { return _chordPlayedBackground; }
            set { SetProperty(ref _chordPlayedBackground, value); }
        }
        #endregion


        #region property MajorChord
        private bool _majorChord;
        public bool MajorChord
        {
            get { return _majorChord; }
            set { SetProperty(ref _majorChord, value); SelectChords(); }
        }
        #endregion

        #region property MajorChordInv1
        private bool _majorChordInv1;
        public bool MajorChordInv1
        {
            get { return _majorChordInv1; }
            set { SetProperty(ref _majorChordInv1, value); SelectChords(); }
        }
        #endregion

        #region property MajorChordInv2
        private bool _majorChordInv2;
        public bool MajorChordInv2
        {
            get { return _majorChordInv2; }
            set { SetProperty(ref _majorChordInv2, value); SelectChords(); }
        }
        #endregion

        #region property MinorChord
        private bool _minorChord;
        public bool MinorChord
        {
            get { return _minorChord; }
            set { SetProperty(ref _minorChord, value); SelectChords(); }
        }
        #endregion

        #region property MinorChordInv1
        private bool _minorChordInv1;
        public bool MinorChordInv1
        {
            get { return _minorChordInv1; }
            set { SetProperty(ref _minorChordInv1, value); SelectChords(); }
        }
        #endregion

        #region property MinorChordInv2
        private bool _minorChordInv2;
        public bool MinorChordInv2
        {
            get { return _minorChordInv2; }
            set { SetProperty(ref _minorChordInv2, value); SelectChords(); }
        }
        #endregion


        #region property MidiDeviceName
        private string _midiDeviceName;
        public string MidiDeviceName
        {
            get { return _midiDeviceName; }
            set { SetProperty(ref _midiDeviceName, value); }
        }
        #endregion


        #region property Game
        private Game _game;
        public Game Game
        {
            get { return _game; }
            set { SetProperty(ref _game, value); }
        }
        #endregion

        #region property IsGameActive
        public bool IsGameActive
        {
            get { return Game != null && Game.ChordToPlay != null; }
        }
        #endregion

        #region property CanStartGame
        private bool _canStartGame;
        public bool CanStartGame
        {
            get { return _canStartGame; }
            set { SetProperty(ref _canStartGame, value); }
        }
        #endregion

        public MainViewModel()
        {
            MajorChord = true;
            SelectChords();

            CanStartGame = true;
        }

        public void StartNewGame()
        {
            CanStartGame = false;

            Game = new Game(Chords.ChordsList, ChordsSelected);

            Game.SelectNextChordToPlay();
            ChordToPlayString = Game.ChordToPlay == null ? "" : Game.ChordToPlay.Name;

            ChordPlayed = null;
            OnPropertyChanged(nameof(IsGameActive));
            ChordPlayedBackground = new SolidColorBrush(Colors.Transparent);
        }

        public void StopGame()
        {
            CanStartGame = true;

            Game = null;

            ChordToPlayString = "";
            ChordPlayed = null;
            OnPropertyChanged(nameof(IsGameActive));
            ChordPlayedBackground = new SolidColorBrush(Colors.Transparent);
        }

        public void NextChord()
        {
            if (Game != null && Game.ChordToPlay != null)
            {
                if (Game.SelectNextChordToPlay())
                {
                    ChordToPlayString = Game.ChordToPlay == null ? "" : Game.ChordToPlay.Name;
                    ChordPlayed = null;
                }
                else
                {
                    ChordToPlayString = "";
                    CanStartGame = false;
                }
            }

            ChordPlayedBackground = new SolidColorBrush(Colors.Transparent);
            OnPropertyChanged(nameof(IsGameActive));
        }

        public void SelectNote(int midiNote)
        {
            if (_notesPlayed.FirstOrDefault(x => x.MidiNote == midiNote) == null)
            {
                NotesPlayed.Add(new Note(midiNote));
            }
        }

        public void DeselectNote(int midiNote)
        {
            Note note = _notesPlayed.FirstOrDefault(x => x.MidiNote == midiNote);
            if (note != null)
            {
                NotesPlayed.Remove(note);
            }
        }

        public void ProcessNotes()
        {
            if (Game == null)
            {
                return;
            }

            var result = Game.CheckChordPlayed(NotesPlayed.ToList());
            ChordPlayed = result.Played;
            ChordPlayedBackground = result.Correct ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        }

        public void SelectChords()
        {
            if (IsGameActive)
            {
                StopGame();
            }

            if (ChordsSelected == null)
            {
                ChordsSelected = new List<Chord>();
            }

            ChordsSelected.Clear();
            if (MajorChord)
            {
                ChordsSelected.AddRange(Chords.ChordsList.Where(x => x.ChordType == ChordType.Major));
            }
            if (MajorChordInv1)
            {
                ChordsSelected.AddRange(Chords.ChordsList.Where(x => x.ChordType == ChordType.MajorInv1));
            }
            if (MajorChordInv2)
            {
                ChordsSelected.AddRange(Chords.ChordsList.Where(x => x.ChordType == ChordType.MajorInv2));
            }

            if (MinorChord)
            {
                ChordsSelected.AddRange(Chords.ChordsList.Where(x => x.ChordType == ChordType.Minor));
            }
            if (MinorChordInv1)
            {
                ChordsSelected.AddRange(Chords.ChordsList.Where(x => x.ChordType == ChordType.MinorInv1));
            }
            if (MinorChordInv2)
            {
                ChordsSelected.AddRange(Chords.ChordsList.Where(x => x.ChordType == ChordType.MinorInv2));
            }
        }
    }
}
