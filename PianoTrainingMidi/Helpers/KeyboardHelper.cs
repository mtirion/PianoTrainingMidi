using System.Collections.Generic;
using System.Linq;

namespace PianoTrainingMidi.Models
{
    public static class KeyboardHelper
    {
        private static string[] PRIMARY_NOTES = { "C", "C#", "D", "E♭", "E", "F", "F#", "G", "G#", "A", "B♭", "B" };
        private static string[] SECUNDARY_NOTES = { "C", "D♭", "D", "D#", "E", "F", "G♭", "G", "A♭", "A", "A#", "B" };
        #region property NotePrimaryNames
        public static string[] NotePrimaryNames
        {
            get { return PRIMARY_NOTES; }
        }
        #endregion

        #region property NoteSecundaryNames
        public static string[] NoteSecundaryNames
        {
            get { return SECUNDARY_NOTES; }
        }
        #endregion

        /// <summary>
        /// Primary name of the midiNote.
        /// </summary>
        /// <param name="midiNote">MIDI note.</param>
        /// <returns>Primary name.</returns>
        public static string GetPrimaryNoteName(int midiNote)
        {
            return PRIMARY_NOTES[midiNote % 12];
        }

        /// <summary>
        /// Secundary name of the midiNote.
        /// </summary>
        /// <param name="midiNote">MIDI note.</param>
        /// <returns>Secundary name.</returns>
        public static string GetScundaryNoteName(int midiNote)
        {
            return SECUNDARY_NOTES[midiNote % 12];
        }
    }
}
