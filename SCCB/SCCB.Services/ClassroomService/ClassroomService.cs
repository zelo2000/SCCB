using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SCCB.Core.DTO;
using SCCB.Repos.UnitOfWork;

namespace SCCB.Services.ClassroomService
{
    public class ClassroomService : IClassroomService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ClassroomService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Classroom сlassroomDto)
        {
            var сlassroom = _mapper.Map<DAL.Entities.Classroom>(сlassroomDto);
            await _unitOfWork.Classrooms.AddAsync(сlassroom);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Classroom> Find(Guid id)
        {
            var classroom = await FindClassroomEntity(id);
            var classroomDto = _mapper.Map<Core.DTO.Classroom>(classroom);
            return classroomDto;
        }

        public async Task Remove(Guid id)
        {
            var classroom = await FindClassroomEntity(id);
            _unitOfWork.Classrooms.Remove(classroom);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(Classroom classroomDto)
        {
            var classroom = await FindClassroomEntity(classroomDto.Id);

            classroom.Number = classroomDto.Number;
            classroom.Building = classroomDto.Building;

            _unitOfWork.Classrooms.Update(classroom);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Classroom>> GetAll()
        {
            var classrooms = await _unitOfWork.Classrooms.GetAllAsync();
            var classroomDtos = _mapper.Map<IEnumerable<Classroom>>(classrooms);
            return classroomDtos;
        }

        private async Task<DAL.Entities.Classroom> FindClassroomEntity(Guid id)
        {
            var classroom = await _unitOfWork.Classrooms.FindAsync(id);

            if (classroom != null)
            {
                return classroom;
            }
            else
            {
                throw new ArgumentException($"Can not find classroom with id {id}");
            }
        }
    }
}
