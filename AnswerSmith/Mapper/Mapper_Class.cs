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
    public static class Mapper_Class
    {
        /// <summary>
        /// Converts a <see cref="Model_Class"/> to a <see cref="DTO_Class"/> model.
        /// </summary>
        /// <param name="modelClass">Instance of <see cref="Model_Class"/></param>
        /// <returns>A instance of <see cref="DTO_Class"/> with other data derived from <paramref name="modeClass"/>.</returns>
        public static DTO_Class ToDto(this Model_Class modelClass) {
            return new DTO_Class {
                Code = modelClass.Code,
                Name = modelClass.Name,
                IsActive = modelClass.IsActive
            };
        }

        /// <summary>
        /// Converts a <see cref="DTO_Class"/> to a <see cref="Model_Class"/> model.
        /// </summary>
        /// <param name="dtoClass">Instance of <see cref="DTO_Class"/></param>
        /// <returns>A instance of <see cref="Model_Class"/> with other data derived from <paramref name="dtoClass"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Class> ToModel(this DTO_Class dtoClass) {
            return await NewModel(dtoClass.Code);
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a <see cref="Model_Class"/> model.
        /// </summary>
        /// <param name="classCode">Instance of <see cref="string"/></param>
        /// <returns>A instance of <see cref="Model_Class"/> with other data derived from <paramref name="classCode"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Class> ToModel(string classCode) {
            return await NewModel(classCode);
        }

        private static async Task<Model_Class> NewModel(string classCode) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string id = String.Empty;
                string code = String.Empty;
                string name = String.Empty;
                string isActive = String.Empty;

                connection.Open();
                string query = "SELECT Top 1 Id, Code, Name, IsActive FROM tbl_class WHERE Code = @code;";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar) { Value = classCode} );

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                if (!sqlData.HasRows) { throw new InvalidDataException($"There is no class with {classCode} code."); }

                int softBreaker = 0;

                while (sqlData.Read()) {
                    softBreaker += 1;
                    id = sqlData["Id"].ToString() ?? throw new InvalidCastException($"Class Code: {classCode} has no id.");
                    code = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Class Code: {classCode} has no code.");
                    name = sqlData["Name"].ToString() ?? throw new InvalidCastException($"Class Code: {classCode} has no name.");
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Class Code: {classCode} has no active / inactive status.");
                }

                if (softBreaker != 1) { throw new InvalidDataException($"There are multiple classes with '{classCode}' code."); }

                sqlData.Close();

                return new Model_Class {
                    Id = Int32.Parse(id),
                    Code = code,
                    Name = name,
                    IsActive = Int32.Parse(isActive) > 0
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }
        }
    }
}