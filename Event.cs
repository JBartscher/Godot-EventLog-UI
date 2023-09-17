using Godot;
using System;

public partial class Event : MarginContainer
{
    [Signal]
    public delegate void VanishedEventHandler();

    private TextureProgressBar _vanishProgressBar;
    private Tween _vanishTween;

    public override void _Ready()
    {
        _vanishProgressBar = GetNode<TextureProgressBar>("Panel/TextureProgressBar");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void Vanish()
    {
        _vanishProgressBar.Visible = true;
        if (_vanishTween != null && _vanishTween.IsRunning())
        {
            _vanishTween.Kill();
        }

        _vanishTween = GetTree().CreateTween();
        _vanishTween.SetTrans(Tween.TransitionType.Sine);
        _vanishTween.TweenProperty(_vanishProgressBar, "value", 0f, 5.0f);
        _vanishTween.TweenCallback(Callable.From(FinishVanish));
    }

    private void FinishVanish()
    {
        EmitSignal(SignalName.Vanished);
        CallDeferred(MethodName.QueueFree);
    }

    public void PauseVanish()
    {
        if (_vanishTween == null || !_vanishTween.IsRunning())
        {
            return;
        }

        _vanishProgressBar.Visible = false;
        _vanishTween.Pause();
    }

    public void StopVanish()
    {
        if (_vanishTween == null)
        {
            return;
        }

        _vanishProgressBar.Visible = false;
        _vanishTween.Stop();
    }

    public void ContinueVanish()
    {
        if (_vanishTween == null)
        {
            return;
        }

        _vanishProgressBar.Visible = true;
        _vanishTween.Play();
    }
}