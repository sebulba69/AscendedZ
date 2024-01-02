using AscendedZ.game_object;
using Godot;
using System;

public partial class RecruitScreenTabs : CenterContainer
{
	private const int MEMBER_REQUEST_FORUM = 1;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TabContainer tabContainer = this.GetNode<TabContainer>("%TabContainer");
		tabContainer.SetTabHidden(MEMBER_REQUEST_FORUM, true);

		GameObject gameObject = PersistentGameObjects.GameObjectInstance();


		var recruitShop = tabContainer.GetNode("%Recruit Shop");
		var memberRequestForum = tabContainer.GetNode("%Member Request Forum");

		var backButtonRecruitShop = recruitShop.GetNode<Button>("%BackButton");
		backButtonRecruitShop.Pressed += _OnBackButtonPressed;

		if(gameObject.MaxTier > 10)
		{
			tabContainer.SetTabHidden(MEMBER_REQUEST_FORUM, false);
            var backButtonForum = memberRequestForum.GetNode<Button>("%BackButton");
            backButtonForum.Pressed += _OnBackButtonPressed;
        }
	}

    private void _OnBackButtonPressed()
    {
        this.QueueFree();
    }
}
