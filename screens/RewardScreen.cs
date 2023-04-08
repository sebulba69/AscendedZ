using AscendedZ;
using AscendedZ.currency;
using Godot;
using System;

public partial class RewardScreen : Control
{
	private ItemList _rewardsList;
	private Button _claimRewardsButton;
	private Currency[] _rewards;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_rewardsList = this.GetNode<ItemList>("%RewardList");
        _claimRewardsButton = this.GetNode<Button>("%ClaimButton");

		int tier = PersistentGameObjects.Instance().Tier;
		_rewards = RewardGenerator.GenerateReward(tier);
		if(_rewards != null)
        {
            foreach (Currency reward in _rewards)
            {
                string rewardString = $"{reward.Name} x{reward.Amount}";
                _rewardsList.AddItem(rewardString, ArtAssets.GenerateIcon(reward.Icon));
            }
        }

		_claimRewardsButton.Pressed += _OnClaimRewardsPressed;
    }

	private void _OnClaimRewardsPressed()
	{
		if(_rewards != null)
        {
            var currency = PersistentGameObjects.Instance().MainPlayer.Wallet.Currency;
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
        }

		this.QueueFree();
	}

}
