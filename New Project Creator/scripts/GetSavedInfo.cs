using System;
using System.IO;
using Eto.Forms;

namespace New_Project_Creator
{
    class GetSavedInfo
    {
        public FileStream saveFile;

        public GetSavedInfo(string saveFileName)
        {
            saveFile = new FileStream(saveFileName + ".txt", FileMode.OpenOrCreate);
        }

        public void SaveInfo()
        {

        }
        public string LoadInfo()
        {
            StreamReader saveReader = new StreamReader(saveFile);
            string output = saveReader.ReadToEnd();

            saveReader.Close();
            return output;
        }

        public void Close()
        {
            saveFile.Close();
        }
    }
}