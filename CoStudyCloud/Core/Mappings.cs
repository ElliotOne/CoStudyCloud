using AutoMapper;
using CoStudyCloud.Core.Models;
using CoStudyCloud.Core.ViewModels;

namespace CoStudyCloud.Core
{
    /// <summary>
    /// Represents mappings between models and view models
    /// </summary>
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<DocumentFormViewModel, Document>();

            CreateMap<StudyGroupWithJoinStatus, StudyGroupWithJoinStatusViewModel>();

            CreateMap<StudySessionFormViewModel, StudySession>();
            CreateMap<StudySessionWithGroup, StudySessionWithGroupViewModel>();

            CreateMap<StudyGroup, GroupFormViewModel>();
            CreateMap<GroupFormViewModel, StudyGroup>();

            CreateMap<User, ProfileFormViewModel>();
            CreateMap<User, UserEntryViewModel>();
        }
    }
}
