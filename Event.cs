using Godot;

namespace EventLogUI;

public partial class Event : MarginContainer
{
    [Signal]
    public delegate void VanishedEventHandler(Event vanishedEvent);

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
        if (_vanishTween != null && _vanishTween.IsRunning())
        {
            return;
        }
        
        _vanishProgressBar.Value = 100;
        _vanishProgressBar.Visible = true;
 
        _vanishTween = GetTree().CreateTween();
        _vanishTween.SetTrans(Tween.TransitionType.Sine);
        _vanishTween.TweenProperty(_vanishProgressBar, "value", 0f, 1.0f);
        _vanishTween.TweenCallback(Callable.From(FinishVanish));
        _vanishTween.Play();
    }

    private void FinishVanish()
    {
        EmitSignal(SignalName.Vanished, this);
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
        _vanishProgressBar.Value = 100;
        _vanishProgressBar.Visible = false;
        _vanishTween.Kill();
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