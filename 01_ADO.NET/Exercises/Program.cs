using MinionsDBExercises.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MinionsDBExercises
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // 01. Initial Setup
            using SqlConnection connection = new SqlConnection(Config.ConnectionString);

            connection.Open();

            /*// 02. Villain Names
            string villianNames = GetVillainNames(connection);
            Console.WriteLine(villianNames);

            // 03. Minion Names
            int villlainId = int.Parse(Console.ReadLine());
            string minionNames = GetMinionNames(connection, villlainId);
            Console.WriteLine(minionNames);*/

            // 04. Add Minion
            /*string[] minionInfo = Console.ReadLine()
                .Split(new string[] { " ", ": " }, StringSplitOptions.RemoveEmptyEntries);

            string[] villainInfo = Console.ReadLine()
                .Split(": ", StringSplitOptions.RemoveEmptyEntries);

            string result = AddMinion(connection, minionInfo, villainInfo);

            Console.WriteLine(result);*/

            // 05. Change Town Names Casing
            string countryName = Console.ReadLine();

            string result = ChangeTownNamesCasing(connection, countryName);

            Console.WriteLine(result);

            connection.Close();
        }

        private static string GetVillainNames(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();

            string villianNamesQuery = @"SELECT
                                            v.[Name],
                                            COUNT(mv.MinionId) AS [Count]
                                         FROM Villains AS v
                                         INNER JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                                         GROUP BY v.[Name]
                                         HAVING COUNT(mv.MinionId) > 3
                                         ORDER BY [Count] DESC";

            SqlCommand getVillianNamesCmd = new SqlCommand(villianNamesQuery, connection);

            using SqlDataReader reader = getVillianNamesCmd.ExecuteReader();

            while (reader.Read())
            {
                sb.AppendLine($"{reader["Name"]} - {reader["Count"]}");
            }

            return sb.ToString().TrimEnd();
        }
        
        private static string GetMinionNames(SqlConnection connection, int villainId)
        {
            StringBuilder sb = new StringBuilder();

            string getVillainNameQuery = @"SELECT 
                                              [Name] 
                                           FROM Villains
                                           WHERE Id = @villainId";

            SqlCommand getVillianNameByIdCmd = new SqlCommand(getVillainNameQuery, connection);
            getVillianNameByIdCmd.Parameters.Add("@villainId", SqlDbType.Int);
            getVillianNameByIdCmd.Parameters["@villainId"].Value = villainId;

            string villainName = (string)getVillianNameByIdCmd.ExecuteScalar();

            if (villainName == null)
            {
                return $"No villain with ID {villainId} exists in the database.";
            }
                
            sb.AppendLine($"Villain: {villainName}");

            string getVillainMinionsQuery = @"SELECT 
	                                             m.[Name] AS MinionName,
	                                             m.Age
                                              FROM MinionsVillains AS mv
                                              INNER JOIN Minions AS m ON mv.MinionId = m.Id
                                              WHERE mv.VillainId = @villainId
                                              ORDER BY m.[Name] ASC";

            SqlCommand getVillainMinionsCmd = new SqlCommand(getVillainMinionsQuery, connection);
            getVillainMinionsCmd.Parameters.Add("@villainId", SqlDbType.Int);
            getVillainMinionsCmd.Parameters["@villainId"].Value = villainId;

            using SqlDataReader minionsReader = getVillainMinionsCmd.ExecuteReader();

            if (!minionsReader.HasRows)
            {
                sb.AppendLine($"(no minions)");

                return sb.ToString().TrimEnd();
            }

            int currRow = 1;

            while (minionsReader.Read())
            {
                sb.AppendLine($"{currRow}. {minionsReader["MinionName"]} {minionsReader["Age"]}");
                currRow++;
            }

            return sb.ToString().TrimEnd();
        }

        private static string AddMinion(SqlConnection connection, string[] minionInfo, string[] villainInfo)
        {
            StringBuilder sb = new StringBuilder();

            string minionName = minionInfo[1];
            int minionAge = int.Parse(minionInfo[2]);
            string minionTown = minionInfo[3];
            string villainName = villainInfo[1];

            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                // Towns logic
                string getMinionTownQuery = @"SELECT 
                                             Id 
                                          FROM Towns
                                          WHERE [Name] = @townName";

                SqlCommand getTownIdCmd = new SqlCommand(getMinionTownQuery, connection, transaction);
                getTownIdCmd.Parameters.AddWithValue("@townName", minionTown);

                var townId = getTownIdCmd.ExecuteScalar();

                if (townId == null)
                {
                    string insertTownQuery = @"INSERT INTO Towns([Name])
                                           VALUES (@townName)";

                    SqlCommand insertTownCmd = new SqlCommand(insertTownQuery, connection, transaction);
                    insertTownCmd.Parameters.AddWithValue("@townName", minionTown);
                    insertTownCmd.ExecuteNonQuery();

                    townId = (int)getTownIdCmd.ExecuteScalar();

                    sb.AppendLine($"Town {minionTown} was added to the database.");
                }

                // Villains logic
                string getVillainIdQuery = @"SELECT 
                                            Id 
                                         FROM Villains
                                         WHERE [Name] = @villainName";

                SqlCommand getVillainIdCmd = new SqlCommand(getVillainIdQuery, connection, transaction);
                getVillainIdCmd.Parameters.AddWithValue("@villainName", villainName);
                var villainId = getVillainIdCmd.ExecuteScalar();

                if (villainId == null)
                {
                    string getEvilnessIdQuery = @"SELECT 
                                                 Id 
                                              FROM EvilnessFactors
                                              WHERE [Name] = 'Evil'";

                    SqlCommand getEvilnessIdCmd = new SqlCommand(getEvilnessIdQuery, connection, transaction);
                    int evilnessId = (int)getEvilnessIdCmd.ExecuteScalar();

                    string insertVillainQuery = @"INSERT INTO Villains([Name], EvilnessFactorId)
                                              VALUES (@villainName, @evilnessFactorId)";

                    SqlCommand insertVillainCmd = new SqlCommand(insertVillainQuery, connection, transaction);
                    insertVillainCmd.Parameters.AddWithValue("@villainName", villainName);
                    insertVillainCmd.Parameters.AddWithValue("@evilnessFactorId", evilnessId);

                    insertVillainCmd.ExecuteNonQuery();

                    villainId = (int)getVillainIdCmd.ExecuteScalar();

                    sb.AppendLine($"Villain {villainName} was added to the database.");
                }

                // Minion logic 
                string insertMinionQuery = @"INSERT INTO Minions([Name], Age, TownId)
                                         VALUES (@minionName, @minionAge, @townId)";

                SqlCommand insertMinionCmd = new SqlCommand(insertMinionQuery, connection, transaction);
                insertMinionCmd.Parameters.AddWithValue("@minionName", minionName);
                insertMinionCmd.Parameters.AddWithValue("@minionAge", minionAge);
                insertMinionCmd.Parameters.AddWithValue("@townId", townId);

                insertMinionCmd.ExecuteNonQuery();

                string getMinionQuery = @"SELECT 
                                         Id 
                                      FROM Minions
                                      WHERE [Name] = @minionName";

                SqlCommand getMinionIdCmd = new SqlCommand(getMinionQuery, connection, transaction);
                getMinionIdCmd.Parameters.AddWithValue("@minionName", minionName);
                var minionIdResult = getVillainIdCmd.ExecuteScalar();

                string insertIntoMinionsVillainsQuery = @"INSERT INTO MinionsVillains(MinionId, VillainId)
                                                      VALUES (@minionId, @villainId)";

                SqlCommand insertIntoMinionsVillainsCmd = new SqlCommand(insertIntoMinionsVillainsQuery, connection, transaction);
                insertIntoMinionsVillainsCmd.Parameters.AddWithValue("@minionId", minionIdResult);
                insertIntoMinionsVillainsCmd.Parameters.AddWithValue("@villainId", villainId);

                sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");

                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return ex.ToString();
            }
        }

        private static string ChangeTownNamesCasing(SqlConnection connection, string countryName)
        {
            StringBuilder sb = new StringBuilder();

            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                string getTownsForCountryQuery = @"SELECT 
                                                        t.[Name] 
                                                   FROM Countries AS c
                                                   LEFT JOIN Towns AS t ON c.Id = t.CountryCode
                                                   WHERE c.[Name] = @countryName";
                
                SqlCommand getTownsForCountryCmd = new SqlCommand(getTownsForCountryQuery, connection, transaction);
                getTownsForCountryCmd.Parameters.AddWithValue("@countryName", countryName);

                using SqlDataReader reader = getTownsForCountryCmd.ExecuteReader();
                
                if (!reader.HasRows)
                {
                    return "No town names were affected.";
                }

                reader.Close();

                string updateTownNamesQuery = @"UPDATE Towns
                                                SET [Name] = UPPER([Name])
                                                WHERE CountryCode IN (
                                                                        SELECT Id FROM Countries
						                                                WHERE [Name] = @countryName
                                                                     )";

                SqlCommand updateTownNamesCmd = new SqlCommand(updateTownNamesQuery, connection, transaction);
                updateTownNamesCmd.Parameters.AddWithValue("@countryName", countryName);
                updateTownNamesCmd.ExecuteNonQuery();

                var towns = new List<string>();

                using SqlDataReader townsReader = getTownsForCountryCmd.ExecuteReader();

                while (townsReader.Read())
                {
                    towns.Add(townsReader["Name"].ToString());
                }

                townsReader.Close();

                sb.AppendLine($"{towns.Count} town names were affected.");
                sb.AppendLine($"[{string.Join(", ", towns)}]");
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return ex.ToString();
            }


           return sb.ToString().TrimEnd();
        }
    }
}
