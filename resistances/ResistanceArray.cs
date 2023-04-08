using AscendedZ.skills;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.resistances
{
    public partial class ResistanceArray
    {
        /// <summary>
        /// Rows = Resistances
        /// Columns = Elements
        /// </summary>
        public int[] RArray { get; set; }

        public ResistanceArray()
        {
            if (this.RArray == null)
            {
                // our number of columns
                int elements = Enum.GetNames(typeof(Elements)).Length;
                this.RArray = new int[elements];

                // Neutral resistance by default
                for (int i = 0; i < elements; i++)
                    this.RArray[i] = (int)ResistanceType.None;
            }
                
        }

        /// <summary>
        /// For asset creation purposes only. This information should be stored in the json database.
        /// </summary>
        /// <param name="rtype"></param>
        /// <param name="element"></param>
        public void CreateResistance(ResistanceType rtype, Elements element)
        {
            this.RArray[(int)element] = (int)rtype;
        }

        /// <summary>
        /// This should be called when changing a unit's armor.
        /// Values should range from positive to negative.
        /// Removing armor should reset the values back to the way they were.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="amount"></param>
        public void AdjustResistanceByAmount(Elements element, int amount)
        {
            this.RArray[(int)element] += amount;

            if (this.RArray[(int)element] < 0)
                this.RArray[(int)element] = 0;

            if (this.RArray[(int)element] > (int)ResistanceType.Dr)
                this.RArray[(int)element] = (int)ResistanceType.Dr;
        }

        /// <summary>
        /// Return a clean printout of our resistances.
        /// </summary>
        /// <returns></returns>
        public string GetResistanceString()
        {
            string[] resistNames = Enum.GetNames(typeof(ResistanceType));
            string[] elementNames = Enum.GetNames(typeof(Elements));

            StringBuilder resString = new StringBuilder();

            string elementDivider = ", ";
            string resDivider = " - ";

            // for each resistance
            for(int i = 0; i < resistNames.Length; i++)
            {
                if (i != (int)ResistanceType.None)
                {
                    StringBuilder elementRes = new StringBuilder();

                    // for each element, check what our resistance is
                    for (int j = 0; j < this.RArray.Length; j++)
                    {
                        if (this.RArray[j] == i)
                        {
                            elementRes.Append($"{elementNames[j]}{elementDivider}");
                        }
                    }

                    string elementString = elementRes.ToString();
                    if (!string.IsNullOrEmpty(elementString))
                    {
                        resString.Append($"{resistNames[i]}: {elementString.Substr(0, elementString.Length - elementDivider.Length)}{resDivider}");
                    }
                }
            }

            // output = "R1: E1, E2 - R2: E1, ..."
            return resString.ToString().Substring(0, resString.Length - resDivider.Length);
        }

        #region Resistance Checks
        public bool IsWeakToElement(Elements element)
        {
            int wk = (int)ResistanceType.Wk;
            return this.RArray[(int)element] == wk;
        }

        public bool IsResistantToElement(Elements element)
        {
            int rs = (int)ResistanceType.Rs;
            return this.RArray[(int)element] == rs;
        }

        public bool IsNullElement(Elements element)
        {
            int nu = (int)ResistanceType.Nu;
            return this.RArray[(int)element] == nu;
        }

        public bool IsDrainElement(Elements element)
        {
            int dr = (int)ResistanceType.Dr;
            return this.RArray[(int)element] == dr;
        }
        #endregion
    }
}
