using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.Data;
using AnswerSmith.DTOs;
using AnswerSmith.Enums;
using AnswerSmith.Interfaces;
using AnswerSmith.Mapper;
using AnswerSmith.Model;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace AnswerSmith.Services {
    public class Service_ClassRoom(IClassHandler classHandler): IClassService {
        private readonly IClassHandler _classHandler = classHandler;

        public async Task<bool> AddClass(DTO_Class dto_Class) {
            if (dto_Class.Code.IsNullOrEmpty()) throw new InvalidDataException("Code is Empty");
            if (dto_Class.Name.IsNullOrEmpty()) throw new InvalidDataException("name is Empty");

            try { return await _classHandler.AddClass(dto_Class); }
            catch (Exception) { throw; }
        }

        public async Task<bool> EditClass(DTO_Class dto_Class) {
            if (dto_Class.Code.IsNullOrEmpty()) throw new InvalidDataException("Code is Empty");
            if (dto_Class.Name.IsNullOrEmpty()) throw new InvalidDataException("name is Empty");

            try { return await _classHandler.EditClass(dto_Class); }
            catch (Exception) { throw; }
        }

        public async Task<DTO_Class> GetClass(string classCode) {
            if (classCode.IsNullOrEmpty()) throw new InvalidDataException("Code is Empty");

            try { return await _classHandler.GetClass(classCode); }
            catch (Exception) { throw; }
        }

        public async Task<Tuple<List<DTO_Class>,Model_Pagination_CurrentPage>> GetClasses(Model_PaginatedClientRequest clientRequest) {
            Dictionary<string,string> KeyMap = new() {
                {"Code", "Code"},
                {"Name", "Name"},
                {"Status", "IsActive"},
            };
            Dictionary<string,Dictionary<string,string>> ValueMap = new() {
                {"Status", new Dictionary<string, string>() {{"active", "1"}, {"inactive", "0"}, {"_", "%"}}}
            };

            Model_PaginatedClientRequest translatedMeaning = new() {
                Search = clientRequest.Search,
                SearchFilter = (clientRequest.SearchFilter ?? []).Select(x => new Model_WhereClause { 
                    Key = KeyMap.ContainsKey(x.Key) ? KeyMap[x.Key] : x.Key,
                    Value = ValueMap.ContainsKey(x.Key) ? ValueMap[x.Key].ContainsKey(x.Value) ? ValueMap[x.Key][x.Value] : x.Value : x.Value,
                    IsFuzzy = x.IsFuzzy
                }).ToList(),
                Order = (clientRequest.Order ?? []).Select(x => new Model_OrderBy {
                    Column_Name = KeyMap.ContainsKey(x.Column_Name) ? KeyMap[x.Column_Name] : x.Column_Name,
                    Order_Mode = x.Order_Mode,
                    Miscelleneous = x.Miscelleneous
                }).ToList(),
                Pagination = clientRequest.Pagination
            };

            try { return await _classHandler.GetClasses(translatedMeaning); 
            } catch (Exception) { throw; }
        }

        public async Task<List<Model_KeyValue>> GetKeyValuePair() {
            try { return await _classHandler.GetKeyValuePair(); }
            catch (Exception) { throw; }
        }
    }
}