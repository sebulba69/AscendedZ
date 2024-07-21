using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.entities
{
    public class PartyNames
    {
        // Pre-made
        public static readonly string Locphiedon = "Locphiedon";
        public static readonly string Gagar = "Gagar";
        public static readonly string Yuudam = "Yuudam";
        public static readonly string Pecheal = "Pecheal";
        public static readonly string Toke = "Toke";
        public static readonly string Maxwald = "Maxwald";
        public static readonly string Halvia = "Halvia";
        public static readonly string Tyhere = "Tyhere";
        public static readonly string Paria = "Paria";
        public static readonly string Joan = "Joan";
        public static readonly string Andmond = "Andmond";

        // Minions
        public static readonly string BuceyRed = "Bucey Red";
        public static readonly string BuceyBlue = "Bucey Blue";
        public static readonly string BuceyGreen = "Bucey Green";

        // Fusion 1
        public static readonly string Ancrow = "Ancrow"; // Fire
        public static readonly string Marchris = "Marchris"; // Fire
        public static readonly string Candun = "Candun"; // Ice
        public static readonly string Thryth = "Thryth"; // Ice
        public static readonly string Samlin = "Samlin"; // Wind
        public static readonly string Everever = "Everever"; // Wind
        public static readonly string Ciavid = "Ciavid"; // Elec
        public static readonly string Eri = "Eri"; // Elec
        public static readonly string Conson = "Conson"; // Light
        public static readonly string Winegeful = "Winegiful"; // Light
        public static readonly string Cermas = "Cermas"; // Dark
        public static readonly string Fledron = "Fledron"; // Dark

        public static readonly string Ride = "Ride"; // Fire
        public static readonly string Muelwise = "Muel-wise"; // Fire
        public static readonly string Pher = "Pher"; // Fire

        public static readonly string Shacy = "Shacy"; // Ice
        public static readonly string Swithwil = "Swithwil"; // Ice
        public static readonly string Isenann = "Isenann"; // Ice

        public static readonly string Lesdan = "Lesdan"; // Wind
        public static readonly string Ronboard = "Ronbaord"; // Wind
        public static readonly string Dusam = "Dusam"; // Wind

        public static readonly string Tinedo = "Tinedo"; // Elec
        public static readonly string Xtrasu = "X'Trasu"; // Elec
        public static readonly string Laanard = "Laanard"; // Elec

        public static readonly string Earic = "Earic"; // Light
        public static readonly string LatauVHurquij = "Latau V'Hurquij"; // Light
        public static readonly string Hallou = "Hallou"; // Light

        public static readonly string Baring = "Baring"; // Dark
        public static readonly string Tami = "Tami"; // Dark
        public static readonly string Dinowaru = "Dinowaru"; // Dark

    }

    public class EnemyNames
    {
        // Enemies (Tutorial)
        public static readonly string Conlen = "Conlen";
        public static readonly string Liamlas = "Liamlas";
        public static readonly string Orachar = "Orahcar";
        public static readonly string Fastrobren = "Fastrobren";
        public static readonly string CattuTDroni = "Cattu T'Droni";

        // Enemies (Randomized for Floors)
        // alternate enemies (no weakness)
        public static readonly string Fledan = "Fledan";
        public static readonly string Walds = "Walds";

        public static readonly string Paca = "Paca";
        public static readonly string Wigfred = "Wigfred";
        public static readonly string Lyley = "Lyley";

        // agro - status
        public static readonly string Thylaf = "Thylaf"; // elec
        public static readonly string Arwig = "Arwig"; // ice
        public static readonly string Riccman = "Riccman"; // wind

        public static readonly string Gormacwen = "Gormacwen"; // fire
        public static readonly string Vidwerd = "Vidwerd"; // dark

        // wex hunters
        public static readonly string Isenald = "Isenald"; // light
        public static readonly string Gardmuel = "Gardmuel"; // dark
        public static readonly string Sachael = "Sachael"; // fire

        public static readonly string Pebrand = "Pebrand"; // ice
        public static readonly string Leofuwil = "Leofuwil"; // elec

        // copy cats
        public static readonly string Naldbear = "Naldbear"; // elec
        public static readonly string Stroma_Hele = "Stroma Hele"; // fire
        public static readonly string Ingesc = "Ingesc"; // dark
        public static readonly string Sylla = "Sylla"; // wind
        public static readonly string Venforth = "Venforth"; // ice

        // resistance changers
        public static readonly string Thony = "Thony"; // Ice --> Fire
        public static readonly string Conson = "Conson"; // Light --> Dark
        public static readonly string Bernasbeorth = "Bernasbeorth"; // Elec --> Wind

        // protector enemies
        public static readonly string Ed = "Ed"; // protects fire
        public static readonly string Otem = "Otem"; // protects ice
        public static readonly string Hesret = "Hesret"; // protects wind
        public static readonly string Isumforth = "Isumforth"; // protects dark
        public static readonly string LaChris = "La-chris"; // protects wind

        // more alt enemies w/ buffs
        public static readonly string Nanfrea = "Nanfrea"; // boost fire
        public static readonly string Anrol = "Anrol"; // boost ice
        public static readonly string David = "Da-vid"; // boost wind
        public static readonly string Ferza = "Ferza"; // boost elec
        public static readonly string Garcar = "Garcar"; // boost dark
        public static readonly string Wennald = "Wennald"; // boost light

        // enemies with all-hits
        public static readonly string Aldmas = "Aldmas"; // all-hit fire (AE)
        public static readonly string Fridan = "Fridan"; // all-hit force (AE)
        public static readonly string Rahfortin = "Rahfortin"; // all-hit WEX + dark (AE)
        public static readonly string Leswith = "Leswith"; // all-hit WEX + elec (AE)

        // enemies that just provide turns
        public static readonly string Bue = "Bue"; // (AE) turns = 2, ice
        public static readonly string Bued = "Bued"; // (AE) turns = 2, fire
        public static readonly string Bureen = "Bureen"; // (AE) turns = 2, wind

        // eye enemies
        public static readonly string Acardeb = "Acardeb";
        public static readonly string Darol = "Darol";
        public static readonly string Hesbet = "Hesbet";

        // Bosses (Normal)
        public static readonly string Harbinger = "Harbinger, Mangler of Legs";
        public static readonly string Elliot_Onyx = "Elliot Onyx";
        public static readonly string Sable_Vonner = "Sable Vonner";
        public static readonly string Cloven_Umbra = "Cloven Umbra";
        public static readonly string Ashen_Ash = "Ashen Ash";
        public static readonly string Ethel_Aura = "Ethel Aura";
        public static readonly string Kellam_Von_Stein = "Kellam Von Stein";
        public static readonly string Drace_Skinner = "Drace Skinner";
        public static readonly string Jude_Stone = "Jude Stone";
        public static readonly string Drace_Razor = "Drace Razor";

        // Bosses (Dungeon)
        public static readonly string Ocura = "Ocura";
        public static readonly string Emush = "Emush";

        
    }
}
