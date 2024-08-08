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

    private const int REWARD_MULTIPLIER = 15;

    private Random _rand;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_rewardsList = this.GetNode<ItemList>("%RewardList");
        _claimRewardsButton = this.GetNode<Button>("%ClaimButton");
        _claimRewardsButton.Pressed += _OnClaimRewardsPressed;
        _gameObject = PersistentGameObjects.GameObjectInstance();

        _rand = new Random();

        _claimRewardsButton.Text = $"[{Controls.GetControlString(Controls.CONFIRM)}] {_claimRewardsButton.Text}";
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(Controls.CONFIRM))
        {
            _OnClaimRewardsPressed();
        }
    }

    public void InitializeSMTRewards()
    {
        int tier = _gameObject.Tier;
        int startingValue = 7;
        if(tier > TierRequirements.TIER6_STRONGER_ENEMIES)
            startingValue += Equations.GetTierIndexBy10(tier);

        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * startingValue },
            new PartyCoin() { Amount = tier }
        };
        SetupRewards();
    }

    public void InitializeDungeonCrawlEncounterRewards()
    {
        int tier = _gameObject.TierDC;
        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * REWARD_MULTIPLIER },
            new PartyCoin() { Amount = (tier/3) + 1 },
            new Dellencoin() { Amount = 2 * tier },
        };
        SetupRewards();
    }

    public void InitializeDungeonCrawlEncounterSpecialRewards()
    {
        int tier = _gameObject.TierDC;
        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * (REWARD_MULTIPLIER + 3) },
            new PartyCoin() { Amount = (tier/2) + 1 },
            new Dellencoin() { Amount = 4 * tier },
        };
        SetupRewards();
    }

    public void InitializeDungeonCrawlEncounterSpecialBossRewards()
    {
        int tier = _gameObject.TierDC;
        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * (REWARD_MULTIPLIER + 5) },
            new PartyCoin() { Amount = (int)(tier * 1.5) },
            new Dellencoin() { Amount = 8 * tier },
        };
        SetupRewards();
    }

    public void InitializeDungeonCrawlTierRewards()
    {
        int tier = _gameObject.TierDC;
        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * (REWARD_MULTIPLIER + 2) },
            new Dellencoin() { Amount = tier * 5 },
            new PartyCoin() { Amount = (tier/4) + 1 }
        };
        SetupRewards();
    }

    public void RandomizeDungeonCrawlRewards()
    {
        int tier = _gameObject.TierDC;
        var rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * (REWARD_MULTIPLIER - 2) },
            new Dellencoin() { Amount = 3 * tier },
            new PartyCoin() { Amount = (tier/6) + 1 }
        };

        _rewards = new List<Currency>() { rewards[_rand.Next(0, rewards.Count)] };
        SetupRewards();
    }

    public void InitializeDungeonCrawlSpecialItems()
    {
        int tier = _gameObject.TierDC;

        _rewards = new List<Currency>()
        {
            new Vorpex() { Amount = tier * (REWARD_MULTIPLIER - 1) },
            new Dellencoin() { Amount = 5 * tier },
            new PartyCoin() { Amount = (tier / 4) + 1 }
        };

        SetupRewards();
    }

    public void InitializePotOfGreedRewards()
    {
        var currency = _gameObject.MainPlayer.Wallet.Currency;
        var keyShard = new KeyShard() { Amount = 1 };

        if (!currency.ContainsKey(keyShard.Name))
            currency.Add(keyShard.Name, new KeyShard() { Amount = 0 });

        if (currency[keyShard.Name].Amount + 1 >= 4)
        {
            var bountyKey = new BountyKey() { Amount = 1 };

            if (!currency.ContainsKey(bountyKey.Name))
                currency.Add(bountyKey.Name, new BountyKey() { Amount = 0 });

            currency[keyShard.Name].Amount -= 3;

            _rewards = new List<Currency>() { bountyKey };
        }
        else
        {
            _rewards = new List<Currency>() { keyShard };
        }
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
