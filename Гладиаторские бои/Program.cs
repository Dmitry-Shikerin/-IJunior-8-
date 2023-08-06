﻿using System;
using System.Collections.Generic;

namespace Гладиаторские_бои
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new Arena();

            arena.Work();
        }
    }

    abstract class Fighter
    {
        protected int Armor;

        public Fighter(string name, int health, int armor, int damage)         
        {  
            Name = name;
            Health = health;                                      
            Armor = armor;
            Damage = damage;
            IsAlive = true;
        }

        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public bool IsAlive { get; private set; }                 
        public int Damage { get; private set; }                   

        public virtual void TakeDamage(int damage)
        {
            if(Armor > damage)
            {
                damage = Armor;
            }

            Health -= damage - Armor;

            if (Health <= 0)                                     
            {
                Health = 0;
                IsAlive = false;
            }
        }

        public virtual void Attack(Fighter enemy)
        {
            int currentDamege = DealDamage();
            enemy.TakeDamage(currentDamege);
        }

        public virtual int DealDamage()
        {
            return Damage;
        }

        public virtual void ShowInfo()
        {
            Console.Write($"Здоровье: {Health}, Броня: {Armor}, Урон: {Damage}");
        }
    }

    class Warriour : Fighter
    {
        public int _rage = 0;                                           

        public Warriour() : base("Воин", 150, 12, 30) { }

        public override int DealDamage()
        {
            int ragePerBloW = 7;
            int ragePerFuriousBlow = 20;
            int damagePerFurriousBlow = 10;

            Console.WriteLine($"Воин накапливает {ragePerBloW} ярости");              
            _rage += ragePerBloW;                                                   

            if (_rage >= ragePerFuriousBlow)
            {
                Console.WriteLine("Воин наносит яростный удар");         
                _rage -= ragePerFuriousBlow;

                return Damage + damagePerFurriousBlow;
            }

            return Damage;
        }

        public override void ShowInfo()
        {
            Console.WriteLine("Воин");
            base.ShowInfo();
            Console.WriteLine($", Ярости: {_rage}");
        }
    }

    class DeathKnight : Fighter
    {
        public DeathKnight() : base("Рыцарь смерти", 150, 15, 25) { }

        public override void TakeDamage(int damage)
        {
            int minValueSlope = 1;
            int maxValueSlope = 100;
            int slopeValue = 25;

            int slope = Utils.GetRandomValue(minValueSlope, maxValueSlope);

            if (slope <= slopeValue)
            {
                Console.WriteLine("Рыцарь Смерти уклонился");
                return;
            }

            base.TakeDamage(damage);
        }

        public override void ShowInfo()
        {
            Console.WriteLine("Рыцарь смерти");
            base.ShowInfo();
        }
    }

    class Priest : Fighter
    {
        private int _mana = 20;

        public Priest() : base("Жрец", 110, 8, 33) { }

        public override void TakeDamage(int damage)
        {
            int manaPerPrayerRestoration = 20;
            int healthPerPrayerRestoration = 25;

            base.TakeDamage(damage);

            if (_mana >= manaPerPrayerRestoration)
            {
                Health += healthPerPrayerRestoration;
                Console.WriteLine($"Жрец восстанавливает {healthPerPrayerRestoration} здоровья");
                _mana -= manaPerPrayerRestoration;
            }
        }

        public override int DealDamage()
        {
            int manaPerBlow = 8;

            _mana += manaPerBlow;
            Console.WriteLine($"Жрец восстанавливает {manaPerBlow} маны");
            return Damage;
        }

        public override void ShowInfo()
        {
            Console.WriteLine("Жрец");
            base.ShowInfo();
            Console.WriteLine($", Мана: {_mana}");
        }
    }

    class Warlock : Fighter
    {
        private int _mana = 20;

        public Warlock() : base("Чернокнижник", 105, 8, 35) { }

        public override int DealDamage()
        {
            int manaPerBlow = 8;
            int manaPerBurnOfFilth = 20;
            int damagePerBurnOfFilt = 25;

            _mana += manaPerBlow;

            Console.WriteLine($"Чернокнижник восстанавливает {manaPerBlow} маны");

            if (_mana >= manaPerBurnOfFilth)
            {
                Console.WriteLine("Чернокнижник использует ожог скверны");
                return Damage + damagePerBurnOfFilt;
            }

            return Damage;
        }

        public override void ShowInfo()
        {
            Console.WriteLine("Чернокнижник");
            base.ShowInfo();
            Console.WriteLine($", Мана: {_mana}");
        }
    }

    class Magician : Fighter
    {
        private int _mana = 20;
        private int _iceArmor = 0;

        public Magician() : base("Маг", 100, 8, 40) { }

        public override void TakeDamage(int damage)
        {
            int damagePerIceArmor = 7;

            if (_iceArmor > 0)
            {
                _iceArmor--;
                damage -= damagePerIceArmor;
                Console.WriteLine($"Маг блокирует {damagePerIceArmor} урона");
            }

            base.TakeDamage(damage);
        }

        public override int DealDamage()
        {
            int manaPerBlow = 8;
            int manaPerIceArmor = 15;
            int stacsPerIceArmor = 4;
            int manaPerFayerBoll = 20;
            int damagePerFayerBoll = 20;

            _mana += manaPerBlow;

            Console.WriteLine("Маг восстанавливаееет 8 маны");

            if (_mana >= manaPerIceArmor && _iceArmor == 0)
            {
                _iceArmor = stacsPerIceArmor;
                _mana -= manaPerIceArmor;
                Console.WriteLine("Маг активирует Ледяной доспех");
            }

            if (_mana >= manaPerFayerBoll && _iceArmor > 0)
            {
                Console.WriteLine($"Маг использует огненный шар и наносит {Damage + damagePerFayerBoll} урона");
                _mana -= manaPerFayerBoll;
                return Damage + damagePerFayerBoll;
            }

            return Damage;
        }

        public override void ShowInfo()
        {
            Console.WriteLine("Маг");
            base.ShowInfo();
            Console.WriteLine($", Мана: {_mana}, Заряды Ледяного доспеха: {_iceArmor}.");
        }
    }

    class Arena
    {
        public void Work()
        {
            Fighter fighter1;
            Fighter fighter2;

            ChooseCharacter(out fighter1, out fighter2);
            StartBattle(fighter1, fighter2);
            ShowResults(fighter1, fighter2);
        }

        private void ChooseCharacter(out Fighter fighter1, out Fighter fighter2)
        {
            do
            {
                Console.WriteLine("Выберете первого бойца");
                fighter1 = CreateWarrior();
                Console.WriteLine("Выберите второго бойца");
                fighter2 = CreateWarrior();

            } while (fighter1 == null || fighter2 == null);
        }

        private void ShowCombatLog(Fighter fighter1, Fighter fighter2)
        {
            Console.WriteLine($"Здороввье первого игока {fighter1.Health},\n" +  
                              $"Здоровье второго игрока {fighter2.Health}");
            Console.WriteLine();
        }

        private void ShowResults(Fighter fighter1, Fighter fighter2)
        {
            if (fighter1.Health == 0 && fighter2.Health == 0)
            {
                Console.WriteLine("Ничья");
            }
            else if (fighter1.Health > 0)
            {
                Console.WriteLine("Игрок 1 победил");
            }
            else if (fighter2.Health > 0)
            {
                Console.WriteLine("Игрок 2 победил");
            }

            Console.ReadKey();
        }

        private void StartBattle(Fighter fighter1, Fighter fighter2)
        {
            while (fighter1.IsAlive && fighter2.IsAlive)
            {
                fighter1.ShowInfo();
                Console.WriteLine();
                fighter2.ShowInfo();
                Console.WriteLine();

                fighter1.Attack(fighter2);
                fighter2.Attack(fighter1);

                Console.WriteLine();
                ShowCombatLog(fighter1, fighter2);
                Console.ReadKey();
            }
        }

        private Fighter CreateWarrior()
        {
            List<Fighter> fighters = new List<Fighter>() 
            { 
                new Warriour(),
                new DeathKnight(),
                new Priest(),
                new Warlock(),
                new Magician()
            };

            for (int i = 0; i < fighters.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {fighters[i].Name}");
            }

            int characterIndex = ReadNumber() - 1;

            if ( characterIndex < 0 || characterIndex >= fighters.Count)
            {
                Console.WriteLine("Такого персонажа не существует");
                return null;
            }

            return fighters[characterIndex];
        }

        private int ReadNumber()
        {
            int number;

            while (int.TryParse(Console.ReadLine(), out number) == false)
            {
                Console.WriteLine("Ошибка. Введите число.");
            }

            return number;
        }
    }

    public static class Utils
    {
        private static Random _random = new Random();

        public static int GetRandomValue(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
