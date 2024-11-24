using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnswerSmith.Model;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace AnswerSmith.Mapper {
    public static class Mapper_WhereClause {
        public static Tuple<string, List<SqlParameter>>? ToSqlCondition(this List<Model_WhereClause> conditions, string logicalOperator = "AND") {
            // If Null or Empty value is not Fuzzy: Column IS NULL
            // If Null or Empty value is Fuzzy: Column LIKE %%
            if (logicalOperator != "AND" && logicalOperator != "OR") {
                throw new ArgumentException("Logical operator must be either 'AND' or 'OR'.", nameof(logicalOperator));
            }
            if (conditions.IsNullOrEmpty()) { return null; }

            var whereClauseBuilder = new StringBuilder();
            var parameters = new List<SqlParameter>();
            // int index = 0;

            foreach (var condition in conditions)
            {
                string paramName = string.Empty;
                string sqlFragment = string.Empty;

                if (condition.Value.IsNullOrEmpty() && !condition.IsFuzzy) { sqlFragment = $"[{condition.Key}] IS NULL"; } 
                else { 
                    // paramName = $"@param{index}";
                    // sqlFragment = condition.IsFuzzy ? $"'[{condition.Key}] LIKE {paramName}" : $"[{condition.Key}] = {paramName}"; 
                    sqlFragment = condition.IsFuzzy ? $"[{condition.Key}] LIKE '%{condition.Value}%'" : $"[{condition.Key}] = '{condition.Value}'"; 
                }

                if (whereClauseBuilder.Length > 0) { whereClauseBuilder.Append($" {logicalOperator} "); }
                whereClauseBuilder.Append(sqlFragment);

                // if (!paramName.IsNullOrEmpty()) { 
                //     parameters.Add(new SqlParameter(paramName, SqlDbType.NVarChar) { Value = condition.IsFuzzy ? $"%{condition.Value}%" : $"{condition.Value}"}); 
                //     index++;
                // }
            }
            return new Tuple<string, List<SqlParameter>> (whereClauseBuilder.ToString(), parameters);
        }
    }
}