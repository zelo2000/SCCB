using AutoMapper;
using Microsoft.Extensions.Options;
using SCCB.Core.DTO;
using SCCB.Core.Helpers;
using SCCB.Core.Settings;
using SCCB.Repos.UnitOfWork;
using System;
using System.Threading.Tasks;

namespace SCCB.Services.LessonService
{
    public class LessonService : ILessonService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LessonService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Add(Lesson lessonDto)
        {
            var lesson = _mapper.Map<DAL.Entities.Lesson>(lessonDto);
            await _unitOfWork.Lessons.AddAsync(lesson);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Lesson> Find(Guid id)
        {
            var lesson = await FindLessonEntity(id);
            var lessonDto = _mapper.Map<Core.DTO.Lesson>(lesson);
            return lessonDto;
        }

        public async Task Remove(Guid id)
        {
            var lesson = await FindLessonEntity(id);
            _unitOfWork.Lessons.Remove(lesson);
            await _unitOfWork.CommitAsync();
        }

        public async Task Update(Lesson lessonDto)
        {
            var lesson = await FindLessonEntity(lessonDto.Id);

            lesson.Title = lessonDto.Title;
            lesson.GroupId = lessonDto.GroupId;
            lesson.LectorId = lessonDto.LectorId;
            lesson.IsDenominator = lessonDto.IsDenominator;
            lesson.IsEnumerator = lessonDto.IsEnumerator;
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
