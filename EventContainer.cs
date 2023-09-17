using Godot;
using System;
using System.Linq;

public partial class EventContainer : VBoxContainer
{
	private PackedScene _eventScene = GD.Load<PackedScene>("res://event.tscn");
	
	
	public override void _Ready()
	{
		GD.Print(GetChildren());
	}
    
	public override void _Input(InputEvent @event)
	{
		GD.Print(@event.AsText());
		// if (@event.IsAction("space"))
		// {
		// 	GD.Print("action space fired");
		// 	Event e =_eventScene.Instantiate<Event>();
		// 	e.Vanished += () => VanishLastChild();
		// 	AddChild(e);
		// }
		//
		// VanishLastChild();
	}

	private void VanishLastChild()
	{
		GD.Print("Event trigger");
		var children = GetChildren();
		foreach (Event e in children)
		{
			e.StopVanish();
		}

		var lastChild = (Event)children.Last();
		if (lastChild != null)
		{
			lastChild.Vanish();
		}
		
	}
}
