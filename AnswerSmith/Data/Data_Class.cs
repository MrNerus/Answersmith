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
    public class Data_Class : IClassHandler
    {
        public async Task<bool> AddClass(DTO_Class dto_Class) {
            try
            {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();

                string query = $"INSERT INTO tbl_Class ([Code], [Name], [IsActive]) VALUES (@code, @name, @isActive)";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar) { Value = dto_Class.Code });
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) { Value = dto_Class.Name });
                command.Parameters.Add(new SqlParameter("@isActive", SqlDbType.TinyInt) { Value = dto_Class.IsActive ? 1 : 0 });

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 1) { return true; }
                return false;
            }
            catch (SqlException) { throw; }
            catch (Exception) { throw; }
        }

        public async Task<bool> EditClass(DTO_Class dto_Class) {
            try {
                
                Model_Class model_Class = await dto_Class.ToModel(); // This is logically redundent as well as failsafe. Ensures data do exist. Exception is thrown if not.

                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();

                using SqlTransaction transaction = connection.BeginTransaction();


                try {
                    string query = $"UPDATE tbl_Class SET [IsActive] = @isActive, [Name] = @name WHERE [Id] = @id;";

                    SqlCommand command = new(query, connection, transaction);

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar){Value = model_Class.Id});
                    command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar){Value = dto_Class.Name});
                    command.Parameters.Add(new SqlParameter("@isActive", SqlDbType.TinyInt){Value = dto_Class.IsActive ? 1 : 0});

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

        public async Task<DTO_Class> GetClass(string classCode) {
            try {
                DTO_Class dto_Class = (await Mapper_Class.ToModel(classCode)).ToDto();
                return dto_Class;
            }
            catch (Exception) { throw; }
        }

        public async Task<Tuple<List<DTO_Class>,Model_Pagination_CurrentPage>> GetClasses(Model_PaginatedClientRequest paginationRequest) {
             try {

                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());

                connection.Open();
                Tuple<string, List<SqlParameter>>? whereClause = (paginationRequest.SearchFilter ?? []).ToSqlCondition("AND");
                string? orderClause = paginationRequest.Order.ToStr();

                string query = @$"-- Get Paginated Classes
                    EXEC CSP_GetPaginatedClasses 
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

                List<DTO_Class> classList = [];

                if (reader.HasRows) {
                    while (await reader.ReadAsync()) {
                        DTO_Class dto_SubjectDetail = new() {
                            SN = int.Parse(reader["CCol_RowNum"].ToString() ?? "-1"),
                            Code = reader["Code"].ToString() ?? "No Data",
                            Name = reader["Name"].ToString() ?? "No Data",
                            IsActive = (reader["IsActive"].ToString() ?? "0") == "1"
                        };   
                        classList.Add(dto_SubjectDetail);
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

                return Tuple.Create(classList, paginationInfo);
            }
            catch (Exception) { throw; }
        }


        public async Task<List<Model_KeyValue>> GetKeyValuePair() {
            try {

                string?[] orderByTable = [null, "Id", "Code", "Class_Code", "Class_Name", "Name"];
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();

                string query = @"SELECT [Code], [Name] FROM tbl_Class WHERE IsActive = 1";

                using SqlCommand command = new(query, connection) { CommandType = CommandType.Text };

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