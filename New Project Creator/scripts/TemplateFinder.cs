using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace New_Project_Creator
{
    class TemplateFinder
    {
        public static string ExecuteBashCommand(string command)
        {
            command = command.Replace("\"", "\"\"");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            process.WaitForExit();

            return process.StandardOutput.ReadToEnd();
        }

        private static string GetIntrolessProjectsList(string rawProjects)
        {
            // Remove the intro from the top
            int introEndIndex = rawProjects.IndexOf("Tags");
            introEndIndex = introEndIndex + 4;
            while (rawProjects[introEndIndex] == '-' || rawProjects[introEndIndex] == ' ' || rawProjects[introEndIndex] == '\n')
            {
                introEndIndex++;
            }

            return rawProjects.Substring(introEndIndex);
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