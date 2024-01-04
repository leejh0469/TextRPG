using Newtonsoft.Json.Linq;

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

    public class Character
    {
        public int Level { get; set; }
        public string Name { get; set; }

        public int _attack;
        public int Attack
        {
            get
            {
                return (MyWeapon != null)? _attack + MyWeapon.Attack : _attack;
            }
            set
            {
                _attack = value;
            }
        }

        public int _defense;
        public int Defense
        {
            get
            {
                return (MyArmor != null)? _defense + MyArmor.Defense : _defense;
            }
            set
            {
                _defense = value;
            }
        }

        public int Health { get; set; }

        public int MaxHealth { get; private set; }
        public int Gold { get; set; }
        public Weapon? MyWeapon { get; set; }
        public Armor? MyArmor { get; set; }

        public List<Item> items;

        public Character(int level, string name, int attack, int defense, int health, int gold)
        {
            Level = level;
            Name = name;
            Attack = attack;
            Defense = defense;
            Health = health;
            Gold = gold;
            items = new List<Item>();
            MaxHealth = 100;
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public void RemoveItem(int index)
        {
            Item item = items[index];
            if (item == MyWeapon) UnEquipWeapon();
            else if(item == MyArmor) UnEquipArmor();

            items.RemoveAt(index);
        }

        public Item GetItemFromIndex(int index)
        {
            return items[index];
        }

        public void Equip(Item item)
        {
            if(item == MyWeapon)
            {
                UnEquipWeapon();
                return;
            }
            else if(item == MyArmor)
            {
                UnEquipArmor();
                return;
            }
            if(item is Weapon weapon) MyWeapon = weapon;
            else if(item is Armor armor) MyArmor = armor;
        }

        public int GetMyWeaponValue()
        {
            return MyWeapon == null ? 0 : MyWeapon.Attack;
        }

        public int GetMyArmorValue()
        {
            return MyArmor == null ? 0 : MyArmor.Defense;
        }

        public void UnEquipWeapon()
        {
            MyWeapon = null;
        }

        public void UnEquipArmor()
        {
            MyArmor = null;
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
            Console.WriteLine("0. 저장 후 종료");
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
                isAvailableInput = true;
                Console.Write(">> ");
                
                string? s;
                s = Console.ReadLine();
                if(!int.TryParse(s, out input))
                {
                    input = -1;
                }

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
            Console.WriteLine($"공격력 : {character.Attack} (+{character.GetMyWeaponValue()})");
            Console.WriteLine($"방어력 : {character.Defense} (+{character.GetMyArmorValue()})");
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

        public void MyInventory(Character character)
        {
            bool isEquipPage = false;
            while (true)
            {
                Console.Clear();
                DrawMyInventory(isEquipPage, character);
                if (!isEquipPage)
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
                                isEquipPage = true;
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
                            input = -1;
                        }

                        if (input == 0)
                        {
                            isEquipPage = !isEquipPage;
                            break;
                        }
                        else if (input >= 1 && input <= character.items.Count)
                        {
                            character.Equip(character.items[input - 1]);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                }
            }
        }

        public void DrawMyInventory(bool isEquipPage, Character character)
        {
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{character.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            int i = 1;
            foreach (var item in character.items)
            {
                Console.Write("- ");
                if (isEquipPage)
                {
                    Console.Write($"{i} ");
                }
                if(item == character.MyWeapon || item == character.MyArmor)
                {
                    Console.Write("[E]");
                }
                Console.Write($"{item.Name}\t| ");
                if (item is Weapon weapon)
                {
                    Console.Write($"공격력 +{weapon.Attack}  | ");
                }
                else if (item is Armor armor)
                {
                    Console.Write($"방어력 +{armor.Defense}  | ");
                }
                else
                {
                    Console.Write($"방어력 +{item.Name}  | ");
                }
                Console.Write($"{item.Description}");
                Console.WriteLine();
                i++;
            }

            Console.WriteLine();
            if (!isEquipPage)
                Console.WriteLine("1. 장착 관리");
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
            SaveLoad sv = new SaveLoad();
            Program program = new Program();
            Character myCharacter = sv.LoadPlayerData();
            List<Item> items = program.InitItem();
            Shop shop = new Shop();
            shop.InitShopItems(sv.LoadShopItem());
            Rest rest = new Rest();
            Dungeon dungeon = new Dungeon();
            

            while (true)
            {
                Console.Clear();
                DrawVillage();
                int input;
                bool isAvailableInput = false;
                do
                {
                    isAvailableInput = false;
                    Console.Write(">> ");

                    string? s;
                    s = Console.ReadLine();
                    if (!int.TryParse(s, out input))
                    {
                        input = -1;
                    }
                    
                    switch (input)
                    {
                        case 1:
                            program.MyStat(myCharacter);
                            break;
                        case 2:
                            program.MyInventory(myCharacter);
                            break;
                        case 3:
                            shop.EnterShop(myCharacter);
                            break;
                        case 4:
                            dungeon.EnterDungeon(myCharacter);
                            break;
                        case 5:
                            rest.EnterRestRoom(myCharacter);
                            break;
                        case 0:
                            sv.SaveCharacterData(myCharacter);
                            sv.SaveShopItem(shop.shopItems);
                            return;
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
