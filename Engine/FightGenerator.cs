using DivineMonad.Engine.Raport;
using DivineMonad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DivineMonad.Engine
{
    public class FightGenerator : IFightGenerator
    {
        Random rand;
        private AdvanceStats Attacker { get; set; }
        private AdvanceStats Defender { get; set; }
        public RaportGenerator Raport { get; set; }
        public IEnumerable<Rarity> Rarities { get; set; }
        public Monster Monster { get; set; }
        public IEnumerable<Item> Items { get; set; }
        public Item ItemLooted { get; set; }
        public int AttackerId { get; set; }
        public int DefenderId { get; set; }
        public int AttackerHp { get; set; }
        public int DefenderHp { get; set; }
        public bool IsCrit { get; set; }
        public bool IsBlock { get; set; }
        public bool IsMiss { get; set; }
        public int Damage { get; set; }
        public int Receive { get; set; }
        public int RoundNumber { get; set; }
        public bool IsExtraAttackDone { get; set; }
        public bool DoExtraAttack { get; set; }
        public bool IsFightOver { get; set; }
        public bool IsHalfRound { get; set; }


        private readonly IAdvanceStats _playerStats;
        private readonly IAdvanceStats _opponentStats;
        private readonly IRarityRepo _rarityRepo;

        public FightGenerator(IAdvanceStats playerStats, IAdvanceStats opponentStats)
        {
            rand = new Random();
            Raport = new RaportGenerator();
            Raport.Rounds = new List<Round>();
            _playerStats = playerStats;
            _opponentStats = opponentStats;

        }

        public FightGenerator(IAdvanceStats playerStats,
            IAdvanceStats opponentStats,
            Monster monster,
            IEnumerable<Item> items,
            IRarityRepo rarityRepo)
        {
            rand = new Random();
            Raport = new RaportGenerator
            {
                Rounds = new List<Round>(),
                Reward = new Reward()
            };
            Monster = monster;
            Items = items;
            _rarityRepo = rarityRepo;
            _playerStats = playerStats;
            _opponentStats = opponentStats;
        }

        public async Task<RaportGenerator> GenerateFight()
        {
            Rarities = await _rarityRepo.GetRaritiesList();

            IsExtraAttackDone = false;
            DoExtraAttack = false;
            IsFightOver = false;
            IsHalfRound = true;
            RoundNumber = 1;

            Action fightAction = new Action(SetMainRaportProps);
            fightAction += SetWhoStart;
            fightAction += SetStartHp;

            fightAction += UpdateBonuses;
            fightAction += UpdateDamage;
            fightAction += UpdateReceiveDamage;
            fightAction += AddRaportRound;
            fightAction += CheckIfDoubleAttack;
            fightAction += CheckIfFightOver;
            fightAction += UpdateToNextRound;

            fightAction();

            fightAction -= SetWhoStart;
            fightAction -= SetStartHp;

            while (!IsFightOver)
            {
                if (RoundNumber >= 20) IsFightOver = true;

                fightAction();
            }

            SetRaportResults();
            SetRewards();

            return Raport;
        }

        private void SetMainRaportProps()
        {
            Raport.IsPvp = _opponentStats.IsPlayer;
            Raport.Player = new Player() 
            {
                ID = _playerStats.CharacterId,
                Name = _playerStats.CharacterName
            };

            Raport.Opponent = new Opponent() 
            { 
                ID = _opponentStats.CharacterId,
                Name = _opponentStats.CharacterName
            };

        }

        private void SetWhoStart()
        {
            if (_playerStats.Speed >= _opponentStats.Speed)
            {
                AttackerId = _playerStats.CharacterId;
                Attacker = (AdvanceStats)_playerStats;
                DefenderId = _opponentStats.CharacterId;
                Defender = (AdvanceStats)_opponentStats;
            }
            else
            {
                AttackerId = _opponentStats.CharacterId;
                Attacker = (AdvanceStats)_opponentStats;
                DefenderId = _playerStats.CharacterId;
                Defender = (AdvanceStats)_playerStats;
            }
        }

        private void UpdateBonuses()
        {

            if (rand.NextDouble() <= Attacker.CritPr) IsCrit = true;
            else IsCrit = false;

            if (rand.NextDouble() <= Defender.DodgePr) IsMiss = true;
            else IsMiss = false;

            if (rand.NextDouble() <= Defender.BlockPr) IsBlock = true;
            else IsBlock = false;
        }

        private void SetStartHp()
        {
            AttackerHp = Attacker.HitPoints;
            DefenderHp = Defender.HitPoints;
        }

        private void UpdateDamage()
        {
            Damage = rand.Next(Attacker.AttackMin, Attacker.AttackMax);

            if (IsCrit) Damage = (int)(Damage * 1.5);
        }

        private void UpdateReceiveDamage()
        {
            if (IsMiss) Receive = 0;
            else
            {
                Receive = (int)(Damage * (1 - Defender.DmgRed));
                if (IsBlock) Receive = (int)(Receive * 0.5);
            }

            DefenderHp -= Receive;
        }

        private void AddRaportRound()
        {
            Raport.Rounds.Add(new Round()
            {
                Attacker = new Attacker()
                {
                    Crit = IsCrit,
                    Damage = Damage,
                    HP = AttackerHp,
                    Name = Attacker.CharacterName,
                    Miss = IsMiss
                },
                Defender = new Defender() 
                { 
                    Block = IsBlock,
                    Receive = Receive,
                    HP = DefenderHp,
                    Name = Defender.CharacterName
                },
                Number = RoundNumber
            });
        }

        private void CheckIfDoubleAttack()
        {
            double speedFactor = (double)Attacker.Speed / Defender.Speed;

            if (speedFactor > 2) speedFactor = 2;
            if (speedFactor > 1)
            {
                double extraAttack = ((((double)RoundNumber / speedFactor) -
                    (int)((double)RoundNumber / speedFactor))
                    / Math.Pow(speedFactor, 1.2)) + 0.04;

                if (extraAttack <= 0.25 && IsExtraAttackDone == false) 
                    DoExtraAttack = true;
                else if (extraAttack > 0.25) IsExtraAttackDone = false;
            }
        }

        private void UpdateToNextRound()
        {
            if (!IsFightOver)
            {
                if (IsHalfRound) IsHalfRound = false;
                else
                {
                    IsHalfRound = true;
                    RoundNumber += 1;
                }

                if (DoExtraAttack)
                {
                    IsExtraAttackDone = true;
                    DoExtraAttack = false;
                }
                else
                {
                    AdvanceStats tempA, tempB;
                    int hpA, hpB, idA, idB;
                    tempA = Attacker;
                    tempB = Defender;
                    hpA = AttackerHp;
                    hpB = DefenderHp;
                    idA = AttackerId;
                    idB = DefenderId;
                    Attacker = tempB;
                    Defender = tempA;
                    AttackerHp = hpB;
                    DefenderHp = hpA;
                    AttackerId = idB;
                    DefenderId = idA;
                }
            }
        }

        private void CheckIfFightOver()
        {
            if (DefenderHp <= 0) IsFightOver = true;
        }
        private void SetRaportResults()
        {
            if (DefenderHp > 0) Raport.Result = "draw";
            else
            {
                if (Defender.CharacterId == _playerStats.CharacterId &&
                    Defender.CharacterName == _playerStats.CharacterName)
                    Raport.Result = "lose";
                else Raport.Result = "win";
            }
        }

        private void SetRewards()
        {
            int index = 0, count = 0;
            var gachiaDraw = rand.NextDouble() * 1000;

            if (!_opponentStats.IsPlayer)
            {
                if (Raport.Result == "win")
                {
                    Raport.Reward.Experience = Monster.Experience;
                    Raport.Reward.Gold = Monster.Gold;
                    Raport.Reward.ItemID = -1;

                    foreach (var rarity in Rarities.OrderBy(r => r.Chance))
                    {
                        if (gachiaDraw <= rarity.Chance * (1 + _playerStats.ExtraDropPr))
                        {
                            count = Items.Where(i => i.Rarity.Name == rarity.Name).Count();
                            if (count > 0)
                            {
                                index = rand.Next(0, count);
                                ItemLooted = Items
                                    .Where(i => i.Rarity.Name == rarity.Name)
                                    .ElementAt(index);
                            }
                            break;
                        }
                    }

                    if (!(ItemLooted is null))
                    {
                        Raport.Reward.ItemID = ItemLooted.ID;
                        Raport.Reward.ItemName = ItemLooted.Name;
                        Raport.Reward.ItemRarity = ItemLooted.Rarity.Name;
                    }
                }
                else
                {
                    Raport.Reward.Gold = 0;
                    Raport.Reward.Experience = 0;
                }
            }
        }
    }
}
