using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using BusinessObjects.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services
{
    public class ContactInfoService : IContactInfoService
    {
        private readonly Wh4lprodContext _dbContext;
        private readonly IMapper _mapper;
        public ContactInfoService(Wh4lprodContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ContactInfoViewModel GetContactInfo(int resumeId)
        {
            var record = _dbContext.ContactInfos.FirstOrDefault(x => x.ResumeId == resumeId);
            ContactInfoViewModel contactInfoViewModel = new ContactInfoViewModel();
            contactInfoViewModel.ContactInfoId = record.ContactInfoId;
            contactInfoViewModel.FirstName = record.FirstName;
            contactInfoViewModel.LastName = record.LastName;
            contactInfoViewModel.Email = record.Email;
            contactInfoViewModel.Address1 = record.Address1;
            contactInfoViewModel.Address2 = record.Address2;
            contactInfoViewModel.ZipCode = record.ZipCode;
            contactInfoViewModel.ResumeId = record.ResumeId;
            contactInfoViewModel.Phone = record.Phone;
            contactInfoViewModel.Email = record.Email;
            contactInfoViewModel.CreatedDate = record.CreatedDate;
            contactInfoViewModel.LastModDate = record.LastModDate;
            contactInfoViewModel.City = record.City;
            contactInfoViewModel.IsComplete = record.IsComplete;
            contactInfoViewModel.StateAbbr = record.StateAbbr;
            return contactInfoViewModel;
        }

        public ContactInfo AddContactInfo(ContactInfoViewModel contact)
        {
            ContactInfo contactInfo = new ContactInfo();
            contactInfo.FirstName = contact.FirstName;
            contactInfo.LastName = contact.LastName;
            contactInfo.Email = contact.Email;
            contactInfo.Address1 = contact.Address1;
            contactInfo.Address2 = contact.Address2;
            contactInfo.ZipCode = contact.ZipCode;
            contactInfo.ResumeId = contact.ResumeId;
            contactInfo.Phone = contact.Phone;
            contactInfo.Email = contact.Email;
            contactInfo.CreatedDate = DateTime.Today;
            contactInfo.LastModDate = DateTime.Today;
            contactInfo.City = contact.City;
            contactInfo.IsComplete = contact.IsComplete;
            contactInfo.StateAbbr = contact.StateAbbr;
           _dbContext.ContactInfos.Add(contactInfo);
            int contactInfoId = _dbContext.SaveChanges();
            return contactInfo;
        }

        public ContactInfo UpdateContactInfo(ContactInfoViewModel contact)
        {
         var model =   _mapper.Map<ContactInfo>(contact);
            _dbContext.ContactInfos.Update(model);
           _dbContext.SaveChanges();
            return model;
        }
    }
}
