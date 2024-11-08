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
    public static class Mapper_Question {
        /// <summary>
        /// Converts a <see cref="Model_Question"/> to a <see cref="DTO_Question"/> model.
        /// </summary>
        /// <param name="modelQuestion">Instance of <see cref="Model_Question"/></param>
        /// <returns>A Instance of <see cref="DTO_Question"/> with data derived from <paramref name="modelQuestion"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<DTO_Question> ToDTO(this Model_Question modelQuestion) {

            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string code = string.Empty;
                string cCode = string.Empty;
                string? parent = string.Empty;
                string? parentCode = string.Empty;
                string? parentName = string.Empty;
                string question = string.Empty;
                string isActive = string.Empty;

                connection.Open();
                string query = "SELECT Code, C_Code, Parent, Parent_Code, Parent_Name, Question, IsActive FROM fn_question(NULL) WHERE Code = @code;";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar).Value = modelQuestion.Code);

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    code = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Question Code: {modelQuestion.Code} has no code.");
                    cCode = sqlData["C_Code"].ToString() ?? throw new InvalidCastException($"Question Code: {modelQuestion.Code} has no code.");
                    parent = sqlData["Parent"].ToString();
                    parentCode = sqlData["Parent_Code"].ToString();
                    parentName = sqlData["Parent_Name"].ToString() ?? throw new InvalidCastException($"Question Code: {modelQuestion.Code} has no class code.");
                    question = sqlData["Question"].ToString() ?? throw new InvalidCastException($"Question Code: {modelQuestion.Code} has no name.");
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Question Code: {modelQuestion.Code} has no active / inactive status.");
                }

                sqlData.Close();

                return new DTO_Question {
                    Code = code,
                    C_Code = cCode,
                    Parent = parent,
                    Parent_Code = parentCode,
                    Parent_Name = parentName,
                    Question = question,
                    IsActive = int.Parse(isActive) > 0
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Converts a <see cref="Model_Question"/> to a <see cref="DTO_Question_Detail"/> model.
        /// </summary>
        /// <param name="modelQuestion">Instance of <see cref="Model_Question"/></param>
        /// <returns>A Instance of <see cref="DTO_Question_Detail"/> with data derived from <paramref name="modelQuestion"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<DTO_Question_Detail> ToDto_Detail(this Model_Question modelQuestion) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string code = string.Empty;
                string cCode = string.Empty;
                string? parent = string.Empty;
                string? parentCode = string.Empty;
                string? parentName = string.Empty;
                string question = string.Empty;
                string? biasImportance = string.Empty;
                string? biasAnswerSize = string.Empty;
                string? biasDate = string.Empty;
                string? biasWeight = string.Empty;
                string isActive = string.Empty;

                connection.Open();
                string query = "SELECT Code, C_Code, Parent, Parent_Code, Parent_Name, Question, Bias_Importance, AnswerSize, Bias_Date, Bias_Weight, IsActive FROM fn_question(NULL) WHERE Code = @code;";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar).Value = modelQuestion.Code);

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    code = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Question Code: {modelQuestion.Code} has no code.");
                    cCode = sqlData["C_Code"].ToString() ?? throw new InvalidCastException($"Question Code: {modelQuestion.Code} has no common code.");
                    parent = sqlData["Parent"].ToString();
                    parentCode = sqlData["Parent_Code"].ToString();
                    parentName = sqlData["Parent_Name"].ToString();
                    question = sqlData["Question"].ToString() ?? throw new InvalidCastException($"Question Code: {modelQuestion.Code} has no Question.");
                    biasImportance = sqlData["Bias_Importance"].ToString();
                    biasAnswerSize = sqlData["AnswerSize"].ToString();
                    biasDate = sqlData["IsActive"].ToString();
                    biasWeight = sqlData["IsActive"].ToString();
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Question Code: {modelQuestion.Code} has no active / inactive status.");
                }

                sqlData.Close();

                return new DTO_Question_Detail {
                    Code = code,
                    C_Code = cCode,
                    Parent = parent,
                    Parent_Code = parentCode,
                    Parent_Name = parentName,
                    Question = question,
                    Bias_Importance = string.IsNullOrEmpty(biasImportance) ? null: short.Parse(biasImportance ?? "0"),
                    Bias_AnswerSize = string.IsNullOrEmpty(biasAnswerSize) ? null: short.Parse(biasAnswerSize ?? "0"),
                    Bias_Date = string.IsNullOrEmpty(biasDate) ? null : DateTime.Parse(biasDate ?? DateTime.Today.ToString()),
                    Bias_Weight = string.IsNullOrEmpty(biasAnswerSize) ? null: short.Parse(biasAnswerSize ?? "0"),
                    IsActive = int.Parse(isActive) > 0
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }

        }

        /// <summary>
        /// Converts a <see cref="Model_Question"/> to a <see cref="DTO_Question_Summary"/> model.
        /// </summary>
        /// <param name="modelQuestion">Instance of <see cref="Model_Question"/></param>
        /// <returns>A Instance of <see cref="DTO_Question_Summary"/> with data derived from <paramref name="modelQuestion"/>.</returns>    
        public static DTO_Question_Summary ToDto_Summary(this Model_Question modelQuestion) {
            return new DTO_Question_Summary {
                Code = modelQuestion.Code,
                C_Code = modelQuestion.C_Code,
                Question = modelQuestion.Question,
                IsActive = modelQuestion.IsActive
            };
        }

        /// <summary>
        /// Converts a <see cref="DTO_Question"/> to a <see cref="Model_Question"/> model.
        /// </summary>
        /// <param name="dtoQuestion">Instance of <see cref="DTO_Question"/></param>
        /// <returns>A instance of <see cref="Model_Question"/> with other data derived from <paramref name="dtoQuestion"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Question> ToModel(this DTO_Question dtoQuestion) {
            return await NewModel(dtoQuestion.Code);
        }

        /// <summary>
        /// Converts a <see cref="DTO_Question_Summary"/> to a <see cref="Model_Question"/> model.
        /// </summary>
        /// <param name="dtoQuestion_Summary">Instance of <see cref="DTO_Question_Summary"/></param>
        /// <returns>A instance of <see cref="Model_Question"/> with other data derived from <paramref name="dtoQuestion_Summary"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Question> ToModel(this DTO_Question_Summary dtoQuestion_Summary) {
            return await NewModel(dtoQuestion_Summary.Code);
        }

        /// <summary>
        /// Converts a <see cref="DTO_Question_Detail"/> to a <see cref="Model_Question"/> model.
        /// </summary>
        /// <param name="dtoQuestion_Detail">Instance of <see cref="DTO_Question_Detail"/></param>
        /// <returns>A instance of <see cref="Model_Question"/> with other data derived from <paramref name="dtoQuestion_Detail"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Question> ToModel(this DTO_Question_Detail dtoQuestion_Detail) {
            return await NewModel(dtoQuestion_Detail.Code);
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a <see cref="Model_Question"/> model.
        /// </summary>
        /// <param name="QuestionCode">Instance of <see cref="string"/></param>
        /// <returns>A instance of <see cref="Model_Question"/> with other data derived from <paramref name="QuestionCode"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Question> ToModel(string questionCode) {
            return await NewModel(questionCode);
        }

        private static async Task<Model_Question> NewModel(string questionCode) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string id = string.Empty;
                string code = string.Empty;
                string cCode = string.Empty;
                string? parent = string.Empty;
                string? parentId = string.Empty;
                string question = string.Empty;
                string? biasImportance = string.Empty;
                string? biasAnswerSize = string.Empty;
                string? biasDate = string.Empty;
                string? biasWeight = string.Empty;
                string isActive = string.Empty;

                connection.Open();
                string query = "SELECT Id, Code, C_Code, Parent, Parent_Id, Question, Bias_Importance, AnswerSize, Bias_Date, Bias_Weight, IsActive FROM fn_question(NULL) WHERE Code = @code;";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar).Value = questionCode);

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    id = sqlData["Id"].ToString() ?? throw new InvalidCastException($"Question Code: {questionCode} has no Id.");
                    code = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Question Code: {questionCode} has no code.");
                    cCode = sqlData["C_Code"].ToString() ?? throw new InvalidCastException($"Question Code: {questionCode} has no common code.");
                    parent = sqlData["Parent"].ToString();
                    parentId = sqlData["Parent_Id"].ToString();
                    question = sqlData["Question"].ToString() ?? throw new InvalidCastException($"Question Code: {questionCode} has no Question.");
                    biasImportance = sqlData["Bias_Importance"].ToString();
                    biasAnswerSize = sqlData["AnswerSize"].ToString();
                    biasDate = sqlData["IsActive"].ToString();
                    biasWeight = sqlData["IsActive"].ToString();
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Question Code: {questionCode} has no active / inactive status.");
                }

                sqlData.Close();

                return new Model_Question {
                    Id = int.Parse(id),
                    Code = code,
                    C_Code = cCode,
                    Parent = parent,
                    Parent_Id = string.IsNullOrEmpty(parentId) ? null : int.Parse(parentId),
                    Question = question,
                    Bias_Importance = string.IsNullOrEmpty(biasImportance) ? null: short.Parse(biasImportance ?? "0"),
                    Bias_AnswerSize = string.IsNullOrEmpty(biasAnswerSize) ? null: short.Parse(biasAnswerSize ?? "0"),
                    Bias_Date = string.IsNullOrEmpty(biasDate) ? null : DateTime.Parse(biasDate ?? DateTime.Today.ToString()),
                    Bias_Weight = string.IsNullOrEmpty(biasAnswerSize) ? null: short.Parse(biasAnswerSize ?? "0"),
                    IsActive = int.Parse(isActive) > 0
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }
        }
        
    }
}