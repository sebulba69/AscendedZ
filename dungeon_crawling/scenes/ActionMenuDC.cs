using AscendedZ;
using AscendedZ.battle.battle_state_machine;
using AscendedZ.battle;
using AscendedZ.dungeon_crawling.combat;
using Godot;
using System;
using System.Collections.Generic;
using AscendedZ.dungeon_crawling.combat.skillsdc;
using Godot.NativeInterop;
using System.Numerics;
using Vector2 = Godot.Vector2;
using AscendedZ.dungeon_crawling.combat.battledc;

public partial class ActionMenuDC : PanelContainer
{
	private Label _apCounter;
    private ItemList _actionList;
	private BDCSystem _bdcSystem;
	private BigInteger _ap;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_actionList = this.GetNode<ItemList>("%ActionList");
		_apCounter = this.GetNode<Label>("%APCounter");
	}

	public void Initialize(BDCSystem bdcSystem)
	{
        _bdcSystem = bdcSystem;
    }

    private void _OnMenuItemClicked(long index, Vector2 at_position, long mouse_button_index)
    {
        if (mouse_button_index == (long)MouseButton.Left)
        {
			/*                        
				_battleSceneObject.SkillSelected?.Invoke(_battleSceneObject, new PlayerTargetSelectedEventArgs
                {
					SkillIndex = _selectedIndex
                });
                _canInput = false;
			*/
        }
    }
}
