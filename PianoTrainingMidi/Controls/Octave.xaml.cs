using PianoTrainingMidi.Models;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace PianoTrainingMidi.Controls
{
    public sealed partial class Octave : UserControl
    {
        #region property StartMidiNote (DP)
        public int StartMidiNote
        {
            get { return (int)GetValue(StartMidiNoteProperty); }
            set { SetValue(StartMidiNoteProperty, value); }
        }

        public static readonly DependencyProperty StartMidiNoteProperty =
            DependencyProperty.Register("StartMidiNote", typeof(int), typeof(Octave), new PropertyMetadata(-1, OnStartNoteChanged));

        private static void OnStartNoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Octave o = (Octave)d;
            if ((int)e.NewValue != -1)
            {
                int midiNote = (int)e.NewValue - ((int)e.NewValue % 12);
                o.BuildOctave(midiNote);
            }
        }
        #endregion

        private Dictionary<int, UIElement> _keys = new Dictionary<int, UIElement>();

        private int[] _blackNotesMargins = { 28, 28, 60, 28, 26 };

        /// <summary>
        /// Constructor of <see cref="Octave"/>.
        /// </summary>
        public Octave()
        {
            this.InitializeComponent();
        }

        public void SetNoteActive(int midiNote)
        {
            if (_keys.TryGetValue(midiNote, out UIElement control))
            {
                if (control is WhiteKey)
                {
                    ((WhiteKey)control).IsActive = true;
                }
                else
                {
                    ((BlackKey)control).IsActive = true;
                }
            }
        }

        public void SetNoteInactive(int midiNote)
        {
            if (_keys.TryGetValue(midiNote, out UIElement control))
            {
                if (control is WhiteKey)
                {
                    ((WhiteKey)control).IsActive = false;
                }
                else
                {
                    ((BlackKey)control).IsActive = false;
                }
            }
        }

        /// <summary>
        /// Build the octave from start MIDI note.
        /// </summary>
        /// <param name="startMidiNote">Start MIDI note.</param>
        public void BuildOctave(int startMidiNote)
        {
            WhiteKeys.Children.Clear();
            BlackKeys.Children.Clear();
            _keys.Clear();

            int blackIndex = 0;
            for (int midiNote = startMidiNote; midiNote < startMidiNote + 12; midiNote++)
            {

                Note note = new Note(midiNote);
                if (note.IsWhiteKey)
                {
                    WhiteKey white = new WhiteKey
                    {
                        MidiNote = note.MidiNote,
                        NoteLetter = note.NoteLetter,
                    };
                    Binding b = new Binding();
                    b.Mode = BindingMode.OneWay;
                    b.Source = note.IsActive;
                    SetBinding(WhiteKey.IsActiveProperty, b);
                    WhiteKeys.Children.Add(white);
                    _keys.Add(midiNote, white);
                }
                else
                {
                    BlackKey black = new BlackKey
                    {
                        MidiNote = note.MidiNote,
                        NoteLetter = note.NoteLetter,
                    };
                    Binding b = new Binding();
                    b.Mode = BindingMode.OneWay;
                    b.Source = note.IsActive;
                    SetBinding(WhiteKey.IsActiveProperty, b);
                    black.Margin = new Thickness(_blackNotesMargins[blackIndex++],0,0,0);
                    BlackKeys.Children.Add(black);
                    _keys.Add(midiNote, black);
                }
            }
        }
    }
}
