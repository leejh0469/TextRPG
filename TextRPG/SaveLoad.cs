using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TextRPG
{
    internal class SaveLoad
    {
        public List<Pair<Item, bool>> shopItems = new List<Pair<Item, bool>>();

        public void SaveCharacterData(Character character)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string json = JsonConvert.SerializeObject(character, settings);
            File.WriteAllText(@"Player.json", json);
        }

        public void SaveShopItem(List<Pair<Item, bool>> shopItems)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            string json = JsonConvert.SerializeObject(shopItems, settings);
            File.WriteAllText(@"ShopItems.json", json);
        }

        public List<Pair<Item, bool>> LoadShopItem()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            if (File.Exists(@"ShopItems.json"))
            {
                string json = File.ReadAllText(@"ShopItems.json");
                shopItems = JsonConvert.DeserializeObject<List<Pair<Item, bool>>>(json, settings);
            }
            else
            {
                List<Item> items = InitItem();
                shopItems = new List<Pair<Item, bool>>();
                foreach (Item item in items)
                {
                    shopItems.Add(new Pair<Item, bool>(item, true));
                }
            }
            return shopItems;
        }

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

        public Character LoadPlayerData()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            Character character;
            if (File.Exists(@"Player.json"))
            {
                string json = File.ReadAllText(@"Player.json");
                character = JsonConvert.DeserializeObject<Character>(json, settings);
                for (int i = 0; i < character.items.Count; i++)
                {
                    foreach(var shopItem in shopItems)
                    {
                        if (character.items[i].Name == shopItem.First.Name)
                        {
                            character.items[i] = shopItem.First;
                        }
                    }
                }
                foreach(var item in character.items)
                {
                    if (character.MyWeapon?.Name == item.Name)
                    {
                        character.MyWeapon = item as Weapon;
                        break;
                    }
                }
                foreach(var item in character.items)
                {
                    if(character.MyArmor?.Name == item.Name)
                    {
                        character.MyArmor = item as Armor;
                        break;
                    }
                }
            }
            else
            {
                character = new Character(1, "Chad", 10, 5, 100, 5000);
            }
            return character;
        }
    }
}
