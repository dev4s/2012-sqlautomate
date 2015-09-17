using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Management.Smo;

namespace SqlAutomate
{
	public static class Generate
	{
		private static Server GenServer { get; set; }
		private static string Comment(string value)
		{
			return string.Format("/************** {0} **************/", value);
		}
		public static void SetServerForGenerate(this Database db, Server server)
		{
			GenServer = server;
		}
		public static IEnumerable<string> AddDatabaseContext(this Database db)
		{
			var scr = new Scripter(GenServer) {Options = {IncludeDatabaseContext = true}};
			var resultScript = new List<string> { Comment("Adding database context") };

			resultScript.AddRange(scr.Script(new[] {db.Tables[0].Urn}).Cast<string>());

			resultScript.RemoveRange(2, resultScript.Count - 2);

			return resultScript;
		}
		public static IEnumerable<string> DropProceduresWithDatabaseContext(this Database db)
		{
			var scr = new Scripter(GenServer);
			var resultScript = new List<string> {Comment("Drop procedures")};

			scr.Options.ScriptDrops = true;
			scr.Options.IncludeIfNotExists = true;
			scr.Options.WithDependencies = true;
			foreach (var procedure in db.StoredProcedures.Cast<StoredProcedure>().Where(x => x.IsSystemObject == false))
			{
				resultScript.AddRange(scr.Script(new[] { procedure.Urn }).Cast<string>());
			}

			return resultScript;
		}
		//public static IEnumerable<string> DropProceduresWithoutDatabaseContext(this Database db)
		//{
		//    var scr = new Scripter(GenServer);
		//    var resultScript = new List<string> {Comment("Drop procedures")};

		//    scr.Options.IncludeDatabaseContext = false;
		//    scr.Options.ScriptDrops = true;
		//    scr.Options.IncludeIfNotExists = true;
		//    scr.Options.WithDependencies = true;
		//    foreach (var procedure in db.StoredProcedures.Cast<StoredProcedure>().Where(x => x.IsSystemObject == false))
		//    {
		//        resultScript.AddRange(scr.Script(new[] { procedure.Urn }).Cast<string>());
		//    }

		//    return resultScript;
		//}
		public static IEnumerable<string> DropViews(this Database db)
		{
			var scr = new Scripter(GenServer);
			var resultScript = new List<string> {Comment("Drop views")};

			scr.Options.ScriptDrops = true;
			scr.Options.IncludeIfNotExists = true;
			scr.Options.WithDependencies = true;
			foreach (var view in db.Views.Cast<View>().Where(x => x.IsSystemObject == false))
			{
				resultScript.AddRange(scr.Script(new[] { view.Urn }).Cast<string>());
			}

			return resultScript;
		}
		public static IEnumerable<string> DropTables(this Database db)
		{
			var scr = new Scripter(GenServer);
			var resultScript = new List<string> { Comment("Drop tables") };

			scr.Options.ScriptDrops = true;
			scr.Options.IncludeIfNotExists = true;
			scr.Options.WithDependencies = true; //automatically adding dependencies for views, procedures, etc.
			foreach (var table in db.Tables.Cast<Table>().Where(x => x.IsSystemObject == false))
			{
				resultScript.AddRange(scr.Script(new[] { table.Urn }).Cast<string>());
			}

			return resultScript;
		}
		public static IEnumerable<string> DropSchemas(this Database db)
		{
			var scr = new Scripter(GenServer);
			var resultScript = new List<string> { Comment("Drop schemas") };

			scr.Options.ScriptDrops = true;
			scr.Options.IncludeIfNotExists = true;
			foreach (var schema in db.Schemas.Cast<Schema>().Where(x => x.IsSystemObject == false))
			{
				resultScript.AddRange(scr.Script(new[] { schema.Urn }).Cast<string>());
			}

			return resultScript;
		}
		public static IEnumerable<string> CreateSchemas(this Database db)
		{
			var scr = new Scripter(GenServer);
			var resultScript = new List<string> { Comment("Create schemas") };

			scr.Options.ScriptDrops = false;
			scr.Options.IncludeIfNotExists = true;
			foreach (var schema in db.Schemas.Cast<Schema>().Where(x => x.IsSystemObject == false))
			{
				resultScript.AddRange(scr.Script(new[] { schema.Urn }).Cast<string>());
			}

			return resultScript;
		}
		public static IEnumerable<string> CreateTablesWithData(this Database db)
		{
			var scr = new Scripter(GenServer);
			var resultScript = new List<string> { Comment("Create tables + insert data") };

			scr.Options.IncludeIfNotExists = true;
			scr.Options.ScriptSchema = true;
			scr.Options.DriPrimaryKey = true;
			scr.Options.DriWithNoCheck = true;
			scr.Options.DriUniqueKeys = true;
			foreach (var table in db.Tables.Cast<Table>().Where(x => x.IsSystemObject == false))
			{
				resultScript.AddRange(scr.Script(new[] { table.Urn }).Cast<string>());

				scr.Options.ScriptData = true;
				scr.Options.ScriptSchema = false;
				resultScript.AddRange(scr.EnumScript(new[] { table.Urn }));
				scr.Options.ScriptData = false;
				scr.Options.ScriptSchema = true;
			}

			return resultScript;
		}
		public static IEnumerable<string> CreateViews(this Database db)
		{
			var scr = new Scripter(GenServer);
			var resultScript = new List<string> { Comment("Create views") };

			scr.Options.IncludeIfNotExists = true;
			scr.Options.ScriptSchema = true;
			foreach (var view in db.Views.Cast<View>().Where(x => x.IsSystemObject == false))
			{
				resultScript.AddRange(scr.Script(new[] { view.Urn }).Cast<string>());
			}

			return resultScript;
		}
		public static IEnumerable<string> CreateProcedures(this Database db)
		{
			var scr = new Scripter(GenServer);
			var resultScript = new List<string> { Comment("Create procedures") };

			scr.Options.IncludeIfNotExists = true;
			scr.Options.ScriptSchema = true;
			foreach (var procedure in db.StoredProcedures.Cast<StoredProcedure>().Where(x => x.IsSystemObject == false))
			{
				resultScript.AddRange(scr.Script(new[] { procedure.Urn }).Cast<string>());
			}

			return resultScript;
		}
		public static IEnumerable<string> CreateKeys(this Database db)
		{
			var scr = new Scripter(GenServer);
			var resultScript = new List<string> { Comment("Create Defaults and FK") };

			//Defaults
			scr.Options.DriDefaults = true;
			scr.Options.IncludeIfNotExists = true;
			foreach (var table in db.Tables.Cast<Table>().Where(x => x.IsSystemObject == false))
			{
				resultScript.AddRange(scr.Script(new[] { table.Urn }).Cast<string>());
			}

			//FK's
			scr.Options.DriDefaults = false;
			scr.Options.DriForeignKeys = true;
			scr.Options.IncludeIfNotExists = true;
			scr.Options.SchemaQualifyForeignKeysReferences = true;
			foreach (var table in db.Tables.Cast<Table>().Where(x => x.IsSystemObject == false))
			{		
				resultScript.AddRange(scr.Script(new[] { table.Urn }).Cast<string>());
			}

			#region Cleanings and Repairs
			for (var i = 0; i < resultScript.Count; i++)
			{
				if (resultScript[i].Contains("CREATE TABLE"))
				{
					resultScript.RemoveAt(i);
					--i;
				}
				else if (resultScript[i].Contains("SET ANSI_NULLS"))
				{
					resultScript.RemoveAt(i);
					--i;
				}
				else if (resultScript[i].Contains("SET QUOTED_IDENTIFIER"))
				{
					resultScript.RemoveAt(i);
					--i;
				}
				else if (resultScript[i].Contains("dbo.sysobjects"))
				{
					var regex = new Regex("ALTER TABLE (?<table_name>\\[.*\\]) ADD  CONSTRAINT (?<default_name>\\[.*\\])  DEFAULT ");
					var result = regex.Match(resultScript[i]);
					var tableName = result.Groups["table_name"].Value;
					var tableDefault = result.Groups["default_name"].Value;
					var stringToAdd = string.Format("IF NOT EXISTS (SELECT * FROM sys.default_constraints WHERE object_id = OBJECT_ID(N'{0}') AND parent_object_id = OBJECT_ID(N'{1}')) \r\nBEGIN \r\n{2} \r\nEND \r\n", tableDefault, tableName, resultScript[i]);
					resultScript.RemoveAt(i);
					resultScript.Insert(i, stringToAdd);

				}
			}
			#endregion
			return resultScript;
		}
		//public static List<string> Cleanings(this Database db, List<string> resultScript)
		//{
		//    //cleanings and repairs
		//    var listOfSchemas = db.Schemas.Cast<Schema>().Where(x => x.IsSystemObject == false).ToList();
		//    Schema schema;

		//    if (listOfSchemas.Count() == 1)
		//    {
		//        schema = listOfSchemas[0];
		//    }
		//    else
		//    {
		//        schema = listOfSchemas[0];
		//    }

		//    if (schema != null)
		//    {

		//        for (var i = 0; i < resultScript.Count; i++)
		//        {
		//            const string name = "REFERENCES ";
		//            if (!resultScript[i].Contains(name)) continue;

		//            var nameToInsert = string.Format("{0}{1}.", name, schema);
		//            var index = resultScript[i].IndexOf("REFERENCES ");

		//            resultScript[i] = resultScript[i].Remove(index, name.Length).Insert(index, nameToInsert);
		//        }
		//    }

		//    return resultScript;
		//}
	}
}