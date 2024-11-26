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
    public static class Mapper_Chapter
    {
        /// <summary>
        /// Converts a <see cref="Model_Chapter"/> to a <see cref="DTO_Chapter"/> model.
        /// </summary>
        /// <param name="modelChapter">Instance of <see cref="Model_Chapter"/></param>
        /// <returns>A Instance of <see cref="DTO_Chapter"/> with data derived from <paramref name="modelChapter"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<DTO_Chapter> ToDTO(this Model_Chapter modelChapter) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string code = string.Empty;
                string subjectCode = string.Empty;
                string name = string.Empty;
                string isActive = string.Empty;

                connection.Open();
                string query = @"SELECT chp.Code Code, sbj.Code Subject_Code, chp.Name Name, chp.IsActive IsActive 
                    FROM tbl_chapter chp 
                    JOIN tbl_subject sbj ON chp.Subject_Id = sbj.Id 
                    WHERE chp.Code = @code";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@Code", SqlDbType.NVarChar) {Value = modelChapter.Code});

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    code = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelChapter.Code} has no code.");
                    subjectCode = sqlData["Subject_Code"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelChapter.Code} has no subject code.");
                    name = sqlData["Name"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelChapter.Code} has no name.");
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Subject Code: {modelChapter.Code} has no active / inactive status.");
                }

                sqlData.Close();

                return new DTO_Chapter {
                    Code = code,
                    Subject_Code = subjectCode,
                    Name = name,
                    IsActive = int.Parse(isActive) > 0
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Converts a <see cref="Model_Chapter"/> to a <see cref="DTO_Chapter_Detail"/> model.
        /// </summary>
        /// <param name="modelChapter">Instance of <see cref="Model_Chapter"/></param>
        /// <returns>A Instance of <see cref="DTO_Chapter_Detail"/> with data derived from <paramref name="modelChapter"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<DTO_Chapter_Detail> ToDto_Detail(this Model_Chapter modelChapter) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string code = string.Empty;
                string subjectCode = string.Empty;
                string subjectName = string.Empty;
                string classCode = string.Empty;
                string className = string.Empty;
                string name = string.Empty;
                string isActive = string.Empty;

                connection.Open();
                string query = @"SELECT chp.Code Code, cls.Code Class_Code, cls.Name Class_Name, sbj.Code Subject_Code, sbj.Name Subject_Name, chp.Name Name, chp.IsActive IsActive 
                    FROM tbl_chapter chp 
                    JOIN tbl_subject sbj ON chp.Subject_Id = sbj.Id 
                    JOIN tbl_class cls ON sbj.Class_Id = cls.Id
                    WHERE chp.Code = @code";
                
                
                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar) {Value = modelChapter.Code});

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    code = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Chapter Code: {modelChapter.Code} has no code.");
                    subjectCode = sqlData["Subject_Code"].ToString() ?? throw new InvalidCastException($"Chapter Code: {modelChapter.Code} has no Subject Code.");
                    subjectName = sqlData["Subject_Name"].ToString() ?? throw new InvalidCastException($"Chapter Code: {modelChapter.Code} has no Subject Name.");
                    classCode = sqlData["Class_Code"].ToString() ?? throw new InvalidCastException($"Chapter Code: {modelChapter.Code} has no class code.");
                    className = sqlData["Class_Name"].ToString() ?? throw new InvalidCastException($"Chapter Code: {modelChapter.Code} has no class name.");
                    name = sqlData["Name"].ToString() ?? throw new InvalidCastException($"Chapter Code: {modelChapter.Code} has no name.");
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Chapter Code: {modelChapter.Code} has no active / inactive status.");
                }

                sqlData.Close();

                return new DTO_Chapter_Detail {
                    Code = code,
                    Subject_Code = subjectCode,
                    Subject_Name = subjectName,
                    Class_Code = classCode,
                    Class_Name = className,
                    Name = name,
                    IsActive = int.Parse(isActive) > 0
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }

        }

        /// <summary>
        /// Converts a <see cref="Model_Chapter"/> to a <see cref="DTO_Chapter_Summary"/> model.
        /// </summary>
        /// <param name="modelChapter">Instance of <see cref="Model_Chapter"/></param>
        /// <returns>A Instance of <see cref="DTO_Chapter_Summary"/> with data derived from <paramref name="modelChapter"/>.</returns>    
        public static DTO_Chapter_Summary ToDto_Summary(this Model_Chapter modelChapter) {
            return new DTO_Chapter_Summary {
                Code = modelChapter.Code,
                Name = modelChapter.Name,
                IsActive = modelChapter.IsActive
            };
        }

        /// <summary>
        /// Converts a <see cref="DTO_Chapter"/> to a <see cref="Model_Chapter"/> model.
        /// </summary>
        /// <param name="dtoChapter">Instance of <see cref="DTO_Chapter"/></param>
        /// <returns>A instance of <see cref="Model_Chapter"/> with other data derived from <paramref name="dtoChapter"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Chapter> ToModel(this DTO_Chapter dtoChapter) {
            return await NewModel(dtoChapter.Code);
        }

        /// <summary>
        /// Converts a <see cref="DTO_Chapter_Summary"/> to a <see cref="Model_Chapter"/> model.
        /// </summary>
        /// <param name="dtoChapter_Summary">Instance of <see cref="DTO_Chapter_Summary"/></param>
        /// <returns>A instance of <see cref="Model_Chapter"/> with other data derived from <paramref name="dtoChapter_Summary"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Chapter> ToModel(this DTO_Chapter_Summary dtoChapter_Summary) {
            return await NewModel(dtoChapter_Summary.Code);
        }

        /// <summary>
        /// Converts a <see cref="DTO_Chapter_Detail"/> to a <see cref="Model_Chapter"/> model.
        /// </summary>
        /// <param name="dtoChapter_Detail">Instance of <see cref="DTO_Chapter_Detail"/></param>
        /// <returns>A instance of <see cref="Model_Chapter"/> with other data derived from <paramref name="dtoChapter_Detail"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Chapter> ToModel(this DTO_Chapter_Detail dtoChapter_Detail) {
            return await NewModel(dtoChapter_Detail.Code);
        }

        public static async Task<Model_Chapter> NewModel(this DTO_Chapter dto_Chapter) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string subjectId = string.Empty;

                connection.Open();
                string query = "SELECT Top 1 ts.Id Subject_Id FROM tbl_Subject ts LEFT JOIN tbl_Class tc ON ts.Class_Id = tc.Id WHERE ts.Code = @code AND ts.IsActive = 1 AND tc.IsActive = 1;";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar) {Value = dto_Chapter.Subject_Code});

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                if (!sqlData.HasRows) { throw new InvalidDataException($"{dto_Chapter.Subject_Code} is not assigned to any active subject."); }
                while (sqlData.Read()) {
                    subjectId = sqlData["Subject_Id"].ToString() ?? throw new InvalidCastException($"{dto_Chapter.Subject_Code} is not assigned to any active subject.");
                }

                sqlData.Close();

                return new Model_Chapter {
                    Id = 0,
                    Code = dto_Chapter.Code,
                    Subject_Id = int.Parse(subjectId),
                    Name = dto_Chapter.Name,
                    IsActive = dto_Chapter.IsActive
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }
        }


        /// <summary>
        /// Converts a <see cref="string"/> to a <see cref="Model_Chapter"/> model.
        /// </summary>
        /// <param name="chapterCode">Instance of <see cref="string"/></param>
        /// <returns>A instance of <see cref="Model_Chapter"/> with other data derived from <paramref name="chapterCode"/>.</returns>
        /// <exception cref="SqlException">Thrown if there is problem with connecting to Database.</exception>
        /// <exception cref="InvalidCastException">Thrown if unexpected data was found while mapping.</exception>
        /// <exception cref="Exception">No idea.</exception>
        public async static Task<Model_Chapter> ToModel(string chapterCode) {
            return await NewModel(chapterCode);
        }

        private static async Task<Model_Chapter> NewModel(string chapterCode) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                string id = string.Empty;
                string code = string.Empty;
                string subjectId = string.Empty;
                string name = string.Empty;
                string isActive = string.Empty;

                connection.Open();
                string query = "SELECT Top 1 Id, Code, Subject_Id, Name, IsActive FROM tbl_chapter WHERE Code = @code;";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar) {Value = chapterCode});

                SqlDataReader sqlData = await command.ExecuteReaderAsync();

                while (sqlData.Read()) {
                    id = sqlData["Id"].ToString() ?? throw new InvalidCastException($"Subject Code: {chapterCode} has no id.");
                    code = sqlData["Code"].ToString() ?? throw new InvalidCastException($"Subject Code: {chapterCode} has no code.");
                    subjectId = sqlData["Subject_Id"].ToString() ?? throw new InvalidCastException($"Subject Code: {chapterCode} has no class id.");
                    name = sqlData["Name"].ToString() ?? throw new InvalidCastException($"Subject Code: {chapterCode} has no name.");
                    isActive = sqlData["IsActive"].ToString() ?? throw new InvalidCastException($"Subject Code: {chapterCode} has no active / inactive status.");
                }

                sqlData.Close();

                return new Model_Chapter {
                    Id = int.Parse(id),
                    Code = code,
                    Subject_Id = int.Parse(subjectId),
                    Name = name,
                    IsActive = int.Parse(isActive) > 0
                };
            }
            catch (SqlException) { throw; }
            catch (InvalidCastException) { throw; }
            catch (Exception) { throw; }
        }


        public static async Task<Model_Chapter> UpdateModel(this DTO_Chapter dto_Chapter) {
            Model_Chapter modelChapter = await dto_Chapter.ToModel();
            modelChapter.Name = dto_Chapter.Name;
            modelChapter.IsActive = dto_Chapter.IsActive;
            return modelChapter;
        }
    }
}