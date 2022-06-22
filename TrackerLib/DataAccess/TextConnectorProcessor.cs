using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using TrackerLib.Models;

//Load the Text File
//COnvert the text to List<PrizeModel>
//Find the max ID
//Add the new record with the new ID (ma + 1)
//Convert the prizes to List<string>
//Save the List<string> to the text file

namespace TrackerLib.DataAccess.TextHelper
{
    public static class TextConnectorProcessor
    {
        public static string GetFullFilePath(this string fileName)
        {
            return $"{ConfigurationManager.AppSettings["filePath"]}\\{fileName}";
        }

        public static List<string> LoadFile(this string file)
        {
            if(!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> prizeModels = new List<PrizeModel>();
            foreach(var line in lines)
            {
                string[] row = line.Split(',');
                PrizeModel prizeModel = new PrizeModel(row[0], row[1], row[2], row[3], row[4]);
                prizeModels.Add(prizeModel);
            }
            return prizeModels;
        }

        public static void SaveToPrizeFile(this List<PrizeModel> prizes, string fileName)
        {
            List<string> lines = new List<string>();
            foreach(PrizeModel prizeModel in prizes)
            {
                lines.Add($"{prizeModel.Id},{prizeModel.PlaceNumber},{ prizeModel.PlaceName},{prizeModel.PrizeAmount},{prizeModel.PrizePercentage}");
            }
            File.WriteAllLines(fileName.GetFullFilePath(), lines);
        }
    }
}
