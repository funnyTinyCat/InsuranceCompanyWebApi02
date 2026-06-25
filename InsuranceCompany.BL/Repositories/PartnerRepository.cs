using Dapper;
using InsuranceCompany.BL.Interfaces;
using InsuranceCompany.Database.Data.Interfaces;
using InsuranceCompany.Model.Dtos;
using InsuranceCompany.Model.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.BL.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly IDapperConnectionService _connectionService;
        public PartnerRepository(IDapperConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<int> CreatePartnerAsync(PartnerCreateDto partner)
        {
            using (var connection = _connectionService.GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int partnerTypeId = await GetPartnerTypeIdAsync(connection, transaction,
                                     partner.PartnerType);

                        string sql = @" 
                            insert into partners (FirstName, lastname, address, partnernumber, croatianpin,
                                partnerTypeId, createByUser, isforeign, externalCode, gender)
                            values (@FirstName, @LastName, @Address, @PartnerNumber, @CroatianPIN, 
                                @PartnerTypeId, @CreateByUser, @IsForeign, @ExternalCode, @Gender); 
                            select cast(scope_identity() as int); ";

                        var id = await connection.QuerySingleAsync<int>(sql, new
                        {
                            partner.FirstName,
                            partner.LastName,
                            partner.Address,
                            partner.PartnerNumber,
                            partner.CroatianPIN,
                            PartnerTypeId = partnerTypeId,
                            partner.CreateByUser,
                            partner.IsForeign,
                            partner.ExternalCode,
                            partner.Gender
                        }, transaction);

                        partner.Id = id;

                        if (partner.Policies != null)
                        {
                            foreach (var policy in partner.Policies)
                            {
                                await CreatePartnerPolicyAsync(connection, new PartnerPolicy
                                {
                                    PartnerId = id,
                                    PolicyId = policy.Id
                                }, transaction);
                            }
                        }

                        transaction.Commit();

                        return id;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }


                }

            }
        }

        public async Task<IEnumerable<PartnerCreateDto>> GetAllPartnersAsync()
        {
            var sql = GetPartnerSql(false);
            var partners = await QueryPartnersAsync(sql);

            return partners;
        }

        public async Task<PartnerCreateDto?> GetPartnerAsync(int id)
        {
            var sql = GetPartnerSql(true);
            var partners = await QueryPartnersAsync(sql, new { Id = id });

            var partner = partners.FirstOrDefault();
            
            if (partner == null)
                return null;

            return partner;
        }

        public async Task UpdatePartnerAsync(int id, PartnerCreateDto partner)
        {
            using (var connection = _connectionService.GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string sql = @"
                                update partners
                                set firstName = @FirstName, lastName = @LastName, address = @Address, partnerNumber = @PartnerNumber,
                                    croatianPin = @CroatianPin, partnerTypeId = @PartnerTypeId, createByUser = @CreateByUser,
                                    isForeign = @IsForeign, externalCode = @ExternalCode, gender = @Gender                                    
                                where id = @Id;
                            ";

                        await connection.ExecuteAsync(sql, new
                        {
                            partner.FirstName,
                            partner.LastName,
                            partner.Address,
                            partner.PartnerNumber,
                            partner.CroatianPIN,
                            partner.PartnerTypeId,
                            partner.CreateByUser,
                            partner.IsForeign,
                            partner.ExternalCode,
                            partner.Gender,
                            id
                        }, transaction);

                        await DeletePartnerPoliciesAsync(connection, partner.Id, transaction);

                        if (partner.Policies != null)
                        {
                            foreach (var policy in partner.Policies)
                            {
                                await CreatePartnerPolicyAsync(connection, new PartnerPolicy
                                {
                                    PartnerId = partner.Id,
                                    PolicyId = policy.Id
                                }, transaction);
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task DeletePartnerAsync(int id)
        {
            using (var connection = _connectionService.GetConnection())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await DeletePartnerPoliciesAsync(connection, id, transaction);

                        string sql = @"delete from partners where id = @Id;";

                        await connection.ExecuteAsync(sql, new { Id = id }, transaction);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<IEnumerable<PartnerTypeDto>> GetAllPartnerTypesAsync()
        {
            using (var connection = _connectionService.GetConnection())
            {
                var sql = @"select * from partnertypes;
                ";

                return await connection.QueryAsync<PartnerTypeDto>(sql);
            }
        }

        // private helper methods

        private string GetPartnerSql(bool withWhereClause)
        {
            var sql = @"select p.*, pt.*, po.*
                    from partners p
                    left join partnertypes pt on p.partnertypeId = pt.id
                    left join partnerspolicies pp on p.id = pp.partnerid
                    left join policies po on po.id = pp.policyid
                ";
            //                     left join policies po on p.id = po.partnerId;

            if (withWhereClause)
            {
                sql += " where p.id = @Id; ";
            }

            return sql;
        }

        private async Task<IEnumerable<PartnerCreateDto>> QueryPartnersAsync(string sql, object? parameters = null)
        {
            using (var connection = _connectionService.GetConnection())
            {
                var partnerDictionary = new Dictionary<int, PartnerCreateDto>();

                var partners = await connection.QueryAsync<PartnerCreateDto, PartnerType, Policy, PartnerCreateDto>(sql,
                    (partner, partnerType, policy) =>
                    {
                        if (!partnerDictionary.TryGetValue(partner.Id, out var currentPartner))
                        {
                            currentPartner = partner;
                            currentPartner.PartnerType = partnerType;

                            partnerDictionary.Add(currentPartner.Id, currentPartner);
                        }

                        if (policy is not null && !currentPartner.Policies.Any(r => r.Id == policy.Id))
                        {
                            currentPartner.Policies.Add(policy);
                        }

                        return currentPartner;
                    }, parameters
                    //splitOn: "Id, Id, VideoGameId, Id, Id"
                );

                return partnerDictionary.Values;
            }
        }

        private async Task<int> GetPartnerTypeIdAsync(SqlConnection connection, SqlTransaction transaction,
            PartnerType? partnerType)
        {
            if (partnerType == null || string.IsNullOrEmpty(partnerType.Type) || string.IsNullOrEmpty(partnerType.Title))
                return 0;

            string checkSql = "select id from partnertypes where type = @Type ";
            var existingPartnerTypeId = await connection.QueryFirstOrDefaultAsync<int?>(checkSql, new { Type = partnerType.Type },
                transaction);

            if (existingPartnerTypeId.HasValue)
            {
                return existingPartnerTypeId.Value;
            }

            return 0;
        }

        private async Task CreatePartnerPolicyAsync(SqlConnection connection, PartnerPolicy partnerPolicy,
            SqlTransaction transaction)
        {
            string sql = @"insert into partnerspolicies (partnerId, policyId) 
                    values(@PartnerId, @PolicyId); ";

            await connection.ExecuteAsync(sql, partnerPolicy, transaction);
        }

        private async Task DeletePartnerPoliciesAsync(SqlConnection connection, int partnerId, SqlTransaction transaction)
        {
            string sql = @"
                    delete from partnerspolicies where partnerid = @PartnerId;
                ";

            await connection.ExecuteAsync(sql, new { PartnerId = partnerId }, transaction);
        }

    }
}


