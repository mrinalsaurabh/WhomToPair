using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Pair.Web.Models;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Pair.Web.Services
{
    public static class PairingData
    {
        public static string FilePath
        {
            get; set;
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

        public static List<CodePair> GetAllCodePairs()
        {
            List<CodePair> allCodePairs = new List<CodePair>();
            List<Individual> allIndivisuals = new List<Individual>();
            GetAllData(out allCodePairs, out allIndivisuals);
            return allCodePairs;
        }

        public static List<Individual> GetAllIndividuals()
        {
            List<CodePair> allCodePairs = new List<CodePair>();
            List<Individual> allIndividuals = new List<Individual>();
            GetAllData(out allCodePairs, out allIndividuals);
            return allIndividuals;
        }

        public static void GetAllData(out List<CodePair> allCodePairs, out List<Individual> allIndividuals)
        {
            string allData = ReadFile();
            allCodePairs = new List<CodePair>();
            allIndividuals = new List<Individual>();

            string[] individualUnits = allData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in individualUnits)
            {
                string[] record = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (record.Length == 3)
                    allCodePairs.Add(new CodePair() { First = record[0], Second = record[1], Count = int.Parse(record[2]) });
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

        public static void DeleteIndividual (Individual individual)
        {
            string allData = ReadFile();
            string[] individualUnits = allData.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string newEntries = "";
            string CodePairString = "";
            foreach (var item in individualUnits)
            {
                string[] record = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (record.Contains(individual.Name))
                    continue;
                else if (record.Length == 2)
                    newEntries += item + ";";
                else if (record.Length == 3)
                {
                    string writable = "{0},{1},{2};";
                    writable = string.Format(writable, record[0], record[1], record[2]);
                    CodePairString += writable;
                }
            } 
            

            string allEntries = CodePairString + newEntries;
            WriteNewFile(allEntries);
        }

        public static void UpdateCodePairs(List<CodePair> allCodePairs)
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
            string CodePairString = "";
            foreach (var item in allCodePairs)
            {
                string writable = "{0},{1},{2};";
                writable = string.Format(writable, item.First, item.Second, item.Count);
                CodePairString += writable;
            }

            string allEntries = CodePairString + newEntries;
            WriteNewFile(allEntries);
        }
    }
}
