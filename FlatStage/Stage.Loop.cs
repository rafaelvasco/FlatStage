using System;
using System.Threading;
using FlatStage.Input;
using FlatStage.Graphics;
using FlatStage.Platform;

namespace FlatStage;

public partial class Stage
{
    private const double DefaultFramerate = 60;

    public static double UpdateRate
    {
        get => _instance._updateRate;
        set => _instance.ResetLoop(value);
    }

    public static bool UnlockFrameRate { get; set; } = false;

    public static TimeSpan InactiveSleepTime
    {
        get => _instance._inactiveSleepTime;
        set
        {
            if (value < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("The time must be positive.", default(Exception));

            _instance._inactiveSleepTime = value;
        }
    }

    public bool IsActive { get; internal set; } = true;

    private bool _running;

    private const int TimeHistoryCount = 4;
    private const int UpdateMult = 1;

    private bool _resync = true;

    private double _fixedDeltatime;
    private double _desiredFrametime;
    private double _vsyncMaxError;
    private double[] _snapFreqs = null!;
    private double[] _timeAverager = null!;
    private double _updateRate;
    private bool _suppressDraw;

    private TimeSpan _inactiveSleepTime = TimeSpan.FromSeconds(0.02);

    private double _prevFrameTime;
    private double _frameAccum;

    internal static void SuppressDraw()
    {
        _instance._suppressDraw = true;
    }

    private void InitLoop()
    {
        ResetLoop(DefaultFramerate);

        double time60Hz = PlatformContext.GetPerfFreq() / 60;
        _snapFreqs = new[]
        {
            time60Hz, //60fps
            time60Hz * 2, //30fps
            time60Hz * 3, //20fps
            time60Hz * 4, //15fps
            (time60Hz + 1) / 2 //120fps
        };

        _timeAverager = new double[TimeHistoryCount];

        for (int i = 0; i < TimeHistoryCount; i++)
        {
            _timeAverager[i] = _desiredFrametime;
        }
    }

    private void ResetLoop(double desiredUpdateRate)
    {
        _updateRate = desiredUpdateRate;

        _frameAccum = 0;
        _prevFrameTime = 0;
        _fixedDeltatime = 1.0 / _updateRate;
        _desiredFrametime = PlatformContext.GetPerfFreq() / _updateRate;
        _vsyncMaxError = PlatformContext.GetPerfCounter() * 0.0002;
    }

    private void Tick(Game game)
    {
        if (!IsActive && InactiveSleepTime.TotalMilliseconds >= 1.0)
        {
            Thread.Sleep((int)_inactiveSleepTime.TotalMilliseconds);
        }

        double currentFrameTime = PlatformContext.GetPerfCounter();

        double deltaTime = currentFrameTime - _prevFrameTime;

        _prevFrameTime = currentFrameTime;

        // Handle unexpected timer anomalies (overflow, extra slow frames, etc)
        if (deltaTime > _desiredFrametime * 8)
        {
            deltaTime = _desiredFrametime;
        }

        if (deltaTime < 0)
        {
            deltaTime = 0;
        }

        // VSync Time Snapping
        for (int i = 0; i < _snapFreqs.Length; ++i)
        {
            var snapFreq = _snapFreqs[i];

            if (Math.Abs(deltaTime - snapFreq) < _vsyncMaxError)
            {
                deltaTime = snapFreq;
                break;
            }
        }

        // Delta Time Averaging
        for (int i = 0; i < TimeHistoryCount - 1; ++i)
        {
            _timeAverager[i] = _timeAverager[i + 1];
        }

        _timeAverager[TimeHistoryCount - 1] = deltaTime;

        deltaTime = 0;

        for (int i = 0; i < TimeHistoryCount; ++i)
        {
            deltaTime += _timeAverager[i];
        }

        deltaTime /= TimeHistoryCount;

        // Add To Accumulator
        _frameAccum += deltaTime;

        // Spiral of Death Protection
        if (_frameAccum > _desiredFrametime * 8)
        {
            _resync = true;
        }

        // Timer Resync Requested
        if (_resync)
        {
            _frameAccum = 0;
            deltaTime = _desiredFrametime;
            _resync = false;
        }

        PlatformContext.ProcessEvents();

        // Unlocked Frame Rate, Interpolation Enabled
        if (UnlockFrameRate)
        {
            double consumedDeltaTime = deltaTime;

            while (_frameAccum >= _desiredFrametime)
            {
                game.InternalFixedUpdate((float)_fixedDeltatime);

                // Cap Variable Update's dt to not be larger than fixed update, 
                // and interleave it (so game state can always get animation frame it needs)
                if (consumedDeltaTime > _desiredFrametime)
                {
                    Control.Update();
                    game.InternalUpdate((float)_fixedDeltatime);
                    consumedDeltaTime -= _desiredFrametime;
                }

                _frameAccum -= _desiredFrametime;
            }

            Control.Update();
            game.InternalUpdate((float)(consumedDeltaTime / PlatformContext.GetPerfFreq()));

            if (!_suppressDraw)
            {
                game.InternalDraw((float)(_frameAccum / _desiredFrametime));
                GraphicsContext.Present();
            }
            else
            {
                _suppressDraw = false;
            }
        }
        // Locked Frame Rate, No Interpolation
        else
        {
            while (_frameAccum >= _desiredFrametime * UpdateMult)
            {
                for (int i = 0; i < UpdateMult; ++i)
                {
                    Control.Update();
                    game.InternalFixedUpdate((float)_fixedDeltatime);
                    game.InternalUpdate((float)_fixedDeltatime);

                    _frameAccum -= _desiredFrametime;
                }
            }

            if (!_suppressDraw)
            {
                game.InternalDraw(1.0f);
                GraphicsContext.Present();
            }
            else
            {
                _suppressDraw = false;
            }
        }
    }
}