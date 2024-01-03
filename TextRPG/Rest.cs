using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    internal class Rest
    {
        public void DoRest()
        {

        }

        public void EnterRestRoom(Character character)
        {
            while (true)
            {
                Console.Clear();
                DrawRestRoom(character);
                int input;
                bool isAvailableInput;
                do
                {
                    isAvailableInput = true;
                    Console.Write(">> ");

                    string? s;
                    s = Console.ReadLine();
                    if (!int.TryParse(s, out input))
                    {
                        input = -1;
                    }

                    switch (input)
                    {
                        case 0:
                            return;
                        case 1:
                            if(character.Gold < 500)
                            {
                                Console.WriteLine("Gold가 부족합니다.");
                                isAvailableInput = false;
                            }
                            else
                            {
                                character.Health = character.MaxHealth;
                                character.Gold -= 500;
                            }
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            isAvailableInput = false;
                            break;
                    }
                } while (!isAvailableInput);
            }
        }

        public void DrawRestRoom(Character character)
        {
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {character.Gold} G, 현재 체력 : {character.Health})");
            Console.WriteLine();
            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
        }
    }
}
