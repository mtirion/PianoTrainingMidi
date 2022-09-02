using PianoTrainingMidi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                o.BuildOctave((int)e.NewValue, o.EndMidiNote);
            }
        }
        #endregion

        #region property EndMidiNote (DP)
        public int EndMidiNote
        {
            get { return (int)GetValue(EndMidiNoteProperty); }
            set { SetValue(EndMidiNoteProperty, value); }
        }

        public static readonly DependencyProperty EndMidiNoteProperty =
            DependencyProperty.Register("EndMidiNote", typeof(int), typeof(Octave), new PropertyMetadata(-1, OnEndNoteChanged));

        private static void OnEndNoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Octave o = (Octave)d;
            if ((int)e.NewValue != -1)
            {
                o.BuildOctave(o.StartMidiNote, (int)e.NewValue);
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

        public void PressNote(int midiNote)
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

        public void UnpressNote(int midiNote)
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
        /// <param name="endMidiNote">End MIDI note.</param>
        public void BuildOctave(int startMidiNote, int endMidiNote)
        {
            WhiteKeys.Children.Clear();
            BlackKeys.Children.Clear();
            _keys.Clear();

            if (endMidiNote < startMidiNote)
            {
                return;
            }

            int startOctave = startMidiNote - (startMidiNote % 12);
            int end = Math.Min(startOctave + 12, endMidiNote + 1);

            int blackIndex = 0;
            bool isFirstBlackKey = true;
            for (int midiNote = startOctave; midiNote < end; midiNote++)
            {
                Note note = new Note(midiNote);
                if (note.IsWhiteKey)
                {
                    if (midiNote >= startMidiNote)
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
                }
                else
                {
                    if (midiNote >= startMidiNote)
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
                        int left = _blackNotesMargins[blackIndex];
                        if (blackIndex > 0 && isFirstBlackKey)
                        {
                            left += left / 2;
                        }
                        black.Margin = new Thickness(left, 0, 0, 0);
                        BlackKeys.Children.Add(black);
                        _keys.Add(midiNote, black);

                        isFirstBlackKey = false;
                    }

                    blackIndex++;
                }
            }
        }
    }
}
