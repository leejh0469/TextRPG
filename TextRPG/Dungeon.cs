using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    public struct DungeonVariable
    {
        public int Damage { get; set; }
        public int Reward { get; set; }
        public int RecommendDefense { get; set; }

        public int DungeonEXP { get; set; }

        public string Name { get; set; }
    }

    internal class Dungeon
    {
        public void EnterDungeon(Character character)
        {
            while (true)
            {
                Console.Clear();
                DrawDungeonEntrance();
                int input;
                bool isAvailableInput = true;
                do
                {
                    isAvailableInput = true;
                    Console.Write(">> ");

                    string? s = Console.ReadLine();
                    if (!int.TryParse(s, out input))
                    {
                        input = -1;
                    }

                    switch (input)
                    {
                        case 0:
                            return;
                        case 1:
                            Enter(Difficulty.Easy, character);
                            break;
                        case 2:
                            Enter(Difficulty.Normal, character);
                            break;
                        case 3:
                            Enter(Difficulty.Hard, character);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            isAvailableInput = false;
                            break;
                    }
                } while (!isAvailableInput);
            }


        }

        public void DrawDungeonEntrance()
        {
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
        }

        public void Enter(Difficulty difficulty, Character character)
        {
            Random random = new Random();
            DungeonVariable dungeon = new DungeonVariable();
            dungeon.Damage = random.Next(20, 36);

            switch (difficulty)
            {
                case Difficulty.Easy:
                    dungeon.Reward = 1000;
                    dungeon.RecommendDefense = 5;
                    dungeon.Name = "쉬운 던전";
                    dungeon.DungeonEXP = 1;
                    break;
                case Difficulty.Normal:
                    dungeon.Reward = 1700;
                    dungeon.RecommendDefense = 11;
                    dungeon.Name = "보통 던전";
                    dungeon.DungeonEXP = 1;
                    break;
                case Difficulty.Hard:
                    dungeon.Reward = 2500;
                    dungeon.RecommendDefense = 17;
                    dungeon.Name = "어려운 던전";
                    dungeon.DungeonEXP = 1;
                    break;
                default:
                    break;
            }

            dungeon.Damage += dungeon.RecommendDefense - character.Defense;

            bool clear = IsClear(dungeon.RecommendDefense, character.Defense, random);

            if(clear)
            {
                int additional = random.Next(character.Attack, character.Attack * 2 + 1);
                dungeon.Reward += dungeon.Reward * additional / 100;
                Clear(character, dungeon);
            }
            else
            {
                Defeat(character, dungeon);
            }
        }

        bool IsClear(int dungeonDefense, int myDefense, Random rand)
        {
            if(dungeonDefense <= myDefense)
            {
                return true;
            }
            else
            {
                int randomValue = rand.Next(0, 10);

                if (randomValue > 4) return false;
                else return true;
            }
        }

        public void Clear(Character character, DungeonVariable dungeon)
        {
            Console.Clear();
            Console.WriteLine("축하합니다!!");
            Console.WriteLine($"{dungeon.Name}을 클리어 하였습니다.");
            Console.WriteLine();
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {character.Health} -> {character.Health - dungeon.Damage}");
            Console.WriteLine($"Gold {character.Gold} -> {character.Gold + dungeon.Reward}");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            character.Health -= dungeon.Damage;
            character.Gold += dungeon.Reward;
            character.AddExp(dungeon.DungeonEXP);

            int input;
            while (true)
            {
                Console.Write(">> ");
                string? s = Console.ReadLine();
                if(!int.TryParse(s, out input))
                {
                    input = -1;
                }

                if(input == 0)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }
            }
        }

        public void Defeat(Character character, DungeonVariable dungeon)
        {
            Console.Clear();
            Console.WriteLine($"{dungeon.Name}을 클리어 하지 못했습니다.");
            Console.WriteLine();
            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"체력 {character.Health} -> {character.Health - dungeon.Damage}");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            character.Health -= dungeon.Damage;

            int input;
            while (true)
            {
                Console.Write(">> ");
                string? s = Console.ReadLine();
                if (!int.TryParse(s, out input))
                {
                    input = -1;
                }

                if (input == 0)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }
            }
        }
    }
}
