using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SqlAutomate
{
	public static class FolderBrowserDialogExtension
	{
		// saves file with specific list of results
		public static void SaveFile(this FolderBrowserDialog dialog, IEnumerable<SqlScript.DatabaseResults> result)
		{
			foreach (var item in result)
			{
				var writer = new StreamWriter(string.Format(@"{0}\{1}.sql", dialog.SelectedPath, item.Name));

				foreach (var script in item.SqlScript)
				{
					writer.Write(script);
					writer.Write("\r\nGO\r\n");
				}
				writer.Close();
			}
		}
	}
}