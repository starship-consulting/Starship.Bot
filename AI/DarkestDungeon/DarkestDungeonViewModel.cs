﻿using System.Collections.Generic;
using Starship.Bot.AI.DarkestDungeon.Data;

namespace Starship.Bot.AI.DarkestDungeon {
    public class DarkestDungeonViewModel {

        public DarkestDungeonViewModel() {
            Party = new List<GameCharacter>();
            Enemies = new List<GameCharacter>();
        }

        /// <summary>
        /// Main sensor loop to synchronize entire game state
        /// </summary>
        public void UpdateState() {
            if(DarkestDungeonGame.CampaignButton.Exists) {
                Status = DarkestDungeonGameStatusTypes.MainMenu;
            }
        }
        
        public bool IsLoaded { get; set; }

        public DarkestDungeonGameStatusTypes Status { get; set; }

        public int Room { get; set; }

        public int HorizontalPosition { get; set; }

        public double LightLevel { get; set; }

        public bool InCombat { get; set; }

        public List<GameCharacter> Party { get; set; }

        public List<GameCharacter> Enemies { get; set; }
    }
}