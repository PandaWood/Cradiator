using System;
using System.Windows;

namespace Cradiator.Model
{
	public class Skin
	{
		readonly string _name;

		public Skin(string skinName)
		{
			_name = skinName;
		}

		public string Name => _name;

		public ResourceDictionary Resource { get; set; }

		public Uri ToUri
		{
			get
			{
				var skinPath = $"./Skins/{Name}Skin.xaml";
				return new Uri(skinPath, UriKind.Relative);
			}
		}
	}
}