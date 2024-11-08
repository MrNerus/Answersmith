using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.Data;
using AnswerSmith.DTOs;
using AnswerSmith.Model;
using Microsoft.Data.SqlClient;

namespace AnswerSmith.Mapper
{
    public static class Mapper_Marks
    {
        /// <summary>
        /// Converts a <see cref="Model_Marks"/> to a <see cref="DTO_Marks"/> model.
        /// </summary>
        /// <param name="modelMarks">Instance of <see cref="Model_Marks"/></param>
        /// <returns>A instance of <see cref="DTO_Marks"/> with other data derived from <paramref name="modelMarks"/>.</returns>
        public static async Task<DTO_Marks> ToDto(this Model_Marks modelMarks) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string id = string.Empty;
                string qsId = string.Empty;
                string qsCode = string.Empty;
                string marks = string.Empty;
                string isActive = string.Empty;

                connection.Open();
                string query = "SELECT Top 1 m.Id Id, m.QS_Id QS_Id, q.Code QS_Code, m.Marks Marks, m.IsActive IsActive FROM tbl_Marks m JOIN tbl_question q ON m.QS_Id = q.Id WHERE m.Id = @id;";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar).Value = modelMarks.Id);

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    id = sqlData["Id"].ToString() ?? throw new InvalidCastException($"Marks Id: {modelMarks.Id} has no id.");
                    qsId = sqlData["QS_Id"].ToString() ?? throw new InvalidCastException($"Marks Id: {modelMarks.Id} has no Question Id.");
                    qsCode = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Marks Id: {modelMarks.Id} has no Cuestion Code.");
                    marks = sqlData["Marks"].ToString() ?? throw new InvalidCastException($"Marks Id: {modelMarks.Id} has no name.");
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Marks Id: {modelMarks.Id} has no active / inactive status.");
                }

                sqlData.Close();

                return new DTO_Marks {
                    Id = int.Parse(id),
                    QS_Id = int.Parse(qsId),
                    QS_Code = qsCode,
                    Marks = float.Parse(marks),
                    IsActive = int.Parse(isActive) > 0
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Converts a <see cref="DTO_Marks"/> to a <see cref="Model_Marks"/> model.
        /// </summary>
        /// <param name="dtoMarks">Instance of <see cref="DTO_Marks"/></param>
        /// <returns>A instance of <see cref="Model_Marks"/> with other data derived from <paramref name="dtoMarks"/>.</returns>
        public static async Task<Model_Marks> ToDto(this DTO_Marks dtoMarks) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string id = string.Empty;
                string qsId = string.Empty;
                string marks = string.Empty;
                string isActive = string.Empty;

                connection.Open();
                string query = "SELECT Top 1 m.Id Id, m.QS_Id QS_Id, m.Marks Marks, m.IsActive IsActive FROM tbl_Marks m JOIN tbl_question q ON m.QS_Id = q.Id WHERE m.Id = @id;";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar).Value = dtoMarks.Id);

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    id = sqlData["Id"].ToString() ?? throw new InvalidCastException($"Marks Id: {dtoMarks.Id} has no id.");
                    qsId = sqlData["QS_Id"].ToString() ?? throw new InvalidCastException($"Marks Id: {dtoMarks.Id} has no Question Id.");
                    marks = sqlData["Marks"].ToString() ?? throw new InvalidCastException($"Marks Id: {dtoMarks.Id} has no name.");
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Marks Id: {dtoMarks.Id} has no active / inactive status.");
                }

                sqlData.Close();

                return new Model_Marks {
                    Id = int.Parse(id),
                    QS_Id = int.Parse(qsId),
                    Marks = float.Parse(marks),
                    IsActive = int.Parse(isActive) > 0
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }
        }
    }
}