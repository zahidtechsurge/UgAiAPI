using AmazonFarmer.Core.Application.DTOs;
using AmazonFarmer.Core.Application.Interfaces;
using AmazonFarmer.Core.Domain.Entities;
using AmazonFarmer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AmazonFarmer.Infrastructure.Services.Repositories
{
    /// <summary>
    /// Repository for managing attachments in the database.
    /// </summary>
    public class AttachmentRepo : IAttachmentRepo
    {
        private AmazonFarmerContext _context;

        /// <summary>
        /// Constructor to initialize the AttachmentRepo with the AmazonFarmerContext.
        /// </summary>
        /// <param name="context">The AmazonFarmerContext for database operations.</param>
        public AttachmentRepo(AmazonFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Uploads an attachment to the database.
        /// </summary>
        /// <param name="req">The AttachmentsDTO containing attachment information.</param>
        /// <returns>The uploaded attachment entity.</returns>
        public async Task<tblAttachment> uploadAttachment(AttachmentsDTO req)
        {
            // Create a new tblAttachment entity with provided information
            tblAttachment _req = new tblAttachment()
            {
                Guid = Guid.NewGuid(),
                SubmittedDate = DateTime.UtcNow,
                FileType = req.fileType,
                Path = req.filePath,
                tblAttachmentTypeID = (int)req.attachmentType,
                Name = req.fileName
            };

                // Add the attachment to the context and save changes
                _req = _context.Attachment.Add(_req).Entity;
                _context.SaveChanges();
            return _req;
        }

        /// <summary>
        /// Attaches an attachment to a user.
        /// </summary>
        /// <param name="req">The attachAttachment containing attachment and user information.</param>
        public async Task attachUserAttachment(attachAttachment req)
        {
            // Create a new tblUserAttachments entity with provided information
            tblUserAttachments _req = new tblUserAttachments()
            {
                tblAttachmentID = req.attachmentID,
                UserID = req.userID,
                Status = EActivityStatus.Active
            };

            // Add the user attachment to the context and save changes
            _context.UserAttachments.Add(_req);
            //_context.SaveChanges();
        }
        public async Task updateUserAttachment(tblUserAttachments req)
        {
            _context.UserAttachments.Update(req);
        }
        public async Task updateFarmAttachment(tblFarmAttachments req)
        {
            _context.FarmAttachments.Update(req);
        }

        /// <summary>
        /// Get an attachment of a user.
        /// </summary>
        /// <param name="attachmentID">The attachmentID to check.</param>
        public async Task<tblUserAttachments> getUserAttachment(int attachmentID)
        {
            return await _context.UserAttachments.Where(x=>x.tblAttachmentID == attachmentID).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Get an attachment of a farm.
        /// </summary>
        /// <param name="attachmentID">The attachmentID to check.</param>
        public async Task<tblFarmAttachments> getfarmAttachment(int attachmentID)
        {
            return await _context.FarmAttachments.Include(x=>x.Farm).Where(x=>x.AttachmentID == attachmentID).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Attaches an attachment to a farm.
        /// </summary>
        /// <param name="req">The attachAttachment containing attachment and farm information.</param>
        public async Task attachFarmAttachment(attachAttachment req)
        {
            // Create a new tblFarmAttachments entity with provided information
            tblFarmAttachments _req = new tblFarmAttachments()
            {
                AttachmentID = req.attachmentID,
                FarmID = req.farmID,
                Status = EActivityStatus.Active
            };

            // Add the farm attachment to the context and save changes
            _context.FarmAttachments.Add(_req);
            //_context.SaveChanges();
        }
        public async Task<tblAttachment> getAttachmentByGUID(Guid GUID)
        {
            return await _context.Attachment.Where(x => x.Guid == GUID).FirstOrDefaultAsync();
        }
        public async Task<tblAttachment> getAttachmentByID(int attachmentID)
        {
            return await _context.Attachment
                .Include(x => x.AttachmentTypes)
                .Where(x => x.ID == attachmentID).FirstOrDefaultAsync();
        }
    }
}
