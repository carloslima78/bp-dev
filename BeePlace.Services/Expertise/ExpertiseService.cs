using BeePlace.Infra.DataBasePersistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BeePlace.Model.Expertise.ValueObject;
using BeePlace.Model.Expertise.Entity;
using System.Text;

namespace BeePlace.Services.Expertise
{
    public class ExpertiseService
    {
        private string Connection { get; }

        public ExpertiseService(string connection)
        {
            this.Connection = connection;
        }

        public List<Model.Expertise.Entity.Expertise> GetTree()
        {
            try
            {
                List<Model.Expertise.Entity.Expertise> expertises = new List<Model.Expertise.Entity.Expertise>();

                StandartPersistence standartPersistence = new StandartPersistence(this.Connection);

                expertises = standartPersistence.GetEntities<Model.Expertise.Entity.Expertise>(CommandType.Text,
                    "SELECT IdExpertise, Name, Description, Price, IdFather FROM Expertise where idfather is null",
                    null).ToList();

                if (expertises != null && expertises.Count > 0)
                {
                    foreach (var secondLevel in expertises)
                    {
                        secondLevel.Childs = new List<Model.Expertise.Entity.Expertise>();
                        secondLevel.Childs = standartPersistence.GetEntities<Model.Expertise.Entity.Expertise>(CommandType.Text,
                        "SELECT IdExpertise, Name, Description, Price, IdFather FROM Expertise WHERE IdFather = @IdExpertise",
                        new { IdExpertise = secondLevel.IdExpertise }).ToList();

                        secondLevel.Details = new List<ExpertiseDetail>();
                        secondLevel.Details = standartPersistence.GetEntities<ExpertiseDetail>(CommandType.Text,
                        "SELECT IdExpertiseDetail, IdExpertise, Title, Description, Image, MinPrice FROM ExpertiseDetail WHERE IdExpertise = @IdExpertise",
                        new { IdExpertise = secondLevel.IdExpertise }).ToList();

                        if (secondLevel.Childs != null && secondLevel.Childs.Count > 0)
                        {
                            foreach (var thirdLevel in secondLevel.Childs)
                            {
                                thirdLevel.Childs = new List<Model.Expertise.Entity.Expertise>();
                                thirdLevel.Childs = standartPersistence.GetEntities<Model.Expertise.Entity.Expertise>(CommandType.Text,
                                "SELECT IdExpertise, Name, Description, Price, IdFather FROM Expertise WHERE IdFather = @IdExpertise",
                                new { IdExpertise = thirdLevel.IdExpertise }).ToList();

                                thirdLevel.Details = new List<ExpertiseDetail>();
                                thirdLevel.Details = standartPersistence.GetEntities<ExpertiseDetail>(CommandType.Text,
                                "SELECT IdExpertiseDetail, IdExpertise, Title, Description, Image, MinPrice FROM ExpertiseDetail WHERE IdExpertise = @IdExpertise",
                                new { IdExpertise = thirdLevel.IdExpertise }).ToList();
                            }
                        }
                    }
                }

                return expertises;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Lista especialidades para cadastro de profissionais.
        /// </summary>
        /// <param name="CNAE">CNAE a qual a especialidade está associada.</param>
        /// <returns></returns>
        public List<Model.Expertise.Entity.Expertise> List(CNAE cnae)
        {
            try
            {
                List<Model.Expertise.Entity.Expertise> expertises = new List<Model.Expertise.Entity.Expertise>();

                StandartPersistence standartPersistence = new StandartPersistence(this.Connection);

                expertises = standartPersistence.GetEntities<Model.Expertise.Entity.Expertise>(CommandType.Text,
                    "SELECT IdExpertise, Name, Description, Price, IdFather FROM Expertise WHERE IdCnae = @IdCnae",
                    new { IdCnae = cnae.IdCnae }).ToList();

                return expertises;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Model.Expertise.Entity.Expertise Get(Model.Expertise.Entity.Expertise expertise)
        {
            try
            {
                StandartPersistence standartPersistence = new StandartPersistence(this.Connection);

                expertise = standartPersistence.GetEntities<Model.Expertise.Entity.Expertise>(CommandType.Text,
                    "SELECT IdExpertise, Name, Description, Price, IdFather FROM Expertise WHERE IdExpertise = @IdExpertise",
                    new { IdExpertise = expertise.IdExpertise }).SingleOrDefault();

                expertise.Details = new List<ExpertiseDetail>();
                expertise.Details = standartPersistence.GetEntities<ExpertiseDetail>(CommandType.Text,
                    "SELECT IdExpertiseDetail, IdExpertise, Title, Description, Image, IdFather FROM ExpertiseDetail WHERE IdExpertise = @IdExpertise",
                    new { IdExpertise = expertise.IdExpertise }).ToList();

                return expertise;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<Model.Expertise.Entity.Expertise> ListByCompany(Model.Profile.Company.Entity.Company company)
        {
            try
            {
                StandartPersistence standartPersistence = new StandartPersistence(this.Connection);
                var expertises = new List<Model.Expertise.Entity.Expertise>();

                StringBuilder select = new StringBuilder();
                select.Append("SELECT e.[IdExpertise], e.[Name], e.[Description], e.[Price], e.[IdMCC], e.[IdFather], e.[Icon], e.[IdCnae], ce.[MinCost] FROM Expertise e ");
                select.Append("INNER JOIN [dbo].[CompanyExpertise] ce ON e.IdExpertise = ce.IdExpertise ");
                select.Append("WHERE ce.IdCompany = @IdCompany");

                expertises =
                    standartPersistence.GetEntities<Model.Expertise.Entity.Expertise>(CommandType.Text, select.ToString(),
                    new { IdCompany = company.Id }).ToList();

                return expertises;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void RemoveCompanyAssociation(Model.Profile.Company.Entity.Company company, 
            Model.Expertise.Entity.Expertise expertise)
        {
            try
            {
                StandartPersistence standartPersistence = new StandartPersistence(this.Connection);

                standartPersistence.Execute(CommandType.Text,
                           "DELETE CompanyExpertise WHERE IdCompany = @IdCompany AND IdExpertise = @IdExpertise",
                           new { IdCompany = company.Id, IdExpertise = expertise.IdExpertise });
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

