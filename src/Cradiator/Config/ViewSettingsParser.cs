using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Cradiator.Config
{
	public class ViewSettingsParser
	{
		const string ProjectRegex = "project-regex";
		const string CategoryRegex = "category-regex";
		const string ServerRegex = "server-regex";
		const string Url = "url";
		const string Skin = "skin";
		const string ViewName = "name";
		const string ShowOnlyBroken = "showOnlyBroken";
		const string ShowServerName = "showServerName";
		const string ShowOutOfDate = "showOutOfDate";
		const string OutOfDateDifferenceInMinutes = "outOfDateDifferenceInMinutes";


		readonly XDocument _xdoc;

		public ViewSettingsParser(TextReader xml)
		{
			_xdoc = XDocument.Parse(xml.ReadToEnd());
		}

		public static ICollection<ViewSettings> Read(string xmlFile)
		{
			using (var stream = new StreamReader(xmlFile))
			{
				var reader = new ViewSettingsParser(stream);
				return reader.ParseXml();
			}
		}

		public ICollection<ViewSettings> ParseXml()
		{
			return new ReadOnlyCollection<ViewSettings>(
				(from view in _xdoc.Elements("configuration")
								   .Elements("views")
								   .Elements("view")
				 let showOnlyBroken = view.Attribute(ShowOnlyBroken)
				 let showServerName = view.Attribute(ShowServerName)
				 let showOutOfDate = view.Attribute(ShowOutOfDate)
				 let outOfDateDifferenceInMinutes = view.Attribute(OutOfDateDifferenceInMinutes)
				 select new ViewSettings
				{		// this not checking-for-null ultimately means (1 - I'm lazy, 2 - they're mandatory by laziness)
					URL = view.Attribute(Url).Value,
					ProjectNameRegEx = view.Attribute(ProjectRegex).Value,
					CategoryRegEx = view.Attribute(CategoryRegex).Value,
					ServerNameRegEx = view.Attribute(ServerRegex).Value,
					SkinName = view.Attribute(Skin).Value,
					ViewName = view.Attribute(ViewName).Value,
					ShowOnlyBroken = showOnlyBroken != null && bool.Parse(showOnlyBroken.Value),
					ShowServerName = showServerName != null && bool.Parse(showServerName.Value),
					ShowOutOfDate = showOutOfDate != null && bool.Parse(showOutOfDate.Value),
					OutOfDateDifferenceInMinutes = outOfDateDifferenceInMinutes != null ? int.Parse(outOfDateDifferenceInMinutes.Value) : 0
				}).ToList());
		}

		//-----
		// modify functionality (below) is only for the settings dialog save functionality
		//-----

		public static void Modify(string xmlFile, ViewSettings viewSettings)
		{
			string xmlUpdated;
			using (var stream = new StreamReader(xmlFile))
			{
				var parser = new ViewSettingsParser(stream);
				xmlUpdated = parser.CreateUpdatedXml(viewSettings);
			}
			using (var stream = new StreamWriter(xmlFile))
			{
				stream.Write(xmlUpdated);
			}
		}

		public string CreateUpdatedXml(IViewSettings settings)
		{
			var view1 = _xdoc.Elements("configuration")
							 .Elements("views")
							 .Elements("view").First(); // only used to update a view when there is 1

			var showOnlyBroken = view1.Attribute(ShowOnlyBroken);
			var showServerName = view1.Attribute(ShowServerName);
			var showOutOfDate = view1.Attribute(ShowOutOfDate);
			var outOfDateDifferenceInMinutes = view1.Attribute(OutOfDateDifferenceInMinutes);

			view1.Attribute(Url).Value = settings.URL;
			view1.Attribute(ProjectRegex).Value = settings.ProjectNameRegEx;
			view1.Attribute(CategoryRegex).Value = settings.CategoryRegEx;
			view1.Attribute(ServerRegex).Value = settings.ServerNameRegEx;
			view1.Attribute(Skin).Value = settings.SkinName;
			view1.Attribute(ViewName).Value = settings.ViewName;
			if (showOnlyBroken != null) showOnlyBroken.Value = settings.ShowOnlyBroken.ToString();
			if (showServerName != null) showServerName.Value = settings.ShowServerName.ToString();
			if (showOutOfDate != null) showOutOfDate.Value = settings.ShowOutOfDate.ToString();
			if (outOfDateDifferenceInMinutes != null) outOfDateDifferenceInMinutes.Value = settings.OutOfDateDifferenceInMinutes.ToString();

			var xml = new StringBuilder();
			using (var xmlWriter = XmlWriter.Create(xml, new XmlWriterSettings
									{
										OmitXmlDeclaration = true,
										NewLineHandling = NewLineHandling.None,
										Indent = true,
									}))
			{
				_xdoc.WriteTo(xmlWriter);
			}
			return xml.ToString();
		}
	}
}