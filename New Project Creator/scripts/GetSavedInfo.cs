using System;
using System.Collections.Generic;
using System.IO;
using Eto.Forms;

namespace New_Project_Creator
{
    class GetSavedInfo
    {
        public string[] defaultText;
        private string saveFileName;

        public GetSavedInfo(string SaveFileName, string[] DefaultText)
        {
            saveFileName = SaveFileName;
            defaultText = DefaultText;
            FileStream saveFile = new FileStream(saveFileName + ".txt", FileMode.OpenOrCreate);
            saveFile.Close();
        }

        public void SaveInfo(string[] TextLines)
        {
            StreamWriter saveWriter = new StreamWriter(saveFileName + ".txt");

            foreach (string line in TextLines)
            {
                saveWriter.WriteLine(line);
            }

            saveWriter.Close();
        }
        public Dictionary<string, string> LoadInfo()
        {
            StreamReader saveReader = new StreamReader(saveFileName + ".txt");
            string[] rawText = saveReader.ReadToEnd().Split('\n');
            saveReader.Close();

            if (rawText[0] == "")
            {
                SaveInfo(defaultText);
            }
            saveReader = new StreamReader(saveFileName + ".txt");
            rawText = saveReader.ReadToEnd().Split('\n');
            var output = GetVariableValues(rawText);

            saveReader.Close();
            return output;
        }

        private Dictionary<string, string> GetVariableValues(string[] Lines)
        {
            var output = new Dictionary<string, string>();

            foreach (string line in Lines)
            {
                if (!line.StartsWith("#") && line != Lines[Lines.Length - 1])
                {
                    // Get VarName
                    string varName = line.Substring(0, line.IndexOf('='));
                    if (varName.Contains(" "))
                    {
                        varName = varName.Replace(" ", "");
                    }

                    // Get VarValue
                    string varValue = line.Substring(line.IndexOf('=') + 1, line.Length - 2 - line.IndexOf('='));
                    if (varValue.Contains(" "))
                    {
                        varValue = varValue.Replace(" ", "");
                    }

                    output.Add(varName, varValue);
                }
            }
            return output;
        }
    }
}