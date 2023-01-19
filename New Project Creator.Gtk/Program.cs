using System;
using Eto.Forms;

namespace New_Project_Creator.Gtk
{
	class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{
			var app = new Application(Eto.Platforms.Gtk);

			var MainPage = new MainForm();

			app.Run(MainPage);
		}
	}
}
