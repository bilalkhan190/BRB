using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface IContactInfoService
    {
        ContactInfo AddContactInfo(ContactInfoViewModel contact);
        ContactInfo UpdateContactInfo(ContactInfoViewModel contact);
        ContactInfoViewModel GetContactInfo(int resumeId);
    }
}
