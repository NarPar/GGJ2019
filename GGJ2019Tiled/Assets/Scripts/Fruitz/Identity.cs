using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Fruitz
{
    public class Identity
    {
        private Identity() { }

        private static int MaxNumber = 1;
        private static Queue<FruitType> FruitChute = new Queue<FruitType>();

        public int Number { get; set; }
        public string Name { get; set; }
        public FruitType Type { get; set; }
        public int Score { get; set; }

        public static Identity GenerateNewIdentity()
        {
            var number = MaxNumber++;
            var type = SelectRandomType();
            var name = GenerateRandomName(type);
            return new Identity() { Number = number, Type = type, Name = name, Score = 0 };
        }

        private static void RefillFruitChute()
        {
            var fruitBowl = new FruitType[] {FruitType.Orange, FruitType.Banana, FruitType.Lime, FruitType.Strawberry}
                .OrderBy(f => UnityEngine.Random.Range(0f, 1f));

            FruitChute = new Queue<FruitType>(fruitBowl);
        }

        private static FruitType SelectRandomType()
        {
            if (FruitChute.Count == 0)
            {
                RefillFruitChute();
            }

            return FruitChute.Dequeue();
        }

        private static List<string> RandomNames = new List<string>()
        {
            "Orangey Porangey","us","sets","card","manufacturing","tail","imagine","activity","must","discovery","cow","chose","soil","follow","screen","series","met","sitting","funny","consider","gas","substance","slope","live","cave","properly","unit","stairs","yellow","theory","blow","mighty","town","drove","loss","sharp","game","shut","pile","current","clay","machinery","silver","nails","family","lower","journey","clean","neighborhood","order","island","load","since","label","jet","shells","felt","aid","courage","past","farther","run","port","canal","volume","crack","audience","underline","went","poem","anywhere","shelf","held","ate","lie","pink","library","double","piano","married","globe","bent","radio","driven","friendly","scene","arm","getting","recall","rubbed","continent","dirt","tiny","had","society","it","stone","throw","bicycle","if","bell","bound","effort","move","step","tea","ring","spider","death","direction","adult","nation","another","cold","street","nearest","time","spoken","snake","upward","using","picture","within","cell","useful","bee","expect","market","start","regular","every","been","afternoon","describe","division","special","boat","attached","birthday","cause","variety","taught","cell","plan","settle","wall","particular","nice","topic","whose","point","cause","anywhere","lead","peace","three","being","hot","taken","was","begun","stiff","as","one","charge","took","five","dear","obtain","fourth","pay","depth","repeat","difference","examine","crack","scale","lips","wall","stood","mine","nearer","feed","point","degree","vertical","cent","sort","environment","rose","age","cut","that","raise","bee","railroad","construction","table","two"
        };

        private static string GenerateRandomName(FruitType type)
        {
            var names = RandomNames.OrderBy(n => UnityEngine.Random.Range(0f, 1f)).Take(2);
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(string.Join(" ", names.ToArray()));
        }
    }
}
