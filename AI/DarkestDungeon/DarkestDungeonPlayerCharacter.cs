namespace Starship.Bot.AI.DarkestDungeon {
    public class DarkestDungeonPlayerCharacter : GameCharacter {

        public int Accuracy { get; set; }

        public int Speed { get; set; }

        public double CriticalChance { get; set; }

        public int Dodge { get; set; }

        public int Protection { get; set; }

        public int Level { get; set; }

        public int Health { get; set; }

        public int MaxHealth { get; set; }

        public int Stress { get; set; }

        public int IsStressed { get; set; }
    }
}