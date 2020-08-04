using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
	static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	static char[] TRIM_CHARS = { '\"' };

	public static List<Event> Read(string file)
	{
		var list = new List<Event>();
		TextAsset data = Resources.Load(file) as TextAsset;

		var lines = Regex.Split(data.text, LINE_SPLIT_RE);

		if (lines.Length <= 1) return list;

		var header = Regex.Split(lines[0], SPLIT_RE);
		for (var i = 1; i < lines.Length; i++)
		{

			var values = Regex.Split(lines[i], SPLIT_RE);
			if (values.Length == 0 || values[0] == "") continue;

			var entry = new Event();
			for (var j = 0; j < header.Length && j < values.Length; j++)
			{
				string value = values[j];
				value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
				object finalvalue = value;
				var propInfo = entry.GetType().GetProperty(header[j]);
				switch (header[j])
				{
					case "time":
						float f;
						float.TryParse(value, out f);
						propInfo.SetValue(entry, f);
						break;
					case "eventName":
						try
						{
							string eveName = (string)finalvalue;
							EventName eventEnum = (EventName)Enum.Parse(typeof(EventName), eveName);
							propInfo.SetValue(entry, eventEnum);
						} catch (Exception e) {
							propInfo.SetValue(entry, EventName.invalid);
						}
						break;
					case "floorNum":
						int floorNum;
						int.TryParse(value, out floorNum);
						propInfo.SetValue(entry, floorNum);
						break;
					case "eleNum":
						int eleNum;
						if (int.TryParse(value, out eleNum)) propInfo.SetValue(entry, eleNum);
						else propInfo.SetValue(entry, null);
						break;
					case "eleDir":
						try
						{
							string eleDir = (string)finalvalue;
							EleDirection dirEnum = (EleDirection)Enum.Parse(typeof(EleDirection), eleDir);
							propInfo.SetValue(entry, dirEnum);
						}
						catch (Exception e)
						{
							propInfo.SetValue(entry, EleDirection.invalid);
						}
						break;
					case "clinicNum":
						int clinicNum;
						if (int.TryParse(value, out clinicNum)) propInfo.SetValue(entry, clinicNum);
						else propInfo.SetValue(entry, null);
						break;
					case "newVal":
						int newVal;
						int.TryParse(value, out newVal);
						propInfo.SetValue(entry, newVal);
						break;
					default:
						Debug.Log("Invalid CSV Entry: " + header[j] + ":" + finalvalue);
							break;

				}
			}
			list.Add(entry);
		}
		return list;
	}
}