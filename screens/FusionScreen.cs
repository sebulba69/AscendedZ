using AscendedZ;
using AscendedZ.entities.partymember_objects;
using AscendedZ.game_object;
using AscendedZ.screens.back_end_screen_scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

public partial class FusionScreen : CenterContainer
{
	private PartyMemberDisplay _displayFusion, _material1, _material2;
	private ItemList _possibleFusionList;
	private Button _fuseButton;
	private FusionScreenObject _fusionScreenObject;
	private int _selectedIndex = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_displayFusion = this.GetNode<PartyMemberDisplay>("%FusionResult");
        _material1 = this.GetNode<PartyMemberDisplay>("%Material1");
        _material2 = this.GetNode<PartyMemberDisplay>("%Material2");
		_possibleFusionList = this.GetNode<ItemList>("%PossibleFusionList");
        _fuseButton = this.GetNode<Button>("%FuseButton");

		Button backButton = this.GetNode<Button>("%BackButton");
		backButton.Pressed += () => { this.QueueFree(); };

		_fusionScreenObject = FusionScreenObject.Instance();

		_possibleFusionList.ItemSelected += (long selected) => 
		{ 
			_selectedIndex = (int)selected;
			DisplayFusion();
        };

		_selectedIndex = 0;
        PopulatePossibleFusionList();
    }

	/// <summary>
	/// For filling out the Fusion list if there were any changes to it.
	/// </summary>
	private void PopulatePossibleFusionList()
	{
		_fusionScreenObject.PopulateMaterialFusionList();

		_possibleFusionList.Clear();

        foreach (var fusion in _fusionScreenObject.Fusions)
        {
			var member = fusion.Fusion;
            _possibleFusionList.AddItem(member.DisplayName, CharacterImageAssets.GetTextureForItemList(member.Image));
        }

		DisplayFusion();
    }

	/// <summary>
	/// For displaying a specific fusion on screen + its materials
	/// </summary>
	/// <param name="index"></param>
	private void DisplayFusion()
	{
		var fusion = _fusionScreenObject.Fusions[_selectedIndex];

		var fusionHolder = new EntityUIWrapper { Entity = fusion.Fusion };
		var mat1Holder = new EntityUIWrapper { Entity = fusion.Material1 };
		var mat2Holder = new EntityUIWrapper { Entity = fusion.Material2 };

		_displayFusion.ShowRandomEntity(fusionHolder);
        _material1.ShowRandomEntity(mat1Holder);
		_material2.ShowRandomEntity(mat2Holder);
    }
}
