using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.Data;
using AnswerSmith.DTOs;
using AnswerSmith.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlTypes;


namespace AnswerSmith.Mapper
{
    public static class Mapper_Subject
    {
        /// <summary>
        /// Converts a <see cref="Model_Subject"/> to a <see cref="DTO_Subject"/> model.
        /// </summary>
        /// <param name="modelSubject">Instance of <see cref="Model_Subject"/></param>
        /// <returns>A Instance of <see cref="DTO_Subject"/> with data derived from <paramref name="modelSubject"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<DTO_Subject> ToDTO(this Model_Subject modelSubject) {

            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string code = String.Empty;
                string classCode = String.Empty;
                string name = String.Empty;
                string isActive = String.Empty;

                connection.Open();
                string query = "SELECT TOP 1 s.Code Code, c.Code Class_Code, s.Name Name, s.IsActive IsActive FROM tbl_subject s LEFT JOIN tbl_class c ON s.Class_Id = c.Id WHERE s.Class_Id = @id;";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar).Value = modelSubject.Id);

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    code = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelSubject.Code} has no code.");
                    classCode = sqlData["Class_Code"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelSubject.Code} has no class code.");
                    name = sqlData["Name"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelSubject.Code} has no name.");
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelSubject.Code} has no active / inactive status.");
                }

                sqlData.Close();

                return new DTO_Subject {
                    Code = code,
                    Class_Code = classCode,
                    Name = name,
                    IsActive = Int32.Parse(isActive) > 0
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Converts a <see cref="Model_Subject"/> to a <see cref="DTO_Subject_Detail"/> model.
        /// </summary>
        /// <param name="modelSubject">Instance of <see cref="Model_Subject"/></param>
        /// <returns>A Instance of <see cref="DTO_Subject_Detail"/> with data derived from <paramref name="modelSubject"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<DTO_Subject_Detail> ToDto_Detail(this Model_Subject modelSubject) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string code = String.Empty;
                string classCode = String.Empty;
                string className = String.Empty;
                string name = String.Empty;
                string isActive = String.Empty;

                connection.Open();
                string query = "SELECT TOP 1 s.Code Code, c.Code Class_Code, c.Name Class_Name, s.Name Name, s.IsActive IsActive FROM tbl_subject s LEFT JOIN tbl_class c ON s.Class_Id = c.Id WHERE s.Class_Id = @id;";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar).Value = modelSubject.Id);

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    code = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelSubject.Code} has no code.");
                    classCode = sqlData["Class_Code"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelSubject.Code} has no class code.");
                    className = sqlData["Class_Name"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelSubject.Code} has no class name.");
                    name = sqlData["Name"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelSubject.Code} has no name.");
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelSubject.Code} has no active / inactive status.");
                }

                sqlData.Close();

                return new DTO_Subject_Detail {
                    Code = code,
                    Class_Code = classCode,
                    Class_Name = className,
                    Name = name,
                    IsActive = Int32.Parse(isActive) > 0
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }

        }

        /// <summary>
        /// Converts a <see cref="Model_Subject"/> to a <see cref="DTO_Subject_Summary"/> model.
        /// </summary>
        /// <param name="modelSubject">Instance of <see cref="Model_Subject"/></param>
        /// <returns>A Instance of <see cref="DTO_Subject_Summary"/> with data derived from <paramref name="modelSubject"/>.</returns>    
        public static DTO_Subject_Summary ToDto_Summary(this Model_Subject modelSubject) {
            return new DTO_Subject_Summary {
                Code = modelSubject.Code,
                Name = modelSubject.Name,
                IsActive = modelSubject.IsActive
            };
        }

        /// <summary>
        /// Converts a <see cref="DTO_Subject"/> to a <see cref="Model_Subject"/> model.
        /// </summary>
        /// <param name="dtoSubject">Instance of <see cref="DTO_Subject"/></param>
        /// <returns>A instance of <see cref="Model_Subject"/> with other data derived from <paramref name="dtoSubject"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Subject> ToModel(this DTO_Subject dtoSubject) {
            return await NewModel(dtoSubject.Code);
        }

        /// <summary>
        /// Converts a <see cref="DTO_Subject_Summary"/> to a <see cref="Model_Subject"/> model.
        /// </summary>
        /// <param name="dtoSubject_Summary">Instance of <see cref="DTO_Subject_Summary"/></param>
        /// <returns>A instance of <see cref="Model_Subject"/> with other data derived from <paramref name="dtoSubject_Summary"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Subject> ToModel(this DTO_Subject_Summary dtoSubject_Summary) {
            return await NewModel(dtoSubject_Summary.Code);
        }

        /// <summary>
        /// Converts a <see cref="DTO_Subject_Detail"/> to a <see cref="Model_Subject"/> model.
        /// </summary>
        /// <param name="dtoSubject_Detail">Instance of <see cref="DTO_Subject_Detail"/></param>
        /// <returns>A instance of <see cref="Model_Subject"/> with other data derived from <paramref name="dtoSubject_Detail"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Subject> ToModel(this DTO_Subject_Detail dtoSubject_Detail) {
            return await NewModel(dtoSubject_Detail.Code);
        }

        /// <summary>
        /// Converts a <see cref="string"/> to a <see cref="Model_Subject"/> model.
        /// </summary>
        /// <param name="subjectCode">Instance of <see cref="string"/></param>
        /// <returns>A instance of <see cref="Model_Subject"/> with other data derived from <paramref name="subjectCode"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Subject> ToModel(string subjectCode) {
            return await NewModel(subjectCode);
        }

        private static async Task<Model_Subject> NewModel(string subjectCode) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string id = String.Empty;
                string code = String.Empty;
                string classId = String.Empty;
                string name = String.Empty;
                string isActive = String.Empty;

                connection.Open();
                string query = "SELECT Top 1 Id, Code, Class_Id, Name, IsActive FROM tbl_subject WHERE Code = @code;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar).Value = subjectCode);

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    id = sqlData["Id"].ToString() ?? throw new InvalidCastException($"Subject Code: {subjectCode} has no id.");
                    code = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Subject Code: {subjectCode} has no code.");
                    classId = sqlData["Class_Id"].ToString() ?? throw new InvalidCastException($"Subject Code: {subjectCode} has no class id.");
                    name = sqlData["Name"].ToString() ?? throw new InvalidCastException($"Subject Code: {subjectCode} has no name.");
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Subject Code: {subjectCode} has no active / inactive status.");
                }

                sqlData.Close();

                return new Model_Subject {
                    Id = Int32.Parse(id),
                    Code = code,
                    Class_Id = Int32.Parse(classId),
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