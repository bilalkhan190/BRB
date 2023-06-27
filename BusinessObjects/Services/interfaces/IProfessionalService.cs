using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services.interfaces
{
    public interface IProfessionalService
    {
        public Professional AddProfessionalMaster(Professional professional);
      public License AddLicense(License license);
      public Affiliation AddAffilation(Affiliation affiliation);
       public Certificate AddCertificate(Certificate certificate);
       public AffiliationPosition AddAffilationPosition(AffiliationPosition affiliationPosition);
    }
}
