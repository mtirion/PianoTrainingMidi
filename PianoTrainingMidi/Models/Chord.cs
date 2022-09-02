using System.Collections.Generic;
using System.Linq;

namespace PianoTrainingMidi.Models
{
    public class Chord
    {
        public ChordType ChordType { get; set; } = ChordType.Major;
        public string Name { get; set; }
        public List<Note> Notes { get; set; } = new List<Note>();

        public string ChordString { get; private set; }
        public string AlternateChordString { get; private set; }

        public void SetString()
        {
            ChordString = string.Join("-", Notes.OrderBy(x => x.MidiNote).Select(x => x.NoteLetter));
            AlternateChordString = string.Join("-", Notes.OrderBy(x => x.MidiNote).Select(x => x.NoteAlternative));
        }

        public Chord GetCopy()
        {
            Chord chord = new Chord()
            {
                ChordType = ChordType,
                Name = Name,
                Notes = new List<Note>()
            };

            foreach (Note baseNote in Notes)
            {
                chord.Notes.Add(baseNote.GetCopy());
            }

            chord.SetString();

            return chord;
        }
    }
}
