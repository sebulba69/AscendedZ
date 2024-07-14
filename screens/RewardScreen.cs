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
    private GameObject _gameObject;

    private Random _rand;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_rewardsList = this.GetNode<ItemList>("%RewardList");
        _claimRewardsButton = this.GetNode<Button>("%ClaimButton");
        _claimRewardsButton.Pressed += _OnClaimRewardsPressed;
        _gameObject = PersistentGameObjects.GameObjectInstance();

        _rand = new Random();
    }

    public void InitializeSMTRewards()
    {
        int tier = _gameObject.Tier;
        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * 5 },
            new PartyCoin() { Amount = 1 }
        };
        SetupRewards();
    }

    public void InitializeDungeonCrawlEncounterRewards()
    {
        int tier = _gameObject.TierDC;
        _rewards = new List<Currency>()
        {
            new Vorpex() {Amount = tier + 5 },
            new PartyCoin() { Amount = 1 },
            new Dellencoin() { Amount = 10 },
        };
        SetupRewards();
    }

    public void InitializeDungeonCrawlTierRewards()
    {
        int tier = _gameObject.TierDC;
        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier + 3 },
            new Dellencoin() { Amount = 20 },
            new PartyCoin() { Amount = 5 }
        };
        SetupRewards();
    }

    public void RandomizeDungeonCrawlRewards()
    {
        int tier = _gameObject.TierDC;
        var rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier + 3 },
            new Dellencoin() { Amount = 20 },
            new PartyCoin() { Amount = 5 }
        };

        _rewards = new List<Currency>() { rewards[_rand.Next(0, rewards.Count)] };
        SetupRewards();
    }

    private void SetupRewards()
    {
        foreach (Currency reward in _rewards)
        {
            string rewardString = $"{reward.Name} x{reward.Amount}";
            _rewardsList.AddItem(rewardString, SkillAssets.GenerateIcon(reward.Icon));
        }
    }

    private void _OnClaimRewardsPressed()
	{
        var currency = _gameObject.MainPlayer.Wallet.Currency;
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
