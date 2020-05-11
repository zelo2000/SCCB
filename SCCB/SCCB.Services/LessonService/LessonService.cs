using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SCCB.Core.DTO;
using SCCB.Repos.UnitOfWork;

namespace SCCB.Services.LessonService
{
    /// <summary>
    /// Lesson service.
    /// </summary>
    public class LessonService : ILessonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="LessonService"/> class.
        /// </summary>
        /// <param name="mapper">Mapper instance.</param>
        /// <param name="unitOfWork">UnitOfWork instance.</param>
        public LessonService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc/>
        public async Task Add(Lesson lessonDto)
        {
            var lessonTime = _mapper.Map<LessonTime>(lessonDto);
            var conflictingLesson = await _unitOfWork.Lessons.FindByGroupIdAndTime(lessonDto.GroupId, lessonTime);

            if (conflictingLesson == null)
            {
                var lesson = _mapper.Map<DAL.Entities.Lesson>(lessonDto);
                await _unitOfWork.Lessons.AddAsync(lesson);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                var group = await _unitOfWork.Groups.FindAsync(lessonDto.GroupId);
                throw new ArgumentException($"Група {group.Name} вже має пару в цей час");
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Lesson>> FindByGroupId(Guid? id)
        {
            if (id != null)
            {
                var lessons = await _unitOfWork.Lessons.FindByGroupId((Guid)id);
                var lessonsDto = _mapper.Map<List<Core.DTO.Lesson>>(lessons);
                return lessonsDto;
            }
            else
            {
                return new List<Lesson>();
            }
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyDictionary<int, IEnumerable<Lesson>>> FindByGroupIdAndWeekday(Guid groupId, string weekday)
        {
            var lessons = await _unitOfWork.Lessons.FindByGroupIdAndWeekday(groupId, weekday);
            var lessonsDto = _mapper.Map<List<Core.DTO.Lesson>>(lessons);

            var lessonGroups = lessonsDto.GroupBy(lesson => lesson.LessonNumber)
                .Select(entry => new KeyValuePair<int, IEnumerable<Lesson>>(
                    entry.Key, entry.OrderBy(x => x.IsDenominator)))
                .ToDictionary(x => x.Key, x => x.Value);

            return lessonGroups;
        }

        /// <inheritdoc/>
        public async Task<Lesson> Find(Guid id)
        {
            var lesson = await FindLessonEntity(id);
            var lessonDto = _mapper.Map<Core.DTO.Lesson>(lesson);
            return lessonDto;
        }

        /// <inheritdoc/>
        public async Task Remove(Guid id)
        {
            var lesson = await FindLessonEntity(id);
            _unitOfWork.Lessons.Remove(lesson);
            await _unitOfWork.CommitAsync();
        }

        /// <inheritdoc/>
        public async Task Update(Lesson lessonDto)
        {
            var lesson = await FindLessonEntity(lessonDto.Id);

            lesson.Title = lessonDto.Title;
            lesson.GroupId = lessonDto.GroupId;
            lesson.LectorId = lessonDto.LectorId;
            lesson.IsDenominator = lessonDto.IsDenominator;
            lesson.IsEnumerator = lessonDto.IsEnumerator;
            lesson.Type = lessonDto.Type;
            lesson.Weekday = lessonDto.Weekday;
            lesson.LessonNumber = lessonDto.LessonNumber;
            lesson.ClassroomId = lessonDto.ClassroomId;

            _unitOfWork.Lessons.Update(lesson);
            await _unitOfWork.CommitAsync();
        }

        private async Task<DAL.Entities.Lesson> FindLessonEntity(Guid id)
        {
            var lesson = await _unitOfWork.Lessons.FindAsync(id);

            if (lesson != null)
            {
                return lesson;
            }
            else
            {
                throw new ArgumentException($"Can not find lesson with id {id}");
            }
        }
    }
}
