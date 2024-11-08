using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.Model;

namespace AnswerSmith.Mapper
{
    public static class Mapper_ConnectionString
    {
        public static string ToStr(this Model_ConnectionString csModel) {
            // return $"Server={csModel.Server};Database={csModel.Database};User Id={csModel.User};Password={csModel.Password};TrustServerCertificate={csModel.Trusted_Certificate}";
            return $"Data Source=tcp:{csModel.Server};Initial Catalog={csModel.Database};User ID={csModel.User};Password={csModel.Password};Trusted_Connection={csModel.Trusted_Certificate};TrustServerCertificate=True";
        }
    }
}