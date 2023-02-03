using System;
using System.Collections.Generic;
using System.IO;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace New_Project_Creator
{	
	public class MainForm : Form
	{
		ComboBox templateSelector;
		TextArea optionsOutput;
		TextBox projectNameTB;
		TextBox saveDirectoryTB;
		TextBox extraOptionsTB;
		GetSavedInfo defaultSettings;

		Dictionary<string, string> templatesList;

		public MainForm()
		{
			XamlReader.Load(this);
			string[] defaultSaveFileText = {
				"# This is the save file for New Project Creator Tool",
				"save_location = \"\""
			};
			defaultSettings = new GetSavedInfo("DefaultSettings", defaultSaveFileText);

			saveDirectoryTB.Text = defaultSettings.LoadInfo()["save_location"];

			templatesList = TemplateFinder.ExtractProjects();

			foreach (string key in templatesList.Keys)
			{
				templateSelector.Items.Add(key);
			}
		}

		protected void ShowMoreOptions(object sender, EventArgs e)
		{
			string selectedTemplate = templateSelector.Text;

			try
			{
				optionsOutput.Text = TemplateFinder.ExecuteBashCommand("dotnet new " + templatesList[selectedTemplate] + " --help");
			}
			catch
			{
				optionsOutput.Text = "Error! Choose a correct project";
			}
		}

		protected void UpdateSave(object sender, EventArgs e)
		{
			defaultSettings.SetValue("save_location", saveDirectoryTB.Text);
		}	

		protected void GenerateProject(object sender, EventArgs e)
		{
			string projectPath;

			if (projectNameTB.Text != "" && saveDirectoryTB.Text != "")
			{
				projectPath = Path.Combine(saveDirectoryTB.Text, projectNameTB.Text);

				if (Directory.Exists(projectPath))
				{
					projectPath += " New";
				}
				try
				{
					Directory.CreateDirectory(projectPath);
					optionsOutput.Text = "Project Folder Created";
				}
				catch
				{
					optionsOutput.Text = "Unknown Directory or Invalid Project Name";
				}

				string newProjectPath = projectPath;

				if (projectPath.Contains(" "))
				{
					for (int i = 0; i < projectPath.Length; i++)
					{
						//Console.WriteLine(projectPath.Length);
						if (projectPath[i] == ' ' && projectPath[i] != projectPath[0])
						{
							newProjectPath = projectPath.Insert(i, "\\");
							i++;
						}
					}
				}

				try
				{
					optionsOutput.Text = TemplateFinder.ExecuteBashCommand($"cd {newProjectPath} && dotnet new {templatesList[templateSelector.Text]} " + extraOptionsTB.Text);
				}
				catch
				{
					optionsOutput.Text = "Error found while Generating Project";
					Directory.Delete(projectPath);
				}
			}
			else
			{
				optionsOutput.Text = "Please fill in the fields";
			}
		}

		protected void QuitApp(object sender, EventArgs e)
		{
			Application.Instance.Quit();
		}
	}
}
