using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Serialization.Xaml;

namespace New_Project_Creator
{	
	public class MainForm : Form
	{
		ComboBox templateSelector;
		TextArea optionsOutput;
		TextBox saveDirectoryTB;
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

		}

		protected void QuitApp(object sender, EventArgs e)
		{
			Application.Instance.Quit();
		}
	}
}
