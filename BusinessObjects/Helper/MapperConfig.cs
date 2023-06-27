using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Helper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<CollageViewModel, College>();
            CreateMap<College, CollageViewModel>();
            CreateMap<ContactInfoViewModel, ContactInfo>();
            CreateMap<ObjectiveSummeryViewModel, ObjectiveSummary>();
            CreateMap<ObjectiveSummeryViewModel, ObjectiveSummary>();
            CreateMap<ResumeViewModels, Resume>();
            CreateMap<AcademicHonorViewModel, AcademicHonor>();
            CreateMap<AcademicScholorshipViewModel, AcademicScholarship>();
            CreateMap<TechnicalSkillViewModel, TechnicalSkill>();
            //CreateMap<BusinessObjects.Models.MetaData.New.ContactInfo, ContactInfo>();
           
        }
    }
}
