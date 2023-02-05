using AutoMapper;
using pnb.api.easybox.domain.Model;

namespace pnb.api.easybox.domain.Mapping
{
    public class AutomapperConfiguration : Profile
    {
        public AutomapperConfiguration()
        {
            CreateMap<InsertEnteprise, Users>()
                .ForMember(x => x.userName, md => md.MapFrom(x => x.userNameUserProfile))
                .ForMember(x => x.numberPhone, md => md.MapFrom(x => x.numberPhoneUserProfile))
                .ForMember(x => x.email, md => md.MapFrom(x => x.emailUserUserProfile))
                .ForMember(x => x.profileType, md => md.MapFrom(x => x.profileTypeUserProfile))
                .ForMember(x => x.password, md => md.MapFrom(x => x.passwordEp))
                .ForMember(x => x.dateCreation, md => md.MapFrom(x => x.dateCreation))
                .ForMember(x => x.hookTeams, md => md.MapFrom(x => x.hookTeamsUserProfile))
                .ForMember(x => x.isAdministrator, md => md.MapFrom(x => x.isAdministratorUserProfile))
                .ForMember(x => x.isActive, md => md.MapFrom(x => x.isActiveUserProfile))
                .ForMember(x => x.permission, md => md.MapFrom(x => x.permissionUserProfile))
                .ForMember(x => x.lastLogin, md => md.MapFrom(x => x.dateCreation));

            CreateMap<InsertEnteprise, Login>()
                .ForMember(x => x.userName, md => md.MapFrom(x => x.userNameEp))
                .ForMember(x => x.numberPhone, md => md.MapFrom(x => x.numberPhoneEp))
                .ForMember(x => x.document, md => md.MapFrom(x => x.documentEp))
                .ForMember(x => x.address, md => md.MapFrom(x => x.addressEp))
                .ForMember(x => x.email, md => md.MapFrom(x => x.emailEp))
                .ForMember(x => x.zip, md => md.MapFrom(x => x.zipEp))
                .ForMember(x => x.country, md => md.MapFrom(x => x.countryEp))
                .ForMember(x => x.numberPhone, md => md.MapFrom(x => x.numberPhoneEp))
                .ForMember(x => x.password, md => md.MapFrom(x => x.passwordEp))
                .ForMember(x => x.webhook, md => md.MapFrom(x => x.webhookUrlEp))
                .ForMember(x => x.hookTeams, md => md.MapFrom(x => x.hookTeamsEp))
                .ForMember(x => x.dateCreation, md => md.MapFrom(x => x.dateCreation))
                .ForMember(x => x.lastLogin, md => md.MapFrom(x => x.dateCreation));
        }
    }
}
