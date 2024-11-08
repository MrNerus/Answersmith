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

        public async Task<Tuple<List<DTO_Class>,Model_Pagination_CurrentPage>> GetClasses(
            int pageNo = 1,
            int rowsPerPage = 20,
            string filter = "%", 
            int orderBy = (int) Enum_Class_OrderBy.Name, 
            int orderMode = (int) Enum_Any_OrderMode.ASC
        ) {
            Enum_Class_OrderBy enum_OrderBy = (Enum_Class_OrderBy) orderBy;
            Enum_Any_OrderMode enum_OrderMode = (Enum_Any_OrderMode) orderMode;
            try { return await _classHandler.GetClasses(pageNo, rowsPerPage, filter, enum_OrderBy, enum_OrderMode); }
            catch (Exception) { throw; }
        }
    }
}