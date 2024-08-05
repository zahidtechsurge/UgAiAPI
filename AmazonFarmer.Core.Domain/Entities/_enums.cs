using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonFarmer.Core.Domain.Entities
{
    public enum EDesignation
    {
        [Description("Employee")]
        Employee = 1,
        [Description("Admin")]
        Admin = 2,
        [Description("Farmer")]
        Farmer = 3
    }

    public enum EActivityStatus
    {
        DeActive = 0,
        Active = 1,
        PendingForApproval = 2,
        Approved = 3
    }
    public enum EOrderType
    {
        Advance = 1,
        Product = 2,
        OrderReconcile = 3
    }
    public enum EFarmStatus
    {
        Draft = 0,
        PendingForApproval = 1,
        Active = 2
    }
    public enum ERequestType
    {
        [Description("Removed")]
        Removed = -1,
        [Description("saved in draft")]
        Draft = 0,
        [Description("is in process")]
        Active = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("Declined")]
        Declined = 3,
        [Description("Completed")]
        Completed = 4
    }

    public enum Email_Type
    {
        OTPEmail = 1,
        ForgetPasswordEmail = 2
    }
    public enum EOPTVerificationType
    {
        forgetPassword = 1,
        accountVerification = 2
    }

    public enum EAttachmentType
    {
        User_CNIC_Document = 1,
        User_NTN_Document = 1,
        Farm_Document = 2
    }
    public enum EApplicationType
    {
        Farm_Application = 1,
    }
}
