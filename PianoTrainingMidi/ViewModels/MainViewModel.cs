using PianoTrainingMidi.Helpers;
using PianoTrainingMidi.Models;
using System.Collections.Generic;
using System.Linq;

namespace PianoTrainingMidi.ViewModels
{
    public class MainViewModel : BaseObject
    {
        #region property Notes
        private List<Note> _notes = new List<Note>();
        public List<Note> Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }
        #endregion

        public MainViewModel()
        {
            Notes.Add(new Note { MidiNote = 48, NoteLetter = "C", NoteAlternative = "C", Octave = 3 });
            Notes.Add(new Note { MidiNote = 49, NoteLetter = "C#", NoteAlternative = "D♭", Octave = 3 });
            Notes.Add(new Note { MidiNote = 50, NoteLetter = "D", NoteAlternative = "D", Octave = 3 });
            Notes.Add(new Note { MidiNote = 51, NoteLetter = "D#", NoteAlternative = "E♭", Octave = 3 });
            Notes.Add(new Note { MidiNote = 52, NoteLetter = "E", NoteAlternative = "E", Octave = 3 });
            Notes.Add(new Note { MidiNote = 53, NoteLetter = "F", NoteAlternative = "F", Octave = 3 });
            Notes.Add(new Note { MidiNote = 54, NoteLetter = "F#", NoteAlternative = "G♭", Octave = 3 });
            Notes.Add(new Note { MidiNote = 55, NoteLetter = "G", NoteAlternative = "G", Octave = 3 });
            Notes.Add(new Note { MidiNote = 56, NoteLetter = "G#", NoteAlternative = "A♭", Octave = 3 });
            Notes.Add(new Note { MidiNote = 57, NoteLetter = "A", NoteAlternative = "A", Octave = 3 });
            Notes.Add(new Note { MidiNote = 58, NoteLetter = "A#", NoteAlternative = "B♭", Octave = 3 });
            Notes.Add(new Note { MidiNote = 59, NoteLetter = "B", NoteAlternative = "B", Octave = 3 });

            Notes.Add(new Note { MidiNote = 60, NoteLetter = "C", NoteAlternative = "C", Octave = 4 });
            Notes.Add(new Note { MidiNote = 61, NoteLetter = "C#", NoteAlternative = "D♭", Octave = 4 });
            Notes.Add(new Note { MidiNote = 62, NoteLetter = "D", NoteAlternative = "D", Octave = 4 });
            Notes.Add(new Note { MidiNote = 63, NoteLetter = "D#", NoteAlternative = "E♭", Octave = 4 });
            Notes.Add(new Note { MidiNote = 64, NoteLetter = "E", NoteAlternative = "E", Octave = 4 });
            Notes.Add(new Note { MidiNote = 65, NoteLetter = "F", NoteAlternative = "F", Octave = 4 });
            Notes.Add(new Note { MidiNote = 66, NoteLetter = "F#", NoteAlternative = "G♭", Octave = 4 });
            Notes.Add(new Note { MidiNote = 67, NoteLetter = "G", NoteAlternative = "G", Octave = 4 });
            Notes.Add(new Note { MidiNote = 68, NoteLetter = "G#", NoteAlternative = "A♭", Octave = 4 });
            Notes.Add(new Note { MidiNote = 69, NoteLetter = "A", NoteAlternative = "A", Octave = 4 });
            Notes.Add(new Note { MidiNote = 70, NoteLetter = "A#", NoteAlternative = "B♭", Octave = 4 });
            Notes.Add(new Note { MidiNote = 71, NoteLetter = "B", NoteAlternative = "B", Octave = 4 });

            Notes.Add(new Note { MidiNote = 72, NoteLetter = "C", NoteAlternative = "C", Octave = 5 });
            Notes.Add(new Note { MidiNote = 73, NoteLetter = "C#", NoteAlternative = "D♭", Octave = 5 });
            Notes.Add(new Note { MidiNote = 74, NoteLetter = "D", NoteAlternative = "D", Octave = 5 });
            Notes.Add(new Note { MidiNote = 75, NoteLetter = "D#", NoteAlternative = "E♭", Octave = 5 });
            Notes.Add(new Note { MidiNote = 76, NoteLetter = "E", NoteAlternative = "E", Octave = 5 });
            Notes.Add(new Note { MidiNote = 77, NoteLetter = "F", NoteAlternative = "F", Octave = 5 });
            Notes.Add(new Note { MidiNote = 78, NoteLetter = "F#", NoteAlternative = "G♭", Octave = 5 });
            Notes.Add(new Note { MidiNote = 79, NoteLetter = "G", NoteAlternative = "G", Octave = 5 });
            Notes.Add(new Note { MidiNote = 80, NoteLetter = "G#", NoteAlternative = "A♭", Octave = 5 });
            Notes.Add(new Note { MidiNote = 81, NoteLetter = "A", NoteAlternative = "A", Octave = 5 });
            Notes.Add(new Note { MidiNote = 82, NoteLetter = "A#", NoteAlternative = "B♭", Octave = 5 });
            Notes.Add(new Note { MidiNote = 83, NoteLetter = "B", NoteAlternative = "B", Octave = 5 });
        }

        public Note GetNote(int midiNote)
        {
            return Notes.FirstOrDefault(x => x.MidiNote == midiNote);
        }
    }
}
