using MeltySynth;

namespace FlatStage;

public class MidiPlayer
{
    private readonly MidiFileSequencer _sequencer;

    private readonly object mutex;

    public MidiPlayer(string soundFontPath)
    {
        var settings = new SynthesizerSettings(44100)
        {
            EnableReverbAndChorus = false
        };

        var synthesizer = new Synthesizer(soundFontPath, settings);
        _sequencer = new MidiFileSequencer(synthesizer);

        mutex = new object();
    }

    public void ProcessAudio(Span<float> buffer, out int samplesWritten)
    {
        lock (mutex)
        {
            _sequencer.RenderInterleaved(buffer);
        }

        samplesWritten = buffer.Length;
    }

    public void Play(MidiFile midiFile, bool loop)
    {
        lock (mutex)
        {
            _sequencer.Play(midiFile, loop);
        }
    }

    public void Stop()
    {
        lock (mutex)
        {
            _sequencer.Stop();
        }
    }
}
