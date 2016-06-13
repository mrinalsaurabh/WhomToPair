using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace WhomToPair.Web
{
    public static class PairingData
    {
        public static string FilePath
        {
            get { return ConfigurationManager.AppSettings["FilePath"]; }
        }

        public static string ReadFile()
        {
            var fileContents = File.ReadAllText(FilePath);
            return fileContents;
        }

        public static void WriteFile(string content)
        {
            File.AppendAllText(FilePath, content);
        }

        public static void WriteNewFile(string content)
        {
            File.WriteAllText(FilePath, content);
        }

        public static List<Pair> GetAllPairs()
        {
            List<Pair> allPairs = new List<Pair>();
            List<Individual> allIndivisuals = new List<Individual>();
            GetAllData(out allPairs, out allIndivisuals);
            return allPairs;
        }

        public static List<Individual> GetAllIndividuals()
        {
            List<Pair> allPairs = new List<Pair>();
            List<Individual> allIndividuals = new List<Individual>();
            GetAllData(out allPairs, out allIndividuals);
            return allIndividuals;
        }

        public static void GetAllData(out List<Pair> allPairs, out List<Individual> allIndividuals)
        {
            string allData = ReadFile();
            allPairs = new List<Pair>();
            allIndividuals = new List<Individual>();

            string[] individualUnits = allData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in individualUnits)
            {
                string[] record = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (record.Length == 3)
                    allPairs.Add(new Pair() { First = record[0], Second = record[1], Count = int.Parse(record[2]) });
                else
                    allIndividuals.Add(new Individual() { Name = record[0], Number = int.Parse(record[1]) });
            }
        }

        public static void AddNew(Individual newIndividual)
        {
            List<Individual> existingIndividuals = GetAllIndividuals();
            if (existingIndividuals.Find(q => q.Name.ToLower() == newIndividual.Name.ToLower()) == null)
            {
                WriteFile(newIndividual.Name + ",0;");
                foreach (var item in existingIndividuals)
                {
                    string writable = "{0},{1},{2};";
                    writable = string.Format(writable, item.Name, newIndividual.Name, 0);
                    WriteFile(writable);
                }
            }
        }

        public static void UpdatePairs(List<Pair> allPairs)
        {
            string allData = ReadFile();
            string[] individualUnits = allData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string newEntries = "";
            foreach (var item in individualUnits)
            {
                string[] record = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (record.Length == 2)
                    newEntries += item + ";";
            }
            string pairString = "";
            foreach (var item in allPairs)
            {
                string writable = "{0},{1},{2};";
                writable = string.Format(writable, item.First, item.Second, item.Count);
                pairString += writable;
            }

            string allEntries = pairString + newEntries;
            WriteNewFile(allEntries);
        }
    }
}
