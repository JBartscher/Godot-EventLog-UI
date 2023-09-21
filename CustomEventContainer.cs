using System.Collections.Generic;
using System.Linq;
using Godot;
using EventLogUI;
using Godot.Collections;

public partial class CustomEventContainer : Control
{
    private PackedScene _eventScene = GD.Load<PackedScene>("res://event.tscn");

    [ExportGroup("EventContainer")]
    [Export]
    private bool Active { get; set; } = false;

    /**
     * This must match the minimum dimensions of the margin container
     */
    [Export]
    public Rect2 EventRect { get; set; }

    private List<Event> _events = new List<Event>();

    public override void _Input(InputEvent @event)
    {
        if (!Active)
        {
            return;
        }

        if (@event.IsActionPressed("space"))
        {
            Event e = _eventScene.Instantiate<Event>();
            _events.Insert(0, e); // put events at the end of the array
            // bounce in from the right
            e.Position = CalculateInitialEventPosition();
            Tween bounceInTween = CreateTween();
            bounceInTween.TweenProperty(e, "position:x", -EventRect.Size.X, 0.3f)
                .SetTrans(Tween.TransitionType.Back); // Back, Spring
            // add to tree
            AddChild(e);

            // when the new element vanishes than a signal is emitted that lets this class handle its demise
            e.Vanished += HandleVanished;

            // stop vanish on all existing events and vanish the oldest one in the array 
            VanishLastChild();
        }
    }

    private Vector2 CalculateInitialEventPosition()
    {
        // calcualte y pos based on position in array and screen size
        float y = DisplayServer.WindowGetSize().Y - (EventRect.Size.Y * _events.Count);
        // x is always 0 as we want to place it just outside the screen and than bounce it in
        return new Vector2(0, y);
    }

    private void UpdateEventPositions()
    {
        Tween moveAlongTween = CreateTween();

        foreach (Event e in _events)
        {
            int i = _events.IndexOf(e);
            moveAlongTween.Parallel().TweenProperty(_events[i], "position:y", EventRect.Size.Y, 0.8f).AsRelative()
                .SetTrans(Tween.TransitionType.Sine); // Back, Spring
        }
        
        moveAlongTween.Play();
        moveAlongTween.TweenCallback(Callable.From(VanishLastChild));
    }

    private void HandleVanished(Event e)
    {
        _events.Remove(e);
        e.QueueFree();
        UpdateEventPositions();
    }

    private void VanishLastChild()
    {
        if (_events.Count > 0)
        {
            _events.Last().Vanish();
        }
    }
}