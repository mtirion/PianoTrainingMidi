using PianoTrainingMidi.Helpers;
using PianoTrainingMidi.Models;
using PianoTrainingMidi.ViewModels;
using System;
using System.Linq;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;
using Windows.UI.Xaml.Controls;

namespace PianoTrainingMidi
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel = new MainViewModel();

        private MidiDeviceWatcher midiInDeviceWatcher;

        public MainPage()
        {
            this.InitializeComponent();

            // Set up the MIDI input device watcher
            this.midiInDeviceWatcher = new MidiDeviceWatcher(MidiInPort.GetDeviceSelector());
            this.midiInDeviceWatcher.OnDeviceListUpdated += MidiInDeviceWatcher_OnDeviceListUpdated;

            // Start watching for devices
            this.midiInDeviceWatcher.Start();
        }

        private async void MidiInDeviceWatcher_OnDeviceListUpdated()
        {
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

                // We have successfully created a MidiInPort; add the device to the list of active devices, and set up message receiving
                currentMidiInputDevice.MessageReceived += MidiInputDevice_MessageReceived;
            }
        }

        private void MidiInputDevice_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
        {
            IMidiMessage msg = args.Message;
            Note note;

            // Add MIDI message parameters to the output, depending on the type of message
            switch (msg.Type)
            {
                case MidiMessageType.NoteOff:
                    MidiNoteOffMessage off = (MidiNoteOffMessage)msg;
                    note = ViewModel.GetNote(off.Note);
                    note.IsActive = true;
                    break;
                case MidiMessageType.NoteOn:
                    MidiNoteOnMessage on = (MidiNoteOnMessage)msg;
                    note = ViewModel.GetNote(on.Note);
                    note.IsActive = false;
                    break;
            }
        }
    }
}
