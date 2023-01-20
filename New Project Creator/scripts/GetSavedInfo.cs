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
            StreamReader checkEmpty = new StreamReader(saveFile);
            if (checkEmpty.ReadToEnd() == "" || checkEmpty.ReadToEnd() == "\n")
            {
                SaveInfo(defaultText);
            }
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
        
        public void SetValue(string Variable, string Value)
        {
            StreamReader saveReader = new StreamReader(saveFileName + ".txt");
            string[] fileContent = saveReader.ReadToEnd().Split('\n');
            saveReader.Close();
            List<int> variableIndexes = new List<int>();

            for (int i = 0; i < fileContent.Length; i++)
            {
                if (fileContent[i].Contains(Variable) && !fileContent[i].StartsWith("#"))
                {
                    variableIndexes.Add(i);
                }
            }

            foreach (int index in variableIndexes)
            {
                var line = fileContent[index];
                var valueStartIndex = line.IndexOf('\"') + 1;
                fileContent[index] = fileContent[index].Substring(0, valueStartIndex) + Value + "\"";
            }

            SaveInfo(fileContent);
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
                    string varValue = line.Substring(line.IndexOf('\"') + 1, line.Length - 2 - line.IndexOf('\"'));

                    output.Add(varName, varValue);
                }
            }
            return output;
        }
    }
}