using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PianoTrainingMidi.Controls
{
    public sealed partial class Keyboard : UserControl
    {
        #region property StartMidiNote (DP)
        public int StartMidiNote
        {
            get { return (int)GetValue(StartMidiNoteProperty); }
            set { SetValue(StartMidiNoteProperty, value); }
        }

        public static readonly DependencyProperty StartMidiNoteProperty =
            DependencyProperty.Register("StartMidiNote", typeof(int), typeof(Keyboard), new PropertyMetadata(-1, OnStartNoteChanged));

        private static void OnStartNoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Keyboard keyboard = (Keyboard)d;
            keyboard.BuildKeyboard((int)e.NewValue, keyboard.EndMidiNote);
        }
        #endregion

        #region property EndMidiNote (DP)
        public int EndMidiNote
        {
            get { return (int)GetValue(EndMidiNoteProperty); }
            set { SetValue(EndMidiNoteProperty, value); }
        }

        public static readonly DependencyProperty EndMidiNoteProperty =
            DependencyProperty.Register("EndMidiNote", typeof(int), typeof(Keyboard), new PropertyMetadata(-1, OnEndNoteChanged));

        private static void OnEndNoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Keyboard keyboard = (Keyboard)d;
            keyboard.BuildKeyboard(keyboard.StartMidiNote, (int)e.NewValue);
        }
        #endregion

        public Keyboard()
        {
            this.InitializeComponent();
        }

        public void PressNote(int midiNote)
        {
            foreach (UIElement octave in Octaves.Children)
            {
                if (octave is Octave)
                {
                    Octave o = octave as Octave;
                    if (midiNote >= o.StartMidiNote && midiNote <= o.EndMidiNote)
                    {
                        o.PressNote(midiNote);
                    }
                }
            }
        }

        public void UnpressNote(int midiNote)
        {
            foreach (UIElement octave in Octaves.Children)
            {
                if (octave is Octave)
                {
                    Octave o = octave as Octave;
                    if (midiNote >= o.StartMidiNote && midiNote <= o.EndMidiNote)
                    {
                        o.UnpressNote(midiNote);
                    }
                }
            }
        }

        public void BuildKeyboard(int startMidiNote, int endMidiNote)
        {
            Octaves.Children.Clear();

            if (endMidiNote < startMidiNote)
            {
                return;
            }

            int midiNote = startMidiNote;
            while (midiNote <= endMidiNote)
            {
                int octaveStart = midiNote - (midiNote % 12);
                int end = Math.Min(octaveStart + 11, endMidiNote);

                Octave octave = new Octave();
                octave.StartMidiNote = midiNote;
                octave.EndMidiNote = end;

                Octaves.Children.Add(octave);

                midiNote = end + 1;
            }
        }
    }
}
