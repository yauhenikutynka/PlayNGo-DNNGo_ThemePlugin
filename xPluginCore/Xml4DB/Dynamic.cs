namespace DNNGo.Modules.ThemePlugin.Xml4DB
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Linq;

   internal class Dynamic
	{
		public static IEnumerable<XElement> LoadAndRules(XElement mXElement, object mObject)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Type type = mObject.GetType();
			string name = type.Name;
			PropertyInfo[] properties = type.GetProperties();
			PropertyInfo[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				object value = propertyInfo.GetValue(mObject, null);
				if (value != null)
				{
					if (propertyInfo.PropertyType == typeof(int))
					{
						if (Convert.ToInt32(value) != 0)
						{
							dictionary.Add(propertyInfo.Name, Convert.ToString(value));
						}
                    }
                    if (propertyInfo.PropertyType == typeof(Boolean?))
                    {
                        if (value != null)
                        {
                            dictionary.Add(propertyInfo.Name, Convert.ToString(value));
                        }
                    }
                    else
					{
						if (propertyInfo.PropertyType == typeof(string))
						{
							if (Convert.ToString(value) != null || Convert.ToString(value) != "")
							{
								dictionary.Add(propertyInfo.Name, Convert.ToString(value));
							}
						}
					}
				}
			}
			IEnumerable<XElement> enumerable = 
				from XmlDB in mXElement.Elements(name)
				select XmlDB;
			IEnumerable<XElement> enumerable2;
			if (dictionary.Count > 0)
			{
				using (Dictionary<string, string>.Enumerator enumerator = dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<string, string> _mValue = enumerator.Current;
						enumerable = enumerable.Where(delegate(XElement XmlDB)
						{
							KeyValuePair<string, string> mValue = _mValue;
                            string arg_2C_0 = XmlDB.Element(mValue.Key).Value;
                            return !String.IsNullOrEmpty(mValue.Value) &&( arg_2C_0 == mValue.Value || arg_2C_0.IndexOf(mValue.Value, StringComparison.CurrentCultureIgnoreCase) >=0);
                        });
					}
				}
				enumerable2 = enumerable.ToList<XElement>();
			}
			else
			{
				enumerable2 = enumerable;
				Console.WriteLine(enumerable2.Count<XElement>());
			}
			return enumerable2;
		}
		public static IEnumerable<XElement> LoadOrRules(XElement mXElement, object mObject)
		{
			IEnumerable<XElement> result = null;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Type type = mObject.GetType();
			string name = type.Name;
			PropertyInfo[] properties = type.GetProperties();
			PropertyInfo[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				if (Convert.ToString(propertyInfo.GetValue(mObject, null)) != "")
				{
					dictionary.Add(propertyInfo.Name, Convert.ToString(propertyInfo.GetValue(mObject, null)));
				}
			}
			return result;
		}
	}
}
