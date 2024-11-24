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
    public static class Mapper_Group
    {
        public static DTO_Group ToDTO(this Model_Group modelGroup) {
            return new() {
                Id = modelGroup.Id,
                Code = modelGroup.Code,
                Title = modelGroup.Title
            };
        }
        public static Model_Group ToModel(this DTO_Group dtoGroup) {
            return new() {
                Id = dtoGroup.Id,
                Code = dtoGroup.Code,
                Title = dtoGroup.Title
            };
        }

        private static async Task<Model_Group> ToModel(string groupId) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string id = string.Empty;
                string code = string.Empty;
                string title = string.Empty;

                connection.Open();
                string query = "SELECT Top 1 Id, Code, Title FROM tbl_group WHERE Id = @id;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar).Value = groupId);

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    id = sqlData["Id"].ToString() ?? throw new InvalidCastException($"Group Id: {groupId} has no id.");
                    code = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Group Id: {groupId} has no code.");
                    title = sqlData["title"].ToString() ?? throw new InvalidCastException($"Group Id: {groupId} has no title.");
                }

                sqlData.Close();

                return new Model_Group {
                    Id = int.Parse(id),
                    Code = code,
                    Title = title
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }
        }
    
    }
}