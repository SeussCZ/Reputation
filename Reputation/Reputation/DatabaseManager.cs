using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;

namespace LuxarPL.Reputation
{
    public class DatabaseManager
    {
        internal DatabaseManager()
        {
            new I18N.West.CP1250();
            CheckSchema();
        }

        private MySqlConnection createConnection()
        {
            MySqlConnection connection = null;
            try
            {
                if (Reputation.Instance.Configuration.Instance.DatabasePort == 0) Reputation.Instance.Configuration.Instance.DatabasePort = 3306;
                connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", Reputation.Instance.Configuration.Instance.DatabaseAddress, Reputation.Instance.Configuration.Instance.DatabaseName, Reputation.Instance.Configuration.Instance.DatabaseUsername, Reputation.Instance.Configuration.Instance.DatabasePassword, Reputation.Instance.Configuration.Instance.DatabasePort));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return connection;
        }

        public int AddRep(string id, int increaseBy)
        {
            int output = 0;
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "update `" + Reputation.Instance.Configuration.Instance.DatabaseTableName + "` set `Reputation` = Reputation + (" + increaseBy + ") where `steamId` = '" + id.ToString() + "'; select `Reputation` from `" + Reputation.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + id.ToString() + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) int.TryParse(result.ToString(), out output);
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return output;
        }

        public int RemoveRep(string id, int increaseBy)
        {
            int output = 0;
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "update `" + Reputation.Instance.Configuration.Instance.DatabaseTableName + "` set `Reputation` = Reputation - (" + increaseBy + ") where `steamId` = '" + id.ToString() + "'; select `Reputation` from `" + Reputation.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + id.ToString() + "'";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) int.TryParse(result.ToString(), out output);
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return output;
        }

        public string GetGroup(string id)
        {
            string output = "x";
            int rep = GetReputation(id);
            int i = 0;
            int s = 0;
            Array.Sort(Reputation.Instance.Configuration.Instance.Groups, delegate (Group group1, Group group2) {
                return group1.NeededRep.CompareTo(group2.NeededRep);
            });
            while (i < Reputation.Instance.Configuration.Instance.Groups.Length)
            {
                Group group = Reputation.Instance.Configuration.Instance.Groups[s];
                if (group.NeededRep <= 0)
                {
                    if (rep <= group.NeededRep)
                    {
                        i = Reputation.Instance.Configuration.Instance.Groups.Length + 1;
                        output = group.Name;
                    }
                    else
                    {
                        i++;
                        s++;
                    }
                }
                else
                {
                    if (rep >= group.NeededRep)
                    {
                        i++;
                        s++;
                    }
                    else
                    {
                        s = s - 1;
                        group = Reputation.Instance.Configuration.Instance.Groups[s];
                        output = group.Name;
                        i = Reputation.Instance.Configuration.Instance.Groups.Length;
                    }
                }
            }
            return output;
        }

        public int GetReputation(string id)
        {
            int output = 0;
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select `Reputation` from `" + Reputation.Instance.Configuration.Instance.DatabaseTableName + "` where `steamId` = '" + id.ToString() + "';";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) int.TryParse(result.ToString(), out output);
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return output;
        }

        public void CheckSetupAccount(Steamworks.CSteamID id)
        {
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                int exists = 0;
                command.CommandText = "SELECT EXISTS(SELECT 1 FROM `" + Reputation.Instance.Configuration.Instance.DatabaseTableName + "` WHERE `steamId` ='" + id + "' LIMIT 1);";
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null) Int32.TryParse(result.ToString(), out exists);
                connection.Close();

                if (exists == 0)
                {
                    command.CommandText = "insert ignore into `" + Reputation.Instance.Configuration.Instance.DatabaseTableName + "` (Reputation,steamId) values(" + Reputation.Instance.Configuration.Instance.DefaultReputation + ",'" + id.ToString() + "')";
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

        }

        internal void CheckSchema()
        {
            try
            {
                MySqlConnection connection = createConnection();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "show tables like '" + Reputation.Instance.Configuration.Instance.DatabaseTableName + "'";
                connection.Open();
                object test = command.ExecuteScalar();

                if (test == null)
                {
                    command.CommandText = "CREATE TABLE `" + Reputation.Instance.Configuration.Instance.DatabaseTableName + "` (`steamId` varchar(32) NOT NULL,`Reputation` INT(32) NOT NULL DEFAULT '10',PRIMARY KEY (`steamId`)) ";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
