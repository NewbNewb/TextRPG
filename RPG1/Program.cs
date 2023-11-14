using System.ComponentModel;
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
    // Enum으로 타입 적용 개선 사항
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

        public static int itemCon = 0;

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

        public void PrintItems(bool withNumber = false, int index = 0)
        {
            Console.Write("- ");
            if (withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0}. ", index);
                Console.ResetColor();
            }

            if (Equip)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("E");
                Console.ResetColor();
                Console.Write("]");
            }
            Console.Write(Name);
            Console.Write(" | ");

            //사망연산자 문법 (Atk >= 0? "+" : "")  조건? 조건이 참이라면 "+"를 출력 : 아니라면 "" 출력
            if (Atk != 0) Console.Write($"공격력 {(Atk >= 0 ? "+" : "")}{Atk}");
            if (Def != 0) Console.Write($"방어력 {(Def >= 0 ? "+" : "")}{Def}");
            if (Hp != 0) Console.Write($"체력 {(Hp >= 0 ? "+" : "")}{Hp}");

            Console.Write(" | ");
            Console.WriteLine(ItemEp);
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
            // 리스트 변경 개선 사항
            items = new Item[10];
            AddItem(new Item(0, "무쇠갑옷", 0, 0, 5, 0, 200, "무쇠로 만들어져 튼튼한 갑옷입니다."));
            AddItem(new Item(1, "낡은 검", 1, 2, 0, 0, 100, "쉽게 볼 수 있는 낡은 검 입니다."));
        }

        // 인벤토리 개수 (지금은 10개 까지만)
        static void AddItem(Item item)
        {
            items[Item.itemCon] = item;
            Item.itemCon++;
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
            HiglightText1("-------------------------------------------------------");
            HiglightText1("             캐릭터의 정보가 표시됩니다.");
            HiglightText1("-------------------------------------------------------");
            HiglightText2("Lv. ", player.Level.ToString("00"), "");
            Console.WriteLine("{0} ( {1} )", player.Name, player.Job);

            //장착 장비에 따른 스텟 추가

            int BonusAtk = getSumBonusAtk();
            int BonusDef = getSumBonusDef();
            int BonusHp = getSumBonusHp();

            HiglightText2("공격력 : ", (player.Atk + BonusAtk).ToString(), BonusAtk > 0 ? string.Format("(+{0})", BonusAtk) : "");
            HiglightText2("방어력 : ", (player.Def + BonusDef).ToString(), BonusDef > 0 ? string.Format("(+{0})", BonusDef) : "");
            HiglightText2("체 력 : ", (player.Hp + BonusHp).ToString(), BonusHp > 0 ? string.Format("(+{0})", BonusHp) : "");
            HiglightText2("Gold : ", player.Gold.ToString(), "G");
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

        // 아이템 장착에 따른 보너스 스텟
        private static int getSumBonusAtk()
        {
            int sum = 0;
            for (int i = 0; i < Item.itemCon; i++)
            {
                if (items[i].Equip)
                {
                    sum += items[i].Atk;
                }
            }
            return sum;
        }
        private static int getSumBonusDef()
        {
            int sum = 0;
            for (int i = 0; i < Item.itemCon; i++)
            {
                if (items[i].Equip)
                {
                    sum += items[i].Def;
                }
            }
            return sum;
        }
        private static int getSumBonusHp()
        {
            int sum = 0;
            for (int i = 0; i < Item.itemCon; i++)
            {
                if (items[i].Equip)
                {
                    sum += items[i].Hp;
                }
            }
            return sum;
        }


        //플레이어 인벤토리
        static void Inventory()
        {
            Console.Clear();
            HiglightText1("-------------------------------------------------------");
            HiglightText1("                       인벤토리");
            HiglightText1("         보유 중인 아이템을 관리할 수 있습니다.");
            HiglightText1("-------------------------------------------------------");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < Item.itemCon; i++)
            {
                items[i].PrintItems();
            }
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

        static void CheckEquipment()
        {
            Console.Clear();
            HiglightText1("-------------------------------------------------------");
            HiglightText1("                 인벤토리 - 장착 관리");
            HiglightText1("        보유 중인 아이템을 관리할 수 있습니다.");
            HiglightText1("-------------------------------------------------------");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < Item.itemCon; i++)
            {
                items[i].PrintItems(true, i + 1);
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            int input = Choice(0, Item.itemCon); // 배열 값에 따라 변경
            switch (input)
            {
                case 0:
                    Inventory();
                    break;
                default:
                    ItemEquip(input - 1);
                    CheckEquipment();
                    break;
            }
        }

        private static void ItemEquip(int index)
        {
            items[index].Equip = !items[index].Equip;
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

        //문자 컬러
        private static void HiglightText1(string text)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        private static void HiglightText2(string s1, string s2, string s3)
        {
            Console.Write(s1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(s2);
            Console.ResetColor();
            Console.WriteLine(s3);
        }


        //스타트 로고
        private static void StartLogo()
        {
            Console.WriteLine("   ____                   _                   ");
            Console.WriteLine("  / ___| ___    __ _ _ __| |_   __ _          ");
            Console.WriteLine("  ＼___ |  _＼ / _` | '__| __| / _` |         ");
            Console.WriteLine("   ___) | |_) | (_| | |  | |_ | (_| |         ");
            Console.WriteLine("  |____/|  __/ ＼___|_|   ＼_| ＼_,_|         ");
            Console.WriteLine("  |  _＼|_|  _ _ __   __ _  ___   ___   ____  ");
            Console.WriteLine("  | | | | | | | '_ ＼/ _` |/ _ ＼/ _ ＼| '_ ＼");
            Console.WriteLine("  | |_| | |_| | | | | (_| |  ___/ (_)  | | | |");
            Console.WriteLine("  |____/ ＼_,_|_| |_|＼__,|＼___|＼___/|_| |_|");
            Console.WriteLine("                      |___/                   ");
            Console.WriteLine("    ________________________________________  ");
            Console.WriteLine("   |   Press any button to start the game   | ");
            Console.WriteLine("   |________________________________________| ");
            Console.ReadKey();
        }
    }
}