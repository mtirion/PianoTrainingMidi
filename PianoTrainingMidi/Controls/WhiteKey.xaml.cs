using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace PianoTrainingMidi.Controls
{
    public sealed partial class WhiteKey : UserControl
    {
        #region property MidiNote (DP)
        public int MidiNote
        {
            get { return (int)GetValue(MidiNoteProperty); }
            set { SetValue(MidiNoteProperty, value); }
        }

        public static readonly DependencyProperty MidiNoteProperty =
            DependencyProperty.Register("MidiNote", typeof(int), typeof(WhiteKey), new PropertyMetadata(0));
        #endregion

        #region property NoteLetter (DP)
        public string NoteLetter
        {
            get { return (string)GetValue(NoteLetterProperty); }
            set { SetValue(NoteLetterProperty, value); }
        }

        public static readonly DependencyProperty NoteLetterProperty =
            DependencyProperty.Register("NoteLetter", typeof(string), typeof(WhiteKey), new PropertyMetadata(""));
        #endregion

        #region property IsActive (DP)
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(WhiteKey), new PropertyMetadata(false));
        #endregion

        public WhiteKey()
        {
            this.InitializeComponent();
        }
    }
}
