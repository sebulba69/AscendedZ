using AscendedZ.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ
{
    /// <summary>
    /// A collection of dialog scenarios to be shown during cutscenes.
    /// </summary>
    public class DialogScenes
    {
        /// <summary>
        /// This is the opening cutscene of the game.
        /// </summary>
        public static string[] Opening = new string[] 
        {
            "Once upon a buce, there was a labyrinth known\nas The Endless Dungeon.",
            "Legend has it that contestants from\naround the world came together to\nconquer it.",
            "Decades passed by, and no one seemed\nup to the challenge.",
            "Until, one day, it happened...\nThe Endless Dungeon, right before\neveryone's eyes...",
            "...Completely disappeared. Someone had\nmanaged to conquer it once and for all.",
            "The opportunity to obtain the ultimate\npower of Ascended Enlightenment was\ngone... but not for long.",
            "Hundreds of years later, a new dungeon\nspawned in its place.",
            "It's said that this one is tougher than\nthe last, but the reward is\ngreater than ever before.",
            "You are a villager who has decided to\ntake on the challenge.",
            "Will you fail like all the others who've\ncome before you, or will you do the\nimpossible?",
            "Do you have what it takes to Ascend?"
        };

        public static Dictionary<string, string[]> DCBossDialog = new Dictionary<string, string[]>() 
        {
            { 
                EnemyNames.Ocura, new string[]
                {
                    "Bah har har har har.",
                    "In my flummers and wiggums as the\nruler of this bucetal sector...",
                    "Yet have I to experience a contestant\nwho has managed to make it thus far.",
                    "Strong as you are, and flippital as\nyou might be...",
                    "You will not reach the bottom of the\nLabrybuce.",
                    "For I, Ocura, will stand opposed\nto all those who wish to ASCEND!!!",
                } 
            },
            {
                EnemyNames.Emush, new string[]
                {
                    "Your tolerance for suffering is admirable,\nyoung Ascended.",
                    "But, I can see it in your eyes.\nYou know not the meaning of this\nquest.",
                    "Thou ought to turn back for the\ntruth that layeth deepest in these\ncaverns is not for the unbuced.",
                    "Of course, being a contestant\nyou thirst for power regardless.",
                    "Shouldst thou not hearst my warnings,\nI will satiate thine need for\nanguish.",
                    "I am Emush. Have at thee when ready.",
                }
            },
        };
    }
}
