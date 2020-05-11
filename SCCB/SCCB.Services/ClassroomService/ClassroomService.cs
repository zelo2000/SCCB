using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SCCB.Core.Constants;
using SCCB.Core.DTO;
using SCCB.Core.Helpers;
using SCCB.DAL.Comparers;
using SCCB.Repos.UnitOfWork;

namespace SCCB.Services.ClassroomService
{
    /// <summary>
    /// Classroom service.
    /// </summary>
    public class ClassroomService : IClassroomService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassroomService"/> class.
        /// </summary>
        /// <param name="mapper">Mapper instance.</param>
        /// <param name="unitOfWork">UnitOfWork instance.</param>
        public ClassroomService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc/>
        public async Task Add(Classroom сlassroomDto)
        {
            var сlassroom = _mapper.Map<DAL.Entities.Classroom>(сlassroomDto);
            await _unitOfWork.Classrooms.AddAsync(сlassroom);
            await _unitOfWork.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task<Classroom> Find(Guid id)
        {
            var classroom = await FindClassroomEntity(id);
            var classroomDto = _mapper.Map<Core.DTO.Classroom>(classroom);
            return classroomDto;
        }

        /// <inheritdoc/>
        public async Task Remove(Guid id)
        {
            var classroom = await FindClassroomEntity(id);
            _unitOfWork.Classrooms.Remove(classroom);
            await _unitOfWork.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task Update(Classroom classroomDto)
        {
            var classroom = await FindClassroomEntity(classroomDto.Id);

            classroom.Number = classroomDto.Number;
            classroom.Building = classroomDto.Building;

            _unitOfWork.Classrooms.Update(classroom);
            await _unitOfWork.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Classroom>> GetAll()
        {
            var classrooms = await _unitOfWork.Classrooms.GetAllAsync();
            var classroomDtos = _mapper.Map<IEnumerable<Classroom>>(classrooms);
            return classroomDtos;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<string, IEnumerable<Classroom>>> GetAllGroupedByBuilding()
        {
            var classrooms = await _unitOfWork.Classrooms.GetAllAsync();
            var classroomDtos = _mapper.Map<IEnumerable<Classroom>>(classrooms);
            var classroomsByBuilding = classroomDtos
                .GroupBy(classroom => classroom.Building)
                .ToDictionary(group => group.Key, group => group.AsEnumerable());
            return classroomsByBuilding;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<string, IEnumerable<Classroom>>> FindFreeClassroomsGroupedByBuilding(LessonTime time)
        {
            if (!string.IsNullOrEmpty(time.Weekday) && time.LessonNumber != null)
            {
                var classrooms = await _unitOfWork.Classrooms.GetAllAsync();
                var assignedForLesson = await _unitOfWork.Classrooms.FindClassroomsAssignedForLesson(time);
                var freeClassrooms = classrooms.Except(assignedForLesson, new ClassroomComparer());

                var classroomDtos = _mapper.Map<IEnumerable<Classroom>>(freeClassrooms);
                var classroomsByBuilding = classroomDtos
                    .GroupBy(classroom => classroom.Building)
                    .ToDictionary(group => group.Key, group => group.AsEnumerable());

                return classroomsByBuilding;
            }
            else
            {
                return new Dictionary<string, IEnumerable<Classroom>>();
            }
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<string, IEnumerable<Classroom>>> FindFreeClassroomsGroupedByBuilding(DateTime? date, int? lessonNumber)
        {
            if (date != null && lessonNumber != null)
            {
                var time = new LessonTime
                {
                    Weekday = Formatter.WeekdayUkrainian((DateTime)date),
                    LessonNumber = lessonNumber,
                };

                int weeksPassed = (int)((DateTime)date - TermSettings.FirstDay).TotalDays / 7;
                if (weeksPassed % 2 == 0)
                {
                    time.IsNumerator = true;
                    time.IsDenominator = false;
                }
                else
                {
                    time.IsNumerator = false;
                    time.IsDenominator = true;
                }

                var classrooms = await _unitOfWork.Classrooms.GetAllAsync();
                var assignedForLesson = await _unitOfWork.Classrooms.FindClassroomsAssignedForLesson(time);
                var booked = await _unitOfWork.Classrooms.FindBookedClassrooms((DateTime)date, (int)lessonNumber);
                var comparer = new ClassroomComparer();
                var freeClassrooms = classrooms.Except(assignedForLesson, comparer).Except(booked, comparer);

                var classroomDtos = _mapper.Map<IEnumerable<Classroom>>(freeClassrooms);
                var classroomsByBuilding = classroomDtos
                    .GroupBy(classroom => classroom.Building)
                    .ToDictionary(group => group.Key, group => group.AsEnumerable());

                return classroomsByBuilding;
            }
            else
            {
                return new Dictionary<string, IEnumerable<Classroom>>();
            }
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
