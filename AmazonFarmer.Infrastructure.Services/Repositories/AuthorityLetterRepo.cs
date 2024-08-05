using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class AuthorityLetterRepo : IAuthorityLetterRepo
    {
        private AmazonFarmerContext _context;

        public AuthorityLetterRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<TblAuthorityLetters>> GetAuthorityLetterLists()
        {
            return _context.AuthorityLetters
                .Include(x => x.AuthorityLetterDetails)
                .ThenInclude(x => x.Products);
        }
    }
}
