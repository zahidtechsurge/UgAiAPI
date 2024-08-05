using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    public class AttachmentRepo : IAttachmentRepo
    {
        private AmazonFarmerContext _context;

        public AttachmentRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        public async Task<tblAttachment> uploadAttachment(AttachmentsDTO req)
        {
            tblAttachment _req = new tblAttachment()
            {
                Guid= Guid.NewGuid(),
                SubmittedDate= DateTime.Now,
                FileType = req.fileType,
                Path = req.filePath,
                tblAttachmentTypeID = (int)req.attachmentType
            };
            _req = _context.Attachment.Add(_req).Entity;
            _context.SaveChanges();
            return _req;
        }

        public async Task attachUserAttachment(attachAttachment req)
        {
            tblUserAttachments _req = new tblUserAttachments()
            {
                tblAttachmentID = req.attachmentID,
                UserID= req.userID,
                Status = EActivityStatus.Active
            };
            _context.UserAttachments.Add(_req);
            _context.SaveChanges();
        }
        public async Task attachFarmAttachment(attachAttachment req)
        {
            tblFarmAttachments _req = new tblFarmAttachments() {
                AttachmentID = req.attachmentID,
                FarmID = req.farmID,
                Status= EActivityStatus.Active
            };
            _context.FarmAttachments.Add(_req);
            _context.SaveChanges();
        }
    }
}
