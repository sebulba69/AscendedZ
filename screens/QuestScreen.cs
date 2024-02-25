using AscendedZ;
using AscendedZ.game_object;
using AscendedZ.game_object.quests;
using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

public partial class QuestScreen : CenterContainer
{
	private RichTextLabel _questDescription;
	private ItemList _questList;
	private GameObject _gameObject;
	private int _selected;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _questDescription = this.GetNode<RichTextLabel>("%QuestDescription");
        _questList = this.GetNode<ItemList>("%QuestList");
		_gameObject = PersistentGameObjects.GameObjectInstance();
		_selected = 0;

		_questList.ItemSelected += (selected) => 
		{ 
			_selected = (int)selected;
            DisplaySelected();
            PopulateQuestList();
        };

		_questList.ItemClicked += (index, at_position, mouse_button_index) =>
		{
			if (mouse_button_index == (long)MouseButton.Right)
			{
				var quest = _gameObject.QuestObject.GetQuests()[(int)index];
				quest.Save = !quest.Save;
				PopulateQuestList();
			}
		};

        Button completeButton = this.GetNode<Button>("%CompleteButton");
		completeButton.Pressed += _OnQuestCompleteButtonPressed;

		Button rerollButton = this.GetNode<Button>("%ReRollButton");
		rerollButton.Pressed += () => 
		{
            QuestObject questObject = _gameObject.QuestObject;
			questObject.Clear();
			PopulateQuestList();
        };

        Button backButton = this.GetNode<Button>("%BackButton");
		backButton.Pressed += () => { this.QueueFree(); };

		PopulateQuestList();
    }

	private void PopulateQuestList()
	{
		_questList.Clear();

		QuestObject questObject = _gameObject.QuestObject;
        questObject.GenerateQuests(_gameObject.MaxTier);
        questObject.CheckDeliveryQuestConditions(_gameObject.MainPlayer.ReserveMembers);

        List<Quest> quests = questObject.GetQuests();

		foreach(Quest quest in quests)
		{
			string name = quest.GetQuestNameString();

			if (quest.Completed)
			{
                name = $"{name} [COMPLETED]";
            }

			if (quest.Save)
			{
				name = $"{name} [SAVED]";
			}

            _questList.AddItem(name, quest.GetIcon());
		}

		if (_selected >= _questList.ItemCount)
			_selected = _questList.ItemCount - 1;

        _questList.Select(_selected);

		DisplaySelected();
    }

	private void DisplaySelected()
	{
        QuestObject questObject = _gameObject.QuestObject;
        List<Quest> quests = questObject.GetQuests();
        _questDescription.Text = quests[_selected].ToString();
    }

	private void _OnQuestCompleteButtonPressed()
	{
        QuestObject questObject = _gameObject.QuestObject;
        List<Quest> quests = questObject.GetQuests();
		Quest quest = quests[_selected];

		if (quest.Completed)
		{
			var wallet = _gameObject.MainPlayer.Wallet;
			wallet.Currency[SkillAssets.VORPEX_ICON].Amount += quest.VorpexReward;
			questObject.Complete(_selected);

			PopulateQuestList();
		}
    }
}
