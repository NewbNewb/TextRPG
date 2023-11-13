using System.Diagnostics;
using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;
using static RPG1.Program;

namespace RPG1
{
    internal class Program
    {
        private static Character player;
        private static Item item;

        static void Main(string[] args)
        {
            CharacterSetting();
            StartVillage();

        }
        // 캐릭터
        static void CharacterSetting()
        {
            // 캐릭터 정보 세팅, 이름, 직업, 레벨, 공격, 방어, 체력, 소지금
            player = new Character("newb", "전 사", 1, 10, 5, 100, 1500);

            // 아이템 정보 세팅, 인덱스, 이름, 공격력, 방어력, 체력, 설명, 가격, 장착여부
            item = new Item(0, "무쇠갑옷", 0, 5, 0, "무쇠로 만들어져 튼튼한 갑옷입니다.", 200,true);
            item = new Item(1, "낡은 검", 2, 0, 0, "쉽게 볼 수 있는 낡은 검 입니다.", 100, false);
        }

        // 시작 마을
        static void StartVillage()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리\n");
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
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
            Console.WriteLine("Lv. {0}", player.Level);
            Console.WriteLine("{0} ( {1} )", player.Name, player.Job);
            Console.WriteLine("공격력 : {0}", player.Atk);
            Console.WriteLine("방어력 : {0}", player.Def);
            Console.WriteLine("체 력 : {0}", player.Hp);
            Console.WriteLine("Gold : {0}G\n", player.Gold);
            Console.WriteLine("0. 나가기\n");
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
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine(" - {0}{1}     | {2} +{3} | {4} ");
            Console.WriteLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기\n");
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
    }
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
    public class Item
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public string ItemEp { get; set; }
        public int Price { get; set; }
        public bool Equip { get; set; }
        public Item(int index, string name, int atk, int def,int hp, string itemEp, int price, bool equip)
        {
            Index = index;
            Name = name;
            Atk = atk;
            Def = def;
            Hp = hp;
            ItemEp = itemEp;
            Price = price;
            Equip = equip;
        }
    }
}