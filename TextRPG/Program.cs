namespace TextRPG
{
    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    };

    public struct Character
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }

        public Character(int level, string name, int attack, int defense, int health, int gold)
        {
            Level = level;
            Name = name;
            Attack = attack;
            Defense = defense;
            Health = health;
            Gold = gold;
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public Item(string name, string description, int price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

    }

    public class Weapon : Item
    {
        public int Attack { get; set; }
        public Weapon(string name, string description, int price, int attack) : base(name, description, price)
        {
            Attack = attack;
        }
    }

    public class Armor : Item
    {
        public int Defense { get; set; }

        public Armor(string name, string description, int price, int defense) : base(name, description, price)
        {
            Defense = defense;
        }
    }

    public class Inventory
    {
        public List<Item> items;

        public Inventory()
        {
            items = new List<Item>();
        }

        public Inventory(List<Item> items)
        {
            this.items = items;
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }
    }

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
    }

    internal class Program
    {
        // 시작 마을
        static void DrawVillage()
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
        }
        
        // 스텟
        #region
        public void MyStat(Character character)
        {
            Console.Clear();
            DrawMyStat(character);
            int input;
            bool isAvailableInput = true;
            do
            {
                Console.Write(">> ");
                input = int.Parse(Console.ReadLine());
                isAvailableInput = true;
                switch (input)
                {
                    case 0:
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        isAvailableInput = false;
                        break;
                }
            } while (!isAvailableInput);
        }

        public void DrawMyStat(Character character)
        {
            Console.WriteLine($"Lv. {character.Level:D2}");
            Console.WriteLine($"{character.Name} ( 전사 )");
            Console.WriteLine($"공격력 : {character.Attack}");
            Console.WriteLine($"방어력 : {character.Defense} ");
            Console.WriteLine($"체 력 : {character.Health}");
            Console.WriteLine($"Gold : {character.Gold} G");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
        }
        #endregion

        // 인벤토리
        #region
        public void MyInventory()
        {
            Console.Clear();

        }

        public void DrawMyInventory()
        {

        }
        #endregion

        // 상점
        #region
        public void Shop(Character character, Shop shop)
        {
            bool isBuyPage = false;
            while (true)
            {
                Console.Clear();
                DrawShop(isBuyPage, character, shop);
                if(!isBuyPage)
                {
                    int input;
                    bool isAvailableInput = true;
                    do
                    {
                        Console.Write(">> ");
                        input = int.Parse(Console.ReadLine());
                        isAvailableInput = true;
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
                    bool isAvailableInput = true;
                    while(true)
                    {
                        Console.Write(">> ");
                        input = int.Parse(Console.ReadLine());
                        if(input == 0)
                        {
                            isBuyPage = !isBuyPage;
                            break;
                        }
                        else if(input >= 1 && input <= shop.shopItems.Count)
                        {
                            if (shop.isEnoughMoney(character.Gold, input - 1))
                            {
                                Console.WriteLine("구매를 완료했습니다.");
                                shop.Buy(input - 1);
                            }
                            else
                            {
                                Console.WriteLine("Gold 가 부족합니다.");
                            }
                        }
                    }
                }
            }
            
        }

        public void DrawShop(bool isBuyPage, Character character, Shop shop)
        {
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{character.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            int i = 1;
            foreach(var item in shop.shopItems)
            {
                Console.Write("- ");
                if(isBuyPage)
                {
                    Console.Write($"{i} ");
                }
                Console.Write($"{item.First.Name}\t| ");
                if(item.First is Weapon weapon)
                {
                    Console.Write($"공격력 +{weapon.Attack}  | ");
                }
                else if(item.First is Armor armor)
                {
                    Console.Write($"방어력 +{armor.Defense}  | ");
                }
                Console.Write($"{item.First.Description}  | ");
                if (item.Second)
                    Console.Write($"{item.First.Price} G");
                else
                    Console.Write("구매완료");
                Console.WriteLine() ;
                i++;
            }

            Console.WriteLine();
            if (!isBuyPage)
                Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
        }
        #endregion

        public List<Item> InitItem()
        {
            List<Item> items = new List<Item>();
            items.Add(new Armor("수련자 갑옷", "수련에 도움을 주는 갑옷입니다.", 1000, 5));
            items.Add(new Armor("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000, 9));
            items.Add(new Armor("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, 9));
            items.Add(new Weapon("낡은 검", "어디선가 사용됐던거 같은 도끼입니다.", 600, 2));
            items.Add(new Weapon("청동 도끼", "쉽게 볼 수 있는 낡은 검 입니다.", 1500, 5));
            items.Add(new Weapon("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000, 7));

            return items;
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            Character myCharacter = new Character(1, "Chad", 10, 5, 100, 1500);
            List<Item> items = program.InitItem();
            Shop shop = new Shop(items);

            while (true)
            {
                Console.Clear();
                DrawVillage();
                int input;
                bool isAvailableInput = false;
                do
                {
                    Console.Write(">> ");
                    input = int.Parse(Console.ReadLine());
                    isAvailableInput = false;
                    switch (input)
                    {
                        case 1:
                            program.MyStat(myCharacter);
                            break;
                        case 2:
                            break;
                        case 3:
                            program.Shop(myCharacter, shop);
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            isAvailableInput = true;
                            break;
                    }
                } while (isAvailableInput);
            }
        }
    }
}
