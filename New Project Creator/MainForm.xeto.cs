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
		GetSavedInfo defaultSettings;

		Dictionary<string, string> templatesList;

		public MainForm()
		{
			XamlReader.Load(this);
			defaultSettings = new GetSavedInfo("DefaultSettings");
			Console.WriteLine(defaultSettings.LoadInfo());
			defaultSettings.Close();

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

		protected void GenerateProject(object sender, EventArgs e)
		{

		}

		protected void QuitApp(object sender, EventArgs e)
		{
			Application.Instance.Quit();
		}
	}
}
