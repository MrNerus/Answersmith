using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.Enums;
using AnswerSmith.Model;

namespace AnswerSmith.Mapper
{
    public static class Mapper_OrderBy {
        public static string ToStr(this Model_OrderBy orderBy) {
            if (!string.IsNullOrWhiteSpace(orderBy.Miscelleneous)) { return orderBy.Miscelleneous; }
            if (string.IsNullOrWhiteSpace(orderBy.Column_Name)) { return "(SELECT NULL)"; }

            string orderModeString = orderBy.Order_Mode switch {
                Enum_Any_OrderMode.ASC => "ASC",
                Enum_Any_OrderMode.DESC => "DESC",
                Enum_Any_OrderMode.NOT_IMPLEMENTED => string.Empty,
                _ => string.Empty
            };

            return string.IsNullOrEmpty(orderModeString)
                ? $""
                : $"[{orderBy.Column_Name}] {orderModeString}";

        }

        public static string ToStr(this List<Model_OrderBy>? orderBys)
        {
            if (orderBys == null || orderBys.Count == 0) { return "(SELECT NULL)"; }

            var orderClauses = new List<string>();

            foreach (var orderBy in orderBys) {
                string clause = orderBy.ToStr();
                if (!string.IsNullOrEmpty(clause)) { orderClauses.Add(clause); }
            }

            return orderClauses.Count > 0
                ? string.Join(", ", orderClauses)
                : "SELECT NULL";
        }
    }
}