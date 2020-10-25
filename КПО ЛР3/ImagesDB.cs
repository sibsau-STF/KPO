using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace КПО_ЛР3
	{
	class ImagesDB : IDisposable
		{
		SqliteConnection connection;
		public ImagesDB ()
			{
			connection = new SqliteConnection("Data Source=images.db");
			connection.Open();
			createTable();
			}

		public List<DBRow> Select ()
			{
			List<DBRow> data = new List<DBRow>();
			using ( var command = connection.CreateCommand() )
				{
				command.CommandText = @"SELECT name, path, process_time, saved_date FROM Image ORDER BY name DESC";
				command.ExecuteNonQuery();
				var result = command.ExecuteReader();
				while ( result.Read() )
					{
					DBRow temp = new DBRow();
					temp.Name = result.GetString(0);
					temp.Path = result.GetString(1);
					temp.ProceedTime = result.GetInt32(2);
					temp.Saved = DateTime.Parse(result.GetString(3));
					data.Add(temp);
					}
				}
			return data;
			}

		public void Insert(DBRow row)
			{
			if ( row.Name == null || row.Saved == null )
				throw new Exception("Empty data passed");
			using ( var command = connection.CreateCommand() )
				{
				command.CommandText = @"INSERT INTO Image(name, path, process_time, saved_date) 
										VALUES($name, $path, $time, $saved)";
				command.Parameters.AddWithValue("$name", row.Name);
				command.Parameters.AddWithValue("$path", row.Path);
				command.Parameters.AddWithValue("$time", row.ProceedTime);
				command.Parameters.AddWithValue("$saved", row.Saved.ToString()).Size = 25;
				command.ExecuteNonQuery();
				}
			}

		void createTable ()
			{
			using ( var command = connection.CreateCommand() )
				{
				command.CommandText = @"
					CREATE TABLE IF NOT EXISTS Image
					(
						[id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
						[name] NVARCHAR(12) NOT NULL, 
						[path] NVARCHAR(20) NOT NULL, 
						[process_time] INT NOT NULL, 
						[saved_date] TEXT NOT NULL
					)";
				command.ExecuteNonQuery();
				}
			}

		public void Dispose ()
			{
			connection.Close();
			}
		}
	}
