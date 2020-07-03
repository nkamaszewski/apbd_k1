using apbd_kolokwium1.DTO;
using apbd_kolokwium1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace apbd_kolokwium1.DAL
{
    public class ActionDBService : IActionDBService
    {
        private string SqlConn = "Data Source=db-mssql;Initial Catalog=s16456;Integrated Security=True";
        public ActionAndFirefigthersDTO GetActionAndFirefighters(int id)
        {
            var result = new ActionAndFirefigthersDTO()
            {
                Action = null,
                Firefighters = null
            };
           

            using (var client = new SqlConnection(SqlConn))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    command.CommandText = $"SELECT IdFirefighter, FirstName, LastName FROM Firefighter INNER JOIN " +
                        $"Firefighter_Action ON Firefigter.IdFirefighter = Firefighter_Action.IdFirefighter" +
                        $" WHERE Firefighter_Action.IdAction @IdAction)";
                    command.Parameters.AddWithValue("IdAction", id);

                    client.Open();
                    var dataReader = command.ExecuteReader();
                    
                    var firefighters = new List<Firefighter>();

                    while (dataReader.Read())
                    {
                        firefighters.Add(new Firefighter
                        {
                            IdFirefighter = int.Parse(dataReader["IdFirefighter"].ToString()),
                            FirstName = dataReader["FirstName"].ToString(),
                            LastName = dataReader["LastName"].ToString(),
                        });
                    }

                    result.Firefighters = firefighters;

                   dataReader.Close();
                    // czyszczenie parametru IdAction z linii 31, jesli ma byc pozniej uzyty
                   command.Parameters.Clear();

                   /// drugie zapytanie do bazy o sama akcje

                    command.CommandText = $"SELECT * FROM Action WHERE IdAction = @IdAction";
                    command.Parameters.AddWithValue("IdAction", id);

                    if (dataReader.Read())
                    {
                        result.Action = new ActionFirefighter()
                        {
                            IdAction = int.Parse(dataReader["IdAction"].ToString()),
                            StartTime = DateTime.Parse(dataReader["StartTime"].ToString()),
                            EndTime = DateTime.Parse(dataReader["EndTime"].ToString()),
                            NeedSpecialEquipment = Byte.Parse(dataReader["NeedSpecialEquipment"].ToString())
                        };
                    }
                }
            }
            return result; 
        }

        public StatusDTO DeleteAction(int id)
        {

            StatusDTO result = new StatusDTO()
            {
                Error = false,
                Message = ""
            };

            using (var client = new SqlConnection(SqlConn))
            {
                using (var command = new SqlCommand())
                {
                    command.Connection = client;
                    client.Open();

                    var transaction = client.BeginTransaction();
                    command.Transaction = transaction;

                    try
                    {

                        command.CommandText = $"SELECT IdAction FROM Action WHERE IdAction = @IdAction AND EndTime IS NULL";
                        command.Parameters.AddWithValue("IdAction", id);

                        var dataReader = command.ExecuteReader();

                        if (!dataReader.Read())
                        {
                            result.Error = true;
                            result.Message = "Nie istnieje akcja o podanym id lub nie jest w trakcie";
                            return result;
                        }
                        dataReader.Close();

                        command.CommandText = $"DELETE FROM Firefighter_Action WHERE IdAction = @IdAction";
                        command.ExecuteNonQuery();

                        command.CommandText = $"DELETE FROM Action WHERE IdAction = @IdAction";
                        command.ExecuteNonQuery();

                        transaction.Commit();

                        result.Error = false;
                        result.Message = "Usunieto akcje o podanym id";
                    }
                    catch (SqlException exc) {
                        transaction.Rollback();
                        result.Error = true;
                        result.Message = "wystapil blad w bazie danych";
                    }

                    return result;
                }
            }
        }
    }
}
