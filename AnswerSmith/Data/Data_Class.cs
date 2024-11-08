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

                    SqlCommand command = new(query, connection);

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

        public async Task<Tuple<List<DTO_Class>,Model_Pagination_CurrentPage>> GetClasses(
            int pageNo = 1,
            int rowsPerPage = 20,
            string filter = "%", 
            Enum_Class_OrderBy orderBy = Enum_Class_OrderBy.Name, 
            Enum_Any_OrderMode orderMode = Enum_Any_OrderMode.ASC
        ) {
            try {
                using SqlConnection connection = new(Data_ConnectionString.GetConnectionString());
                connection.Open();

                using SqlTransaction transaction = connection.BeginTransaction();
                string query = $"CSP_GetPaginatedClasses";

                SqlCommand command = new(query, connection, transaction){ CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@Page_Number", SqlDbType.Int) {Value = pageNo});
                command.Parameters.Add(new SqlParameter("@Number_Of_Rows_Per_Page", SqlDbType.Int) {Value = rowsPerPage});
                command.Parameters.Add(new SqlParameter("@filter", SqlDbType.NVarChar) {Value = filter});
                command.Parameters.Add(new SqlParameter("@Order_By", SqlDbType.Int) {Value = (int) orderBy});
                command.Parameters.Add(new SqlParameter("@Order_Mode", SqlDbType.Int) {Value = (int) orderMode});

                using SqlDataReader reader = await command.ExecuteReaderAsync();

                List<DTO_Class> classList = [];

                if (reader.HasRows) {
                    while (await reader.ReadAsync()) {
                        DTO_Class dTO_Class = new() {
                            Code = reader["Code"].ToString() ?? "No Data",
                            Name = reader["Name"].ToString() ?? "No Data",
                            IsActive = (reader["IsActive"].ToString() ?? "0") == "1"
                        };   
                        classList.Add(dTO_Class);
                    }
                }

                await reader.NextResultAsync();
                Model_Pagination_CurrentPage paginationInfo = Model_Pagination_CurrentPage.Empty();
                if (reader.HasRows) {
                    while (await reader.ReadAsync()) {
                        paginationInfo = new() {
                            Page_Number = int.Parse(reader["Page_Number"].ToString() ?? "1"),
                            Max_Page = int.Parse(reader["Max_Page"].ToString() ?? "0"),
                            Data_Count = int.Parse(reader["Data_Count"].ToString() ?? "0"),
                            Total_Count = int.Parse(reader["Total_Count"].ToString() ?? "0")
                        };
                    }
                }

                return Tuple.Create(classList, paginationInfo);
            }
            catch (Exception) { throw; }
        }
    }
}