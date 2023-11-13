using System.Diagnostics;
using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;
using static RPG1.Program;

namespace RPG1
{
    //캐릭터 스텟 설정
    public class Character
    {
        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public int Gold { get; }

        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }
    }

    //아이템 설정
    public class Item
    {
        public int Index { get; }
        public string Name { get; }
        public int Type { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public string ItemEp { get; }
        public int Price { get; }
        public bool Equip { get; set; }

        public static int ItemCon = 0;

        public Item(int index, string name, int type, int atk, int def, int hp, int price, string itemEp, bool equip = false)
        {
            Index = index;
            Name = name;
            Type = type;
            Atk = atk;
            Def = def;
            Hp = hp;
            ItemEp = itemEp;
            Price = price;
            Equip = equip;
        }
    }
    internal class Program
    {
        private static Character player;
        private static Item[] items;

        static void Main(string[] args)
        {
            CharacterSetting();
            StartLogo();
            StartVillage();

        }
        // 캐릭터 세팅
        static void CharacterSetting()
        {
            // 캐릭터 정보 세팅: 이름, 직업, 레벨, 공격, 방어, 체력, 소지금
            player = new Character("newb", "전 사", 1, 10, 5, 100, 1500);

            // 아이템 정보 세팅: 인덱스, 이름, 타입, 공격력, 방어력, 체력, 가격, 설명, 장착여부
            items = new Item[10];
            AddItem(new Item(0, "무쇠갑옷", 0, 0, 5, 0, 200, "무쇠로 만들어져 튼튼한 갑옷입니다."));
            AddItem(new Item(1, "낡은 검", 1, 2, 0, 0, 100, "쉽게 볼 수 있는 낡은 검 입니다."));
        }

        // 인벤토리 개수 (지금은 10개 까지만)
        static void AddItem(Item item)
        {
            if (Item.ItemCon == 10) return;
            items[Item.ItemCon] = item;
            Item.ItemCon++;
        }

        

        // 시작 마을
        static void StartVillage()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("       스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("  이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine("-------------------------------------------------------\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            int input = Choice(1, 2);
            switch (input)
            {
                case 1:
                    Status();
                    break;
                case 2:
                    Inventory();
                    break;
            }
        }

        //플레이어 스텟
        static void Status()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("             캐릭터의 정보가 표시됩니다.");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("Lv. {0}", player.Level.ToString(00));
            Console.WriteLine("{0} ( {1} )", player.Name, player.Job);
            Console.WriteLine("공격력 : {0}", player.Atk.ToString(00));
            Console.WriteLine("방어력 : {0}", player.Def.ToString(00));
            Console.WriteLine("체 력 : {0}", player.Hp.ToString(00));
            Console.WriteLine("Gold : {0}G", player.Gold.ToString(00));
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            int input = Choice(0, 0);
            switch (input)
            {
                case 0:
                    StartVillage();
                    break;
            }
        }

        //플레이어 인벤토리
        static void Inventory()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("                       인벤토리");
            Console.WriteLine("         보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("아이템 배열 필요");
            Console.WriteLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            int input = Choice(0, 1);
            switch (input)
            {
                case 0:
                    StartVillage();
                    break;
                case 1:
                    CheckEquipment();
                    break;
            }


        }

        // 장착 관리 미완성
        static void CheckEquipment()
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("                 인벤토리 - 장착 관리");
            Console.WriteLine("        보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("아이템 배열 필요");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            int input = Choice(0, 0); // 배열 값에 따라 변경
            switch (input)
            {
                case 0:
                    Inventory();
                    break;
            }
        }

        // 플레이어 숫자 고르기
        static int Choice(int min, int max)
        {
            while (true)
            {
                string choiceString = Console.ReadLine();
                bool choiceBool = int.TryParse(choiceString, out int choiceInt);
                if (choiceBool)
                {
                    if (choiceInt >= min && choiceInt <= max)
                    {
                        return choiceInt;
                    }
                }
                Console.WriteLine("잘못된 입력입니다.");
                Console.Write(">> ");
            }
        }

        //스타트 로고
        static void StartLogo()
        {
            Console.WriteLine("   ____                   _                 ");
            Console.WriteLine("  / ___| ___    __ _ _ __| |_   __ _          ");
            Console.WriteLine("  ＼___ |  _＼ / _` | '__| __| / _` |         ");
            Console.WriteLine("   ___) | |_) | (_| | |  | |_ | (_| |         ");
            Console.WriteLine("  |____/|  __/ ＼___|_|   ＼_| ＼_,_|         ");
            Console.WriteLine("  |  _＼|_|  _ _ __   __ _  ___   ___   ____");
            Console.WriteLine("  | | | | | | | '_ ＼/ _` |/ _ ＼/ _ ＼| '_ ＼");
            Console.WriteLine("  | |_| | |_| | | | | (_| |  ___/ (_)  | | | |");
            Console.WriteLine("  |____/ ＼_,_|_| |_|＼__,|＼___|＼___/|_| |_|");
            Console.WriteLine("                      |___/                  ");
            Console.WriteLine("    ________________________________________  ");
            Console.WriteLine("   |   Press any button to start the game   |");
            Console.WriteLine("   |________________________________________|");
            Console.ReadKey();
        }
    }
}