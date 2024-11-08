using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnswerSmith.Model
{
    public class Model_ConnectionString
    {
        public string Server {get; set;} = String.Empty;
        public string User {get; set;} = String.Empty;
        public string Password {get; set;} = String.Empty;
        public string Database {get; set;} = String.Empty;
        public string Trusted_Certificate {get; set;} = String.Empty;
    }
}