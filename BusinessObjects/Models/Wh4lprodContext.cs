using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObjects.Models;

public partial class Wh4lprodContext : DbContext
{
    public Wh4lprodContext()
    {
    }

    public Wh4lprodContext(DbContextOptions<Wh4lprodContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configuration.GetConnectionString("myConn");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    public virtual DbSet<AcademicHonor> AcademicHonors { get; set; }

    public virtual DbSet<AcademicScholarship> AcademicScholarships { get; set; }

    public virtual DbSet<Affiliation> Affiliations { get; set; }

    public virtual DbSet<AffiliationPosition> AffiliationPositions { get; set; }

    public virtual DbSet<Brbadmin> Brbadmins { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<CertificateList> CertificateLists { get; set; }

    public virtual DbSet<ChangeTypeList> ChangeTypeLists { get; set; }

    public virtual DbSet<College> Colleges { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyJob> CompanyJobs { get; set; }

    public virtual DbSet<ContactInfo> ContactInfos { get; set; }

    public virtual DbSet<CountryList> CountryLists { get; set; }

    public virtual DbSet<DegreeList> DegreeLists { get; set; }

    public virtual DbSet<Education> Educations { get; set; }

    public virtual DbSet<FontList> FontLists { get; set; }

    public virtual DbSet<JobAward> JobAwards { get; set; }

    public virtual DbSet<JobCategoryList> JobCategoryLists { get; set; }

    public virtual DbSet<JobQuestion> JobQuestions { get; set; }

    public virtual DbSet<JobResponsibility> JobResponsibilities { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LanguageAbilityList> LanguageAbilityLists { get; set; }

    public virtual DbSet<LanguageSkill> LanguageSkills { get; set; }

    public virtual DbSet<License> Licenses { get; set; }

    public virtual DbSet<LivingSituationList> LivingSituationLists { get; set; }

    public virtual DbSet<MajorList> MajorLists { get; set; }

    public virtual DbSet<MajorSpecialtyList> MajorSpecialtyLists { get; set; }

    public virtual DbSet<MilitaryExperience> MilitaryExperiences { get; set; }

    public virtual DbSet<MilitaryPosition> MilitaryPositions { get; set; }

    public virtual DbSet<MinorList> MinorLists { get; set; }

    public virtual DbSet<ObjectiveList> ObjectiveLists { get; set; }

    public virtual DbSet<ObjectiveSummary> ObjectiveSummaries { get; set; }

    public virtual DbSet<OfferedProductList> OfferedProductLists { get; set; }

    public virtual DbSet<OrgExperience> OrgExperiences { get; set; }

    public virtual DbSet<OrgPosition> OrgPositions { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<OverseasExperience> OverseasExperiences { get; set; }

    public virtual DbSet<OverseasStudy> OverseasStudies { get; set; }

    public virtual DbSet<PositionTypeList> PositionTypeLists { get; set; }

    public virtual DbSet<Professional> Professionals { get; set; }

    public virtual DbSet<ResponsibilityOption> ResponsibilityOptions { get; set; }

    public virtual DbSet<ResponsibilityQuestion> ResponsibilityQuestions { get; set; }

    public virtual DbSet<Resume> Resumes { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<StateList> StateLists { get; set; }

    public virtual DbSet<TechnicalSkill> TechnicalSkills { get; set; }

    public virtual DbSet<UserActivation> UserActivations { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<VolunteerExperience> VolunteerExperiences { get; set; }

    public virtual DbSet<VolunteerOrg> VolunteerOrgs { get; set; }

    public virtual DbSet<VolunteerPosition> VolunteerPositions { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

    public virtual DbSet<WorkCompany> WorkCompanies { get; set; }

    public virtual DbSet<WorkExperience> WorkExperiences { get; set; }

    public virtual DbSet<WorkPosition> WorkPositions { get; set; }

    public virtual DbSet<WorkRespOption> WorkRespOptions { get; set; }

    public virtual DbSet<WorkRespQuestion> WorkRespQuestions { get; set; }

    public virtual DbSet<YearsOfExperienceList> YearsOfExperienceLists { get; set; }
    public virtual DbSet<ResponsibilityOptionsResponse> ResponsibilityOptionsResponses { get; set; }



       

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicHonor>(entity =>
        {
            entity.ToTable("AcademicHonor");

            entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.HonorMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.HonorName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HonorYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<AcademicScholarship>(entity =>
        {
            entity.ToTable("AcademicScholarship");

            entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ScholarshipCriteria)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ScholarshipMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ScholarshipName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ScholarshipYear)
                .HasMaxLength(4)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ResponsibilityOptionsResponse>(entity =>
        {
            entity.ToTable("ResponsibilityOptionsResponse");

        });
            modelBuilder.Entity<Affiliation>(entity =>
        {
            entity.ToTable("Affiliation");

            entity.Property(e => e.AffiliationName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EndedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.StartedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StartedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.StateAbbr)
                .HasMaxLength(2)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AffiliationPosition>(entity =>
        {
            entity.ToTable("AffiliationPosition");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EndedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.OtherInfo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.StartedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StartedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Brbadmin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BRBAdmin");

            entity.Property(e => e.CreatedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.ToTable("Certificate");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ReceivedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ReceivedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.StateAbbr)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CertificateList>(entity =>
        {
            entity.HasKey(e => e.CertificateId);

            entity.ToTable("CertificateList");

            entity.Property(e => e.CertificateDesc)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.SortOrder).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<ChangeTypeList>(entity =>
        {
            entity.HasKey(e => e.ChangeTypeId);

            entity.ToTable("ChangeTypeList");

            entity.Property(e => e.ChangeTypeDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<College>(entity =>
        {
            entity.HasKey(e => e.CollegeId).HasName("PK_Education");

            entity.ToTable("College");

            entity.Property(e => e.CertificateOther)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CollegeCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CollegeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CollegeStateAbbr)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.DegreeOther)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gpa)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("GPA");
            entity.Property(e => e.GradDate).HasColumnType("smalldatetime");
            entity.Property(e => e.HonorProgram)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IncludeGpa).HasColumnName("IncludeGPA");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.MajorOther)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MajorSpecialtyOther)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MinorOther)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SchoolName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.CompanyCity)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyStateAbbr)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EndedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.StartedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StartedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CompanyJob>(entity =>
        {
            entity.HasKey(e => e.CompanyJobId).HasName("PK_CompanyPosition");

            entity.ToTable("CompanyJob");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EndedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.ImprovementDesc)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ProductivityIncrease)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.RevenueIncrease)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.SpecialProject1)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.SpecialProject2)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.StartedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StartedYear)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ContactInfo>(entity =>
        {
            entity.ToTable("ContactInfo");

            entity.Property(e => e.Address1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StateAbbr)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(5)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CountryList>(entity =>
        {
            entity.HasKey(e => e.CountryId);

            entity.ToTable("CountryList");

            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Isoalpha)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("ISOAlpha");
        });

        modelBuilder.Entity<DegreeList>(entity =>
        {
            entity.HasKey(e => e.DegreeId);

            entity.ToTable("DegreeList");

            entity.Property(e => e.DegreeDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.HasKey(e => e.EducationId).HasName("PK_Education_1");

            entity.ToTable("Education");

            entity.HasIndex(e => e.ResumeId, "IX_Education_ResumeId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<FontList>(entity =>
        {
            entity.HasKey(e => e.FontId);

            entity.ToTable("FontList");

            entity.Property(e => e.FontDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<JobAward>(entity =>
        {
            entity.ToTable("JobAward");

            entity.Property(e => e.AwardDesc)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AwardedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.AwardedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<JobCategoryList>(entity =>
        {
            entity.HasKey(e => e.JobCategoryId).HasName("PK_PrimaryResponsibility");

            entity.ToTable("JobCategoryList");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.JobCategoryDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<JobQuestion>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("JobQuestion");

            entity.HasIndex(e => new { e.CompanyJobId, e.CategoryId, e.Number }, "UK_ResponsibilityQuestion_1").IsUnique();

            entity.Property(e => e.Answer)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<JobResponsibility>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("JobResponsibility");

            entity.Property(e => e.OtherText)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.ToTable("Language");

            entity.Property(e => e.Ability)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LanguageName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<LanguageAbilityList>(entity =>
        {
            entity.HasKey(e => e.LanguageAbilityId);

            entity.ToTable("LanguageAbilityList");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.LanguageAbilityDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LanguageSkill>(entity =>
        {
            entity.HasKey(e => e.LanguageSkillId).HasName("PK_LanguageSkills");

            entity.ToTable("LanguageSkill");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<License>(entity =>
        {
            entity.ToTable("License");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.ReceivedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ReceivedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.StateAbbr)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LivingSituationList>(entity =>
        {
            entity.HasKey(e => e.LivingSituationId);

            entity.ToTable("LivingSituationList");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.LivingSituationDesc)
                .HasMaxLength(75)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MajorList>(entity =>
        {
            entity.HasKey(e => e.MajorId);

            entity.ToTable("MajorList");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.MajorDesc)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.SortOrder).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MajorSpecialtyList>(entity =>
        {
            entity.HasKey(e => e.MajorSpecialtyId);

            entity.ToTable("MajorSpecialtyList");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.MajorSpecialtyDesc)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.SortOrder).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<MilitaryExperience>(entity =>
        {
            entity.ToTable("MilitaryExperience");

            entity.Property(e => e.Branch)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EndedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Rank)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.StartedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StartedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MilitaryPosition>(entity =>
        {
            entity.ToTable("MilitaryPosition");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EndedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.MainTraining)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.OtherInfo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.StartedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StartedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MinorList>(entity =>
        {
            entity.HasKey(e => e.MinorId);

            entity.ToTable("MinorList");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.MinorDesc)
                .HasMaxLength(75)
                .IsUnicode(false);
            entity.Property(e => e.SortOrder).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<ObjectiveList>(entity =>
        {
            entity.HasKey(e => e.ObjectiveId);

            entity.ToTable("ObjectiveList");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.ObjectiveDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ObjectiveSummary>(entity =>
        {
            entity.ToTable("ObjectiveSummary");

            entity.HasIndex(e => e.ResumeId, "IX_ObjectiveSummary_ResumeId");

            entity.Property(e => e.ChangeTypeOther)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.CurrentCompanyType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FieldsOfExperience)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.PositionTypeOther)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OfferedProductList>(entity =>
        {
            entity.HasKey(e => e.OfferedProductId);

            entity.ToTable("OfferedProductList");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.OfferedProductName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OrgExperience>(entity =>
        {
            entity.ToTable("OrgExperience");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<OrgPosition>(entity =>
        {
            entity.ToTable("OrgPosition");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EndedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.OtherInfo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.StartedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StartedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("Organization");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EndedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.OrgName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StartedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StartedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.StateAbbr)
                .HasMaxLength(2)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OverseasExperience>(entity =>
        {
            entity.HasKey(e => e.OverseasExperienceId).HasName("PK_OverseasStudy_1");

            entity.ToTable("OverseasExperience");

            entity.HasIndex(e => e.ResumeId, "IX_OverseasExperience_ResumeId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<OverseasStudy>(entity =>
        {
            entity.ToTable("OverseasStudy");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClassesCompleted)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CollegeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LivingSituationOther)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OtherInfo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.StartedDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<PositionTypeList>(entity =>
        {
            entity.HasKey(e => e.PositionTypeId);

            entity.ToTable("PositionTypeList");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.PositionTypeDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Professional>(entity =>
        {
            entity.ToTable("Professional");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<ResponsibilityOption>(entity =>
        {
            entity.HasKey(e => e.RespOptionId);

            entity.ToTable("ResponsibilityOption");

            entity.Property(e => e.Caption)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ResponsibilityQuestion>(entity =>
        {
            entity.HasKey(e => e.RespQuestionId);

            entity.ToTable("ResponsibilityQuestion");

            entity.Property(e => e.Caption)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Resume>(entity =>
        {
            entity.ToTable("Resume");

            entity.Property(e => e.ChosenFont)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.GeneratedDate).HasColumnType("smalldatetime");
            entity.Property(e => e.GeneratedFileName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.OfferedProductId).HasDefaultValueSql("((1))");
            entity.Property(e => e.VoucherCode)
                .HasMaxLength(32)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId).HasName("PK_ResumeSection");

            entity.ToTable("Section");

            entity.Property(e => e.SectionId).ValueGeneratedNever();
            entity.Property(e => e.SectionDesc)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SectionUrl)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StateList>(entity =>
        {
            entity.HasKey(e => e.StateAbbr);

            entity.ToTable("StateList");

            entity.Property(e => e.StateAbbr)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.StateCountry)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.StateName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TechnicalSkill>(entity =>
        {
            entity.ToTable("TechnicalSkill");

            entity.HasIndex(e => e.ResumeId, "IX_TechnicalSkill_ResumeId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.Msexcel).HasColumnName("MSExcel");
            entity.Property(e => e.Msoutlook).HasColumnName("MSOutlook");
            entity.Property(e => e.MspowerPoint).HasColumnName("MSPowerPoint");
            entity.Property(e => e.Msword).HasColumnName("MSWord");
            entity.Property(e => e.MswordPerfect).HasColumnName("MSWordPerfect");
            entity.Property(e => e.OtherDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OtherProgram)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserActivation>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserActivation");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.UniqueToken)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserProfile");

            entity.HasIndex(e => e.UserName, "IX_UserProfile_UserName");

            entity.Property(e => e.Address1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StateAbbr)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(5)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VolunteerExperience>(entity =>
        {
            entity.ToTable("VolunteerExperience");

            entity.HasIndex(e => e.ResumeId, "IX_VolunteerExperience_ResumeId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<VolunteerOrg>(entity =>
        {
            entity.ToTable("VolunteerOrg");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EndedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.StartedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StartedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.StateAbbr)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.VolunteerOrg1)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("VolunteerOrg");
        });

        modelBuilder.Entity<VolunteerPosition>(entity =>
        {
            entity.ToTable("VolunteerPosition");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.EndedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.EndedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.OtherInfo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Responsibility3)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.StartedMonth)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StartedYear)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.ToTable("Voucher");

            entity.HasIndex(e => e.Code, "uc_Voucher_Code").IsUnique();

            entity.Property(e => e.Code)
                .HasMaxLength(32)
                .IsUnicode(false);
            entity.Property(e => e.Comment)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EffectiveDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ExpirationDate).HasColumnType("smalldatetime");
            entity.Property(e => e.GeneratedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.InitialCount).HasDefaultValueSql("((1))");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.OfferedProductId).HasDefaultValueSql("((1))");
            entity.Property(e => e.UrlRestriction)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WorkCompany>(entity =>
        {
            entity.HasKey(e => e.CompanyId);

            entity.ToTable("WorkCompany");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(2)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WorkExperience>(entity =>
        {
            entity.HasKey(e => e.WorkExperienceId).HasName("PK__WorkExpe__55A2B8891920BF5C");

            entity.ToTable("WorkExperience");

            entity.HasIndex(e => e.ResumeId, "IX_WorkExperience_ResumeId");

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
            entity.Property(e => e.LastModDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<WorkPosition>(entity =>
        {
            entity.HasKey(e => e.PositionId);

            entity.ToTable("WorkPosition");

            entity.Property(e => e.OtherResponsibility)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WorkRespOption>(entity =>
        {
            entity.HasKey(e => e.WorkRespId);

            entity.ToTable("WorkRespOption");

            entity.Property(e => e.Other)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WorkRespQuestion>(entity =>
        {
            entity.ToTable("WorkRespQuestion");

            entity.Property(e => e.Answer)
                .HasMaxLength(25)
                .IsUnicode(false);
        });

        modelBuilder.Entity<YearsOfExperienceList>(entity =>
        {
            entity.HasKey(e => e.YearsOfExperienceId);

            entity.ToTable("YearsOfExperienceList");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.YearsOfExperienceDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
