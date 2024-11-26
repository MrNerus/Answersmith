using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.DTOs;
using AnswerSmith.Enums;
using AnswerSmith.Interfaces;
using AnswerSmith.Mapper;
using AnswerSmith.Model;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace AnswerSmith.Data
{
    public class Data_Chapter : IChapterHandler
    {
        public async Task<bool> AddChapter(DTO_Chapter dTO_Chapter)
        {
            try
            {
                Model_Chapter modelChapter = await dTO_Chapter.NewModel();

                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();

                using SqlTransaction transaction = connection.BeginTransaction();


                try {
                    string query = $"INSERT INTO tbl_Chapter ([Code], [Subject_Id], [Name], [IsActive]) VALUES (@code, @subject_id, @name, @isActive)";

                    SqlCommand command = new(query, connection, transaction);

                    command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar) { Value = modelChapter.Code });
                    command.Parameters.Add(new SqlParameter("@subject_id", SqlDbType.Int) { Value = modelChapter.Subject_Id });
                    command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) { Value = modelChapter.Name });
                    command.Parameters.Add(new SqlParameter("@isActive", SqlDbType.TinyInt) { Value = modelChapter.IsActive ? 1 : 0 });

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 1) { 
                        await transaction.CommitAsync();
                        return true; 
                    }
                    await transaction.RollbackAsync();
                    return false;
                } catch (SqlException) { 
                    await transaction.RollbackAsync();
                    throw; 
                }
            }
            catch (SqlException) { throw; }
            catch (Exception) { throw; }
        }

        public async Task<bool> EditChapter(DTO_Chapter dTO_Chapter) {
            try {
                Model_Chapter modelChapter = await dTO_Chapter.UpdateModel(); 

                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();

                using SqlTransaction transaction = connection.BeginTransaction();

                try {
                    string query = $"UPDATE tbl_Chapter SET [IsActive] = @isActive, [Name] = @name WHERE [Id] = @id;";

                    SqlCommand command = new(query, connection, transaction);

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar){Value = modelChapter.Id});
                    command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar){Value = modelChapter.Name});
                    command.Parameters.Add(new SqlParameter("@isActive", SqlDbType.TinyInt){Value = modelChapter.IsActive ? 1 : 0});

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 1) { 
                        await transaction.CommitAsync();
                        return true; 
                    }
                    await transaction.RollbackAsync();
                    return false;
                } catch (SqlException) { 
                    await transaction.RollbackAsync();
                    throw; 
                }
            }
            catch (Exception) { throw; }
        }

        public async Task<DTO_Chapter_Detail> GetChapter(string chapterCode)
        {
           try {
                DTO_Chapter_Detail dto_Chapter = await (await Mapper_Chapter.ToModel(chapterCode)).ToDto_Detail();
                return dto_Chapter;
            }
            catch (Exception) { throw; }
        }

        public async Task<Tuple<List<DTO_Chapter_Detail>, Model_Pagination_CurrentPage>> GetChapters(Model_PaginatedClientRequest paginationRequest) {
            try {

                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();


                // string queryx = @"SELECT -1 [CCol_RowNum], ts.[Id] ts_Id, ts.[Code] ts_Code, ts.[Name] ts_Name, ts.[IsActive] ts_IsActive, tc.[Id] tc_Id, tc.[Code] tc_Code, tc.[Name] tc_Name, tc.[IsActive] tc_IsActive  FROM tbl_Subject ts JOIN tbl_class tc ON ts.Class_Id = tc.Id";
                Tuple<string, List<SqlParameter>>? whereClause = (paginationRequest.SearchFilter ?? []).ToSqlCondition("AND");
                string? orderClause = paginationRequest.Order.ToStr();

                string query = @$"-- Get Paginated Chapters
                    EXEC CSP_GetPaginatedChapters 
                        @AnySearch = @AnySearch,
                        @searchFilter = @searchFilterParam,
                        @order = @orderParam,
                        @pageNo = @pageNo,
                        @rowsPerPage = @rowsPerPage
--";

                using SqlCommand command = new(query, connection) { CommandType = CommandType.Text };
                command.Parameters.Add(new SqlParameter("@AnySearch", SqlDbType.NVarChar) { Value = paginationRequest.Search.IsNullOrEmpty() ? DBNull.Value : paginationRequest.Search });
                command.Parameters.Add(new SqlParameter("@searchFilterParam", SqlDbType.NVarChar) { Value = (whereClause is not null) ? whereClause.Item1 : DBNull.Value });

                if (orderClause is not null) {
                    command.Parameters.Add(new SqlParameter("@orderParam", SqlDbType.NVarChar) { Value = orderClause });
                } else {
                    command.Parameters.Add(new SqlParameter("@orderParam", SqlDbType.NVarChar) { Value = DBNull.Value });
                }
                
                command.Parameters.Add(new SqlParameter("@pageNo", SqlDbType.Int) { Value = paginationRequest.Pagination.Page_Number });
                command.Parameters.Add(new SqlParameter("@rowsPerPage", SqlDbType.Int) { Value = paginationRequest.Pagination.Rows_Per_Page });

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                List<DTO_Chapter_Detail> chapterList = [];

                if (reader.HasRows) {
                    while (await reader.ReadAsync()) {
                        DTO_Chapter_Detail dto_ChapterDetail = new() {
                            SN = int.Parse(reader["CCol_RowNum"].ToString() ?? "-1"),
                            Class_Code = reader["Class_Code"].ToString() ?? "No Data",
                            Subject_Code = reader["Subject_Code"].ToString() ?? "No Data",
                            Code = reader["Code"].ToString() ?? "No Data",
                            Class_Name = reader["Class_Name"].ToString() ?? "No Data",
                            Subject_Name = reader["Subject_Name"].ToString() ?? "No Data",
                            Name = reader["Name"].ToString() ?? "No Data",
                            IsActive = (reader["IsActive"].ToString() ?? "0") == "1"
                        };   
                        chapterList.Add(dto_ChapterDetail);
                    }
                }

                await reader.NextResultAsync();
                Model_Pagination_CurrentPage paginationInfo = Model_Pagination_CurrentPage.Empty();
                if (reader.HasRows) {
                    while (await reader.ReadAsync()) {
                        paginationInfo = new() {
                            Page_Number = int.Parse(reader["Page_Number"].ToString() ?? "1"),
                            Max_Page = int.Parse(reader["Max_Page"].ToString() ?? "0"),
                            Rows_Count = int.Parse(reader["Rows_Count"].ToString() ?? "0"),
                            Rows_Per_Page = int.Parse(reader["Rows_Per_Page"].ToString() ?? "0"),
                            Total_Rows = int.Parse(reader["Total_Rows"].ToString() ?? "0")
                        };
                    }
                }

                return Tuple.Create(chapterList, paginationInfo);
            }
            catch (Exception) { throw; }
        }

        public async Task<List<Model_KeyValue>> GetKeyValuePair(string parentCode) {
            try {

                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();

                string query = @"--
                SELECT chp.[Code] Code, chp.[Name] Name 
                FROM tbl_Chapter chp JOIN tbl_Subject sub ON chp.Subject_Id = sub.Id JOIN tbl_class cls ON sub.Class_Id = cls.Id 
                WHERE chp.IsActive = 1 and sub.IsActive = 1 AND cls.IsActive = 1 AND sub.Code = @parentCode
--";

                using SqlCommand command = new(query, connection) { CommandType = CommandType.Text };

                command.Parameters.Add(new SqlParameter("@parentCode", SqlDbType.VarChar) { Value = parentCode });


                using SqlDataReader reader = await command.ExecuteReaderAsync();

                List<Model_KeyValue> pairs = [];

                if (reader.HasRows) {
                    while (await reader.ReadAsync()) {
                        Model_KeyValue model_KeyValue = new() {
                            Key = reader["Code"].ToString() ?? "No Data",
                            Value = reader["Name"].ToString() ?? "No Data",
                        };   
                        pairs.Add(model_KeyValue);
                    }
                }

                return pairs;
            }
            catch (Exception) { throw; }
        }
     }
}