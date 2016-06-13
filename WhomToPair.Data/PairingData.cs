using System;
using System.Collections.Generic;
using System.IO;
using WhomToPair.Entities;

namespace WhomToPair.Data
{
    public static class PairingData
    {
        public static List<Pair> GetAllPairs(string allData)
        {
            List<Pair> AllPairs = new List<Pair>();
            List<Indivisual> Individuals = new List<Indivisual>();

            string[] individualUnits = allData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in individualUnits)
            {
                string[] record = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (record.Length == 3)
                    AllPairs.Add(new Pair() { First = record[0], Second = record[1], Count = int.Parse(record[2]) });
                else
                    Individuals.Add(new Indivisual() { Name = record[0], Number = int.Parse(record[1]) });
            }
            return AllPairs;
        }
    }
}
