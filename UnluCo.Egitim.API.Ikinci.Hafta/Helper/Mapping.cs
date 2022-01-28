using AutoMapper;
using UnluCo.Egitim.API.Ikinci.Hafta.Models;
using UnluCo.Egitim.API.Ikinci.Hafta.Models.Dto;

namespace UnluCo.Egitim.API.Ikinci.Hafta.Helper
{
    public class Mapping: Profile
    {
        public Mapping()
        {
            CreateMap<School, SchoolDto>();
            CreateMap<Student, StudentDto>()
                .ForMember(x => x.SchoolName, p => p.MapFrom(u => u.SchoolId.ToString()));
        }
    }
}
