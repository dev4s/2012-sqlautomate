using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace SqlAutomate
{
	public class SqlScript
	{
		private readonly ServerConnection _serverConnection = new ServerConnection { ServerInstance = @"" };
		private readonly Server _server;
		
		// connection to server when creating class
		public SqlScript()
		{
			_server = new Server(_serverConnection);
		}

		// get database name with specific type
		public IEnumerable<string> DatabaseNames
		{
			get
			{
				return
					_server.Databases.Cast<Database>().Where(
						x => x.Name != "master" && 
							 x.Name != "model" && 
							 x.Name != "msdb" && 
							 x.Name != "tempdb").
							 Select(x => x.Name);
			}
		}

		// generates sql script for (almost) all things in selected databases
		public List<DatabaseResults> GenerateSqlScript(IEnumerable<string> selectedItems)
		{
			var databaseResult = new List<DatabaseResults>();
			var resultScript = new List<string>();

			foreach (var db in from string item in selectedItems select _server.Databases[item])
			{
				db.SetServerForGenerate(_server);

				//TODO: logins, extended stored procedures, triggers, synonims, rules, roles
				//add database context
				resultScript.AddRange(db.AddDatabaseContext());

				//drop procedures
				resultScript.AddRange(db.DropProceduresWithDatabaseContext());
				//drop views
				resultScript.AddRange(db.DropViews());
				//drop tables
				resultScript.AddRange(db.DropTables());
				//drop schemas
				resultScript.AddRange(db.DropSchemas());

				//add schemas
				resultScript.AddRange(db.CreateSchemas());
				//add tables with data
				resultScript.AddRange(db.CreateTablesWithData());
				//add views
				resultScript.AddRange(db.CreateViews());
				//add procedures
				resultScript.AddRange(db.CreateProcedures());
				//add PK and FK
				resultScript.AddRange(db.CreateKeys());

				//cleanings
				//resultScript = db.Cleanings(resultScript);
				
				databaseResult.Add(new DatabaseResults { Name = db.Name, SqlScript = resultScript });
				resultScript = new List<string>();
			}

			return databaseResult;
		}

		// disconnects from server
		public void Disconnect()
		{
			_serverConnection.Disconnect();
		}

		// used for file name (get from database), and script file
		public class DatabaseResults
		{
			public string Name { get; set; }
			public List<string> SqlScript { get; set; }
		}
	}
}