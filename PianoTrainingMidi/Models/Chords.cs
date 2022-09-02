using System;
using System.Collections.Generic;
using System.Linq;

namespace PianoTrainingMidi.Models
{
    public class Chords
    {
        public List<Note> NotesList { get; set; } = new List<Note>();
        public List<Chord> ChordsList { get; set; } = new List<Chord>();

        public Chords()
        {
            for (int midiNote = 60; midiNote < 72; midiNote++)
            {
                NotesList.Add(new Note(midiNote));
            }

            foreach (Note note in NotesList)
            {
                Chord chord = GetMajorChord(note);
                ChordsList.Add(chord);
                ChordsList.Add(GetInv1Chord(chord));
                ChordsList.Add(GetInv2Chord(chord));

                chord = GetMinorChord(note);
                ChordsList.Add(chord);
                ChordsList.Add(GetInv1Chord(chord));
                ChordsList.Add(GetInv2Chord(chord));
            }
        }

        public bool IsChordPlayed(List<Note> notes)
        {
            if (notes == null || !notes.Any())
            {
                return false;
            }

            string played = string.Join("-", notes.OrderBy(x => x.MidiNote).Select(x => x.NoteLetter));
            return ChordsList.FirstOrDefault(x => x.ChordString == played) != null;
        }

        public Chord GetChordPlayed(List<Note> notes)
        {
            if (notes == null || !notes.Any())
            {
                return null;
            }

            string playedString = string.Join("-", notes.OrderBy(x => x.MidiNote).Select(x => x.NoteLetter));
            return ChordsList.FirstOrDefault(x => x.ChordString == playedString);
        }

        private Chord GetMajorChord(Note note)
        {
            int index = NotesList.IndexOf(note);

            Chord chord = new Chord
            {
                ChordType = ChordType.Major,
                Name = $"{note.NoteLetter}",
            };

            chord.Notes.Add(note.GetCopy());

            Note next = NotesList[(index + 4) % 12].GetCopy();
            if (index + 4 >= NotesList.Count)
            {
                next.MidiNote += 12;
            }
            chord.Notes.Add(next);


            next = NotesList[(index + 4 + 3) % 12].GetCopy();
            if (index + 4 + 3 >= NotesList.Count)
            {
                next.MidiNote += 12;
            }
            chord.Notes.Add(next);

            chord.SetString();

            return chord;
        }

        private Chord GetMinorChord(Note note)
        {
            int index = NotesList.IndexOf(note);

            Chord chord = new Chord
            {
                ChordType = ChordType.Minor,
                Name = $"{note.NoteLetter}min",
            };

            chord.Notes.Add(note.GetCopy());

            Note next = NotesList[(index + 3) % 12].GetCopy();
            if (index + 3 >= NotesList.Count)
            {
                next.MidiNote += 12;
            }
            chord.Notes.Add(next);


            next = NotesList[(index + 3 + 4) % 12].GetCopy();
            if (index + 3 + 4 >= NotesList.Count)
            {
                next.MidiNote += 12;
            }
            chord.Notes.Add(next);

            chord.SetString();

            return chord;
        }

        private Chord GetInv1Chord(Chord baseChord)
        {
            Chord chord = baseChord.GetCopy();
            chord.Name += " Inv1";
            chord.ChordType = baseChord.ChordType + 1;
            chord.Notes[0].MidiNote += 12;
            chord.Notes = chord.Notes.OrderBy(x => x.MidiNote).ToList();
            chord.SetString();
            return chord;
        }

        private Chord GetInv2Chord(Chord baseChord)
        {
            Chord chord = baseChord.GetCopy();
            chord.Name += " Inv2";
            chord.ChordType = baseChord.ChordType + 2;
            chord.Notes[0].MidiNote += 12;
            chord.Notes[1].MidiNote += 12;
            chord.Notes = chord.Notes.OrderBy(x => x.MidiNote).ToList();
            chord.SetString();
            return chord;
        }
    }
}
