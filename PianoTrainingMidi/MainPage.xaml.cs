using PianoTrainingMidi.Controls;
using PianoTrainingMidi.Helpers;
using PianoTrainingMidi.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace PianoTrainingMidi
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel = new MainViewModel();
        private MidiDeviceWatcher midiInDeviceWatcher;
        private DispatcherTimer _processTimer;
        private DispatcherTimer _indicationTimer;

        public MainPage()
        {
            this.InitializeComponent();

            _processTimer = new DispatcherTimer();
            _processTimer.Interval = TimeSpan.FromMilliseconds(500);
            _processTimer.Tick += ProcessNotes;

            _indicationTimer = new DispatcherTimer();
            _indicationTimer.Interval = TimeSpan.FromMilliseconds(3000);
            _indicationTimer.Tick += RemoveIndication;

            // Set up the MIDI input device watcher
            this.midiInDeviceWatcher = new MidiDeviceWatcher(MidiInPort.GetDeviceSelector());
            this.midiInDeviceWatcher.OnDeviceListUpdated += MidiInDeviceWatcher_OnDeviceListUpdated;

            // Start watching for devices
            this.midiInDeviceWatcher.Start();
        }

        private void RemoveIndication(object sender, object e)
        {
            _indicationTimer.Stop();
            ViewModel.ChordPlayedBackground = new SolidColorBrush(Colors.Transparent);
        }

        private void ProcessNotes(object sender, object e)
        {
            _processTimer.Stop();
            ViewModel.ProcessNotes();
            _indicationTimer.Start();
        }

        private async void MidiInDeviceWatcher_OnDeviceListUpdated()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { ViewModel.MidiDeviceName = string.Empty; });
            if (this.midiInDeviceWatcher.DeviceNames.Any())
            {
                DeviceInformationCollection devInfoCollection = this.midiInDeviceWatcher.GetDeviceInformationCollection();
                if (devInfoCollection == null)
                {
                    return;
                }

                DeviceInformation devInfo = devInfoCollection.FirstOrDefault();
                if (devInfo == null)
                {
                    return;
                }

                var currentMidiInputDevice = await MidiInPort.FromIdAsync(devInfo.Id);
                if (currentMidiInputDevice == null)
                {
                    return;
                }

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => { ViewModel.MidiDeviceName = devInfo.Name; });
                
                // We have successfully created a MidiInPort; add the device to the list of active devices, and set up message receiving
                currentMidiInputDevice.MessageReceived += MidiInputDevice_MessageReceived;
            }
        }

        private async void MidiInputDevice_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
        {
            IMidiMessage msg = args.Message;

            try
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
                {
                    switch (msg.Type)
                    {
                        case MidiMessageType.NoteOff:
                            _processTimer.Stop();
                            MidiNoteOffMessage off = (MidiNoteOffMessage)msg;
                            DeselectNote(off.Note);

                            if (ViewModel.NotesPlayed.Count >= 3)
                            {
                                _processTimer.Start();
                            }
                            break;

                        case MidiMessageType.NoteOn:
                            _processTimer.Stop();
                            MidiNoteOnMessage on = (MidiNoteOnMessage)msg;
                            if (on.Velocity > 0)
                            {
                                SelectNote(on.Note);
                            }
                            else
                            {
                                DeselectNote(on.Note);
                            }

                            if (ViewModel.NotesPlayed.Count >= 3)
                            {
                                _processTimer.Start();
                            }

                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Receive error: {ex}");
            }
        }

        private void SelectNote(int midiNote)
        {
            foreach (UIElement octave in Keyboard.Children)
            {
                if (octave is Octave)
                {
                    ((Octave)octave).SetNoteActive(midiNote);
                }
            }
            ViewModel.SelectNote(midiNote);
        }

        private void DeselectNote(int midiNote)
        {
            foreach (UIElement octave in Keyboard.Children)
            {
                if (octave is Octave)
                {
                    ((Octave)octave).SetNoteInactive(midiNote);
                }
            }
            ViewModel.DeselectNote(midiNote);
        }

        private void Practice_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartNewGame();
            RestartButton.Content = "Restart";
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NextChord();
        }
    }
}
