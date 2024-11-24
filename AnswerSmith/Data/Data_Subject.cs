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
    public class Data_Subject : ISubjectHandler
    {
        public async Task<bool> AddSubject(DTO_Subject dTO_Subject)
        {
            try
            {
                Model_Subject modelSubject = await dTO_Subject.NewModel();

                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();

                string query = $"INSERT INTO tbl_Subject ([Code], [Class_Id], [Name], [IsActive]) VALUES (@code, @class_id, @name, @isActive)";

                SqlCommand command = new(query, connection);

                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar) { Value = modelSubject.Code });
                command.Parameters.Add(new SqlParameter("@class_id", SqlDbType.Int) { Value = modelSubject.Class_Id });
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar) { Value = modelSubject.Name });
                command.Parameters.Add(new SqlParameter("@isActive", SqlDbType.TinyInt) { Value = modelSubject.IsActive ? 1 : 0 });

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 1) { return true; }
                return false;
            }
            catch (SqlException) { throw; }
            catch (Exception) { throw; }
        }

        public async Task<bool> EditSubject(DTO_Subject dTO_Subject) {
            try {

                
                Model_Subject modelSubject = await dTO_Subject.UpdateModel(); 

                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();

                using SqlTransaction transaction = connection.BeginTransaction();


                try {
                    string query = $"UPDATE tbl_Subject SET [IsActive] = @isActive, [Name] = @name WHERE [Id] = @id;";

                    SqlCommand command = new(query, connection, transaction);

                    command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar){Value = modelSubject.Id});
                    command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar){Value = modelSubject.Name});
                    command.Parameters.Add(new SqlParameter("@isActive", SqlDbType.TinyInt){Value = modelSubject.IsActive ? 1 : 0});

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

//         public async Task<Tuple<List<DTO_Class>,Model_Pagination_CurrentPage>> GetClasses(
//             string searchKeyword = "%",
//             int? searchColumn = null,
//             string filterKeyword = "%",
//             string? filterColumn = null,
//             string? where_Clause = null,
//             Enum_Class_OrderBy orderBy = Enum_Class_OrderBy.Name, 
//             Enum_Any_OrderMode orderMode = Enum_Any_OrderMode.ASC,
//             string? orderBy_Clause = null,

//             int pageNo = 1,
//             int rowsPerPage = 20
//         ) {
//             try {

//                 string?[] orderByTable = [null, "Id", "Code", "Name"];
//                 using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
//                 connection.Open();

//                 using SqlTransaction transaction = connection.BeginTransaction();

//                 string query = @"-- Get Paginated Result
//                     EXEC CSP_GetPaginatedClasses
//                         @searchKeyword = @searchKeyword, 
//                         @searchColumn = @searchColumn, 
//                         @filterKeyword = @filterKeyword, 
//                         @filterColumn = @filterColumn, 
//                         @whereClause = @whereClause,

// 						@orderBy = @orderBy, 
// 						@orderMode = @orderMode, 
// 						@orderBy_Clause = @orderBy_Clause, 

//                         @pageNo = @pageNo, 
//                         @rowsPerPage = @rowsPerPage;
// --";

//                 using SqlCommand command = new(query, connection, transaction)
//                 {
//                     CommandType = CommandType.Text
//                 };

//                 // Add parameters for `CSP_SearchAndFilter` stored procedure
//                 command.Parameters.Add(new SqlParameter("@searchKeyword", SqlDbType.NVarChar) { Value = searchKeyword });
//                 command.Parameters.Add(new SqlParameter("@searchColumn", SqlDbType.NVarChar) { Value = (orderByTable[searchColumn ?? 0 ] is not null) ? orderByTable[searchColumn ?? 0 ] : DBNull.Value });
//                 command.Parameters.Add(new SqlParameter("@filterKeyword", SqlDbType.NVarChar) { Value = filterKeyword });
//                 command.Parameters.Add(new SqlParameter("@filterColumn", SqlDbType.NVarChar) { Value = (filterColumn is not null) ? filterColumn : DBNull.Value });
//                 command.Parameters.Add(new SqlParameter("@whereClause", SqlDbType.NVarChar) { Value = (where_Clause is not null) ? where_Clause : DBNull.Value });
//                 command.Parameters.Add(new SqlParameter("@orderBy_Clause", SqlDbType.NVarChar) { Value = (orderBy_Clause is not null) ? orderBy_Clause : DBNull.Value });
//                 command.Parameters.Add(new SqlParameter("@OrderBy", SqlDbType.NVarChar) { Value = orderByTable[(int) orderBy] });
//                 command.Parameters.Add(new SqlParameter("@OrderMode", SqlDbType.Int) { Value = orderMode });

//                 // Add parameters for `CSP_Paginate` stored procedure
//                 command.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.Int) { Value = pageNo });
//                 command.Parameters.Add(new SqlParameter("@RowsPerPage", SqlDbType.Int) { Value = rowsPerPage });

//                 using SqlDataReader reader = await command.ExecuteReaderAsync();

//                 List<DTO_Class> classList = [];

//                 if (reader.HasRows) {
//                     while (await reader.ReadAsync()) {
//                         DTO_Class dTO_Class = new() {
//                             SN = int.Parse(reader["CCol_RowNum"].ToString() ?? "-1"),
//                             Code = reader["Code"].ToString() ?? "No Data",
//                             Name = reader["Name"].ToString() ?? "No Data",
//                             IsActive = (reader["IsActive"].ToString() ?? "0") == "1"
//                         };   
//                         classList.Add(dTO_Class);
//                     }
//                 }

//                 await reader.NextResultAsync();
//                 Model_Pagination_CurrentPage paginationInfo = Model_Pagination_CurrentPage.Empty();
//                 if (reader.HasRows) {
//                     while (await reader.ReadAsync()) {
//                         paginationInfo = new() {
//                             Page_Number = int.Parse(reader["Page_Number"].ToString() ?? "1"),
//                             Max_Page = int.Parse(reader["Max_Page"].ToString() ?? "0"),
//                             Rows_Count = int.Parse(reader["Rows_Count"].ToString() ?? "0"),
//                             Rows_Per_Page = int.Parse(reader["Rows_Per_Page"].ToString() ?? "0"),
//                             Total_Rows = int.Parse(reader["Total_Rows"].ToString() ?? "0")
//                         };
//                     }
//                 }

//                 return Tuple.Create(classList, paginationInfo);
//             }
//             catch (Exception) { throw; }
//         }

        public async Task<DTO_Subject_Detail> GetSubject(string subjectCode)
        {
           try {
                DTO_Subject_Detail dto_Subject = await (await Mapper_Subject.ToModel(subjectCode)).ToDto_Detail();
                return dto_Subject;
            }
            catch (Exception) { throw; }
        }

//         public Task<Tuple<List<DTO_Subject_Detail>, Model_Pagination_CurrentPage>> GetSubjects(string searchKeyword = "%", int? searchColumn = null, string filterKeyword = "%", string? filterColumn = null, List<Model_WhereClause>? where_Clause = null, Enum_Class_OrderBy orderBy = Enum_Class_OrderBy.Name, Enum_Any_OrderMode orderMode = Enum_Any_OrderMode.ASC, string? orderBy_Clause = null, int pageNo = 1, int rowsPerPage = 20) {
//             try {

//                 string?[] orderByTable = [null, "Id", "Code", "Class_Code", "Class_Name", "Name"];
//                 using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
//                 connection.Open();

//                 using SqlTransaction transaction = connection.BeginTransaction();

//                 string query = @"-- Get Paginated Result
//                     EXEC CSP_GetPaginatedClasses
//                         @searchKeyword = @searchKeyword, 
//                         @searchColumn = @searchColumn, 
//                         @filterKeyword = @filterKeyword, 
//                         @filterColumn = @filterColumn, 
//                         @whereClause = @whereClause,

// 						@orderBy = @orderBy, 
// 						@orderMode = @orderMode, 
// 						@orderBy_Clause = @orderBy_Clause, 

//                         @pageNo = @pageNo, 
//                         @rowsPerPage = @rowsPerPage;
// --";

//                 using SqlCommand command = new(query, connection, transaction)
//                 {
//                     CommandType = CommandType.Text
//                 };

//                 // Add parameters for `CSP_SearchAndFilter` stored procedure
//                 command.Parameters.Add(new SqlParameter("@searchKeyword", SqlDbType.NVarChar) { Value = searchKeyword });
//                 command.Parameters.Add(new SqlParameter("@searchColumn", SqlDbType.NVarChar) { Value = (orderByTable[searchColumn ?? 0 ] is not null) ? orderByTable[searchColumn ?? 0 ] : DBNull.Value });
//                 command.Parameters.Add(new SqlParameter("@filterKeyword", SqlDbType.NVarChar) { Value = filterKeyword });
//                 command.Parameters.Add(new SqlParameter("@filterColumn", SqlDbType.NVarChar) { Value = (filterColumn is not null) ? filterColumn : DBNull.Value });
//                 command.Parameters.Add(new SqlParameter("@whereClause", SqlDbType.NVarChar) { Value = (where_Clause is not null) ? where_Clause : DBNull.Value });
//                 command.Parameters.Add(new SqlParameter("@orderBy_Clause", SqlDbType.NVarChar) { Value = (orderBy_Clause is not null) ? orderBy_Clause : DBNull.Value });
//                 command.Parameters.Add(new SqlParameter("@OrderBy", SqlDbType.NVarChar) { Value = orderByTable[(int) orderBy] });
//                 command.Parameters.Add(new SqlParameter("@OrderMode", SqlDbType.Int) { Value = orderMode });

//                 // Add parameters for `CSP_Paginate` stored procedure
//                 command.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.Int) { Value = pageNo });
//                 command.Parameters.Add(new SqlParameter("@RowsPerPage", SqlDbType.Int) { Value = rowsPerPage });

//                 using SqlDataReader reader = await command.ExecuteReaderAsync();

//                 List<DTO_Class> classList = [];

//                 if (reader.HasRows) {
//                     while (await reader.ReadAsync()) {
//                         DTO_Class dTO_Class = new() {
//                             SN = int.Parse(reader["CCol_RowNum"].ToString() ?? "-1"),
//                             Code = reader["Code"].ToString() ?? "No Data",
//                             Name = reader["Name"].ToString() ?? "No Data",
//                             IsActive = (reader["IsActive"].ToString() ?? "0") == "1"
//                         };   
//                         classList.Add(dTO_Class);
//                     }
//                 }

//                 await reader.NextResultAsync();
//                 Model_Pagination_CurrentPage paginationInfo = Model_Pagination_CurrentPage.Empty();
//                 if (reader.HasRows) {
//                     while (await reader.ReadAsync()) {
//                         paginationInfo = new() {
//                             Page_Number = int.Parse(reader["Page_Number"].ToString() ?? "1"),
//                             Max_Page = int.Parse(reader["Max_Page"].ToString() ?? "0"),
//                             Rows_Count = int.Parse(reader["Rows_Count"].ToString() ?? "0"),
//                             Rows_Per_Page = int.Parse(reader["Rows_Per_Page"].ToString() ?? "0"),
//                             Total_Rows = int.Parse(reader["Total_Rows"].ToString() ?? "0")
//                         };
//                     }
//                 }

//                 return Tuple.Create(classList, paginationInfo);
//             }
//             catch (Exception) { throw; }
//         }



        public async Task<Tuple<List<DTO_Subject_Detail>, Model_Pagination_CurrentPage>> GetSubjects(Model_PaginatedClientRequest paginationRequest) {
            try {

                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();


                // string queryx = @"SELECT -1 [CCol_RowNum], ts.[Id] ts_Id, ts.[Code] ts_Code, ts.[Name] ts_Name, ts.[IsActive] ts_IsActive, tc.[Id] tc_Id, tc.[Code] tc_Code, tc.[Name] tc_Name, tc.[IsActive] tc_IsActive  FROM tbl_Subject ts JOIN tbl_class tc ON ts.Class_Id = tc.Id";
                Tuple<string, List<SqlParameter>>? whereClause = (paginationRequest.SearchFilter ?? []).ToSqlCondition("AND");
                string? orderClause = paginationRequest.Order.ToStr();

                string query = @$"-- Get Paginated Subjects
                    EXEC CSP_GetPaginatedSubjects 
                        @AnySearch = @AnySearch,
                        @searchFilter = @searchFilterParam,
                        @order = @orderParam,
                        @pageNo = @pageNo,
                        @rowsPerPage = @rowsPerPage
--";

                using SqlCommand command = new(query, connection) { CommandType = CommandType.Text };
                command.Parameters.Add(new SqlParameter("@AnySearch", SqlDbType.NVarChar) { Value = paginationRequest.Search.IsNullOrEmpty() ? DBNull.Value : paginationRequest.Search });
                command.Parameters.Add(new SqlParameter("@searchFilterParam", SqlDbType.NVarChar) { Value = (whereClause is not null) ? whereClause.Item1 : DBNull.Value });

                // if (whereClause is not null) {
                //     foreach (SqlParameter sqlParameter in whereClause.Item2) {
                //         command.Parameters.Add(sqlParameter);
                //     }
                // } else {
                //     command.Parameters.Add(new SqlParameter("@searchFilterParam", SqlDbType.NVarChar) { Value = DBNull.Value });
                // }

                if (orderClause is not null) {
                    command.Parameters.Add(new SqlParameter("@orderParam", SqlDbType.NVarChar) { Value = orderClause });
                } else {
                    command.Parameters.Add(new SqlParameter("@orderParam", SqlDbType.NVarChar) { Value = DBNull.Value });
                }
                
                command.Parameters.Add(new SqlParameter("@pageNo", SqlDbType.Int) { Value = paginationRequest.Pagination.Page_Number });
                command.Parameters.Add(new SqlParameter("@rowsPerPage", SqlDbType.Int) { Value = paginationRequest.Pagination.Rows_Per_Page });

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                List<DTO_Subject_Detail> subjectList = [];

                if (reader.HasRows) {
                    while (await reader.ReadAsync()) {
                        DTO_Subject_Detail dto_SubjectDetail = new() {
                            SN = int.Parse(reader["CCol_RowNum"].ToString() ?? "-1"),
                            Code = reader["Code"].ToString() ?? "No Data",
                            Class_Code = reader["Class_Code"].ToString() ?? "No Data",
                            Name = reader["Name"].ToString() ?? "No Data",
                            Class_Name = reader["Class_Name"].ToString() ?? "No Data",
                            IsActive = (reader["IsActive"].ToString() ?? "0") == "1"
                        };   
                        subjectList.Add(dto_SubjectDetail);
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

                return Tuple.Create(subjectList, paginationInfo);
            }
            catch (Exception) { throw; }
        }

        public async Task<List<Model_KeyValue>> GetKeyValuePair(string parentCode) {
            try {

                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();

                string query = @"SELECT ts.[Code] ts_Code, ts.[Name] ts_Name FROM tbl_Subject ts JOIN tbl_class tc ON ts.Class_Id = tc.Id WHERE ts.IsActive = 1 and tc.IsActive = 1 AND tc.Code = @parentCode";

                using SqlCommand command = new(query, connection) { CommandType = CommandType.Text };

                command.Parameters.Add(new SqlParameter("@parentCode", SqlDbType.VarChar) { Value = parentCode });


                using SqlDataReader reader = await command.ExecuteReaderAsync();

                List<Model_KeyValue> pairs = [];

                if (reader.HasRows) {
                    while (await reader.ReadAsync()) {
                        Model_KeyValue model_KeyValue = new() {
                            Key = reader["ts_Code"].ToString() ?? "No Data",
                            Value = reader["ts_Value"].ToString() ?? "No Data",
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