using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using AnswerSmith.Model;
using AnswerSmith.Mapper;

namespace AnswerSmith.Data
{
    public class Data_ConnectionString
    {
        public static Model_ConnectionString GetConnectionStringModel() {
            // <summary> returns </summary>
            /// <exception cref="FileNotFoundException">
            /// <exception cref="JsonException">
            /// <exception cref="FileLoadException">
            /// <exception cref="Exception">
            
            try {
                string filePath = "./Config/ConnectionString.json";
                string jsonString = File.ReadAllText(filePath) ?? throw new FileLoadException("Couldn't read any data from ConnectionString.json");

                return JsonSerializer.Deserialize<Model_ConnectionString>(jsonString) ?? throw new FileLoadException("Couldn't find required data from ConnectionString.json");
            }
            catch (FileNotFoundException ex) { throw new Exception($"Couldn't find ConnectionString.json. System Says:\n{ex.Message}"); }
            catch (JsonException ex) { throw new Exception($"There was an error on fetching data from ConnectionString.json. System Says:\n{ex.Message}"); } 
            catch (FileLoadException) { throw; }
            catch (Exception ex) { throw new Exception($"An error occurred. System Says:\n{ex.Message}"); }
        }

        public static string GetConnectionString() {
            try {
                Model_ConnectionString connectionStringModel = GetConnectionStringModel();
                return connectionStringModel.ToStr() ?? throw new InvalidOperationException("Panic Condition: Got null while converting Model_ConnectionString to String");
            } 
            catch (InvalidOperationException) {throw;}
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
    }
}