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
            CreateMap<DocumentWithOwnerStatus, DocumentWithOwnerStatusViewModel>()
                .ForMember(dest => dest.UserFullName,
                    opt =>
                        opt.MapFrom<DocumentWithOwnerStatusViewModelUserFullNameResolver>());

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

    internal class DocumentWithOwnerStatusViewModelUserFullNameResolver
        : IValueResolver<DocumentWithOwnerStatus, DocumentWithOwnerStatusViewModel, string>
    {
        public string Resolve(DocumentWithOwnerStatus source, DocumentWithOwnerStatusViewModel destination, string destMember, ResolutionContext context)
        {
            return source.UploaderUserFirstName + " " + source.UploaderUserLastName;
        }
    }
}
