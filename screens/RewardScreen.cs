using AscendedZ;
using AscendedZ.currency;
using AscendedZ.currency.rewards;
using AscendedZ.game_object;
using Godot;
using System;
using System.Collections.Generic;

public partial class RewardScreen : Control
{
	private ItemList _rewardsList;
	private Button _claimRewardsButton;
	private List<Currency> _rewards;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rewardsList = this.GetNode<ItemList>("%RewardList");
        _claimRewardsButton = this.GetNode<Button>("%ClaimButton");

		int tier = PersistentGameObjects.GameObjectInstance().Tier;
        _rewards = new List<Currency>() 
        {
            new Vorpex() { Amount = tier },
            new PartyCoin() { Amount = 1 }
        };

        foreach (Currency reward in _rewards)
        {
            string rewardString = $"{reward.Name} x{reward.Amount}";
            _rewardsList.AddItem(rewardString, SkillAssets.GenerateIcon(reward.Icon));
        }

        _claimRewardsButton.Pressed += _OnClaimRewardsPressed;
    }

	private void _OnClaimRewardsPressed()
	{
        var currency = PersistentGameObjects.GameObjectInstance().MainPlayer.Wallet.Currency;
        foreach (Currency reward in _rewards)
        {
            if (currency.ContainsKey(reward.Name))
            {
                currency[reward.Name].Amount += reward.Amount;
            }
            else
            {
                currency.Add(reward.Name, reward);
            }
        }

        this.QueueFree();
	}

}
