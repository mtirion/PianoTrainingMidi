using PianoTrainingMidi.Helpers;

namespace PianoTrainingMidi.Models
{
    public class Note : BaseObject
    {
        #region property MidiNote
        private int _midiNote;
        public int MidiNote
        {
            get { return _midiNote; }
            set { SetProperty(ref _midiNote, value); OnPropertyChanged(nameof(IsBlackKey)); OnPropertyChanged(nameof(IsWhiteKey)); }
        }
        #endregion

        #region property NoteLetter
        private string _noteLetter;
        public string NoteLetter
        {
            get { return _noteLetter; }
            set { SetProperty(ref _noteLetter, value); }
        }
        #endregion

        #region property NoteAlternative
        private string _noteAlternative;
        public string NoteAlternative
        {
            get { return _noteAlternative; }
            set { SetProperty(ref _noteAlternative, value); }
        }
        #endregion

        #region property Octave
        private int _octave;
        public int Octave
        {
            get { return _octave; }
            set { SetProperty(ref _octave, value); }
        }
        #endregion

        #region property IsActive
        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value); }
        }
        #endregion

        #region property IsBlackKey
        public bool IsBlackKey
        {
            get 
            {
                int note = MidiNote % 12;
                return note == 1 || note == 3 || note == 6 || note == 8 || note == 10; 
            }
        }
        #endregion

        #region property IsWhiteKey
        public bool IsWhiteKey
        {
            get
            {
                return !IsBlackKey;
            }
        }
        #endregion

        public Note(int midiNote)
        {
            MidiNote = midiNote;
            NoteLetter = KeyboardHelper.GetPrimaryNoteName(midiNote);
            NoteAlternative = KeyboardHelper.GetScundaryNoteName(midiNote);
            Octave = midiNote / 12;
        }

        private Note()
        {
        }

        public Note GetCopy()
        {
            return new Note()
            {
                MidiNote = _midiNote,
                NoteLetter = _noteLetter,
                NoteAlternative = _noteAlternative,
                IsActive = _isActive,
                Octave = _octave,
            };
        }
    }
}
