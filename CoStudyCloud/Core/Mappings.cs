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
            CreateMap<User, ProfileFormViewModel>();
        }
    }
}
