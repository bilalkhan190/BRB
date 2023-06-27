using AutoMapper;
using BusinessObjects.Models;
using BusinessObjects.Services.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services
{
    public class ProfessionalService : IProfessionalService
    {
        private readonly Wh4lprodContext _dbContext;
        public ProfessionalService(Wh4lprodContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Affiliation AddAffilation(Affiliation affiliation)
        {
            _dbContext.Affiliations.Add(affiliation);
            _dbContext.SaveChanges();
            return affiliation;
        }

        public AffiliationPosition AddAffilationPosition(AffiliationPosition affiliationPosition)
        {
            _dbContext.AffiliationPositions.Add(affiliationPosition);
            _dbContext.SaveChanges();
            return affiliationPosition;
        }

        public Certificate AddCertificate(Certificate certificate)
        {
            _dbContext.Certificates.Add(certificate);
            _dbContext.SaveChanges();
            return certificate;
        }

        public License AddLicense(License license)
        {
            _dbContext.Licenses.Add(license);
            _dbContext.SaveChanges();
            return license;
        }

        public Professional AddProfessionalMaster(Professional professional)
        {
            _dbContext.Professionals.Add(professional);
            _dbContext.SaveChanges();   
            return professional;
        }
    }
}
