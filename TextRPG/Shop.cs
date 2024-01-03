using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Shop
    {
        public List<Pair<Item, bool>> shopItems;
        public Shop(List<Item> items)
        {
            shopItems = new List<Pair<Item, bool>>();
            foreach (Item item in items)
            {
                shopItems.Add(new Pair<Item, bool>(item, true));
            }
        }

        public bool isEnoughMoney(int characterGold, int index)
        {
            return shopItems[index].First.Price <= characterGold;
        }

        public void Buy(int index)
        {
            shopItems[index].Second = false;
        }

        public bool CanBuy(int index)
        {
            return shopItems[index].Second;
        }

        public void EnterShop(Character character)
        {
            bool isBuyPage = false;
            while (true)
            {
                Console.Clear();
                DrawShop(isBuyPage, character);
                if (!isBuyPage)
                {
                    int input;
                    bool isAvailableInput = true;
                    do
                    {
                        isAvailableInput = true;
                        Console.Write(">> ");

                        string? s = Console.ReadLine();
                        if(!int.TryParse(s, out input))
                        {
                            input = -1;
                        }
                        
                        switch (input)
                        {
                            case 0:
                                return;
                            case 1:
                                isBuyPage = true;
                                break;
                            default:
                                Console.WriteLine("잘못된 입력입니다.");
                                isAvailableInput = false;
                                break;
                        }
                    } while (!isAvailableInput);
                }
                else
                {
                    int input;
                    while (true)
                    {
                        Console.Write(">> ");
                        string? s = Console.ReadLine();
                        if (!int.TryParse(s, out input))
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                            continue;
                        }

                        if (input == 0)
                        {
                            isBuyPage = !isBuyPage;
                            break;
                        }
                        else if (input >= 1 && input <= shopItems.Count)
                        {
                            if (isEnoughMoney(character.Gold, input - 1))
                            {
                                if(CanBuy(input - 1))
                                {
                                    Console.WriteLine("구매를 완료했습니다.");
                                    Buy(input - 1);
                                    character.Gold -= shopItems[input - 1].First.Price;
                                    character.AddItem(shopItems[input - 1].First);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("이미 구매한 아이템입니다.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Gold 가 부족합니다.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                }
            }

        }

        public void DrawShop(bool isBuyPage, Character character)
        {
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{character.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            int i = 1;
            foreach (var item in shopItems)
            {
                Console.Write("- ");
                if (isBuyPage)
                {
                    Console.Write($"{i} ");
                }
                Console.Write($"{item.First.Name}\t| ");
                if (item.First is Weapon weapon)
                {
                    Console.Write($"공격력 +{weapon.Attack}  | ");
                }
                else if (item.First is Armor armor)
                {
                    Console.Write($"방어력 +{armor.Defense}  | ");
                }
                Console.Write($"{item.First.Description}  | ");
                if (item.Second)
                    Console.Write($"{item.First.Price} G");
                else
                    Console.Write("구매완료");
                Console.WriteLine();
                i++;
            }

            Console.WriteLine();
            if (!isBuyPage)
                Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
        }
    }
}
