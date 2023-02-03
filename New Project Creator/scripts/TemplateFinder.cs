using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace New_Project_Creator
{
    class TemplateFinder
    {
        public static string ExecuteBashCommand(string Command)
        {
            Command = Command.Replace("\"", "\"\"");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + Command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();

            if (output != "")
            {
                return output;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static string GetIntrolessProjectsList(string RawProjects)
        {
            // Remove the intro from the top
            int introEndIndex = RawProjects.IndexOf("Tags");
            introEndIndex = introEndIndex + 4;
            while (RawProjects[introEndIndex] == '-' || RawProjects[introEndIndex] == ' ' || RawProjects[introEndIndex] == '\n')
            {
                introEndIndex++;
            }

            return RawProjects.Substring(introEndIndex);
        }

        public static Dictionary<string, string> ExtractProjects()
        {
            Dictionary<string, string> templateNames = new Dictionary<string, string>();
            string projects = GetIntrolessProjectsList(ExecuteBashCommand("dotnet new -l"));

            // Get different components from bloat free list

            foreach (string line in projects.Split('\n'))
            {
                if (line == projects.Split('\n')[projects.Split('\n').Length - 1])
                {
                    break;
                }

                int charIndex = 0;
                while (true)
                {
                    if (line[charIndex] == ' ' && line[charIndex + 1] == ' ')
                    {
                        break;
                    }
                    else
                    {
                        charIndex++;
                    }
                }

                // Display Name
                string templateFullName = line.Substring(0, charIndex);

                int shortNameIndexStart = 0;
                int shortNameIndexEnd = 0;

                if (line[charIndex] == ' ' && line[charIndex + 1] == ' ')
                    {
                        while (line[charIndex] == ' ')
                        {
                            charIndex++;
                        }

                        shortNameIndexStart = charIndex;

                        while (line[charIndex] != ' ')
                        {
                            charIndex++;
                        }

                        shortNameIndexEnd = charIndex;
                    }

                string templateShortName = line.Substring(shortNameIndexStart, shortNameIndexEnd - shortNameIndexStart);
                
                if (templateShortName.Contains(","))
                {
                    templateShortName = templateShortName.Substring(0, templateShortName.IndexOf(','));
                }

                templateNames.Add(templateFullName, templateShortName);
            }

            return templateNames;
        }
    }
}