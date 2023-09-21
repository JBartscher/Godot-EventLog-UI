using Godot;
using System.Linq;

namespace EventLogUI;

public partial class EventContainer : VBoxContainer
{
    private PackedScene _eventScene = GD.Load<PackedScene>("res://event.tscn");

    [ExportGroup("EventContainer")]
    [Export]
    private bool Active { get; set; } = false;

    public override void _Ready()
    {
        GD.Print(GetChildren());
    }

    public override void _Input(InputEvent @event)
    {
        if (!Active)
        {
            return;
        }
        if (@event.IsActionPressed("space"))
        {
            GD.Print("action space fired");
            Event e = _eventScene.Instantiate<Event>();
            // e.Vanished += VanishedHandler;
            AddChild(e);
            VanishLastChild();
        }
    }

    private void VanishedHandler()
    {
        GD.Print("Event triggerd, vanish children");
        VanishLastChild();
    }

    private void VanishLastChild()
    {
        var children = GetChildren();
        foreach (var node in children)
        {
            var e = (Event)node;
            e.StopVanish();
        }

        var lastChild = (Event)children.Last();
        GD.Print(lastChild);
        if (lastChild != null)
        {
            lastChild.Vanish();
        }
    }
}